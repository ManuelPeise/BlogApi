import { Routes } from '@angular/router';
import { Signin } from './components/signin/signin';
import { Signup } from './components/signup/signup';
import { Profile } from './components/profile/profile';
import { ChangePassword } from './components/change-password/change-password';
import { ImressumPage } from './components/imressum-page/imressum-page';
import { PrivacyPage } from './components/privacy-page/privacy-page';

export const routes: Routes = [
  { path: 'signin', component: Signin },
  { path: 'signup', component: Signup },
  { path: 'profile', component: Profile },
  { path: 'change-password', component: ChangePassword },
  { path: 'impressum', component: ImressumPage },
  { path: 'privacy', component: PrivacyPage },
];
