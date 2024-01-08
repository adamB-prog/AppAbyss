/**
 * Represents a token model for authentication.
 */
export interface ITokenModel {
  /**
   * The expiration date of the token.
   */
  readonly expiration: Date;

  /**
   * The token string.
   */
  readonly token: string;
}
