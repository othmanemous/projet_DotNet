import { getCurrentAuth } from '../services/authService';

export default function ProfilePage() {
  const user = getCurrentAuth()?.user || {};
  return (
    <div className="card dashboard-card border-0 shadow-sm"><div className="card-body p-4">
      <div className="d-flex flex-column align-items-center text-center gap-3">
        <div className="profile-avatar">{user.avatar || 'UM'}</div>
        <div>
          <h3 className="mb-1">{user.fullName}</h3>
          <div className="text-secondary">{user.email}</div>
          <div className="soft-badge mt-3">{user.role}</div>
        </div>
      </div>
    </div></div>
  );
}
