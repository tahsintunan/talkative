import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { SearchResultComponent } from './pages/search-result/search-result.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
    children: [
      {
        path: '',
        redirectTo: 'tweet',
        pathMatch: 'full',
      },
      {
        path: 'tweet',
        loadChildren: () =>
          import('../tweet/tweet.module').then((m) => m.TweetModule),
      },
      {
        path: 'profile/:userId',
        loadChildren: () =>
          import('../profile/profile.module').then((m) => m.ProfileModule),
      },
      {
        path: 'notifications',
        loadChildren: () =>
          import('./../notification/notification.module').then(
            (m) => m.NotificationModule
          ),
      },
      {
        path: 'search',
        component: SearchResultComponent,
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class HomeRoutingModule {}
