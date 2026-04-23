import { Navigate, Route, Routes } from 'react-router-dom';
import AppLayout from './components/layout/AppLayout';
import PrivateRoute from './components/layout/PrivateRoute';
import RoleRoute from './components/layout/RoleRoute';
import HomeRedirect from './components/layout/HomeRedirect';
import LoginPage from './pages/LoginPage';
import DashboardPage from './pages/DashboardPage';
import EstablishmentsPage from './pages/EstablishmentsPage';
import DepartmentsPage from './pages/DepartmentsPage';
import FilieresPage from './pages/FilieresPage';
import StudentsPage from './pages/StudentsPage';
import ProfessorsPage from './pages/ProfessorsPage';
import LaboratoriesPage from './pages/LaboratoriesPage';
import CoursesPage from './pages/CoursesPage';
import TimetablePage from './pages/TimetablePage';
import ResourcesPage from './pages/ResourcesPage';
import AssignmentsPage from './pages/AssignmentsPage';
import QuizzesPage from './pages/QuizzesPage';
import ForumsPage from './pages/ForumsPage';
import NotificationsPage from './pages/NotificationsPage';
import ProfilePage from './pages/ProfilePage';
import NotFoundPage from './pages/NotFoundPage';

export default function App() {
  return (
    <Routes>
      <Route path="/login" element={<LoginPage />} />
      <Route path="/" element={<PrivateRoute><AppLayout /></PrivateRoute>}>
        <Route index element={<HomeRedirect />} />

        <Route path="admin/dashboard" element={<RoleRoute allowedRoles={['Administration']}><DashboardPage /></RoleRoute>} />
        <Route path="admin/establishments" element={<RoleRoute allowedRoles={['Administration']}><EstablishmentsPage /></RoleRoute>} />
        <Route path="admin/departments" element={<RoleRoute allowedRoles={['Administration']}><DepartmentsPage /></RoleRoute>} />
        <Route path="admin/filieres" element={<RoleRoute allowedRoles={['Administration']}><FilieresPage /></RoleRoute>} />
        <Route path="admin/students" element={<RoleRoute allowedRoles={['Administration']}><StudentsPage /></RoleRoute>} />
        <Route path="admin/professors" element={<RoleRoute allowedRoles={['Administration']}><ProfessorsPage /></RoleRoute>} />
        <Route path="admin/laboratories" element={<RoleRoute allowedRoles={['Administration']}><LaboratoriesPage /></RoleRoute>} />
        <Route path="admin/courses" element={<RoleRoute allowedRoles={['Administration']}><CoursesPage /></RoleRoute>} />
        <Route path="admin/timetable" element={<RoleRoute allowedRoles={['Administration']}><TimetablePage /></RoleRoute>} />
        <Route path="admin/resources" element={<RoleRoute allowedRoles={['Administration']}><ResourcesPage /></RoleRoute>} />
        <Route path="admin/assignments" element={<RoleRoute allowedRoles={['Administration']}><AssignmentsPage /></RoleRoute>} />
        <Route path="admin/quizzes" element={<RoleRoute allowedRoles={['Administration']}><QuizzesPage /></RoleRoute>} />
        <Route path="admin/forums" element={<RoleRoute allowedRoles={['Administration']}><ForumsPage /></RoleRoute>} />
        <Route path="admin/notifications" element={<RoleRoute allowedRoles={['Administration']}><NotificationsPage /></RoleRoute>} />
        <Route path="admin/profile" element={<RoleRoute allowedRoles={['Administration']}><ProfilePage /></RoleRoute>} />

        <Route path="professor/dashboard" element={<RoleRoute allowedRoles={['Professeur']}><DashboardPage /></RoleRoute>} />
        <Route path="professor/courses" element={<RoleRoute allowedRoles={['Professeur']}><CoursesPage /></RoleRoute>} />
        <Route path="professor/timetable" element={<RoleRoute allowedRoles={['Professeur']}><TimetablePage readOnly /></RoleRoute>} />
        <Route path="professor/resources" element={<RoleRoute allowedRoles={['Professeur']}><ResourcesPage /></RoleRoute>} />
        <Route path="professor/assignments" element={<RoleRoute allowedRoles={['Professeur']}><AssignmentsPage /></RoleRoute>} />
        <Route path="professor/quizzes" element={<RoleRoute allowedRoles={['Professeur']}><QuizzesPage /></RoleRoute>} />
        <Route path="professor/forums" element={<RoleRoute allowedRoles={['Professeur']}><ForumsPage /></RoleRoute>} />
        <Route path="professor/profile" element={<RoleRoute allowedRoles={['Professeur']}><ProfilePage /></RoleRoute>} />

        <Route path="student/dashboard" element={<RoleRoute allowedRoles={['Etudiant']}><DashboardPage /></RoleRoute>} />
        <Route path="student/courses" element={<RoleRoute allowedRoles={['Etudiant']}><CoursesPage readOnly /></RoleRoute>} />
        <Route path="student/timetable" element={<RoleRoute allowedRoles={['Etudiant']}><TimetablePage readOnly /></RoleRoute>} />
        <Route path="student/resources" element={<RoleRoute allowedRoles={['Etudiant']}><ResourcesPage readOnly /></RoleRoute>} />
        <Route path="student/assignments" element={<RoleRoute allowedRoles={['Etudiant']}><AssignmentsPage readOnly /></RoleRoute>} />
        <Route path="student/quizzes" element={<RoleRoute allowedRoles={['Etudiant']}><QuizzesPage readOnly /></RoleRoute>} />
        <Route path="student/forums" element={<RoleRoute allowedRoles={['Etudiant']}><ForumsPage readOnly /></RoleRoute>} />
        <Route path="student/profile" element={<RoleRoute allowedRoles={['Etudiant']}><ProfilePage /></RoleRoute>} />
      </Route>
      <Route path="*" element={<NotFoundPage />} />
    </Routes>
  );
}
