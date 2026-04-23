import { useEffect, useState } from 'react';
import api from '../services/api';

export default function NotificationsPage() {
  const [rows, setRows] = useState([]);
  useEffect(() => { api.get('/notifications').then(({ data }) => setRows(data)).catch(() => {}); }, []);
  return (
    <div className="card dashboard-card border-0 shadow-sm"><div className="card-body p-4">
      <div className="section-kicker">Notifications</div>
      <h3 className="mb-3">Alertes et annonces</h3>
      <div className="table-responsive">
        <table className="table modern-table align-middle mb-0">
          <thead><tr><th>Titre</th><th>Utilisateur</th><th>Type</th><th>Date</th></tr></thead>
          <tbody>
            {rows.length === 0 ? <tr><td colSpan="4" className="text-center py-4 text-secondary">Aucune notification.</td></tr> : rows.map((row) => (
              <tr key={row.id}><td>{row.title}</td><td>{row.userName}</td><td><span className="soft-badge">{row.type}</span></td><td>{row.createdAt}</td></tr>
            ))}
          </tbody>
        </table>
      </div>
    </div></div>
  );
}
