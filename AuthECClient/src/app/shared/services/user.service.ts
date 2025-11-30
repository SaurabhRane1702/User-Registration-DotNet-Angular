import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { AuthService } from './auth.service';
import { BehaviorSubject } from 'rxjs';
import { Book } from '../../models/book.model';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  constructor(private http: HttpClient, private authService: AuthService) {}
  private formData = new BehaviorSubject<any>({});
  baseUrl = environment.apiBaseUrl;

  getUserProfile() {
    //This is added to HttpInterceptors
    // const reqHeader = new HttpHeaders({
    //   Authorization: 'Bearer ' + this.authService.getToken(),
    // });
    //One more way of writing
    // let reqHeader = new HttpHeaders();
    // reqHeader = reqHeader.set(
    //   'Authorization',
    //   'Bearer ' + this.authService.getToken()
    // );
    //We use this when we indivually pass the header, now we are going to use HttpInterceptor
    //return this.http.get(this.baseUrl + '/userprofile', { headers: reqHeader });
    return this.http.get(this.baseUrl + '/userprofile');
  }

  fetchTimeTable() {
    return this.http.get(this.baseUrl + '/fetchtimetable');
  }

  fetchBooks() {
    return this.http.get(this.baseUrl + '/fetchbooks');
  }

  // get data() {
  //   return this.formData.asObservable();
  // }

  getData(){
    return this.formData.value;
  }

  updateData(newData:any){
    this.formData.next({...this.formData.value, ...newData});
  }

  // setFormData(Step: string, data:any){
  //   this.for
  // }

  clearData(){
    this.formData.next({});
  }

  addTimeTable(formData : any){
    console.log('FormData before calling service',formData.value);
    const httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };

    return this.http.post(this.baseUrl + '/addtimetable', formData, httpOptions);
  }

  fetchUsers(){
    console.log("In fetchUsers of UserService");
    return this.http.get(this.baseUrl + '/fetchallusers');
  }

  fetchUsersEmailId(){
    console.log("In fetchUsersEmailId of UserService");
    return this.http.get(this.baseUrl + '/fetchallemail');
  }

  fetchUserDetailsOnEmail(email:string){
    console.log("In fetchUserDetails of UserService for email:", email);
    const httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };
    return this.http.get(this.baseUrl + '/fetchuseronemail' + '?email=' + email, httpOptions);
  }

  addBook(bookData : any){
    console.log('Book Data before calling service',bookData);
    const httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };
    return this.http.post(this.baseUrl + '/addbooks', bookData, httpOptions);
  }

  borrowBook(bookId: number){
    console.log("In borrowBook of UserService for bookId:", bookId);
    const httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };
    // return this.http.post(this.baseUrl + '/borrowbook',null, 
    //   {
    //     ...httpOptions, 
    //     params: new HttpParams().set('bookId', bookId.toString())
    //   });
    return this.http.post(this.baseUrl + '/borrowbook?bookId='+bookId, null, httpOptions);
  }
  
  fetchBorrowedBooks() {
    console.log("In fetchBorrowedBooks of UserService");
    return this.http.get(this.baseUrl + '/fetchborrowedbooks');
  }

  submitBorrowedBook(bookId: number){
    console.log("In submitBorrowedBook of UserService for bookId:", bookId);
    const httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };
    return this.http.post(this.baseUrl + '/submitbooks?bookId='+bookId, null, httpOptions);
  }

  updateUserDetails(userData : any){
    console.log('User Data before calling updateUserDetails service',userData);
    const httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };
    return this.http.patch(this.baseUrl + '/updateuserdetails', userData, httpOptions);
  }
}
