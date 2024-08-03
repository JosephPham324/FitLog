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
  return decodedToken ? decodedToken.role || '' : '';
}
