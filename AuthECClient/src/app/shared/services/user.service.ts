import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  constructor(private http: HttpClient, private authService: AuthService) {}

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
}
