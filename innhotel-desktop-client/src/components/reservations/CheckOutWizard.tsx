import { useState } from "react";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "@/components/ui/textarea";
import { CheckCircle, ClipboardCheck, DollarSign, MessageSquare, Star } from "lucide-react";
import { toast } from "sonner";
import type { ReservationResponse } from "@/types/api/reservation";
import { reservationService } from "@/services/reservationService";

interface CheckOutWizardProps {
  reservation: ReservationResponse;
  onComplete: () => void;
  onCancel: () => void;
}

type WizardStep = "inspection" | "charges" | "payment" | "feedback" | "complete";

export function CheckOutWizard({ reservation, onComplete, onCancel }: CheckOutWizardProps) {
  const [currentStep, setCurrentStep] = useState<WizardStep>("inspection");
  const [roomsInspected, setRoomsInspected] = useState<Record<number, boolean>>({});
  const [additionalCharges, setAdditionalCharges] = useState<Array<{ description: string; amount: number }>>([]);
  const [newChargeDesc, setNewChargeDesc] = useState("");
  const [newChargeAmount, setNewChargeAmount] = useState("");
  const [paymentSettled, setPaymentSettled] = useState(false);
  const [feedback, setFeedback] = useState("");
  const [rating, setRating] = useState(0);

  const steps: { id: WizardStep; title: string; icon: any }[] = [
    { id: "inspection", title: "Room Inspection", icon: ClipboardCheck },
    { id: "charges", title: "Additional Charges", icon: DollarSign },
    { id: "payment", title: "Payment Settlement", icon: DollarSign },
    { id: "feedback", title: "Guest Feedback", icon: MessageSquare },
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

  const handleRoomInspection = (roomId: number) => {
    setRoomsInspected(prev => ({ ...prev, [roomId]: true }));
  };

  const allRoomsInspected = reservation.rooms.every(room => roomsInspected[room.roomId]);

  const handleAddCharge = () => {
    if (!newChargeDesc || !newChargeAmount) {
      toast.error("Please enter charge description and amount");
      return;
    }
    setAdditionalCharges([...additionalCharges, { 
      description: newChargeDesc, 
      amount: parseFloat(newChargeAmount) 
    }]);
    setNewChargeDesc("");
    setNewChargeAmount("");
    toast.success("Charge added");
  };

  const totalAdditionalCharges = additionalCharges.reduce((sum, charge) => sum + charge.amount, 0);
  const finalTotal = reservation.totalCost + totalAdditionalCharges;

  const handleCompleteCheckOut = async () => {
    try {
      // Call API to update reservation status to CheckedOut
      await reservationService.updateStatus(reservation.id, "CheckedOut");
      toast.success("Check-out completed successfully!");
      onComplete();
    } catch (error) {
      toast.error("Failed to complete check-out");
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
          {currentStep === "inspection" && (
            <div className="space-y-4">
              <p className="text-sm text-muted-foreground">
                Inspect each room and mark as complete
              </p>
              <div className="space-y-2">
                {reservation.rooms.map((room) => (
                  <div key={room.roomId} className="flex items-center justify-between p-3 border rounded">
                    <div>
                      <p className="font-medium">Room {room.roomNumber}</p>
                      <p className="text-sm text-muted-foreground">{room.roomTypeName}</p>
                    </div>
                    {roomsInspected[room.roomId] ? (
                      <CheckCircle className="h-5 w-5 text-green-500" />
                    ) : (
                      <Button size="sm" onClick={() => handleRoomInspection(room.roomId)}>
                        Mark Inspected
                      </Button>
                    )}
                  </div>
                ))}
              </div>
            </div>
          )}

          {currentStep === "charges" && (
            <div className="space-y-4">
              <div>
                <Label>Original Total</Label>
                <Input value={`$${reservation.totalCost.toFixed(2)}`} disabled />
              </div>
              
              <div className="space-y-2">
                <Label>Additional Charges</Label>
                {additionalCharges.map((charge, index) => (
                  <div key={index} className="flex items-center justify-between p-2 border rounded">
                    <span>{charge.description}</span>
                    <span className="font-medium">${charge.amount.toFixed(2)}</span>
                  </div>
                ))}
              </div>

              <div className="flex gap-2">
                <Input
                  placeholder="Charge description"
                  value={newChargeDesc}
                  onChange={(e) => setNewChargeDesc(e.target.value)}
                />
                <Input
                  type="number"
                  placeholder="Amount"
                  value={newChargeAmount}
                  onChange={(e) => setNewChargeAmount(e.target.value)}
                  className="w-32"
                />
                <Button onClick={handleAddCharge}>Add</Button>
              </div>

              <div className="pt-4 border-t">
                <div className="flex justify-between font-semibold">
                  <span>Final Total</span>
                  <span>${finalTotal.toFixed(2)}</span>
                </div>
              </div>
            </div>
          )}

          {currentStep === "payment" && (
            <div className="space-y-4">
              <div>
                <Label>Final Amount Due</Label>
                <Input value={`$${finalTotal.toFixed(2)}`} disabled />
              </div>
              <div className="flex items-center space-x-2">
                <input
                  type="checkbox"
                  id="paymentSettled"
                  checked={paymentSettled}
                  onChange={(e) => setPaymentSettled(e.target.checked)}
                  className="h-4 w-4"
                />
                <Label htmlFor="paymentSettled">
                  Payment has been settled in full
                </Label>
              </div>
            </div>
          )}

          {currentStep === "feedback" && (
            <div className="space-y-4">
              <div>
                <Label>Guest Rating</Label>
                <div className="flex gap-2 mt-2">
                  {[1, 2, 3, 4, 5].map((star) => (
                    <button
                      key={star}
                      onClick={() => setRating(star)}
                      className="focus:outline-none"
                    >
                      <Star
                        className={`h-8 w-8 ${
                          star <= rating ? "fill-yellow-400 text-yellow-400" : "text-gray-300"
                        }`}
                      />
                    </button>
                  ))}
                </div>
              </div>
              <div>
                <Label htmlFor="feedback">Feedback or Comments</Label>
                <Textarea
                  id="feedback"
                  placeholder="Enter guest feedback..."
                  value={feedback}
                  onChange={(e) => setFeedback(e.target.value)}
                  rows={5}
                />
              </div>
            </div>
          )}

          {currentStep === "complete" && (
            <div className="space-y-4 text-center py-8">
              <CheckCircle className="h-16 w-16 text-green-500 mx-auto" />
              <h3 className="text-xl font-semibold">Ready to Complete Check-Out</h3>
              <p className="text-muted-foreground">
                All steps completed. Click "Complete Check-Out" to finalize.
              </p>
              <div className="text-left max-w-md mx-auto space-y-2 pt-4">
                <div className="flex justify-between">
                  <span>Rooms Inspected:</span>
                  <span className="font-medium">{reservation.rooms.length}</span>
                </div>
                <div className="flex justify-between">
                  <span>Additional Charges:</span>
                  <span className="font-medium">${totalAdditionalCharges.toFixed(2)}</span>
                </div>
                <div className="flex justify-between">
                  <span>Guest Rating:</span>
                  <span className="font-medium">{rating}/5 stars</span>
                </div>
              </div>
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
          {currentStep === "inspection" && (
            <Button onClick={handleNext} disabled={!allRoomsInspected}>
              Continue
            </Button>
          )}
          {currentStep === "charges" && (
            <Button onClick={handleNext}>Continue</Button>
          )}
          {currentStep === "payment" && (
            <Button onClick={handleNext} disabled={!paymentSettled}>
              Continue
            </Button>
          )}
          {currentStep === "feedback" && (
            <Button onClick={handleNext}>Continue</Button>
          )}
          {currentStep === "complete" && (
            <Button onClick={handleCompleteCheckOut}>
              Complete Check-Out
            </Button>
          )}
        </div>
      </div>
    </div>
  );
}