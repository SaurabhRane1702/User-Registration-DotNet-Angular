import { claimReq } from './../shared/utils/claimReq-utils';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../shared/services/auth.service';
import { UserService } from '../shared/services/user.service';
import { HideIfClaimsNotMetDirective } from '../shared/directives/hide-if-claims-not-met.directive';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dashboard',
  imports: [HideIfClaimsNotMetDirective,CommonModule],
  templateUrl: './dashboard.component.html',
  styles: ``,
})
export class DashboardComponent implements OnInit {
  fullName: string = '';

  currentDate = new Date();
  studentName = 'John Doe';
  libraryId = 'LIB12345';


  claimReq = claimReq;
  constructor(
    private router: Router,
    private authService: AuthService,
    private userService: UserService
  ) {}

  ngOnInit(): void {
    this.userService.getUserProfile().subscribe({
      next: (res: any) => {
        this.fullName = res.fullName;
      },
      error: (err: any) => {
        console.log('error while retrieving user profile');
      },
    });
  }

  lectures = [
    {
      subject: 'Mathematics',
      teacher: 'Mr. John Doe',
      time: '9:00 AM - 10:30 AM',
      duration: '1 hour 30 minutes',
      color: '#FFCDD2'
    },
    {
      subject: 'Physics',
      teacher: 'Ms. Jane Smith',
      time: '10:45 AM - 12:15 PM',
      duration: '1 hour 30 minutes',
      color: '#C8E6C9'
    },
    {
      subject: 'Chemistry',
      teacher: 'Dr. Emily Clark',
      time: '1:00 PM - 2:30 PM',
      duration: '1 hour 30 minutes',
      color: '#BBDEFB'
    }
  ];
}
