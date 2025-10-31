import { useNavigate } from "react-router-dom";
import { toast } from "sonner";
import type { EmployeeFormData } from "@/schemas/employeeSchema";
import { EmployeeForm } from "@/components/employees/EmployeeForm";
import FormLayout from "@/layouts/FormLayout";
import { employeeService } from "@/services/employeeService";
import { RoleGuard } from "@/hooks/RoleGuard";
import { UserRole } from "@/types/api/user";

const RegisterEmployee = () => {
  const navigate = useNavigate();

  RoleGuard(UserRole.SuperAdmin);

  const handleSubmit = async (data: EmployeeFormData) => {
    try {
      // Form data already matches API expectations with camelCase
      const employeeData = {
        firstName: data.firstName,
        lastName: data.lastName,
        email: data.email || null,
        phone: data.phone || null,
        branchId: data.branchId,
        hireDate: data.hireDate,
        position: data.position,
        userId: null
      };

      await employeeService.create(employeeData);
      toast.success("Employee registered successfully");
      navigate(-1);
    } catch (error) {
      toast.error(error instanceof Error ? error.message : "Failed to register employee");
    }
  };

  return (
    <FormLayout
      title="Register New Employee"
      description="Add a new employee to the system."
    >
      <EmployeeForm onSubmit={handleSubmit} />
    </FormLayout>
  );
};

export default RegisterEmployee;