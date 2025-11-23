import { ActivatedRouteSnapshot, RouterStateSnapshot, Router, CanActivateFn, UrlTree } from '@angular/router';
import { inject } from '@angular/core';
import { createAuthGuard, AuthGuardData } from 'keycloak-angular';

const isAccessAllowed = async (
  route: ActivatedRouteSnapshot,
  _: RouterStateSnapshot,
  authData: AuthGuardData
): Promise<boolean | UrlTree> => {
  const { authenticated, grantedRoles } = authData;

  const requiredRole: string | undefined = route.data['role'];
  const requiredRoles: string[] | undefined = route.data['roles'];
  if (!requiredRole && (!requiredRoles || requiredRoles.length === 0)) {
    return false;
  }

  const hasRole = (role: string): boolean =>
    Object.values(grantedRoles.resourceRoles).some((roles) => roles.includes(role));

  const allowed = authenticated && (
    (requiredRole && hasRole(requiredRole)) ||
    (requiredRoles && requiredRoles.some(r => hasRole(r)))
  );

  if (allowed) {
    return true;
  }

  const router = inject(Router);
  return router.parseUrl('/forbidden');
};

export const canActivateAuthRole = createAuthGuard<CanActivateFn>(isAccessAllowed);