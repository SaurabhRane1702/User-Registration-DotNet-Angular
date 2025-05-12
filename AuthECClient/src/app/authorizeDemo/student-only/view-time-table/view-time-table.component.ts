import { Component, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { UserService } from '../../../shared/services/user.service';
import { AuthService } from '../../../shared/services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';
import { TimeTable } from '../../../models/timeTable.model';

@Component({
  selector: 'app-view-time-table',
  imports: [CommonModule,RouterLink],
  templateUrl: './view-time-table.component.html',
  styles: ``
})
export class ViewTimeTableComponent implements OnInit {

  // displayTimeTable: boolean = false;
  timeTable: TimeTable[] = [];

  constructor(
      private router: Router,
      private authService: AuthService,
      private userService: UserService,
      private toastr: ToastrService
    ) {}
  ngOnInit(): void {
    this.userService.fetchTimeTable().subscribe({
      next: (res: any) => {
        this.timeTable = res;
        console.table(res);
        console.table(res.status);
        console.table(this.timeTable);
      },
      error:(err:any)=>{
        console.log(err.message);
        this.toastr.error('Fetch Time Table Failed', 'Some Error occured while fetching time table');
      }
    })
    
  }
}
