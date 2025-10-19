import { useState } from "react";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
import { CheckCircle, User, Home, CreditCard, FileText } from "lucide-react";
import { toast } from "sonner";
import type { ReservationResponse } from "@/types/api/reservation";
import { reservationService } from "@/services/reservationService";

interface CheckInWizardProps {
  reservation: ReservationResponse;
  onComplete: () => void;
  onCancel: () => void;
}

type WizardStep = "verification" | "room-assignment" | "payment" | "special-requests" | "complete";

export function CheckInWizard({ reservation, onComplete, onCancel }: CheckInWizardProps) {
  const [currentStep, setCurrentStep] = useState<WizardStep>("verification");
  const [guestVerified, setGuestVerified] = useState(false);
  const [idNumber, setIdNumber] = useState("");
  const [specialRequests, setSpecialRequests] = useState("");
  const [paymentConfirmed, setPaymentConfirmed] = useState(false);

  const steps: { id: WizardStep; title: string; icon: any }[] = [
    { id: "verification", title: "Guest Verification", icon: User },
    { id: "room-assignment", title: "Room Assignment", icon: Home },
    { id: "payment", title: "Payment Verification", icon: CreditCard },
    { id: "special-requests", title: "Special Requests", icon: FileText },
    { id: "complete", title: "Complete", icon: CheckCircle },
  ];

  const currentStepIndex = steps.findIndex(s => s.id === currentStep);

  const handleNext = () => {
    const nextIndex = currentStepIndex + 1;
    if (nextIndex < steps.length) {
      setCurrentStep(steps[nextIndex].id);
    }
  };

  const handlePrevious = () => {
    const prevIndex = currentStepIndex - 1;
    if (prevIndex >= 0) {
      setCurrentStep(steps[prevIndex].id);
    }
  };

  const handleVerifyGuest = () => {
    if (!idNumber) {
      toast.error("Please enter guest ID number");
      return;
    }
    setGuestVerified(true);
    toast.success("Guest verified successfully");
    handleNext();
  };

  const handleCompleteCheckIn = async () => {
    try {
      // Call API to update reservation status to CheckedIn
      await reservationService.updateStatus(reservation.id, "CheckedIn");
      toast.success("Check-in completed successfully!");
      onComplete();
    } catch (error) {
      toast.error("Failed to complete check-in");
      console.error(error);
    }
  };

  return (
    <div className="space-y-6">
      {/* Progress Steps */}
      <div className="flex items-center justify-between">
        {steps.map((step, index) => {
          const StepIcon = step.icon;
          const isActive = currentStep === step.id;
          const isCompleted = index < currentStepIndex;

          return (
            <div key={step.id} className="flex items-center">
              <div className={`flex flex-col items-center ${index > 0 ? 'ml-4' : ''}`}>
                <div
                  className={`flex h-10 w-10 items-center justify-center rounded-full border-2 ${
                    isActive
                      ? "border-primary bg-primary text-primary-foreground"
                      : isCompleted
                      ? "border-primary bg-primary text-primary-foreground"
                      : "border-muted bg-background"
                  }`}
                >
                  <StepIcon className="h-5 w-5" />
                </div>
                <span className="mt-2 text-xs text-center max-w-[80px]">{step.title}</span>
              </div>
              {index < steps.length - 1 && (
                <div className={`h-0.5 w-16 ${isCompleted ? 'bg-primary' : 'bg-muted'}`} />
              )}
            </div>
          );
        })}
      </div>

      {/* Step Content */}
      <Card>
        <CardHeader>
          <CardTitle>
            {steps.find(s => s.id === currentStep)?.title}
          </CardTitle>
          <CardDescription>
            Reservation #{reservation.id} - {reservation.guestName}
          </CardDescription>
        </CardHeader>
        <CardContent className="space-y-4">
          {currentStep === "verification" && (
            <div className="space-y-4">
              <div>
                <Label htmlFor="guestName">Guest Name</Label>
                <Input id="guestName" value={reservation.guestName} disabled />
              </div>
              <div>
                <Label htmlFor="idNumber">ID Number *</Label>
                <Input
                  id="idNumber"
                  placeholder="Enter guest ID number"
                  value={idNumber}
                  onChange={(e) => setIdNumber(e.target.value)}
                />
              </div>
              <div>
                <Label>Check-in Date</Label>
                <Input value={reservation.checkInDate} disabled />
              </div>
              <div>
                <Label>Check-out Date</Label>
                <Input value={reservation.checkOutDate} disabled />
              </div>
            </div>
          )}

          {currentStep === "room-assignment" && (
            <div className="space-y-4">
              <div>
                <Label>Assigned Rooms</Label>
                <div className="mt-2 space-y-2">
                  {reservation.rooms.map((room) => (
                    <div key={room.roomId} className="flex items-center justify-between p-3 border rounded">
                      <div>
                        <p className="font-medium">Room {room.roomNumber}</p>
                        <p className="text-sm text-muted-foreground">{room.roomTypeName}</p>
                      </div>
                      <CheckCircle className="h-5 w-5 text-green-500" />
                    </div>
                  ))}
                </div>
              </div>
            </div>
          )}

          {currentStep === "payment" && (
            <div className="space-y-4">
              <div>
                <Label>Total Cost</Label>
                <Input value={`$${reservation.totalCost.toFixed(2)}`} disabled />
              </div>
              <div className="flex items-center space-x-2">
                <input
                  type="checkbox"
                  id="paymentConfirmed"
                  checked={paymentConfirmed}
                  onChange={(e) => setPaymentConfirmed(e.target.checked)}
                  className="h-4 w-4"
                />
                <Label htmlFor="paymentConfirmed">
                  Payment has been verified and processed
                </Label>
              </div>
            </div>
          )}

          {currentStep === "special-requests" && (
            <div className="space-y-4">
              <div>
                <Label htmlFor="specialRequests">Special Requests or Notes</Label>
                <Textarea
                  id="specialRequests"
                  placeholder="Enter any special requests or notes..."
                  value={specialRequests}
                  onChange={(e) => setSpecialRequests(e.target.value)}
                  rows={5}
                />
              </div>
            </div>
          )}

          {currentStep === "complete" && (
            <div className="space-y-4 text-center py-8">
              <CheckCircle className="h-16 w-16 text-green-500 mx-auto" />
              <h3 className="text-xl font-semibold">Ready to Complete Check-In</h3>
              <p className="text-muted-foreground">
                All information has been verified. Click "Complete Check-In" to finalize.
              </p>
            </div>
          )}
        </CardContent>
      </Card>

      {/* Navigation Buttons */}
      <div className="flex justify-between">
        <Button variant="outline" onClick={onCancel}>
          Cancel
        </Button>
        <div className="space-x-2">
          {currentStepIndex > 0 && currentStep !== "complete" && (
            <Button variant="outline" onClick={handlePrevious}>
              Previous
            </Button>
          )}
          {currentStep === "verification" && (
            <Button onClick={handleVerifyGuest} disabled={!idNumber}>
              Verify & Continue
            </Button>
          )}
          {currentStep === "room-assignment" && (
            <Button onClick={handleNext}>Continue</Button>
          )}
          {currentStep === "payment" && (
            <Button onClick={handleNext} disabled={!paymentConfirmed}>
              Continue
            </Button>
          )}
          {currentStep === "special-requests" && (
            <Button onClick={handleNext}>Continue</Button>
          )}
          {currentStep === "complete" && (
            <Button onClick={handleCompleteCheckIn}>
              Complete Check-In
            </Button>
          )}
        </div>
      </div>
    </div>
  );
}