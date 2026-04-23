export default function StatCard({ icon, label, value, color = 'purple' }) {
  return (
    <div className={`metric-card metric-${color}`}>
      <div>
        <div className="metric-label">{label}</div>
        <div className="metric-value">{value}</div>
      </div>
      <div className="metric-icon"><i className={`bi ${icon}`} /></div>
    </div>
  );
}
