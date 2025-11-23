import { Injectable, inject } from '@angular/core';
import Keycloak from 'keycloak-js';
import { Observable, BehaviorSubject } from 'rxjs';
import { DynamicService } from './dynamic.service';
import { User } from './models/user.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private keycloak = inject(Keycloak);
  private currentUser$ = new BehaviorSubject<User | null>(null);

  constructor(private dynamicService: DynamicService) {
    this.loadCurrentUser();
  }

  hasRole(role: string): boolean {
    const realmTokenRoles: string[] = this.keycloak?.tokenParsed?.['realm_access']?.['roles'] || [];
    const resourceAccess = this.keycloak?.tokenParsed?.['resource_access'] || {};
    const clientRoles: string[] = Object.values(resourceAccess).flatMap((r: any) => r?.roles || []);
    const userRoles: string[] = this.currentUser$.value?.realmRoles || [];

    const all = new Set<string>([...realmTokenRoles, ...clientRoles, ...userRoles]);
    const has = all.has(role);

    return has;
  }

  getKeycloakUserId(): string | undefined {
    return this.keycloak?.tokenParsed?.['sub'];
  }

  getUserId(): string | null {
    return this.currentUser$.value?.id ?? null;
  }

  getLocationId(): string | null {
    return this.currentUser$.value?.locationId ?? null;
  }

  getUserEmail(): string | undefined {
    return this.keycloak?.tokenParsed?.['email'];
  }

  getCurrentUser(): Observable<User | null> {
    return this.currentUser$.asObservable();
  }

  getCurrentUserValue(): User | null {
    return this.currentUser$.value;
  }

  private loadCurrentUser(): void {
    const keycloakId = this.getKeycloakUserId();

    if (!keycloakId) {
      return;
    }

    this.dynamicService.getAll<User[]>('users').subscribe({
      next: (users) => {
        const matched = users.find((u) => u.keycloakId == keycloakId);
        this.currentUser$.next(matched ?? null);
      },
      error: () => {
        this.currentUser$.next(null);
      },
    });
  }

  refreshCurrentUser(): void {
    this.loadCurrentUser();
  }
}
