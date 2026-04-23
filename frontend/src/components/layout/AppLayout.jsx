import { Link, NavLink, Outlet, useNavigate } from 'react-router-dom';
import { navigationByRole } from '../../config/modules';
import { getCurrentAuth, getHomePath, logout } from '../../services/authService';
import { useState } from 'react';

export default function AppLayout() {
  const navigate = useNavigate();
  const auth = getCurrentAuth();
  const user = auth?.user || { fullName: 'Utilisateur SUPMTI', role: 'Etudiant', avatar: 'SM' };
  const navItems = navigationByRole[user.role] || [];
  const [open, setOpen] = useState(false);
  const sectionPrefix = user.role === 'Administration' ? '/admin' : user.role === 'Professeur' ? '/professor' : '/student';

  return (
    <div className="app-shell">
      <aside className={`app-sidebar ${open ? 'show' : ''}`}>
        <div className="sidebar-brand">
          <div className="brand-mark"><img src="/public/images.png" alt="" className="logo" /></div>
          <div>
            <h5 className="mb-0">SUPMTI Campus</h5>
            <small className="text-secondary">{user.role}</small>
          </div>
        </div>
        <div className="role-ribbon"><i className="bi bi-shield-check me-2" />{user.role}</div>
        <nav className="nav flex-column gap-1 mt-3">
          {navItems.map((item) => (
            <NavLink key={item.to} to={item.to} className={({ isActive }) => `nav-link app-nav-link ${isActive ? 'active' : ''}`} onClick={() => setOpen(false)}>
              <i className={`bi ${item.icon}`} /><span>{item.label}</span>
            </NavLink>
          ))}
        </nav>
        <div className="sidebar-user mt-auto">
          <div className="avatar-badge">{user.avatar}</div>
          <div>
            <div className="fw-semibold small">{user.fullName}</div>
            <div className="text-secondary small">{user.email}</div>
          </div>
        </div>
      </aside>
      {open && <div className="sidebar-backdrop" onClick={() => setOpen(false)} />}

      <div className="app-main">
        <header className="topbar">
          <div className="d-flex align-items-center gap-3">
            <button className="btn btn-outline-primary d-lg-none" onClick={() => setOpen((v) => !v)}><i className="bi bi-list" /></button>
            <div>
            
              <h4 className="mb-0">Tableau de bord</h4>
            </div>
          </div>
          <div className="d-flex align-items-center gap-2 flex-wrap justify-content-end">
            <Link className="btn btn-light topbar-btn" to={getHomePath(user.role)}><i className="bi bi-house-door-fill me-2" />Accueil</Link>
            <Link className="btn btn-light topbar-btn" to={`${sectionPrefix}/profile`}><i className="bi bi-person-circle me-2" />Profil</Link>
            <button className="btn btn-danger-subtle text-danger-emphasis" onClick={() => { logout(); navigate('/login'); }}>
              <i className="bi bi-box-arrow-right me-2" />Déconnexion
            </button>
          </div>
        </header>
        <main className="app-content"><Outlet /></main>
      </div>
    </div>
  );
}
