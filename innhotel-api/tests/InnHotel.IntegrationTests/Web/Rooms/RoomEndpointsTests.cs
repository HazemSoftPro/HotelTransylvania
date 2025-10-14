using System.Net;
using System.Net.Http.Json;
using InnHotel.Core.RoomAggregate;
using InnHotel.Web.Rooms;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace InnHotel.IntegrationTests.Web.Rooms;

public class RoomEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public RoomEndpointsTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task CreateRoom_WithValidManualPrice_ShouldReturnCreated()
    {
        // Arrange
        var request = new CreateRoomRequest
        {
            BranchId = 1,
            RoomTypeId = 1,
            RoomNumber = "TEST-101",
            Status = RoomStatus.Available,
            Floor = 1,
            ManualPrice = 150.75m
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/rooms", request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        
        var roomResponse = await response.Content.ReadFromJsonAsync<RoomRecord>();
        Assert.NotNull(roomResponse);
        Assert.Equal(request.ManualPrice, roomResponse.ManualPrice);
        Assert.Equal(request.RoomNumber, roomResponse.RoomNumber);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100.50)]
    public async Task CreateRoom_WithInvalidManualPrice_ShouldReturnBadRequest(decimal invalidPrice)
    {
        // Arrange
        var request = new CreateRoomRequest
        {
            BranchId = 1,
            RoomTypeId = 1,
            RoomNumber = "TEST-102",
            Status = RoomStatus.Available,
            Floor = 1,
            ManualPrice = invalidPrice
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/rooms", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateRoom_WithValidManualPrice_ShouldReturnOk()
    {
        // Arrange - First create a room
        var createRequest = new CreateRoomRequest
        {
            BranchId = 1,
            RoomTypeId = 1,
            RoomNumber = "TEST-103",
            Status = RoomStatus.Available,
            Floor = 1,
            ManualPrice = 100.00m
        };

        var createResponse = await _client.PostAsJsonAsync("/api/rooms", createRequest);
        var createdRoom = await createResponse.Content.ReadFromJsonAsync<RoomRecord>();
        
        var updateRequest = new UpdateRoomRequest
        {
            RoomTypeId = 1,
            RoomNumber = "TEST-103-UPDATED",
            Status = RoomStatus.Occupied,
            Floor = 2,
            ManualPrice = 200.50m
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/rooms/{createdRoom!.Id}", updateRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var updatedRoom = await response.Content.ReadFromJsonAsync<RoomRecord>();
        Assert.NotNull(updatedRoom);
        Assert.Equal(updateRequest.ManualPrice, updatedRoom.ManualPrice);
        Assert.Equal(updateRequest.RoomNumber, updatedRoom.RoomNumber);
        Assert.Equal(updateRequest.Status, updatedRoom.Status);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-50.25)]
    public async Task UpdateRoom_WithInvalidManualPrice_ShouldReturnBadRequest(decimal invalidPrice)
    {
        // Arrange - First create a room
        var createRequest = new CreateRoomRequest
        {
            BranchId = 1,
            RoomTypeId = 1,
            RoomNumber = "TEST-104",
            Status = RoomStatus.Available,
            Floor = 1,
            ManualPrice = 100.00m
        };

        var createResponse = await _client.PostAsJsonAsync("/api/rooms", createRequest);
        var createdRoom = await createResponse.Content.ReadFromJsonAsync<RoomRecord>();
        
        var updateRequest = new UpdateRoomRequest
        {
            RoomTypeId = 1,
            RoomNumber = "TEST-104",
            Status = RoomStatus.Available,
            Floor = 1,
            ManualPrice = invalidPrice
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/rooms/{createdRoom!.Id}", updateRequest);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetRoom_ShouldReturnRoomWithManualPrice()
    {
        // Arrange - First create a room
        var createRequest = new CreateRoomRequest
        {
            BranchId = 1,
            RoomTypeId = 1,
            RoomNumber = "TEST-105",
            Status = RoomStatus.Available,
            Floor = 1,
            ManualPrice = 175.25m
        };

        var createResponse = await _client.PostAsJsonAsync("/api/rooms", createRequest);
        var createdRoom = await createResponse.Content.ReadFromJsonAsync<RoomRecord>();

        // Act
        var response = await _client.GetAsync($"/api/rooms/{createdRoom!.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var room = await response.Content.ReadFromJsonAsync<RoomRecord>();
        Assert.NotNull(room);
        Assert.Equal(createRequest.ManualPrice, room.ManualPrice);
        Assert.Equal(createRequest.RoomNumber, room.RoomNumber);
    }

    [Fact]
    public async Task ListRooms_ShouldReturnRoomsWithManualPrices()
    {
        // Arrange - Create multiple rooms
        var room1Request = new CreateRoomRequest
        {
            BranchId = 1,
            RoomTypeId = 1,
            RoomNumber = "TEST-106",
            Status = RoomStatus.Available,
            Floor = 1,
            ManualPrice = 120.00m
        };

        var room2Request = new CreateRoomRequest
        {
            BranchId = 1,
            RoomTypeId = 1,
            RoomNumber = "TEST-107",
            Status = RoomStatus.Available,
            Floor = 1,
            ManualPrice = 180.50m
        };

        await _client.PostAsJsonAsync("/api/rooms", room1Request);
        await _client.PostAsJsonAsync("/api/rooms", room2Request);

        // Act
        var response = await _client.GetAsync("/api/rooms");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var rooms = await response.Content.ReadFromJsonAsync<IEnumerable<RoomRecord>>();
        Assert.NotNull(rooms);
        
        var testRooms = rooms.Where(r => r.RoomNumber.StartsWith("TEST-")).ToList();
        Assert.True(testRooms.Count >= 2);
        Assert.All(testRooms, room => Assert.True(room.ManualPrice > 0));
    }

    [Fact]
    public async Task SearchRooms_ShouldReturnRoomsWithManualPrices()
    {
        // Arrange - Create a room for searching
        var createRequest = new CreateRoomRequest
        {
            BranchId = 1,
            RoomTypeId = 1,
            RoomNumber = "SEARCH-108",
            Status = RoomStatus.Available,
            Floor = 1,
            ManualPrice = 250.75m
        };

        await _client.PostAsJsonAsync("/api/rooms", createRequest);

        // Act
        var response = await _client.GetAsync("/api/rooms/search?roomNumber=SEARCH-108");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var rooms = await response.Content.ReadFromJsonAsync<IEnumerable<RoomRecord>>();
        Assert.NotNull(rooms);
        
        var searchedRoom = rooms.FirstOrDefault(r => r.RoomNumber == "SEARCH-108");
        Assert.NotNull(searchedRoom);
        Assert.Equal(createRequest.ManualPrice, searchedRoom.ManualPrice);
    }

    [Fact]
    public async Task UpdateRoomStatus_ShouldMaintainManualPrice()
    {
        // Arrange - First create a room
        var createRequest = new CreateRoomRequest
        {
            BranchId = 1,
            RoomTypeId = 1,
            RoomNumber = "TEST-109",
            Status = RoomStatus.Available,
            Floor = 1,
            ManualPrice = 300.00m
        };

        var createResponse = await _client.PostAsJsonAsync("/api/rooms", createRequest);
        var createdRoom = await createResponse.Content.ReadFromJsonAsync<RoomRecord>();

        // Act
        var response = await _client.PatchAsync($"/api/rooms/{createdRoom!.Id}/status", 
            JsonContent.Create(new { Status = RoomStatus.UnderMaintenance }));

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var updatedRoom = await response.Content.ReadFromJsonAsync<RoomRecord>();
        Assert.NotNull(updatedRoom);
        Assert.Equal(RoomStatus.UnderMaintenance, updatedRoom.Status);
        Assert.Equal(createRequest.ManualPrice, updatedRoom.ManualPrice); // Price should remain unchanged
    }
}
