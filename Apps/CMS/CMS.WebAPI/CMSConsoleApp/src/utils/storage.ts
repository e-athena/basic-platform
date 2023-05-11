export function getItem(key: string): string {
  return localStorage.getItem(key) || '';
}

export function setItem(key: string, value: string) {
  return localStorage.setItem(key, value);
}

export function removeItem(key: string) {
  return localStorage.removeItem(key);
}

export function getSessionItem(key: string): string {
  return sessionStorage.getItem(key) || '';
}

export function setSessionItem(key: string, value: string) {
  return sessionStorage.setItem(key, value);
}

export function removeSessionItem(key: string) {
  return sessionStorage.removeItem(key);
}

export default {
  getItem,
  setItem,
  removeItem,
  getSessionItem,
  setSessionItem,
  removeSessionItem,
};
