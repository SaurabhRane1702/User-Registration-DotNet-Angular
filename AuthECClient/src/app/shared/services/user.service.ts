import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { AuthService } from './auth.service';
import { BehaviorSubject } from 'rxjs';

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

  fetchUsersEmailId(){
    console.log("In fetchUsersEmailId of UserService");
    return this.http.get(this.baseUrl + '/fetchallemail');
  }

  addBook(bookData : any){
    console.log('Book Data before calling service',bookData);
    const httpOptions = {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    };
    return this.http.post(this.baseUrl + '/addbooks', bookData, httpOptions);
  }
  
}
