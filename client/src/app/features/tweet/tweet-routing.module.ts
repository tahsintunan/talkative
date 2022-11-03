import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TweetDetailsComponent } from '../home/pages/tweet-details/tweet-details.component';

const routes: Routes = [
  {
    path: '',
    component: TweetDetailsComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class TweetRoutingModule {}
