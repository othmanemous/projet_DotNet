import { Navigate } from 'react-router-dom';
import { getCurrentAuth } from '../../services/authService';

export default function RoleRoute({ allowedRoles, children }) {
  const role = getCurrentAuth()?.user?.role;
  return allowedRoles.includes(role) ? children : <Navigate to="/login" replace />;
}
