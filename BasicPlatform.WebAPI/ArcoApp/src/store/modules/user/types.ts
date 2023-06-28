export type RoleType = '' | '*' | 'admin' | 'user';
export interface UserState {
  userName?: string;
  realName?: string;
  avatar?: string;
  userId?: string;
  email?: string;
  signature?: string;
  title?: string;
  group?: string;
  tags?: { key?: string; label?: string }[];
  notifyCount?: number;
  unreadCount?: number;
  country?: string;
  geographic?: {
    province?: { label?: string; key?: string };
    city?: { label?: string; key?: string };
  };
  address?: string;
  phoneNumber?: string;
}
