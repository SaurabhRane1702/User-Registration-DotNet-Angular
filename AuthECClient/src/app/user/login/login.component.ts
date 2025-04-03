import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../shared/services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, CommonModule, RouterLink],
  templateUrl: './login.component.html',
  styles: ``,
})
export class LoginComponent implements OnInit {
  isSubmitted: Boolean = false;
  constructor(
    public formBuilder: FormBuilder,
    private service: AuthService,
    private router: Router,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    if (this.service.isLoggedIn()) {
      this.router.navigateByUrl('/dashboard');
    }
  }

  form = this.formBuilder.group({
    email: ['', Validators.required],
    password: ['', Validators.required],
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
      this.service.signIn(this.form.value).subscribe({
        next: (res: any) => {
          // localStorage.setItem('token', res.token);
          this.service.saveToken(res.token);
          this.router.navigateByUrl('/dashboard');
        },
        error: (err) => {
          if (err.status == 400) {
            this.toastr.error('Incorrect email or Password.', 'Login Failed');
          } else {
            console.log('Error during login: \n', err);
          }
        },
      });
    }
  }
}
