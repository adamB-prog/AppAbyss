
import { FormGroup } from '@angular/forms';

/**
 * Helper class for validating form submission.
 */
export class FormSubmitValidate {

  /**
   * Validates the given form group.
   * @param formGroup The form group to validate.
   * @returns True if the form group is valid, false otherwise.
   */
  validateFormGroup(formGroup: FormGroup): boolean {
    let isValid = true;

    Object.keys(formGroup.controls).forEach(field => {
      const control = formGroup.get(field);

      if (control) {

        if (control.value === null || control.value === '' || control.errors !== null) {
          isValid = false;
        }

      } else {
        isValid = false;
      }
    });

    return isValid;
  }
}
