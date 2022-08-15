import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SigninPage } from './feature/signin/signin.page';
import { SignupPage } from './feature/signup/signup.page';

const routes: Routes = [
  { path: '', redirectTo: '/auth/signin', pathMatch: 'full' },
  {
    path: 'signup',
    component: SignupPage,
  },
  {
    path: 'signin',
    component: SigninPage,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AuthRoutingModule {}
