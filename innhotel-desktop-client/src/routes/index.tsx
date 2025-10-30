import { Routes, Route, Outlet } from 'react-router-dom';
import { ROUTES } from '@/constants/routes';
import MainLayout from '@/layouts/MainLayout';
import AppLayout from '@/layouts/AppLayout';
import Home from '@/pages/Home';
import Login from '@/pages/Login';
import Rooms from '@/pages/Rooms';
import AddRoom from '@/pages/AddRoom';
import Guests from '@/pages/Guests';
import RegisterEmployee from '@/pages/RegisterEmployee';
import Employees from '@/pages/Employees';
import EmployeeDetails from '@/pages/EmployeeDetails';
import Branches from '@/pages/Branches';
import AddBranch from '@/pages/AddBranch';
import Reservations from '@/pages/Reservations';
import AddReservation from '@/pages/AddReservation';
import BranchDetails from '@/pages/BranchDetails';
import GuestDetails from '@/pages/GuestDetails';
import RoomDetails from '@/pages/RoomDetails';
import RoomTypes from '@/pages/RoomTypes';
import AddRoomType from '@/pages/AddRoomType';
import RoomTypeDetails from '@/pages/RoomTypeDetails';
import Services from '@/pages/Services';
import AddService from '@/pages/AddService';
import ServiceDetails from '@/pages/ServiceDetails';
import { ProtectedRoute } from '@/components/auth/ProtectedRoute';
import AddGuest from '@/pages/AddGuest';
import Dashboard from '@/pages/Dashboard';

const AppRoutes = () => {
  return (
    <Routes>
      {/* Public routes */}
      <Route path={ROUTES.HOME} element={<MainLayout><Outlet /></MainLayout>}>
        <Route index element={
          <ProtectedRoute requireAuth={false}>
            <Home />
          </ProtectedRoute>
        } />
        <Route path={ROUTES.LOGIN} element={
          <ProtectedRoute requireAuth={false}>
            <Login />
          </ProtectedRoute>
        } />
      </Route>

      {/* Protected routes */}
      <Route path={ROUTES.HOME} element={
        <ProtectedRoute>
          <AppLayout><Outlet /></AppLayout>
        </ProtectedRoute>
      }>
        <Route path={ROUTES.DASHBOARD} element={<Dashboard />} />
        <Route path={ROUTES.ROOMS} element={<Rooms />} />
        <Route path={ROUTES.ADD_ROOM} element={<AddRoom />} />
        <Route path={ROUTES.ROOM_DETAILS} element={<RoomDetails />} />
        <Route path={ROUTES.ROOM_TYPES} element={<RoomTypes />} />
        <Route path={ROUTES.ADD_ROOM_TYPE} element={<AddRoomType />} />
        <Route path={ROUTES.ROOM_TYPE_DETAILS} element={<RoomTypeDetails />} />
        <Route path={ROUTES.GUESTS} element={<Guests />} />
        <Route path={ROUTES.ADD_GUEST} element={<AddGuest />} />
        <Route path={ROUTES.GUEST_DETAILS} element={<GuestDetails />} />
        <Route path={ROUTES.EMPLOYEES} element={<Employees />} />
        <Route path={ROUTES.EMPLOYEE_DETAILS} element={<EmployeeDetails />} />
        <Route path={ROUTES.REGISTER_EMPLOYEE} element={<RegisterEmployee />} />
        <Route path={ROUTES.BRANCHES} element={<Branches />} />
        <Route path={ROUTES.ADD_BRANCH} element={<AddBranch />} />
        <Route path={ROUTES.RESERVATIONS} element={<Reservations />} />
        <Route path={ROUTES.ADD_RESERVATION} element={<AddReservation />} />
        <Route path={ROUTES.BRANCH_DETAILS} element={<BranchDetails />} />
        <Route path={ROUTES.SERVICES} element={<Services />} />
        <Route path={ROUTES.ADD_SERVICE} element={<AddService />} />
        <Route path={ROUTES.SERVICE_DETAILS} element={<ServiceDetails />} />
      </Route>
    </Routes>
  );
};

export default AppRoutes;
