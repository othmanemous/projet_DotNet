const AUTH_KEY = 'supmti_auth';

const USERS = {
  'admin@supmti.ma': { fullName: 'Administration SUPMTI', role: 'Administration', avatar: 'SM', email: 'admin@supmti.ma', password: '123456' },
  'prof@supmti.ma': { fullName: 'Pr. Nadia Chafai', role: 'Professeur', avatar: 'NC', email: 'prof@supmti.ma', password: '123456' },
  'student@supmti.ma': { fullName: 'Étudiant SUPMTI', role: 'Etudiant', avatar: 'ES', email: 'student@supmti.ma', password: '123456' }
};

export async function login({ email, password }) {
  const user = USERS[email?.toLowerCase?.() || ''];
  if (!user || user.password !== password) {
    throw new Error('Email ou mot de passe incorrect.');
  }
  const auth = { token: 'demo-token', user: { fullName: user.fullName, role: user.role, avatar: user.avatar, email: user.email } };
  localStorage.setItem(AUTH_KEY, JSON.stringify(auth));
  return auth;
}

export function getCurrentAuth() {
  try {
    return JSON.parse(localStorage.getItem(AUTH_KEY) || 'null');
  } catch {
    return null;
  }
}

export function isAuthenticated() {
  return !!getCurrentAuth()?.token;
}

export function logout() {
  localStorage.removeItem(AUTH_KEY);
}

export function getHomePath(role) {
  if (role === 'Administration') return '/admin/dashboard';
  if (role === 'Professeur') return '/professor/dashboard';
  return '/student/dashboard';
}
