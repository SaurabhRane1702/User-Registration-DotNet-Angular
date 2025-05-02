import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { UserService } from '../../shared/services/user.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-only',
  imports: [CommonModule,ReactiveFormsModule],
  templateUrl: './admin-only.component.html',
  styles: ``
})
export class AdminOnlyComponent implements OnInit {
  timetableForm = this.fb.group({
    subjectName: ['', [Validators.required, Validators.maxLength(50)]],
    className: ['',[Validators.required, Validators.maxLength(10)]],
    inputEmail: ['',[Validators.required, Validators.email]],
    // dateRange: this.fb.group({
    //   startDate: ['', Validators.required],
    //   endDate: ['', Validators.required]
    // }),
    timeActivity: ['', Validators.required],
    //weekDays: this.fb.array([]),
    weekDayActivity: ['', Validators.required],
    teacherSelection: ['', Validators.required],
    //assignedStaff: [[], Validators.required]
  });
  isSubmitted : boolean = false;

  weekDays = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'];
  timeSlots = ['9:00 AM - 10:00 AM', '10:00 AM - 11:00 AM', '11:00 AM - 12:00 PM'];
  teachers = ['Ram', 'Shyam' , 'Jags'];

  constructor(private fb: FormBuilder,
    private userService: UserService,
    private toastr:ToastrService,
    private router:Router
  ) {}

  ngOnInit(): void {
    }

    hasDisplayableError(controlName: string): Boolean {
      const control = this.timetableForm.get(controlName);
      return (
        Boolean(control?.invalid) &&
        (this.isSubmitted || Boolean(control?.touched) || Boolean(control?.dirty))
      );
    }

    onSave() {
    this.isSubmitted = true;
    console.log('formAdditional values',this.timetableForm.value);
    // if (this.timetableForm.valid) {
    //   this.service.createUser(mergedForm).subscribe({
    //     next: (res: any) => {
    //       if (res.succeeded) {
    //         this.formAdditional.reset();
    //         this.isSubmitted = false;
    //         this.toastr.success('New user Created', 'Registration Successful');
    //         this.router.navigateByUrl('/signin');
    //       }
    //     },
    //     error: (err) => {
    //       if (err.error.errors) {
    //         err.error.errors.forEach((x: any) => {
    //           switch (x.code) {
    //             case 'DuplicateUserName':
    //               break;
    //             case 'DuplicateEmail':
    //               this.toastr.error(
    //                 'Email is already taken',
    //                 'Registration Failed'
    //               );
    //               break;
    //             default:
    //               this.toastr.error(
    //                 'Something went wrong',
    //                 'Registration Failed'
    //               );
    //               console.log(x);
    //               break;
    //           }
    //         });
    //       } else console.log('error', err);
    //     },
    //   });
    //   console.table(mergedForm);

      if (this.timetableForm.valid) {
        console.log('Timetable Data:', this.timetableForm.value);
        this.userService.addTimeTable(this.timetableForm.value).subscribe({
          next: (res:any) => {
            console.log('Response for addtimetable',res);
            this.toastr.success(
              'Timetable added for the user successfully',
              'Timetable added'
            );
            this.router.navigateByUrl('/dashboard')
          },
          error: (err : any) => {
            this.toastr.error(
              'Incoreect email',
              'Add Timetable Failed'
            );
            console.log('error from addtimetable',err);
          }
        });
      } else {
        alert('Please fill in all required fields!');
      }
    }
  
    onReset(): void {
      this.timetableForm.reset();
    }
  
    onCancel(): void {
      console.log('Cancel button clicked');
      alert('Action canceled');
    }
}
