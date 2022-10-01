import { Homepage } from './feature/home/home.page';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FeedComponent } from './feature/feed/feed.component';
import { NotificationsComponent } from './feature/notifications/notifications.component';
import { ProfileComponent } from './feature/profile/profile.component';

const routes: Routes = [
  {
    path: '',
    component: Homepage,
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
        path: 'profile',
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
