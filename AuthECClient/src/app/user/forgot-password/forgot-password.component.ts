import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../shared/services/auth.service';
import { Router, RouterLink } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-forgot-password',
  imports: [ReactiveFormsModule, CommonModule, RouterLink],
  templateUrl: './forgot-password.component.html',
  styles: ``,
})
export class ForgotPasswordComponent {
  isSubmitted: boolean = false;

  constructor(
    public formBuilder: FormBuilder,
    private service: AuthService,
    private router: Router,
    private toastr: ToastrService
  ) {}
  form = this.formBuilder.group({
    email: ['', Validators.required],
    oldPassword: ['', Validators.required],
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
    if (this.form.valid) {
      this.service.forgotPassword(this.form.value).subscribe({
        next: (res: any) => {
          //this.service.saveToken(res.token);
          this.router.navigateByUrl('/signin');
          this.toastr.success(
            'Password changed Successfully',
            'New Password Set'
          );
        },
        error: (err) => {
          console.log('error occured', err);
          this.toastr.error(
            'Incoreect email or oldPassword',
            'Password Reset Failed'
          );
          // if (err.status == 400) {
          //   this.toastr.error('Incorrect email or Password.', 'Login Failed');
          // } else {
          //   console.log('Error during login: \n', err);
          // }
        },
      });
    }
  }
}
