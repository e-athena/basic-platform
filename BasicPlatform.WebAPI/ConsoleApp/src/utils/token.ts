import Storage from '@/utils/storage';

const TokenKey = APP_TOKEN_KEY || 'basic_platform_token';

export function getToken(): string {
  return Storage.getItem(TokenKey);
}

export function setToken(token: string) {
  return Storage.setItem(TokenKey, token);
}

export function removeToken() {
  return Storage.removeItem(TokenKey);
}
