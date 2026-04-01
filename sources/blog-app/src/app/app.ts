import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterLink, RouterOutlet, RouterLinkActive, Router } from '@angular/router';
import { AuthenticationService } from './services/AuthenticationService';
import { environment } from '../environments/environment';
import { FormsModule } from '@angular/forms';
import { LoadingSpinner } from './components/customComponents/loading-spinner/loading-spinner';
import { Footer } from './components/footer/footer';
import i18n from './I18nService';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive, FormsModule, LoadingSpinner, Footer],
  templateUrl: './app.html',
  styleUrls: [],
})
export class App implements OnInit {
  protected readonly title = signal(environment.appName);
  router = inject(Router);
  authService = inject(AuthenticationService);
  labels: any;

  ngOnInit(): void {
    this.loadLabels();
    this.authService.loadCurrentUser();

    if (this.authService.currentUserSignal() != null) {
      this.router.navigate(['/']);
    }
  }

  onLogout() {
    this.authService.signOut();
  }

  loadLabels() {
    this.labels = {
      navBlogs: i18n.t('labelBlogs', { ns: 'common' }),
      navProfile: i18n.t('labelProfile', { ns: 'common' }),
      navCreateBlog: i18n.t('labelCreateBlog', { ns: 'common' }),
      navSignOut: i18n.t('labelSignOut', { ns: 'common' }),
      labelMyBlogs: i18n.t('labelMyBlogs', { ns: 'common' }),
    };
  }
}
