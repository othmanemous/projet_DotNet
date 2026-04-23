export const moduleConfigs = {
  establishments: {
    endpoint: '/establishments',
    title: 'Gestion des établissements',
    description: 'Créer et organiser les campus, écoles et entités de SUPMTI.',
    columns: [
      { key: 'name', label: 'Nom' },
      { key: 'code', label: 'Code' },
      { key: 'city', label: 'Ville' },
      { key: 'email', label: 'Email' }
    ],
    fields: [
      { name: 'name', label: 'Nom', type: 'text', required: true },
      { name: 'code', label: 'Code', type: 'text', required: true },
      { name: 'city', label: 'Ville', type: 'text', required: true },
      { name: 'address', label: 'Adresse', type: 'text', required: true },
      { name: 'phone', label: 'Téléphone', type: 'text' },
      { name: 'email', label: 'Email', type: 'email' }
    ]
  },
  departments: {
    endpoint: '/departments',
    title: 'Gestion des départements',
    description: 'Organiser les départements et leurs responsables.',
    columns: [
      { key: 'name', label: 'Département' },
      { key: 'code', label: 'Code' },
      { key: 'headName', label: 'Responsable' },
      { key: 'establishmentName', label: 'Établissement' }
    ],
    fields: [
      { name: 'name', label: 'Nom', type: 'text', required: true },
      { name: 'code', label: 'Code', type: 'text', required: true },
      { name: 'headName', label: 'Responsable', type: 'text' },
      { name: 'establishmentId', label: 'Établissement', type: 'select', optionsKey: 'establishments', required: true }
    ]
  },
  filieres: {
    endpoint: '/filieres',
    title: 'Gestion des filières',
    description: 'Définir les parcours et cycles de formation.',
    columns: [
      { key: 'name', label: 'Filière' },
      { key: 'code', label: 'Code' },
      { key: 'cycle', label: 'Cycle' },
      { key: 'departmentName', label: 'Département' }
    ],
    fields: [
      { name: 'name', label: 'Nom', type: 'text', required: true },
      { name: 'code', label: 'Code', type: 'text', required: true },
      { name: 'cycle', label: 'Cycle', type: 'text', required: true },
      { name: 'departmentId', label: 'Département', type: 'select', optionsKey: 'departments', required: true }
    ]
  },
  students: {
    endpoint: '/students',
    title: 'Gestion des étudiants',
    description: 'Gérer les profils étudiants, inscriptions et niveaux.',
    columns: [
      { key: 'fullName', label: 'Étudiant' },
      { key: 'studentNumber', label: 'Matricule' },
      { key: 'filiereName', label: 'Filière' },
      { key: 'status', label: 'Statut', badge: true }
    ],
    fields: [
      { name: 'fullName', label: 'Nom complet', type: 'text', required: true },
      { name: 'email', label: 'Email', type: 'email', required: true },
      { name: 'password', label: 'Mot de passe', type: 'password', required: true, onlyCreate: true },
      { name: 'avatar', label: 'Initiales avatar', type: 'text' },
      { name: 'studentNumber', label: 'Matricule', type: 'text', required: true },
      { name: 'cne', label: 'CNE', type: 'text', required: true },
      { name: 'gender', label: 'Genre', type: 'select', options: [{ value: 'Femme', label: 'Femme' }, { value: 'Homme', label: 'Homme' }] },
      { name: 'level', label: 'Niveau', type: 'text', required: true },
      { name: 'status', label: 'Statut', type: 'select', options: [{ value: 'Actif', label: 'Actif' }, { value: 'Suspendu', label: 'Suspendu' }] },
      { name: 'filiereId', label: 'Filière', type: 'select', optionsKey: 'filieres', required: true },
      { name: 'enrollmentDate', label: 'Date d’inscription', type: 'date' }
    ]
  },
  professors: {
    endpoint: '/professors',
    title: 'Gestion des professeurs',
    description: 'Gérer les enseignants SUPMTI, leurs spécialités et affectations pédagogiques.',
    columns: [
      { key: 'fullName', label: 'Professeur' },
      { key: 'grade', label: 'Grade' },
      { key: 'speciality', label: 'Spécialité' },
      { key: 'departmentName', label: 'Département' }
    ],
    fields: [
      { name: 'fullName', label: 'Nom complet', type: 'text', required: true },
      { name: 'email', label: 'Email', type: 'email', required: true },
      { name: 'password', label: 'Mot de passe', type: 'password', required: true, onlyCreate: true },
      { name: 'avatar', label: 'Initiales avatar', type: 'text' },
      { name: 'departmentId', label: 'Département', type: 'select', optionsKey: 'departments', required: true },
      { name: 'grade', label: 'Grade', type: 'text', required: true },
      { name: 'speciality', label: 'Spécialité', type: 'text', required: true },
      { name: 'office', label: 'Bureau', type: 'text' },
      { name: 'status', label: 'Statut', type: 'select', options: [{ value: 'Actif', label: 'Actif' }, { value: 'En congé', label: 'En congé' }] }
    ]
  },
  laboratories: {
    endpoint: '/laboratories',
    title: 'Gestion des laboratoires',
    description: 'Organiser les laboratoires SUPMTI, leurs ressources et responsables.',
    columns: [
      { key: 'name', label: 'Laboratoire' },
      { key: 'code', label: 'Code' },
      { key: 'departmentName', label: 'Département' },
      { key: 'responsibleProfessorName', label: 'Responsable' }
    ],
    fields: [
      { name: 'name', label: 'Nom', type: 'text', required: true },
      { name: 'code', label: 'Code', type: 'text', required: true },
      { name: 'location', label: 'Localisation', type: 'text', required: true },
      { name: 'mainEquipment', label: 'Équipement principal', type: 'text' },
      { name: 'capacity', label: 'Capacité', type: 'number' },
      { name: 'departmentId', label: 'Département', type: 'select', optionsKey: 'departments', required: true },
      { name: 'responsibleProfessorId', label: 'Professeur responsable', type: 'select', optionsKey: 'professors' }
    ]
  },
  courses: {
    endpoint: '/courses',
    title: 'Gestion des cours',
    description: 'Créer les modules SUPMTI et les lier aux professeurs et filières.',
    columns: [
      { key: 'title', label: 'Cours' },
      { key: 'code', label: 'Code' },
      { key: 'professorName', label: 'Professeur' },
      { key: 'filiereName', label: 'Filière' }
    ],
    fields: [
      { name: 'code', label: 'Code', type: 'text', required: true },
      { name: 'title', label: 'Titre', type: 'text', required: true },
      { name: 'description', label: 'Description', type: 'textarea' },
      { name: 'credits', label: 'Crédits', type: 'number', required: true },
      { name: 'semester', label: 'Semestre', type: 'select', options: [{ value: 'S1', label: 'S1' }, { value: 'S2', label: 'S2' }, { value: 'S3', label: 'S3' }, { value: 'S4', label: 'S4' }, { value: 'S5', label: 'S5' }, { value: 'S6', label: 'S6' }] },
      { name: 'isOnline', label: 'En ligne', type: 'checkbox' },
      { name: 'filiereId', label: 'Filière', type: 'select', optionsKey: 'filieres', required: true },
      { name: 'professorId', label: 'Professeur', type: 'select', optionsKey: 'professors', required: true }
    ]
  },
  timetable: {
    endpoint: '/timetable',
    title: 'Gestion des emplois du temps',
    description: 'Planifier les créneaux, salles et groupes des classes SUPMTI.',
    columns: [
      { key: 'day', label: 'Jour' },
      { key: 'timeSlot', label: 'Horaire' },
      { key: 'courseTitle', label: 'Cours' },
      { key: 'room', label: 'Salle' },
      { key: 'groupName', label: 'Groupe' }
    ],
    fields: [
      { name: 'courseId', label: 'Cours', type: 'select', optionsKey: 'courses', required: true },
      { name: 'professorId', label: 'Professeur', type: 'select', optionsKey: 'professors', required: true },
      { name: 'filiereId', label: 'Filière', type: 'select', optionsKey: 'filieres', required: true },
      { name: 'day', label: 'Jour', type: 'select', options: [{ value: 'Lundi', label: 'Lundi' }, { value: 'Mardi', label: 'Mardi' }, { value: 'Mercredi', label: 'Mercredi' }, { value: 'Jeudi', label: 'Jeudi' }, { value: 'Vendredi', label: 'Vendredi' }, { value: 'Samedi', label: 'Samedi' }] },
      { name: 'timeSlot', label: 'Horaire', type: 'text', required: true },
      { name: 'room', label: 'Salle', type: 'text', required: true },
      { name: 'groupName', label: 'Groupe', type: 'text', required: true },
      { name: 'sessionType', label: 'Type séance', type: 'select', options: [{ value: 'Cours', label: 'Cours' }, { value: 'TD', label: 'TD' }, { value: 'TP', label: 'TP' }] }
    ]
  },
  resources: {
    endpoint: '/resources',
    title: 'Supports de cours',
    description: 'Publier les supports pédagogiques, vidéos et liens e-learning de SUPMTI.',
    columns: [
      { key: 'title', label: 'Support' },
      { key: 'type', label: 'Type' },
      { key: 'courseTitle', label: 'Cours' },
      { key: 'published', label: 'Publié', badge: true }
    ],
    fields: [
      { name: 'courseId', label: 'Cours', type: 'select', optionsKey: 'courses', required: true },
      { name: 'title', label: 'Titre', type: 'text', required: true },
      { name: 'type', label: 'Type', type: 'select', options: [{ value: 'PDF', label: 'PDF' }, { value: 'Vidéo', label: 'Vidéo' }, { value: 'Lien', label: 'Lien' }] },
      { name: 'url', label: 'URL ou chemin', type: 'text', required: true },
      { name: 'published', label: 'Publié', type: 'checkbox' }
    ]
  },
  assignments: {
    endpoint: '/assignments',
    title: 'Gestion des devoirs',
    description: 'Créer des devoirs, TP et projets avec suivi des échéances.',
    columns: [
      { key: 'title', label: 'Devoir' },
      { key: 'courseTitle', label: 'Cours' },
      { key: 'dueDate', label: 'Échéance' },
      { key: 'status', label: 'Statut', badge: true }
    ],
    fields: [
      { name: 'courseId', label: 'Cours', type: 'select', optionsKey: 'courses', required: true },
      { name: 'title', label: 'Titre', type: 'text', required: true },
      { name: 'description', label: 'Description', type: 'textarea' },
      { name: 'dueDate', label: 'Date limite', type: 'datetime-local', required: true },
      { name: 'totalPoints', label: 'Points', type: 'number', required: true },
      { name: 'status', label: 'Statut', type: 'select', options: [{ value: 'Ouvert', label: 'Ouvert' }, { value: 'Fermé', label: 'Fermé' }] }
    ]
  },
  quizzes: {
    endpoint: '/quizzes',
    title: 'Gestion des quiz',
    description: 'Créer des quiz en ligne et gérer leur disponibilité pour les étudiants SUPMTI.',
    columns: [
      { key: 'title', label: 'Quiz' },
      { key: 'courseTitle', label: 'Cours' },
      { key: 'questions', label: 'Questions' },
      { key: 'availability', label: 'Disponibilité', badge: true }
    ],
    fields: [
      { name: 'courseId', label: 'Cours', type: 'select', optionsKey: 'courses', required: true },
      { name: 'title', label: 'Titre', type: 'text', required: true },
      { name: 'questions', label: 'Nombre de questions', type: 'number', required: true },
      { name: 'durationMinutes', label: 'Durée (minutes)', type: 'number', required: true },
      { name: 'availability', label: 'Disponibilité', type: 'select', options: [{ value: 'Disponible', label: 'Disponible' }, { value: 'Brouillon', label: 'Brouillon' }, { value: 'Fermé', label: 'Fermé' }] },
      { name: 'startAt', label: 'Début', type: 'datetime-local' },
      { name: 'endAt', label: 'Fin', type: 'datetime-local' }
    ]
  },
  forums: {
    endpoint: '/forums',
    title: 'Plateforme de discussion',
    description: 'Créer des sujets, lier un cours et gérer les échanges.',
    columns: [
      { key: 'title', label: 'Sujet' },
      { key: 'forum', label: 'Forum' },
      { key: 'createdByName', label: 'Créé par' },
      { key: 'courseTitle', label: 'Cours' }
    ],
    fields: [
      { name: 'title', label: 'Sujet', type: 'text', required: true },
      { name: 'forum', label: 'Forum', type: 'text', required: true },
      { name: 'createdByUserId', label: 'Créé par', type: 'select', optionsKey: 'users', required: true },
      { name: 'courseId', label: 'Cours lié', type: 'select', optionsKey: 'courses' },
      { name: 'isClosed', label: 'Fermé', type: 'checkbox' }
    ]
  }
};

export const navigationByRole = {
  Administration: [
    { to: '/admin/dashboard', icon: 'bi-speedometer2', label: 'Dashboard' },
    { to: '/admin/establishments', icon: 'bi-buildings-fill', label: 'Établissements' },
    { to: '/admin/departments', icon: 'bi-diagram-3-fill', label: 'Départements' },
    { to: '/admin/filieres', icon: 'bi-journal-bookmark-fill', label: 'Filières' },
    { to: '/admin/students', icon: 'bi-mortarboard-fill', label: 'Étudiants' },
    { to: '/admin/professors', icon: 'bi-person-badge-fill', label: 'Professeurs' },
    { to: '/admin/laboratories', icon: 'bi-cpu-fill', label: 'Laboratoires' },
    { to: '/admin/courses', icon: 'bi-book-half', label: 'Cours' },
    { to: '/admin/timetable', icon: 'bi-calendar3', label: 'Emploi du temps' },
    { to: '/admin/resources', icon: 'bi-cloud-arrow-up-fill', label: 'Supports' },
    { to: '/admin/assignments', icon: 'bi-clipboard-check-fill', label: 'Devoirs' },
    { to: '/admin/quizzes', icon: 'bi-patch-question-fill', label: 'Quiz' },
    { to: '/admin/forums', icon: 'bi-chat-dots-fill', label: 'Discussion' },
    { to: '/admin/notifications', icon: 'bi-bell-fill', label: 'Notifications' },
    { to: '/admin/profile', icon: 'bi-person-circle', label: 'Profil' }
  ],
  Professeur: [
    { to: '/professor/dashboard', icon: 'bi-speedometer2', label: 'Dashboard' },
    { to: '/professor/courses', icon: 'bi-book-half', label: 'Mes cours' },
    { to: '/professor/timetable', icon: 'bi-calendar3', label: 'Horaire' },
    { to: '/professor/resources', icon: 'bi-cloud-arrow-up-fill', label: 'Supports' },
    { to: '/professor/assignments', icon: 'bi-clipboard-check-fill', label: 'Devoirs' },
    { to: '/professor/quizzes', icon: 'bi-patch-question-fill', label: 'Quiz' },
    { to: '/professor/forums', icon: 'bi-chat-dots-fill', label: 'Discussion' },
    { to: '/professor/profile', icon: 'bi-person-circle', label: 'Profil' }
  ],
  Etudiant: [
    { to: '/student/dashboard', icon: 'bi-speedometer2', label: 'Dashboard' },
    { to: '/student/courses', icon: 'bi-book-half', label: 'Mes cours' },
    { to: '/student/timetable', icon: 'bi-calendar3', label: 'Emploi du temps' },
    { to: '/student/resources', icon: 'bi-cloud-arrow-up-fill', label: 'Supports' },
    { to: '/student/assignments', icon: 'bi-clipboard-check-fill', label: 'Devoirs' },
    { to: '/student/quizzes', icon: 'bi-patch-question-fill', label: 'Quiz' },
    { to: '/student/forums', icon: 'bi-chat-dots-fill', label: 'Discussion' },
    { to: '/student/profile', icon: 'bi-person-circle', label: 'Profil' }
  ]
};
