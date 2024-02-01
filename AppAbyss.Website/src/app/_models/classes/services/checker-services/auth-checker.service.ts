import {Router} from "@angular/router";
import {JwtHelperService} from '@auth0/angular-jwt'
import {Injectable} from "@angular/core";

// make this clas injectable




/**
 * Service for checking authentication status and handling route navigation based on authentication.
 */
@Injectable({
  providedIn: 'root'
})
export class AuthCheckerService{
  readonly router: Router;
  readonly jwtHelper: JwtHelperService;

  /**
   * Constructs an instance of AuthCheckerService.
   * @param router - The Angular router service.
   */
  constructor(router: Router){
    this.router = router;
    this.jwtHelper = new JwtHelperService();
  }

  /**
   * Checks if the user is logged in by verifying the presence of a token and its expiration date.
   * @returns A boolean indicating if the user is logged in.
   */
  public isLoggedIn(): boolean {
    return this.tokenCheck() && this.dateCheck()
  }

  /**
   * Checks if the user is authenticated and can access a route.
   * If the user is not authenticated, it navigates to the login page.
   * @returns A boolean indicating if the user can access the route.
   */
  public canActivate() : boolean {
    if (!this.isLoggedIn()) {
      this.router.navigate(['/login'])
      return false
    }
    return true
  }

  /**
   * Checks if a token is present in the local storage.
   * @returns A boolean indicating if a token is present.
   */
  private tokenCheck():boolean
  {
    let token = localStorage.getItem('ncore-token');
    let expirationDate = localStorage.getItem('ncore-token-expiration');

    return token!== null && expirationDate!== null;
  }

  /**
   * Checks if the token has expired using the JWT helper service.
   * @returns A boolean indicating if the token has expired.
   */
  private dateCheck(): boolean {
    let result:boolean = true;
    let token = localStorage.getItem('ncore-token');
    if (this.jwtHelper.isTokenExpired(token)) {
      result = false;
    }

    return result;
  }
}
