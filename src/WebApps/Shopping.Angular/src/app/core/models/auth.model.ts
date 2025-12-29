export interface AuthenticateUserRequest {
  email: string;
  password: string;
}

export interface AuthenticateUserResponse {
  token: string;
  name: string;
  userId: string;
}

export interface RegisterUserRequest {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
}

export interface RegisterUserResponse {
  id: string;
  email: string;
}

export interface AuthenticatedUser extends AuthenticateUserResponse {}
