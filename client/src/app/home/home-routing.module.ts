import { Homepage } from './feature/home/home.page';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ChatComponent } from './ui/chat/chat.component';

const routes: Routes = [
  {
    path: '',
    component: Homepage,
    children: [
      {
        path: 'chat/:userId',
        component: ChatComponent,
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class HomeRoutingModule {}
