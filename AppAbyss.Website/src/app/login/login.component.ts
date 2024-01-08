import { Component } from '@angular/core';
import { ErrorStateMatcher } from "@angular/material/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { MatSnackBar } from "@angular/material/snack-bar";
import { MyErrorStateMatcher } from "../_models/classes/form-helpers/my-error-state-matcher";
import { ILoginModel } from "../_models/interfaces/Dtos/login-model.interface";
import { ErrorTextChecker } from "../_models/classes/form-helpers/error-text-checker";
import { FormSubmitValidate } from "../_models/classes/form-helpers/form-submit-validate";
import { AuthApiService } from "../_models/classes/services/api-services/auth-api.service";

/**
 * Represents the LoginComponent of the application.
 */
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  /**
   * Represents the error state matcher for form validation.
   */
  public matcher: ErrorStateMatcher;

  /**
   * Represents the error text checker for form validation.
   */
  public readonly errorTextChecker: ErrorTextChecker;

  /**
   * Represents the form submit validator for form validation.
   */
  public readonly formSubmitValidate: FormSubmitValidate;

  /**
   * Represents the login form group.
   */
  public loginFormGroup: FormGroup;

  /**
   * Represents the router service.
   */
  private readonly router: Router;

  /**
   * Represents the snackbar service.
   */
  private readonly snackBar: MatSnackBar;

  /**
   * Represents the form builder service.
   */
  private readonly formBuilder: FormBuilder;

  /**
   * Represents the authentication API service.
   */
  private readonly authApiService: AuthApiService;

  /**
   * Initializes a new instance of the LoginComponent class.
   * @param authApiService The authentication API service.
   * @param router The router service.
   * @param snackBar The snackbar service.
   * @param formBuilder The form builder service.
   */
  constructor(authApiService: AuthApiService, router: Router, snackBar: MatSnackBar, formBuilder: FormBuilder) {
    this.matcher = new MyErrorStateMatcher();
    this.authApiService = authApiService;
    this.router = router;
    this.snackBar = snackBar;
    this.formBuilder = formBuilder;
    this.loginFormGroup = this.formBuilder.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      password: ['', [Validators.required, Validators.minLength(8)]]
    });
    this.errorTextChecker = new ErrorTextChecker();
    this.formSubmitValidate = new FormSubmitValidate();
  }

  /**
   * Submits the login form.
   */
  SubmitLogin() {
    const loginModel: ILoginModel = {
      username: this.loginFormGroup.get('username')?.value,
      password: this.loginFormGroup.get('password')?.value
    };

    this.authApiService.login(loginModel)
      .subscribe({
        next: (success) => {
          localStorage.setItem('ncore-token', success.token);
          localStorage.setItem('ncore-token-expiration', success.expiration.toString());
          console.log(success);
          this.router.navigate(['/home']);
        },
        error: (error) => {
          console.log(error);
          this.snackBar.open("An error happened, please try again.", "Close", { duration: 5000 });
        }
      });
  }
}
