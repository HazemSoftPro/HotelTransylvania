using InnHotel.Core.RoomAggregate;
using Xunit;

namespace InnHotel.UnitTests.Core.RoomAggregate;

public class RoomTypeTests
{
    [Fact]
    public void Constructor_WithValidParameters_ShouldCreateRoomType()
    {
        // Arrange
        var branchId = 1;
        var name = "Standard Room";
        var capacity = 2;
        var description = "A comfortable standard room";

        // Act
        var roomType = new RoomType(branchId, name, capacity, description);

        // Assert
        Assert.Equal(branchId, roomType.BranchId);
        Assert.Equal(name, roomType.Name);
        Assert.Equal(capacity, roomType.Capacity);
        Assert.Equal(description, roomType.Description);
    }

    [Fact]
    public void Constructor_WithNullDescription_ShouldCreateRoomType()
    {
        // Arrange
        var branchId = 1;
        var name = "Standard Room";
        var capacity = 2;
        string? description = null;

        // Act
        var roomType = new RoomType(branchId, name, capacity, description);

        // Assert
        Assert.Equal(branchId, roomType.BranchId);
        Assert.Equal(name, roomType.Name);
        Assert.Equal(capacity, roomType.Capacity);
        Assert.Null(roomType.Description);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public void Constructor_WithInvalidBranchId_ShouldThrowArgumentException(int invalidBranchId)
    {
        // Arrange
        var name = "Standard Room";
        var capacity = 2;
        var description = "A comfortable standard room";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => 
            new RoomType(invalidBranchId, name, capacity, description));
    }

    [Theory]
    [InlineData("")]
    public void Constructor_WithInvalidName_ShouldThrowArgumentException(string? invalidName)
    {
        // Arrange
        var branchId = 1;
        var capacity = 2;
        var description = "A comfortable standard room";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => 
            new RoomType(branchId, invalidName!, capacity, description));
    }

    [Fact]
    public void Constructor_WithNullName_ShouldThrowArgumentNullException()
    {
        // Arrange
        var branchId = 1;
        var capacity = 2;
        var description = "A comfortable standard room";

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            new RoomType(branchId, null!, capacity, description));
    }

    [Theory]
    [InlineData("   ")]
    public void Constructor_WithWhitespaceName_ShouldNotThrowException(string? invalidName)
    {
        // Arrange
        var branchId = 1;
        var capacity = 2;
        var description = "A comfortable standard room";

        // Act & Assert - Guard.Against.NullOrEmpty allows whitespace-only strings
        var roomType = new RoomType(branchId, invalidName!, capacity, description);
        Assert.Equal(invalidName, roomType.Name);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-5)]
    public void Constructor_WithInvalidCapacity_ShouldThrowArgumentException(int invalidCapacity)
    {
        // Arrange
        var branchId = 1;
        var name = "Standard Room";
        var description = "A comfortable standard room";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => 
            new RoomType(branchId, name, invalidCapacity, description));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(4)]
    [InlineData(10)]
    public void Constructor_WithValidCapacity_ShouldSetCapacity(int validCapacity)
    {
        // Arrange
        var branchId = 1;
        var name = "Standard Room";
        var description = "A comfortable standard room";

        // Act
        var roomType = new RoomType(branchId, name, validCapacity, description);

        // Assert
        Assert.Equal(validCapacity, roomType.Capacity);
    }

    [Fact]
    public void UpdateDetails_WithValidParameters_ShouldUpdateRoomType()
    {
        // Arrange
        var roomType = new RoomType(1, "Standard Room", 2, "Original description");
        var newBranchId = 2;
        var newName = "Deluxe Room";
        var newCapacity = 4;
        var newDescription = "Updated description";

        // Act
        roomType.UpdateDetails(newBranchId, newName, newCapacity, newDescription);

        // Assert
        Assert.Equal(newBranchId, roomType.BranchId);
        Assert.Equal(newName, roomType.Name);
        Assert.Equal(newCapacity, roomType.Capacity);
        Assert.Equal(newDescription, roomType.Description);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void UpdateDetails_WithInvalidBranchId_ShouldThrowArgumentException(int invalidBranchId)
    {
        // Arrange
        var roomType = new RoomType(1, "Standard Room", 2, "Description");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => 
            roomType.UpdateDetails(invalidBranchId, "Valid Name", 2, "Valid Description"));
    }

    [Theory]
    [InlineData("")]
    public void UpdateDetails_WithInvalidName_ShouldThrowArgumentException(string? invalidName)
    {
        // Arrange
        var roomType = new RoomType(1, "Standard Room", 2, "Description");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => 
            roomType.UpdateDetails(1, invalidName!, 2, "Valid Description"));
    }

    [Fact]
    public void UpdateDetails_WithNullName_ShouldThrowArgumentNullException()
    {
        // Arrange
        var roomType = new RoomType(1, "Standard Room", 2, "Description");

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            roomType.UpdateDetails(1, null!, 2, "Valid Description"));
    }

    [Theory]
    [InlineData("   ")]
    public void UpdateDetails_WithWhitespaceName_ShouldNotThrowException(string? invalidName)
    {
        // Arrange
        var roomType = new RoomType(1, "Standard Room", 2, "Description");

        // Act & Assert - Guard.Against.NullOrEmpty allows whitespace-only strings
        roomType.UpdateDetails(1, invalidName!, 2, "Valid Description");
        Assert.Equal(invalidName, roomType.Name);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void UpdateDetails_WithInvalidCapacity_ShouldThrowArgumentException(int invalidCapacity)
    {
        // Arrange
        var roomType = new RoomType(1, "Standard Room", 2, "Description");

        // Act & Assert
        Assert.Throws<ArgumentException>(() => 
            roomType.UpdateDetails(1, "Valid Name", invalidCapacity, "Valid Description"));
    }

    [Fact]
    public void UpdateDetails_WithNullDescription_ShouldSetDescriptionToNull()
    {
        // Arrange
        var roomType = new RoomType(1, "Standard Room", 2, "Original description");

        // Act
        roomType.UpdateDetails(1, "Updated Name", 2, null);

        // Assert
        Assert.Null(roomType.Description);
    }

    [Theory]
    [InlineData("Single Room")]
    [InlineData("Double Room")]
    [InlineData("Suite")]
    [InlineData("Presidential Suite")]
    public void Constructor_WithValidName_ShouldSetName(string validName)
    {
        // Arrange
        var branchId = 1;
        var capacity = 2;
        var description = "Description";

        // Act
        var roomType = new RoomType(branchId, validName, capacity, description);

        // Assert
        Assert.Equal(validName, roomType.Name);
    }
}
