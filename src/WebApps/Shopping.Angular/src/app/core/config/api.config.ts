import { environment } from '../../environments/environment';

export const API_BASE_URL = environment.production
  ? environment.gatewayUrls.production
  : environment.gatewayUrls.local;
