import { Navigate } from 'react-router-dom';
import { getCurrentAuth, getHomePath } from '../../services/authService';

export default function HomeRedirect() {
  const role = getCurrentAuth()?.user?.role;
  return <Navigate to={getHomePath(role)} replace />;
}
