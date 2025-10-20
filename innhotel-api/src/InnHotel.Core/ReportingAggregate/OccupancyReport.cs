namespace InnHotel.Core.ReportingAggregate;

/// <summary>
/// Represents occupancy statistics for a given period
/// </summary>
public class OccupancyReport
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TotalRooms { get; set; }
    public int OccupiedRooms { get; set; }
    public int AvailableRooms { get; set; }
    public decimal OccupancyRate { get; set; }
    public int TotalReservations { get; set; }
    public int CheckIns { get; set; }
    public int CheckOuts { get; set; }
    public decimal AverageDailyRate { get; set; }
    public decimal RevenuePAR { get; set; } // Revenue Per Available Room
    
    public List<DailyOccupancy> DailyBreakdown { get; set; } = new();
    public List<RoomTypeOccupancy> RoomTypeBreakdown { get; set; } = new();
}

public class DailyOccupancy
{
    public DateTime Date { get; set; }
    public int OccupiedRooms { get; set; }
    public int AvailableRooms { get; set; }
    public decimal OccupancyRate { get; set; }
    public decimal Revenue { get; set; }
}

public class RoomTypeOccupancy
{
    public string RoomType { get; set; } = string.Empty;
    public int TotalRooms { get; set; }
    public int OccupiedRooms { get; set; }
    public decimal OccupancyRate { get; set; }
    public decimal Revenue { get; set; }
}