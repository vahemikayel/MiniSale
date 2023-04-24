import { Component, OnInit } from '@angular/core';
import { AccountService } from 'src/app/account/account.service';

@Component({
  selector: 'app-toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss']
})
export class ToolbarComponent implements OnInit {
  isAuthorized = false;
  constructor(private accountService: AccountService) {
    this.accountService.currentUser$.subscribe(auth => {
      this.isAuthorized = auth;
    });

  }

  ngOnInit(): void {
  }

  logout(): void {
    this.accountService.logout();
  }

}
