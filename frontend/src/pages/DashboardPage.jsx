import { useEffect, useState } from 'react';
import api from '../services/api';
import { getCurrentAuth } from '../services/authService';
import StatCard from '../components/common/StatCard';

export default function DashboardPage() {
  const role = getCurrentAuth()?.user?.role || 'Etudiant';
  const [stats, setStats] = useState({});

  useEffect(() => {
    api.get('/dashboard/stats').then(({ data }) => setStats(data)).catch(() => {});
  }, []);

  const cards = [
    { icon: 'bi-buildings-fill', label: 'Établissements', value: stats.establishments ?? 0, color: 'purple' },
    { icon: 'bi-diagram-3-fill', label: 'Départements', value: stats.departments ?? 0, color: 'blue' },
    { icon: 'bi-journal-bookmark-fill', label: 'Filières', value: stats.filieres ?? 0, color: 'green' },
    { icon: 'bi-mortarboard-fill', label: 'Étudiants', value: stats.students ?? 0, color: 'lime' },
    { icon: 'bi-person-badge-fill', label: 'Professeurs', value: stats.professors ?? 0, color: 'purple' },
    { icon: 'bi-book-half', label: 'Cours', value: stats.courses ?? 0, color: 'blue' },
    { icon: 'bi-calendar3', label: 'Créneaux', value: stats.timetable ?? 0, color: 'green' },
    { icon: 'bi-chat-dots-fill', label: 'Forums', value: stats.forums ?? 0, color: 'lime' }
  ];

  return (
    <>
      <div className="hero-panel mb-4">
        <div>
          <div className="section-kicker">Tableau de bord</div>
          <h2 className="hero-title">Bienvenue dans l’espace {role} SUPMTI</h2>
          <p className="hero-text">Espace centralisé de pilotage pour SUPMTI : suivi académique, organisation pédagogique et gestion administrative dans une seule interface.</p>
        </div>
      </div>
      <div className="row g-4 mb-4">
        {cards.map((card) => <div className="col-12 col-sm-6 col-xl-3" key={card.label}><StatCard {...card} /></div>)}
      </div>
      <div className="row g-4">
        <div className="col-12 col-xl-8">
          <div className="card dashboard-card border-0 shadow-sm"><div className="card-body p-4">
            <h4 className="mb-3">Résumé de la plateforme SUPMTI</h4>
            <p className="text-secondary">La plateforme SUPMTI gère les campus, départements, filières, étudiants, professeurs, laboratoires, cours, emplois du temps, ressources pédagogiques, devoirs, quiz et espaces de discussion. Les accès sont séparés entre administration, professeurs et étudiants.</p>
            <div className="tips-list mt-4">
              {['Administration SUPMTI : gestion complète des données académiques', 'Professeur : modules, supports, devoirs, quiz et communication', 'Étudiant : consultation du parcours, emploi du temps et ressources', 'Base MySQL/XAMPP prête pour les démonstrations et tests locaux'].map((tip) => <div className="tip-item" key={tip}><span className="tip-icon"><i className="bi bi-check2-circle" /></span><span>{tip}</span></div>)}
            </div>
          </div></div>
        </div>
        <div className="col-12 col-xl-4">
          <div className="card dashboard-card border-0 shadow-sm"><div className="card-body p-4">
            <h4 className="mb-3">Comptes de démonstration SUPMTI</h4>
            <ul className="list-unstyled mb-0 small text-secondary d-grid gap-2">
              <li><strong>Admin:</strong> admin@supmti.ma / 123456</li>
              <li><strong>Professeur:</strong> prof@supmti.ma / 123456</li>
              <li><strong>Étudiant:</strong> student@supmti.ma / 123456</li>
            </ul>
          </div></div>
        </div>
      </div>
    </>
  );
}
