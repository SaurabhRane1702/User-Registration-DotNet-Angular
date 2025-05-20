import { Component, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../shared/services/auth.service';
import { UserService } from '../../../shared/services/user.service';
import { ToastrService } from 'ngx-toastr';
import { Book } from '../../../models/book.model';
import { CommonModule } from '@angular/common';
import { HideIfClaimsNotMetDirective } from '../../../shared/directives/hide-if-claims-not-met.directive';

@Component({
  selector: 'app-view-books',
  imports: [CommonModule,RouterLink,HideIfClaimsNotMetDirective],
  templateUrl: './view-books.component.html',
  styles: ``
})
export class ViewBooksComponent implements OnInit {
book: Book[] = [];


  constructor(
      private router: Router,
      private userService: UserService,
      private toastr: ToastrService
    ) {}
  ngOnInit(): void {
    this.userService.fetchBooks().subscribe({
      next: (res: any) => {
        this.book = res;
        console.table("raw",res);
        console.table("status", res.status);
        console.table(this.book);
      },
      error:(err:any)=>{
        console.log(err.message);
        this.toastr.error('Fetch Time Table Failed', 'Some Error occured while fetching time table');
      }
    })
    
  }
}
