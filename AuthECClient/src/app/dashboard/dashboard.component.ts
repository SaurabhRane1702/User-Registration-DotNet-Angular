import { claimReq } from './../shared/utils/claimReq-utils';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../shared/services/auth.service';
import { UserService } from '../shared/services/user.service';
import { HideIfClaimsNotMetDirective } from '../shared/directives/hide-if-claims-not-met.directive';

@Component({
  selector: 'app-dashboard',
  imports: [HideIfClaimsNotMetDirective],
  templateUrl: './dashboard.component.html',
  styles: ``,
})
export class DashboardComponent implements OnInit {
  fullName: string = '';
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
}
