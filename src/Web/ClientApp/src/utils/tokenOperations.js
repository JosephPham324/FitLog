import { jwtDecode } from "jwt-decode";
import { getCookie } from './cookiesOperations';

export function getDecodedToken() {
  const jwtHeaderPayload = getCookie('jwtHeaderPayload');
  const jwtSignature = getCookie('jwtSignature');

  if (jwtHeaderPayload && jwtSignature) {
    const token = `${jwtHeaderPayload}.${jwtSignature}`;
    try {
      return jwtDecode(token);
    } catch (error) {
      console.error('Invalid token', error);
      return null;
    }
  }
  return null;
}

export function getUserRole() {
  const decodedToken = getDecodedToken();
  //console.log(decodedToken);
  if (decodedToken) {
    // Check for both possible role claim types
    return decodedToken.role || decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || '';
  }

  return '';
}


export function getUserId() {
  const decodedToken = getDecodedToken();
  return decodedToken ? decodedToken.Id || '' : '';
}