import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { UserService } from '../../../shared/services/user.service';
import { Router, RouterLink } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../shared/services/auth.service';
import { GENDERS, ROLES } from '../../../models/form.model';

@Component({
  selector: 'app-additional-registration-details',
  imports: [ReactiveFormsModule,CommonModule,RouterLink],
  templateUrl: './additional-registration-details.component.html',
  styles: ``
})
export class AdditionalRegistrationDetailsComponent implements OnInit {

  genders = GENDERS;
  roles = ROLES;
constructor(
    public formBuilder: FormBuilder,
    private service: AuthService,
    private userService: UserService,
    private router: Router,
    private toastr: ToastrService
  ) {}
  isSubmitted: Boolean = false;
  ngOnInit(): void {
    console.log(this.userService.getData());
  }
  formAdditional = this.formBuilder.group(
      {
        gender:['',Validators.required],
        age: ['', [Validators.required]],
        role:['',Validators.required],
        librabyId : ['']
      //   password: [
      //     '',
      //     [
      //       Validators.required,
      //       Validators.minLength(6),
      //       Validators.pattern(/(?=. *[^a-zA-Z0-9])/),
      //     ],
      //   ],
      //   confirmPassword: [''],
      // },
      // { validators: this.passWordMatchValidator }
      }
    );
    previous(){
      this.isSubmitted = true;
      this.router.navigateByUrl('/signup')
    }
    onSubmit() {
    this.isSubmitted = true;
    console.log('formAdditional values',this.formAdditional.value);
    if (this.formAdditional.valid) {
      // FormGroup mergedForm = new FormGroup({
      //   form1 : this.userService.getData(),
      //   form2: this.formAdditional
      // })
      const mergedForm = {...this.userService.getData(),...this.formAdditional.value};
      console.table(mergedForm);
      this.service.createUser(mergedForm).subscribe({
        next: (res: any) => {
          if (res.succeeded) {
            this.formAdditional.reset();
            this.isSubmitted = false;
            this.toastr.success('New user Created', 'Registration Successful');
            this.router.navigateByUrl('/signin');
          }
        },
        error: (err) => {
          if (err.error.errors) {
            err.error.errors.forEach((x: any) => {
              switch (x.code) {
                case 'DuplicateUserName':
                  break;
                case 'DuplicateEmail':
                  this.toastr.error(
                    'Email is already taken',
                    'Registration Failed'
                  );
                  break;
                default:
                  this.toastr.error(
                    'Something went wrong',
                    'Registration Failed'
                  );
                  console.log(x);
                  break;
              }
            });
          } else console.log('error', err);
        },
      });
      console.table(mergedForm);
    }
  }

    // onSubmit(){
    //   console.log(this.userService.getData());
    // }

    hasDisplayableError(controlName: string): Boolean {
      const control = this.formAdditional.get(controlName);
      return (
        Boolean(control?.invalid) &&
        (this.isSubmitted || Boolean(control?.touched) || Boolean(control?.dirty))
      );
    }
}
