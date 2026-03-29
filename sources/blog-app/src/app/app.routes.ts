import { Routes } from '@angular/router';
import { Signin } from './components/signin/signin';
import { Signup } from './components/signup/signup';

export const routes: Routes = [
  { path: 'signin', component: Signin },
  { path: 'signup', component: Signup },
];
