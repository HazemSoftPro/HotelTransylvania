namespace InnHotel.Web.RoomTypes;

public record RoomTypeRecord(
    int Id,
    int BranchId,
    string BranchName,
    string Name,
    decimal BasePrice,
    int Capacity,
    string? Description);
