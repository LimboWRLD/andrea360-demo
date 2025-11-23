import { Service } from './service.model';

export interface UserService {
  id: string;
  userId: string;
  serviceId: string;
  service: Service;
  remainingSessions: number;
}
