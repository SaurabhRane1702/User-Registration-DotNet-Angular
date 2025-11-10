import { Component, OnInit } from '@angular/core';
import { UserService } from '../../../shared/services/user.service';
import { CommonModule, NgForOfContext } from '@angular/common';
import { Book } from '../../../models/book.model';
import { FormsModule } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-submit-books',
  imports: [CommonModule,FormsModule,RouterLink],
  templateUrl: './submit-books.component.html',
  styles: ``
})
export class SubmitBooksComponent implements OnInit {

  borrowedBooks: Book[] = [];
  constructor(private userService:UserService, private toastr:ToastrService) { }

  ngOnInit(): void {
    this.getBorrowedBooks();
  }

  getBorrowedBooks(): void{
    this.userService.fetchBorrowedBooks().subscribe({
      next:(res:any)=>{
        console.log("Borrowed Books fetched successfully",res);
        this.borrowedBooks = res;
        console.log("borrowedBooks array:", this.borrowedBooks);
      },
      error:(err:any)=>{
        console.log("Error while fetching borrowed books",err);
      }
    });
  }

  submitBook(bookId:number): void{
    console.log("Submitting book with ID:", bookId);
    this.userService.submitBorrowedBook(bookId).subscribe({
      next:(res:any)=>{
        console.log("Book submitted successfully",res);
        this.toastr.success('Book Submitted Successfully', 'Success');
        this.getBorrowedBooks(); // Refresh the list after submission
      },
      error:(err:any)=>{
        console.log("Error while submitting book",err);
      }
    });
  }
}
