import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FeedComponent } from './pages/feed/feed.component';
import { TweetDetailsComponent } from './pages/tweet-details/tweet-details.component';

const routes: Routes = [
  {
    path: '',
    component: FeedComponent,
  },
  {
    path: ':tweetId',
    component: TweetDetailsComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TweetRoutingModule {}
