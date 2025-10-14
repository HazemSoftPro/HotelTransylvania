using InnHotel.Core.RoomAggregate;
using Xunit;

namespace InnHotel.UnitTests.Core.RoomAggregate;

public class RoomTests
{
    [Fact]
    public void Constructor_WithValidParameters_ShouldCreateRoom()
    {
        // Arrange
        var branchId = 1;
        var roomTypeId = 1;
        var roomNumber = "101";
        var status = RoomStatus.Available;
        var floor = 1;
        var manualPrice = 100.50m;

        // Act
        var room = new Room(branchId, roomTypeId, roomNumber, status, floor, manualPrice);

        // Assert
        Assert.Equal(branchId, room.BranchId);
        Assert.Equal(roomTypeId, room.RoomTypeId);
        Assert.Equal(roomNumber, room.RoomNumber);
        Assert.Equal(status, room.Status);
        Assert.Equal(floor, room.Floor);
        Assert.Equal(manualPrice, room.ManualPrice);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100.50)]
    public void Constructor_WithInvalidManualPrice_ShouldThrowArgumentException(decimal invalidPrice)
    {
        // Arrange
        var branchId = 1;
        var roomTypeId = 1;
        var roomNumber = "101";
        var status = RoomStatus.Available;
        var floor = 1;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => 
            new Room(branchId, roomTypeId, roomNumber, status, floor, invalidPrice));
    }

    [Theory]
    [InlineData(0.01)]
    [InlineData(50.00)]
    [InlineData(999.99)]
    [InlineData(1500.75)]
    public void Constructor_WithValidManualPrice_ShouldSetManualPrice(decimal validPrice)
    {
        // Arrange
        var branchId = 1;
        var roomTypeId = 1;
        var roomNumber = "101";
        var status = RoomStatus.Available;
        var floor = 1;

        // Act
        var room = new Room(branchId, roomTypeId, roomNumber, status, floor, validPrice);

        // Assert
        Assert.Equal(validPrice, room.ManualPrice);
    }

    [Fact]
    public void UpdateDetails_WithValidParameters_ShouldUpdateRoom()
    {
        // Arrange
        var room = new Room(1, 1, "101", RoomStatus.Available, 1, 100.00m);
        var newRoomTypeId = 2;
        var newRoomNumber = "102";
        var newStatus = RoomStatus.Occupied;
        var newFloor = 2;
        var newManualPrice = 150.75m;

        // Act
        room.UpdateDetails(newRoomTypeId, newRoomNumber, newStatus, newFloor, newManualPrice);

        // Assert
        Assert.Equal(newRoomTypeId, room.RoomTypeId);
        Assert.Equal(newRoomNumber, room.RoomNumber);
        Assert.Equal(newStatus, room.Status);
        Assert.Equal(newFloor, room.Floor);
        Assert.Equal(newManualPrice, room.ManualPrice);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-50.25)]
    public void UpdateDetails_WithInvalidManualPrice_ShouldThrowArgumentException(decimal invalidPrice)
    {
        // Arrange
        var room = new Room(1, 1, "101", RoomStatus.Available, 1, 100.00m);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => 
            room.UpdateDetails(1, "101", RoomStatus.Available, 1, invalidPrice));
    }

    [Fact]
    public void UpdateStatus_WithValidStatus_ShouldUpdateStatus()
    {
        // Arrange
        var room = new Room(1, 1, "101", RoomStatus.Available, 1, 100.00m);
        var newStatus = RoomStatus.UnderMaintenance;

        // Act
        room.UpdateStatus(newStatus);

        // Assert
        Assert.Equal(newStatus, room.Status);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidRoomNumber_ShouldThrowArgumentException(string invalidRoomNumber)
    {
        // Arrange
        var branchId = 1;
        var roomTypeId = 1;
        var status = RoomStatus.Available;
        var floor = 1;
        var manualPrice = 100.00m;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => 
            new Room(branchId, roomTypeId, invalidRoomNumber, status, floor, manualPrice));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-10)]
    public void Constructor_WithNegativeFloor_ShouldThrowArgumentException(int invalidFloor)
    {
        // Arrange
        var branchId = 1;
        var roomTypeId = 1;
        var roomNumber = "101";
        var status = RoomStatus.Available;
        var manualPrice = 100.00m;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => 
            new Room(branchId, roomTypeId, roomNumber, status, invalidFloor, manualPrice));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(50)]
    public void Constructor_WithValidFloor_ShouldSetFloor(int validFloor)
    {
        // Arrange
        var branchId = 1;
        var roomTypeId = 1;
        var roomNumber = "101";
        var status = RoomStatus.Available;
        var manualPrice = 100.00m;

        // Act
        var room = new Room(branchId, roomTypeId, roomNumber, status, validFloor, manualPrice);

        // Assert
        Assert.Equal(validFloor, room.Floor);
    }
}
