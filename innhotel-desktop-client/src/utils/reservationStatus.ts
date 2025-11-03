// Reservation status enum mapping
// Matches the C# enum: Pending=0, Confirmed=1, CheckedIn=2, CheckedOut=3, Cancelled=4

export const ReservationStatusEnum = {
  Pending: 0,
  Confirmed: 1,
  CheckedIn: 2,
  CheckedOut: 3,
  Cancelled: 4,
} as const;

export type ReservationStatusValue = typeof ReservationStatusEnum[keyof typeof ReservationStatusEnum];

export interface StatusStyle {
  color: string;
  bg: string;
  label: string;
}

// Map both string and numeric enum values to display styles
export const statusStyles: Record<string | number, StatusStyle> = {
  // String values (from API when configured with JsonStringEnumConverter)
  'Pending': { color: 'text-yellow-600', bg: 'border-yellow-600/20', label: 'Pending' },
  'Confirmed': { color: 'text-blue-600', bg: 'border-blue-600/20', label: 'Confirmed' },
  'CheckedIn': { color: 'text-green-600', bg: 'border-green-600/20', label: 'Checked In' },
  'CheckedOut': { color: 'text-gray-600', bg: 'border-gray-600/20', label: 'Checked Out' },
  'Cancelled': { color: 'text-red-600', bg: 'border-red-600/20', label: 'Cancelled' },
  // Numeric enum values (from API when not configured with JsonStringEnumConverter)
  [ReservationStatusEnum.Pending]: { color: 'text-yellow-600', bg: 'border-yellow-600/20', label: 'Pending' },
  [ReservationStatusEnum.Confirmed]: { color: 'text-blue-600', bg: 'border-blue-600/20', label: 'Confirmed' },
  [ReservationStatusEnum.CheckedIn]: { color: 'text-green-600', bg: 'border-green-600/20', label: 'Checked In' },
  [ReservationStatusEnum.CheckedOut]: { color: 'text-gray-600', bg: 'border-gray-600/20', label: 'Checked Out' },
  [ReservationStatusEnum.Cancelled]: { color: 'text-red-600', bg: 'border-red-600/20', label: 'Cancelled' },
};

// Helper function to get status style safely
export const getStatusStyle = (status: string | number | undefined): StatusStyle => {
  if (status === undefined || status === null) {
    return statusStyles[ReservationStatusEnum.Pending];
  }
  return statusStyles[status] || statusStyles[ReservationStatusEnum.Pending];
};

// Helper function to get status label
export const getStatusLabel = (status: string | number | undefined): string => {
  return getStatusStyle(status).label;
};

// Map status string to enum number for filtering
export const statusToNumber: Record<string, number> = {
  'Pending': ReservationStatusEnum.Pending,
  'Confirmed': ReservationStatusEnum.Confirmed,
  'CheckedIn': ReservationStatusEnum.CheckedIn,
  'CheckedOut': ReservationStatusEnum.CheckedOut,
  'Cancelled': ReservationStatusEnum.Cancelled,
};
