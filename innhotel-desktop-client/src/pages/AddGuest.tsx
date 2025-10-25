import { GuestForm } from "@/components/guests/GuestForm";
import type { GuestReq } from "@/types/api/guest";
import { useNavigate } from "react-router-dom";
import FormLayout from "@/layouts/FormLayout";
import { guestService } from "@/services/guestService";
import { useState } from "react";
import { toast } from "sonner";

const AddGuest = () => {
  const navigate = useNavigate();
  const [isLoading, setIsLoading] = useState(false);

  const handleSubmit = async (data: GuestReq) => {
    try {
      setIsLoading(true);
      // Convert GuestReq to Guest for service call
      const guestData = {
        ...data,
        gender: data.gender === 'Male' ? 0 : 1,
        idProofType: data.idProofType === 'Passport' ? 0 : data.idProofType === 'DriverLicense' ? 1 : 2,
      } as const;
      await guestService.create(guestData);
      toast.success("Guest added successfully");
      navigate(-1);
    } catch (error) {
      console.error("Failed to create guest:", error);
      toast.error("Failed to create guest");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <FormLayout
      title="Add New Guest"
      description="Register a new guest in the system."
    >
      <GuestForm onSubmit={handleSubmit} isLoading={isLoading} />
    </FormLayout>
  );
};

export default AddGuest;
