import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup = new FormGroup('');
  returnUrl: string = '/';
  isAuthorized = false;

  constructor(private accountService: AccountService, private router: Router, private activatedRoute: ActivatedRoute) {
    this.accountService.currentUser$.subscribe(auth => {
      this.isAuthorized = auth;
    });
  }

  ngOnInit(): void {
    this.returnUrl = this.activatedRoute.snapshot.queryParams['returnUrl'] || '/';
    this.createLoginForm();
  }

  createLoginForm() {
    this.loginForm = new FormGroup({
      userName: new FormControl('', /* [Validators.required, Validators.pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$')] */),
      password: new FormControl('', /* Validators.required */)
    });
  }

  onSubmit() {
    this.accountService.login(this.loginForm?.value).subscribe({
      next: () => {
        this.router.navigateByUrl(this.returnUrl);
      },
      error: (error) => {
        console.log(error);
      }
    });
  }
}
