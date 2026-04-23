function renderField(field, value, onChange, lookups) {
  if (field.type === 'select') {
    const options = field.options || (lookups[field.optionsKey] || []);
    return (
      <select
        className="form-select"
        value={value ?? ''}
        onChange={(e) => onChange(field.name, e.target.value)}
        required={field.required}
      >
        <option value="">Sélectionner...</option>
        {options.map((option) => (
          <option key={option.value ?? option.id} value={option.value ?? option.id}>
            {option.label ?? option.name ?? option.title}
          </option>
        ))}
      </select>
    );
  }

  if (field.type === 'textarea') {
    return <textarea className="form-control" rows="4" value={value ?? ''} onChange={(e) => onChange(field.name, e.target.value)} required={field.required} />;
  }

  if (field.type === 'checkbox') {
    return (
      <div className="form-check form-switch mt-2">
        <input className="form-check-input" type="checkbox" checked={!!value} onChange={(e) => onChange(field.name, e.target.checked)} />
        <label className="form-check-label">Oui</label>
      </div>
    );
  }

  return (
    <input
      className="form-control"
      type={field.type || 'text'}
      value={value ?? ''}
      onChange={(e) => onChange(field.name, e.target.value)}
      required={field.required}
    />
  );
}

export default function FormModal({ show, title, fields, formData, onChange, onClose, onSubmit, lookups, editing }) {
  if (!show) return null;

  return (
    <div className="modal-backdrop-custom">
      <div className="crud-modal card border-0 shadow-lg">
        <div className="card-body p-4">
          <div className="d-flex justify-content-between align-items-start mb-3">
            <div>
              <div className="section-kicker">Formulaire manuel</div>
              <h4 className="mb-1">{title}</h4>
              <p className="text-secondary mb-0">{editing ? 'Modifier les informations du module.' : 'Ajouter un nouvel élément.'}</p>
            </div>
            <button className="btn btn-light" onClick={onClose}><i className="bi bi-x-lg" /></button>
          </div>

          <form className="row g-3" onSubmit={onSubmit}>
            {fields.map((field) => (
              <div key={field.name} className={field.type === 'textarea' ? 'col-12' : 'col-md-6'}>
                <label className="form-label fw-semibold">{field.label}</label>
                {renderField(field, formData[field.name], onChange, lookups)}
              </div>
            ))}

            <div className="col-12 d-flex justify-content-end gap-2 mt-2">
              <button type="button" className="btn btn-light px-4" onClick={onClose}>Annuler</button>
              <button type="submit" className="btn btn-gradient px-4">{editing ? 'Enregistrer' : 'Ajouter'}</button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
}
