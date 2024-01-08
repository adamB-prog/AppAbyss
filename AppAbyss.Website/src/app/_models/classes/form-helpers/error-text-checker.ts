import {AbstractControl, FormGroup} from "@angular/forms";

/**
 * Helper class for checking form errors and validation.
 */
export class ErrorTextChecker{
  /**
   * Checks if a form field in a FormGroup is empty.
   * @param formGroup - The FormGroup to check.
   * @param formFieldName - The name of the form field.
   * @returns True if the form field is empty, false otherwise.
   */
  isFormGroupFieldEmpty(formGroup:FormGroup, formFieldName:string):boolean {

    const control = formGroup.get(formFieldName);
    const result = control ? control.hasError('required') : null;
    return result !== null ? result : true;
  }

  /**
   * Checks if a form field in a FormGroup is valid.
   * @param formGroup - The FormGroup to check.
   * @param formFieldName - The name of the form field.
   * @param validationError - The validation error to check.
   * @returns True if the form field is invalid, false otherwise.
   */
  isFormGroupFieldValid(formGroup: FormGroup, formFieldName: string, validationError: string): boolean {
    const control = formGroup.get(formFieldName);
    return this.isFormControlInvalid(control, validationError);
  }

  /**
   * Checks if a form control is invalid.
   * @param formControl - The form control to check.
   * @param validationError - The validation error to check.
   * @returns True if the form control is invalid, false otherwise.
   */
  isFormControlInvalid(formControl: AbstractControl<any, any> | null, validationError: string): boolean {
    let result: boolean;

    if (!formControl) {
      result = false;
    } else {
      result = formControl.value && formControl.hasError(validationError);
    }
    return result;
  }

  /**
   * Checks if a form control is empty.
   * @param formControl - The form control to check.
   * @returns True if the form control is empty, false otherwise.
   */
  isFormControlEmpty(formControl: AbstractControl<any, any> | null): boolean {
    const result = formControl ? formControl.hasError('required') : null;
    return result !== null ? result : true;
  }
}
