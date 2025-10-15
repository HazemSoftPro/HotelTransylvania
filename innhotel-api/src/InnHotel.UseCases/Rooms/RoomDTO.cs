namespace InnHotel.UseCases.Rooms;

public record RoomDTO(
    int Id,
    int BranchId,
    string BranchName,
    int RoomTypeId,
    string RoomTypeName,
    int Capacity,
    string RoomNumber,
    RoomStatus Status,
    int Floor,
    decimal ManualPrice
);
