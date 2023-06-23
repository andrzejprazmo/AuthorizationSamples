export interface CurrentUser {
  firstName: string,
  lastName: string,
  emailAddress: string,
  roles: string[]
}

export interface TokenModel {
  token: string,
}
