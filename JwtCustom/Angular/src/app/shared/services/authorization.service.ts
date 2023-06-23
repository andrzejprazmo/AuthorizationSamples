import { Injectable } from '@angular/core';
import { CurrentUser } from '../models/authorization.models';

@Injectable({
  providedIn: 'root'
})
export class AuthorizationService {

  get storageKey(): string {
    return 'user-token';
  }

  isAuthenticated(): boolean {
    const currentUser = sessionStorage.getItem(this.storageKey);
    return currentUser != null;
  }

  signIn(user: CurrentUser) {
    sessionStorage.setItem(this.storageKey, JSON.stringify(user));
  }

  signOut() {
    sessionStorage.removeItem(this.storageKey);
  }

  isInRoles(roles: string[]): boolean {
    const currentUserString = sessionStorage.getItem(this.storageKey);
    if (currentUserString) {
      const currentUser = JSON.parse(currentUserString) as CurrentUser;
      if (currentUser && currentUser.roles.some(r => roles.includes(r))) {
        return true;
      }
    }
    return false;
  }
}
