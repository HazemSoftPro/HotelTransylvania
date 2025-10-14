namespace InnHotel.UseCases.RoomTypes;

public record RoomTypeDTO(
    int Id,
    int BranchId,
    string BranchName,
    string Name,
    int Capacity,
    string? Description);
