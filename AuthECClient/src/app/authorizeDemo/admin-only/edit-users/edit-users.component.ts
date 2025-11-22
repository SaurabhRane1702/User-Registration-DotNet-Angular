import { Component, Input } from '@angular/core';
import { UserService } from '../../../shared/services/user.service';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';
import { User } from '../../../models/user.model';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-edit-users',
  imports: [CommonModule,FormsModule],
  templateUrl: './edit-users.component.html',
  styles: ``
})
export class EditUsersComponent {

  constructor(private userService:UserService, private toastrService:ToastrService) { }

  @Input() email!:string;
  userDetails:User | null = null;
  ngOnInit(): void {
    //this.loadUserDetails();
    console.log("Edit User ID:", this.email);
    this.fetchUserDetails(this.email);
  }

  fetchUserDetails(email: string): void {
    this.userService.fetchUserDetailsOnEmail(email).subscribe({
      next: (res: any) => {
        this.toastrService.success('User details fetched successfully', 'Success');
        this.userDetails = res;
        console.log("User details fetched successfully", res);
        // Populate your form fields with the fetched user details here
      },
      error: (err: any) => {
        console.log("Error while fetching user details", err);
      } 
    });
  }

  onSubmit(userForm: any){
    if(userForm.valid){
      console.log("Updated User Details to be submitted:", this.userDetails);
    }
  }
}
