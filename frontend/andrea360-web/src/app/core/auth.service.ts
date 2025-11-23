import { Injectable, inject } from '@angular/core';
import Keycloak from 'keycloak-js';
import { Observable, BehaviorSubject } from 'rxjs';
import { DynamicService } from './dynamic.service';
import { User } from './models/user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private keycloak = inject(Keycloak);
  private currentUser$ = new BehaviorSubject<User | null>(null);

  constructor(private dynamicService: DynamicService) {
    this.loadCurrentUser();    
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

  console.log("[AuthService] Keycloak ID from token:", keycloakId);

  if (!keycloakId) {
    console.warn("[AuthService] No Keycloak user ID found in token.");
    return;
  }

  this.dynamicService.getAll<User[]>('users').subscribe({
    next: (users) => {
      console.log("[AuthService] Loaded users from backend:", users);

      const matched = users.find(u => u.keycloakId == keycloakId);

      if (!matched) {
        console.warn("[AuthService] No user in local DB matches Keycloak ID:", keycloakId);
      } else {
        console.log("[AuthService] Matched local user:", matched);
      }

      this.currentUser$.next(matched ?? null);
    },
    error: (err) => {
      console.error("[AuthService] Failed to load current user:", err);
    }
  });
}


  refreshCurrentUser(): void {
    this.loadCurrentUser();
  }
}
