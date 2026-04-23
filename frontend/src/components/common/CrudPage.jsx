import { useEffect, useMemo, useState } from 'react';
import api from '../../services/api';
import { moduleConfigs } from '../../config/modules';
import FormModal from './FormModal';
import StatCard from './StatCard';

const cardPalette = ['purple', 'blue', 'green', 'lime'];

function defaultState(fields) {
  return fields.reduce((acc, field) => {
    if (field.type === 'checkbox') acc[field.name] = false;
    else acc[field.name] = '';
    return acc;
  }, {});
}

function sanitizePayload(fields, formData) {
  const payload = {};
  for (const field of fields) {
    const value = formData[field.name];
    if (field.type === 'number') {
      payload[field.name] = value === '' ? null : Number(value);
    } else if (field.type === 'select') {
      payload[field.name] = value === '' ? null : (String(value).match(/^\d+$/) ? Number(value) : value);
    } else {
      payload[field.name] = value;
    }
  }
  return payload;
}

export default function CrudPage({ moduleKey, readOnly = false, titleOverride, descriptionOverride }) {
  const config = moduleConfigs[moduleKey];
  const [rows, setRows] = useState([]);
  const [lookups, setLookups] = useState({});
  const [query, setQuery] = useState('');
  const [showModal, setShowModal] = useState(false);
  const [editingItem, setEditingItem] = useState(null);
  const [formData, setFormData] = useState(defaultState(config.fields));
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);

  const formFields = useMemo(() => {
    if (editingItem) return config.fields.filter((field) => !field.onlyCreate);
    return config.fields;
  }, [config.fields, editingItem]);

  const filteredRows = useMemo(() => {
    const search = query.trim().toLowerCase();
    if (!search) return rows;
    return rows.filter((row) => Object.values(row).some((value) => String(value ?? '').toLowerCase().includes(search)));
  }, [rows, query]);

  const stats = useMemo(() => {
    const badgeColumn = config.columns.find((column) => column.badge);
    const activeCount = badgeColumn ? rows.filter((row) => String(row[badgeColumn.key] ?? '').toLowerCase().includes('act') || String(row[badgeColumn.key] ?? '').toLowerCase().includes('publ') || String(row[badgeColumn.key] ?? '').toLowerCase().includes('disp')).length : rows.length;
    return [
      { icon: 'bi-grid-1x2-fill', label: 'Total éléments', value: rows.length, color: cardPalette[0] },
      { icon: 'bi-check-circle-fill', label: 'Actifs / publiés', value: activeCount, color: cardPalette[1] },
      { icon: 'bi-search', label: 'Résultats filtrés', value: filteredRows.length, color: cardPalette[2] },
      { icon: 'bi-pencil-square', label: readOnly ? 'Mode' : 'CRUD', value: readOnly ? 'Lecture seule' : 'Complet', color: cardPalette[3] }
    ];
  }, [rows, filteredRows.length, config.columns, readOnly]);

  async function loadAll() {
    try {
      setLoading(true);
      setError('');
      const [rowsResponse, lookupsResponse] = await Promise.all([
        api.get(config.endpoint),
        api.get('/lookups')
      ]);
      setRows(rowsResponse.data);
      setLookups(lookupsResponse.data);
    } catch (err) {
      setError(err.response?.data?.message || 'Impossible de charger les données. Vérifie le backend MySQL.');
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => { loadAll(); }, [moduleKey]);

  const openCreate = () => {
    setEditingItem(null);
    setFormData(defaultState(config.fields));
    setShowModal(true);
  };

  const openEdit = (item) => {
    setEditingItem(item);
    const nextState = defaultState(config.fields);
    config.fields.forEach((field) => {
      nextState[field.name] = item[field.name] ?? nextState[field.name];
    });
    setFormData(nextState);
    setShowModal(true);
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    try {
      setSaving(true);
      const payload = sanitizePayload(formFields, formData);
      if (editingItem) await api.put(`${config.endpoint}/${editingItem.id}`, payload);
      else await api.post(config.endpoint, payload);
      setShowModal(false);
      await loadAll();
    } catch (err) {
      setError(err.response?.data?.message || 'Enregistrement impossible. Vérifie les relations et les champs requis.');
    } finally {
      setSaving(false);
    }
  };

  const handleDelete = async (item) => {
    if (!window.confirm(`Supprimer « ${item.name || item.title || item.fullName} » ?`)) return;
    try {
      await api.delete(`${config.endpoint}/${item.id}`);
      await loadAll();
    } catch (err) {
      setError(err.response?.data?.message || 'Suppression impossible. L’élément est peut-être lié à d’autres données.');
    }
  };

  const handleChange = (name, value) => setFormData((prev) => ({ ...prev, [name]: value }));

  return (
    <>
      <div className="hero-panel mb-4">
        <div>
          <div className="section-kicker">Module CRUD</div>
          <h2 className="hero-title">{titleOverride || config.title}</h2>
          <p className="hero-text">{descriptionOverride || config.description}</p>
        </div>
        {!readOnly && (
          <button className="btn btn-gradient px-4" onClick={openCreate}>
            <i className="bi bi-plus-circle-fill me-2" />Ajouter manuellement
          </button>
        )}
      </div>

      <div className="row g-4 mb-4">
        {stats.map((stat) => (
          <div key={stat.label} className="col-12 col-sm-6 col-xl-3">
            <StatCard {...stat} />
          </div>
        ))}
      </div>

      <div className="card dashboard-card border-0 shadow-sm">
        <div className="card-body p-4">
          <div className="d-flex flex-wrap justify-content-between align-items-center gap-3 mb-3">
            <div>
              <h4 className="mb-1">Liste du module</h4>
              <p className="text-secondary mb-0">Recherche, création, modification et suppression manuelles.</p>
            </div>
            <div className="search-inline">
              <i className="bi bi-search" />
              <input value={query} onChange={(e) => setQuery(e.target.value)} placeholder="Rechercher..." />
            </div>
          </div>

          {error && <div className="alert alert-warning mb-3">{error}</div>}

          {loading ? (
            <div className="text-center py-5 text-secondary">Chargement des données...</div>
          ) : (
            <div className="table-responsive">
              <table className="table modern-table align-middle mb-0">
                <thead>
                  <tr>
                    {config.columns.map((column) => <th key={column.key}>{column.label}</th>)}
                    <th className="text-end">Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {filteredRows.length === 0 ? (
                    <tr><td colSpan={config.columns.length + 1} className="text-center py-5 text-secondary">Aucune donnée disponible.</td></tr>
                  ) : filteredRows.map((row) => (
                    <tr key={row.id}>
                      {config.columns.map((column) => {
                        const value = row[column.key];
                        return (
                          <td key={column.key}>
                            {column.badge ? <span className="soft-badge">{String(value)}</span> : String(value ?? '—')}
                          </td>
                        );
                      })}
                      <td className="text-end">
                        <div className="d-inline-flex gap-2">
                          {!readOnly && (
                            <>
                              <button className="btn btn-sm btn-outline-primary" onClick={() => openEdit(row)}><i className="bi bi-pencil-square" /></button>
                              <button className="btn btn-sm btn-outline-danger" onClick={() => handleDelete(row)}><i className="bi bi-trash3" /></button>
                            </>
                          )}
                          {readOnly && <span className="text-secondary small">Lecture seule</span>}
                        </div>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}
        </div>
      </div>

      <FormModal
        show={showModal}
        title={titleOverride || config.title}
        fields={formFields}
        formData={formData}
        onChange={handleChange}
        onClose={() => setShowModal(false)}
        onSubmit={handleSubmit}
        lookups={lookups}
        editing={!!editingItem}
      />

      {saving && <div className="saving-indicator">Enregistrement...</div>}
    </>
  );
}
