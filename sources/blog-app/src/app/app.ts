import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterLink, RouterOutlet, RouterLinkActive } from '@angular/router';
import { AuthenticationService } from './services/AuthenticationService';
import { environment } from '../environments/environment';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './app.html',
  styleUrls: [],
})
export class App implements OnInit {
  protected readonly title = signal(environment.appName);
  authService = inject(AuthenticationService);

  ngOnInit(): void {
    this.authService.loadCurrentUser();
  }

  onLogout() {
    this.authService.signOut();
  }
}
