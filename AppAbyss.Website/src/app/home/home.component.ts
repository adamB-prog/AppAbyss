import { Component } from '@angular/core';
import {AuthCheckerService} from "../_models/classes/services/checker-services/auth-checker.service";


/**
 * Represents the HomeComponent class that is responsible for rendering the application's home page.
 */
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {
  public readonly authCheckerService: AuthCheckerService;

  /**
   * Constructs a new instance of the HomeComponent class.
   * @param authCheckerService The AuthCheckerService used for authentication checking.
   */
  constructor(authCheckerService: AuthCheckerService) {
    this.authCheckerService = authCheckerService;
  }
}
