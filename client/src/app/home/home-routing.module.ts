import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FeedComponent } from './feature/feed/feed.component';
import { HomeComponent } from './feature/home/home.component';
import { NotificationsComponent } from './feature/notifications/notifications.component';
import { ProfileComponent } from './feature/profile/profile.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
    children: [
      {
        path: '',
        redirectTo: 'feed',
        pathMatch: 'full',
      },
      {
        path: 'feed',
        component: FeedComponent,
      },
      {
        path: 'profile/:userId',
        component: ProfileComponent,
      },
      {
        path: 'notifications',
        component: NotificationsComponent,
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class HomeRoutingModule {}
