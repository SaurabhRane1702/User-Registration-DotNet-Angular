import { CommonModule } from '@angular/common';
import { Component, NgModule, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, Validators } from '@angular/forms';
import { Book } from '../../../models/book.model';
import { UserService } from '../../../shared/services/user.service';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-add-book',
  imports: [FormsModule,CommonModule],
  templateUrl: './add-book.component.html',
  styles: ``
})
export class AddBookComponent implements OnInit {
book: Book ={
  id: 0,
  bookTitle: '',
  genre: '',
  isBorrowed: false,
  borrowedByEmail: ''
};
books:Book[]=[];
emails:string[]=[];

constructor(private userService:UserService, private toastr:ToastrService){}

ngOnInit(): void {
  this.getEmails();
  this.getBookDetails();
}

getEmails(){
  //call user service to get email ids of all users
  this.userService.fetchUsersEmailId().subscribe({
    next:(res:any)=>{
      console.log("Emails fetched successfully",res);
      this.emails = res;
    },
    error:(err:any)=>{
      console.log("Error while fetching emails",err);
    }
  })
}
getBookDetails(){
  this.userService.fetchBooks().subscribe({
    next: (res: any) => {
      this.books = res;
      console.table("raw",res);
    },
    error:(err:any)=>{
      console.log(err.message);
      this.toastr.error('Fetch Books Failed', 'Some Error occured while fetching books');
    }
  });
}
onSubmit(bookForm: any){
  
  if(bookForm.valid){
    if(this.book.borrowedByEmail !== ''){
      this.book.isBorrowed = true;

    }
    console.log("book",this.book,"Form value",bookForm.value);
    this.userService.addBook(this.book).subscribe({
      next:(res:any)=>{
        this.toastr.success("Book added successfully","Success");
        bookForm.resetForm();
        this.getBookDetails();
      },
      error:(err:any)=>{
        this.toastr.error("Error while adding book","Error");
      }
    });
  }
  else{
    this.toastr.error("Please fill all required fields","Invalid Form");
  }
 }
}
