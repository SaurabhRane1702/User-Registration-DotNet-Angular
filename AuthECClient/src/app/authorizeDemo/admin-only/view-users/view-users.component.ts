import { Component, OnInit } from '@angular/core';
import { UserService } from '../../../shared/services/user.service';
import { User } from '../../../models/user.model';
import { ToastrService } from 'ngx-toastr';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { EditUsersComponent } from '../edit-users/edit-users.component';

@Component({
  selector: 'app-view-users',
  imports: [FormsModule,RouterLink,CommonModule,EditUsersComponent],
  templateUrl: './view-users.component.html',
  styles: ``
})
export class ViewUsersComponent implements OnInit {
  users:User[]=[];
  emailToEdit!:string;
  constructor(private userService:UserService, private toastr:ToastrService) {}
  ngOnInit(): void {
    this.fetchUsers();
  }

  editUser(email:string){
    console.log("Edit User clicked for User ID:", email);
    this.emailToEdit = email;
    console.log("User ID to edit in Parent component:", this.emailToEdit);
  }

  fetchUsers(){
    this.userService.fetchUsers().subscribe({
      next:(res:any)=>{
        this.toastr.success('Users fetched successfully', 'Success');
        console.log("Users fetched successfully",res);
        this.users = res;  
      },
      error:(err:any)=>{
        console.log("Error while fetching users",err);
      }
    });
  }
}
