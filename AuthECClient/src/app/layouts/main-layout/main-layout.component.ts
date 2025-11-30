import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router, RouterLink, RouterOutlet } from '@angular/router';
import { AuthService } from '../../shared/services/auth.service';
import { HideIfClaimsNotMetDirective } from '../../shared/directives/hide-if-claims-not-met.directive';
import { claimReq } from '../../shared/utils/claimReq-utils';
import { FormControl, ɵInternalFormsSharedModule, ReactiveFormsModule, FormGroup } from '@angular/forms';
import { debounceTime, distinctUntilChanged, Subscription } from 'rxjs';

@Component({
  selector: 'app-main-layout',
  imports: [RouterOutlet, RouterLink, HideIfClaimsNotMetDirective, ɵInternalFormsSharedModule, ReactiveFormsModule],
  templateUrl: './main-layout.component.html',
  styles: ``,
})
export class MainLayoutComponent implements OnInit {
  claimReq = claimReq;
  //searchcontrol = new FormControl('');
  form: FormGroup = new FormGroup({
    searchTerm: new FormControl() });
  
  constructor(private router: Router, private authService: AuthService) {}
  
  ngOnInit(): void {
    this.form.valueChanges.pipe(
      debounceTime(500), // Wait for 500ms pause in events
      //distinctUntilChanged() // Only emit when the current value is different than the last one
    ).subscribe((searchTerm) => {
       // Perform search operation with the debounced search value
      console.log('Searching for:', searchTerm);
      //what operation to perform with searchTerm
      // For example, you can navigate to a search results page or filter a list
      //on what bases will i navigate?
      
    })
    
    ;
  }

  onLogout() {
    this.authService.deleteToken();
    this.router.navigateByUrl('/signin');
  }
}
