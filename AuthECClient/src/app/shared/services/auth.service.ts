import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TOKEN_KEY } from '../constants';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(private http: HttpClient) {}
  baseUrl = environment.apiBaseUrl;

  createUser(formData: any) {
    //---WARNING! TODO
    //default value for Role, Gender, Age, LibraryID?
    //instead of regsitration Form, there shoulw some other form that needs to implementated
    //to update these details of the User
    //--TODO End
    formData.role = 'Teacher';
    formData.gender = 'Female';
    formData.age = 35;
    return this.http.post(this.baseUrl + '/signup', formData);
  }

  signIn(formData: any) {
    return this.http.post(this.baseUrl + '/signin', formData);
  }

  getToken() {
    return localStorage.getItem(TOKEN_KEY);
  }

  isLoggedIn() {
    return this.getToken() != null ? true : false;
  }

  saveToken(token: string) {
    localStorage.setItem(TOKEN_KEY, token);
  }

  deleteToken() {
    localStorage.removeItem(TOKEN_KEY);
  }

  getClaims() {
    return JSON.parse(window.atob(this.getToken()!.split('.')[1]));
  }
}
