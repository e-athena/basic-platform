import Storage from '@/utils/storage';

const tokenKey = APP_TOKEN_KEY || 'basic_platform_token';
const sessionCodeKey = APP_SESSION_CODE_KEY || 'basic_platform_session_code';

export function getToken(): string {
  return Storage.getItem(tokenKey);
}

export function setToken(token: string) {
  return Storage.setItem(tokenKey, token);
}

export function removeToken() {
  return Storage.removeItem(tokenKey);
}

export function getSessionCode(): string {
  return Storage.getItem(sessionCodeKey);
}

export function setSessionCode(token: string) {
  return Storage.setItem(sessionCodeKey, token);
}

export function removeSessionCode() {
  return Storage.removeItem(sessionCodeKey);
}
