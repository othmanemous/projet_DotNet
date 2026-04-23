import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { getHomePath, login } from '../services/authService';

export default function LoginPage() {
  const navigate = useNavigate();
  const [form, setForm] = useState({ email: 'admin@supmti.ma', password: '123456' });
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError('');
    try {
      const auth = await login(form);
      navigate(getHomePath(auth.user.role));
    } catch (err) {
      setError(err.message || 'Connexion impossible');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="auth-page">
      <div className="container">
        <div className="row align-items-center min-vh-100 py-5 g-4">
          <div className="col-lg-6">
            <div className="auth-showcase">
              <span className="badge rounded-pill text-bg-primary px-3 py-2 mb-3">SUPMTI</span>
              <h1 className="display-5 fw-bold">Plateforme de gestion académique SUPMTI</h1>
              <p className="lead text-secondary">Solution web moderne pour la gestion des étudiants, professeurs, filières, cours et services académiques de SUPMTI.</p>
              <div className="row g-3 mt-3">
                {[
                  ['bi-buildings-fill','Campus, départements et filières SUPMTI'],
                  ['bi-mortarboard-fill','Étudiants, parcours, notes et absences'],
                  ['bi-person-badge-fill','Professeurs, modules et affectations pédagogiques'],
                  ['bi-laptop-fill','E-learning, devoirs, quiz et échanges']
                ].map(([icon, text]) => (
                  <div className="col-sm-6" key={text}><div className="mini-feature"><i className={`bi ${icon}`} /><div><h6>{text}</h6><p>Interface adaptée au contexte d’une école d’ingénieurs et de management.</p></div></div></div>
                ))}
              </div>
            </div>
          </div>
          <div className="col-lg-6">
            <div className="auth-card">
              <div className="text-center mb-4">
                <div className="brand-mark mx-auto mb-3">SM</div>
                <h2 className="mb-1">Connexion</h2>
                <p className="text-secondary mb-0">Choisis un compte de démonstration SUPMTI.</p>
              </div>
              <form onSubmit={handleSubmit} className="row g-3">
                <div className="col-12">
                  <label className="form-label">Email</label>
                  <input className="form-control custom-input" type="email" value={form.email} onChange={(e) => setForm({ ...form, email: e.target.value })} required />
                </div>
                <div className="col-12">
                  <label className="form-label">Mot de passe</label>
                  <input className="form-control custom-input" type="password" value={form.password} onChange={(e) => setForm({ ...form, password: e.target.value })} required />
                </div>
                {error && <div className="col-12"><div className="alert alert-danger mb-0">{error}</div></div>}
                <div className="col-12 d-grid"><button className="btn btn-gradient btn-lg" disabled={loading}>{loading ? 'Connexion...' : 'Se connecter'}</button></div>
                <div className="col-12">
                  <div className="demo-box">
                    <div className="fw-semibold mb-2">Comptes</div>
                    <div className="small text-secondary">Administration: admin@supmti.ma / 123456</div>
                    <div className="small text-secondary">Professeur: prof@supmti.ma / 123456</div>
                    <div className="small text-secondary">Étudiant: student@supmti.ma / 123456</div>
                  </div>
                </div>
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
