import { Component } from '@angular/core';
import {AuthCheckerService} from "../_models/classes/services/checker-services/auth-checker.service";

/**
 * Represents the NavbarComponent class that is responsible for rendering the application's navbar.
 */
@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent {
  readonly authCheckerService: AuthCheckerService;

  /**
   * Constructs a new instance of the NavbarComponent class.
   * @param authCheckerService The AuthCheckerService used for authentication checking.
   */
  constructor(authCheckerService: AuthCheckerService) {
    this.authCheckerService = authCheckerService;
  }

  /**
   * Removes the token and its expiration date from the local storage.
   */
  public removeItemsFromLocalStore()
  {
    localStorage.removeItem('ncore-token');
    localStorage.removeItem('ncore-token-expiration');
    console.log("Token and its expiration date removed from local storage.")
  }
}
