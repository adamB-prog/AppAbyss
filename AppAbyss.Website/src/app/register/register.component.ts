import {Component} from '@angular/core';
import {ErrorStateMatcher} from '@angular/material/core';
import {IRegisterModel} from "../_models/interfaces/Dtos/register-model.interface";
import {MyErrorStateMatcher} from "../_models/classes/form-helpers/my-error-state-matcher";
import {Router} from "@angular/router";
import {MatSnackBar} from "@angular/material/snack-bar";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {ErrorTextChecker} from "../_models/classes/form-helpers/error-text-checker";
import {FormSubmitValidate} from "../_models/classes/form-helpers/form-submit-validate";
import {AuthApiService} from "../_models/classes/services/api-services/auth-api.service";


/**
 * Represents the Register Component.
 * This component is responsible for handling user registration.
 */
@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent{

  /**
   * Represents the error state matcher for form validation.
   */
  public matcher: ErrorStateMatcher

  /**
   * Represents the register form group.
   */
  public registerFormGroup: FormGroup

  /**
   * Represents the form submit validation helper.
   */
  public readonly formSubmitValidate: FormSubmitValidate

  /**
   * Represents the error text checker helper.
   */
  public readonly errorTextChecker : ErrorTextChecker

  /**
   * Represents the authentication API service.
   */
  private readonly authApiService: AuthApiService

  /**
   * Represents the router service.
   */
  private readonly router: Router

  /**
   * Represents the snackbar service.
   */
  private readonly snackBar: MatSnackBar

  /**
   * Represents the form builder service.
   */
  private readonly formBuilder: FormBuilder

  /**
   * Constructs a new instance of the RegisterComponent.
   * @param authApiService The authentication API service.
   * @param router The router service.
   * @param snackBar The snackbar service.
   * @param formBuilder The form builder service.
   */
  constructor(authApiService: AuthApiService,router:Router, snackBar:MatSnackBar, formBuilder: FormBuilder) {
    this.matcher = new MyErrorStateMatcher()
    this.authApiService = authApiService;
    this.router = router
    this.snackBar = snackBar
    this.formBuilder = formBuilder
    this.registerFormGroup = this.formBuilder.group({
      username : ['',[Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]]
    })
    this.errorTextChecker = new ErrorTextChecker()
    this.formSubmitValidate = new FormSubmitValidate()
  }

  /**
   * Submits the user registration form.
   */
  SubmitRegister() {

    const registerModel: IRegisterModel = {
      email: this.registerFormGroup.get('email')?.value,
      username: this.registerFormGroup.get('username')?.value,
      password: this.registerFormGroup.get('password')?.value
    };

    console.log(registerModel)

    this.authApiService.register(registerModel)
      .subscribe({
        next: (success) => {
          this.snackBar.open("Registration was successful!", "Close", {duration: 5000})
            .afterDismissed()
            .subscribe(() => {
              this.router.navigate(['/login'])
            })
        },
        error: (error) => {
          console.log(error)
          this.snackBar.open("An error happened, please try again.", "Close", {duration: 5000})
        }
      });
  }
}
