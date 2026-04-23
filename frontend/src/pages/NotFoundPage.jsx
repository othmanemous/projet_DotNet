import { Link } from 'react-router-dom';

export default function NotFoundPage() {
  return (
    <div className="card dashboard-card border-0 shadow-sm"><div className="card-body p-5 text-center">
      <h2>Page introuvable</h2>
      <p className="text-secondary">La page demandée n’existe pas.</p>
      <Link to="/" className="btn btn-gradient px-4">Retour</Link>
    </div></div>
  );
}
