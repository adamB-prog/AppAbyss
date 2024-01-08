import {ErrorStateMatcher} from "@angular/material/core";
import {FormControl, FormGroupDirective, NgForm} from "@angular/forms";

/**
 * Custom error state matcher for Angular Material form controls.
 * Implements the ErrorStateMatcher interface.
 */
export class MyErrorStateMatcher implements ErrorStateMatcher {
  /**
   * Determines whether the form control is in an error state.
   * @param control - The form control to check.
   * @param form - The form group directive or NgForm instance.
   * @returns A boolean indicating whether the control is in an error state.
   */
  isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null): boolean {
    const isSubmitted = form && form.submitted;
    return !!(control && control.invalid && (control.dirty || control.touched || isSubmitted));
  }
}
