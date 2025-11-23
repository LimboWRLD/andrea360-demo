export interface User {
    id: string;
    firstName: string;
    lastName: string;
    email: string;
    createdAtTimestamp: number;
    keycloakId: string,
    enabled: boolean | null;
    locationId: string;
    realmRoles: string[];
}
