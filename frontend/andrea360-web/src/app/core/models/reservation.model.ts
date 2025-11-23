export interface Reservation {
  id: string;
  sessionId: string;
  reservedAt: string; 
  userId: string;
  fullName: string;
  isCancelled: boolean;
}
