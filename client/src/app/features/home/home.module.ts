import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { SharedModule } from 'src/app/shared/shared.module';
import { HomeRoutingModule } from './home-routing.module';
import { HomeComponent } from './pages/home/home.component';
import { SearchResultComponent } from './pages/search-result/search-result.component';
import { NavbarComponent } from './ui/navbar/navbar.component';
import { UserListItemComponent } from './ui/user-list-item/user-list-item.component';

@NgModule({
  declarations: [
    HomeComponent,
    NavbarComponent,
    SearchResultComponent,
    UserListItemComponent,
  ],
  imports: [
    HomeRoutingModule,
    CommonModule,
    SharedModule,
    InfiniteScrollModule,
    MatIconModule,
    MatToolbarModule,
    MatSidenavModule,
  ],
})
export class HomeModule {}
