const PRODUCTION_GATEWAY_URL = 'https://yarp-gateway.livelystone-e93757da.brazilsouth.azurecontainerapps.io';
const LOCAL_GATEWAY_URL = 'https://localhost:6064';

export const environment = {
  production: true,
  apiBaseUrl: PRODUCTION_GATEWAY_URL,
  gatewayUrls: {
    production: PRODUCTION_GATEWAY_URL,
    local: LOCAL_GATEWAY_URL
  }
};
