import Storage from '@/utils/storage';

const TokenKey = APP_NAV_THEME_KEY || 'basic_platform_nav_theme';

export function removeNavTheme() {
  return Storage.removeSessionItem(TokenKey);
}

export function getNavTheme(): 'realDark' | 'light' | undefined {
  const value = Storage.getSessionItem(TokenKey);
  if (value !== 'realDark' && value !== 'light') {
    return undefined;
  }
  return value;
}

export function setNavTheme(navTheme?: string) {
  if (navTheme === undefined) {
    removeNavTheme();
    return;
  }
  return Storage.setSessionItem(TokenKey, navTheme);
}