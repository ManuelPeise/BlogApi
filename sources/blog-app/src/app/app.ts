import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterLink, RouterOutlet, RouterLinkActive } from '@angular/router';
import { AuthenticationService } from './services/AuthenticationService';
import { environment } from '../environments/environment';
import { FormsModule } from '@angular/forms';
import { LoadingSpinner } from './components/customComponents/loading-spinner/loading-spinner';
import i18n from './I18nService';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive, FormsModule, LoadingSpinner],
  templateUrl: './app.html',
  styleUrls: [],
})
export class App implements OnInit {
  protected readonly title = signal(environment.appName);
  authService = inject(AuthenticationService);
  labels: any;

  ngOnInit(): void {
    this.authService.loadCurrentUser();
    this.loadLabels();
  }

  onLogout() {
    this.authService.signOut();
  }

  loadLabels() {
    this.labels = {
      navBlogs: i18n.t('Blogs', { ns: 'common' }),
      navProfile: i18n.t('Profile', { ns: 'common' }),
      navCreateBlog: i18n.t('Create Blog', { ns: 'common' }),
      navSignOut: i18n.t('Sign Out', { ns: 'common' }),
    };
  }
}
