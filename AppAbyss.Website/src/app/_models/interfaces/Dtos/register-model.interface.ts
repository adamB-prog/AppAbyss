/**
 * Represents the interface for registering a user.
 */
export interface IRegisterModel {
  /**
   * The email of the user.
   */
  readonly email: string;
  /**
   * The username of the user.
   */
  readonly username: string;
  /**
   * The password of the user.
   */
  readonly password: string;
}
