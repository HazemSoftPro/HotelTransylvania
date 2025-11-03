import { useState, useEffect } from "react";
import { ReservationsTable } from "@/components/reservations/ReservationsTable";
import type { ReservationResponse } from "@/types/api/reservation";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Plus, Search, Filter } from "lucide-react";
import { useNavigate } from "react-router-dom";
import { ROUTES } from "@/constants/routes";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { reservationService } from "@/services/reservationService";
import { toast } from "sonner";
import type { ReservationStatus } from "@/types/reservation";
import { statusToNumber } from "@/utils/reservationStatus";

const Reservations = () => {
  const navigate = useNavigate();
  const [searchQuery, setSearchQuery] = useState("");
  const [statusFilter, setStatusFilter] = useState<string>("all");
  const [reservations, setReservations] = useState<ReservationResponse[]>([]);
  const [, setIsLoading] = useState(true);

  useEffect(() => {
    const fetchReservations = async () => {
      try {
        setIsLoading(true);
        const response = await reservationService.getAll(1, 100);
        setReservations(response.items);
      } catch (error) {
        toast.error("Failed to fetch reservations");
        console.error("Error fetching reservations:", error);
      } finally {
        setIsLoading(false);
      }
    };

    fetchReservations();
  }, []);

  const filteredReservations = reservations.filter(reservation => {
    const matchesSearch = 
      reservation.guestName?.toLowerCase().includes(searchQuery.toLowerCase()) ||
      reservation.rooms.some(room => room.roomNumber.includes(searchQuery));

    // Handle both string and numeric status values
    const matchesStatus = statusFilter === "all" || 
      reservation.status === statusFilter || 
      reservation.status === statusToNumber[statusFilter];

    return matchesSearch && matchesStatus;
  });

  const handleReservationClick = (reservation: ReservationResponse) => {
    console.log("Reservation clicked:", reservation);
  };

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div className="space-y-1">
          <h1 className="text-2xl font-bold">Reservations</h1>
          <p className="text-muted-foreground">
            View and manage hotel reservations.
          </p>
        </div>
        <Button onClick={() => navigate(ROUTES.ADD_RESERVATION)}>
          <Plus className="mr-2 h-4 w-4" />
          New Reservation
        </Button>
      </div>

      <div className="flex items-center gap-2">
        <div className="relative flex-1">
          <Search className="absolute left-2.5 top-2.5 h-4 w-4 text-muted-foreground" />
          <Input
            placeholder="Search reservations..."
            className="pl-8"
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
          />
        </div>
        <Select
          value={statusFilter}
          onValueChange={(value) => setStatusFilter(value as ReservationStatus | "all")}
        >
          <SelectTrigger className="w-[180px]">
            <div className="flex items-center gap-2">
              <Filter className="h-4 w-4" />
              <SelectValue placeholder="Filter by status" />
            </div>
          </SelectTrigger>
          <SelectContent>
            <SelectItem value="all">All Statuses</SelectItem>
            <SelectItem value="Pending">Pending</SelectItem>
            <SelectItem value="Confirmed">Confirmed</SelectItem>
            <SelectItem value="CheckedIn">Checked In</SelectItem>
            <SelectItem value="CheckedOut">Checked Out</SelectItem>
            <SelectItem value="Cancelled">Cancelled</SelectItem>
          </SelectContent>
        </Select>
      </div>

      <ReservationsTable
        reservations={filteredReservations}
        onReservationClick={handleReservationClick}
      />
    </div>
  );
};

export default Reservations;
