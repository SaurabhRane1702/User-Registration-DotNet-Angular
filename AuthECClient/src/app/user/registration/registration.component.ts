import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  ReactiveFormsModule,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { FirstKeyPipe } from '../../shared/pipes/first-key.pipe';
import { AuthService } from '../../shared/services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { Router, RouterLink } from '@angular/router';
import { UserService } from '../../shared/services/user.service';

@Component({
  selector: 'app-registration',
  imports: [ReactiveFormsModule, CommonModule, FirstKeyPipe, RouterLink],
  templateUrl: './registration.component.html',
  styles: ``,
})
export class RegistrationComponent implements OnInit {
  constructor(
    public formBuilder: FormBuilder,
    private service: AuthService,
    private userService: UserService,
    private router: Router,
    private toastr: ToastrService
  ) {}
  isSubmitted: Boolean = false;
  ngOnInit(): void {
    if (this.service.isLoggedIn()) {
      this.router.navigateByUrl('/dashboard');
    }
  }
  //Custom Validation Example in Angular
  passWordMatchValidator: ValidatorFn = (control: AbstractControl): null => {
    const password = control.get('password');
    const confirmPassword = control.get('confirmPassword');

    if (password && confirmPassword && password.value != confirmPassword.value)
      confirmPassword?.setErrors({ passwordMismatch: true });
    else confirmPassword?.setErrors(null);
    return null;
  };

  form = this.formBuilder.group(
    {
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: [
        '',
        [
          Validators.required,
          Validators.minLength(6),
          Validators.pattern(/(?=. *[^a-zA-Z0-9])/),
        ],
      ],
      confirmPassword: [''],
    },
    { validators: this.passWordMatchValidator }
  );

  nextStep() {
    if(this.form.valid){
      this.userService.updateData(this.form.value);
      this.router.navigateByUrl('/additionalRegDetails');
    }
  }
  // onSubmit() {
  //   this.isSubmitted = true;
  //   if (this.form.valid) {
  //     this.service.createUser(this.form.value).subscribe({
  //       next: (res: any) => {
  //         if (res.succeeded) {
  //           this.form.reset();
  //           this.isSubmitted = false;
  //           this.toastr.success('New user Created', 'Registration Successful');
  //         }
  //       },
  //       error: (err) => {
  //         if (err.error.errors) {
  //           err.error.errors.forEach((x: any) => {
  //             switch (x.code) {
  //               case 'DuplicateUserName':
  //                 break;
  //               case 'DuplicateEmail':
  //                 this.toastr.error(
  //                   'Email is already taken',
  //                   'Registration Failed'
  //                 );
  //                 break;
  //               default:
  //                 this.toastr.error(
  //                   'Something went wrong',
  //                   'Registration Failed'
  //                 );
  //                 console.log(x);
  //                 break;
  //             }
  //           });
  //         } else console.log('error', err);
  //       },
  //     });
  //     console.table(this.form.value);
  //   }
  //}

  hasDisplayableError(controlName: string): Boolean {
    const control = this.form.get(controlName);
    return (
      Boolean(control?.invalid) &&
      (this.isSubmitted || Boolean(control?.touched) || Boolean(control?.dirty))
    );
  }
}
