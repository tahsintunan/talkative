import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTabsModule } from '@angular/material/tabs';
import { ImageCropperModule } from 'ngx-image-cropper';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { SharedModule } from 'src/app/shared/shared.module';
import { ProfilePeopleItemComponent } from '../../shared/ui/profile-people-item/profile-people-item.component';
import { ProfileComponent } from './pages/profile/profile.component';
import { ProfileRoutingModule } from './profile-routing.module';
import { PasswordUpdateDialogComponent } from './ui/password-update-dialog/password-update-dialog.component';
import { ProfileDetailsComponent } from './ui/profile-details/profile-details.component';
import { ProfileImageUploaderDialogComponent } from './ui/profile-image-uploader-dialog/profile-image-uploader-dialog.component';
import { ProfilePeoplesComponent } from './ui/profile-peoples/profile-peoples.component';
import { ProfileUpdateDialogComponent } from './ui/profile-update-dialog/profile-update-dialog.component';

@NgModule({
  declarations: [
    ProfileComponent,
    ProfileDetailsComponent,
    ProfileUpdateDialogComponent,
    PasswordUpdateDialogComponent,
    ProfilePeoplesComponent,
    ProfileImageUploaderDialogComponent,
  ],
  imports: [
    ProfileRoutingModule,
    CommonModule,
    SharedModule,
    InfiniteScrollModule,
    FormsModule,
    ReactiveFormsModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatTabsModule,
    MatDividerModule,
    MatIconModule,
    MatButtonModule,
    ImageCropperModule,
  ],
  exports: [ProfilePeopleItemComponent],
})
export class ProfileModule {}
