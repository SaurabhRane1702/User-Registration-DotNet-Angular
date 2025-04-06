import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-forgot-password',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './forgot-password.component.html',
  styles: ``,
})
export class ForgotPasswordComponent {
  isSubmitted: boolean = false;

  constructor(public formBuilder: FormBuilder) {}
  form = this.formBuilder.group({
    email: ['', Validators.required],
    newPassword: ['', Validators.required],
  });

  hasDisplayableError(controlName: string): Boolean {
    const control = this.form.get(controlName);
    return (
      Boolean(control?.invalid) &&
      (this.isSubmitted || Boolean(control?.touched) || Boolean(control?.dirty))
    );
  }

  onSubmit() {
    this.isSubmitted = true;
    console.table(this.form.value);
  }
}
