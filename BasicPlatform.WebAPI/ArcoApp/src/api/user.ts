import axios from 'axios';
import type { RouteRecordNormalized } from 'vue-router';
import { UserState } from '@/store/modules/user/types';
import { post, get } from '@/utils/request';

export interface LoginData {
  username: string;
  password: string;
}

export interface LoginRes {
  status?: string;
  type?: string;
  currentAuthority?: string;
  errorMessage?: string;
  sessionCode?: string;
}
export function login(data: LoginData) {
  return post<LoginData, LoginRes>('/api/account/login', data);
}

export function logout() {
  return axios.post<LoginRes>('/api/user/logout');
}

export function getUserInfo() {
  return get<UserState>('/api/Account/currentUser');
}

export function getMenuList() {
  return axios.post<RouteRecordNormalized[]>('/api/user/menu');
}
