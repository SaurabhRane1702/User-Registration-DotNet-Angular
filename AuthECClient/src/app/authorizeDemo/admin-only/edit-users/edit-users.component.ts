import { Component, Input } from '@angular/core';
import { UserService } from '../../../shared/services/user.service';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-edit-users',
  imports: [CommonModule],
  templateUrl: './edit-users.component.html',
  styles: ``
})
export class EditUsersComponent {

  constructor(private userService:UserService, private toastrService:ToastrService) { }

  @Input() email!:string;

  ngOnInit(): void {
    //this.loadUserDetails();
    console.log("Edit User ID:", this.email);
  }

}
