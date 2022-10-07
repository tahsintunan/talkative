import { Component, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { UtilityService } from 'src/app/shared/services/utility.service';
import { UserModel } from '../../models/user.model';
import { UserService } from '../../services/user.service';
import { ProfileUpdateDialogComponent } from '../profile-update-dialog/profile-update-dialog.component';

@Component({
  selector: 'app-profile-details',
  templateUrl: './profile-details.component.html',
  styleUrls: ['./profile-details.component.css'],
})
export class ProfileDetailsComponent implements OnInit {
  @Input() data?: UserModel;

  userAuth?: UserModel;

  constructor(
    private dialog: MatDialog,
    private userService: UserService,
    protected utilityService: UtilityService
  ) {}

  ngOnInit(): void {
    this.userService.userAuth.subscribe((res) => {
      this.userAuth = res;
    });
  }

  onEditProfileClick() {
    const dialogRef = this.dialog.open(ProfileUpdateDialogComponent, {
      width: '500px',
      data: {
        id: this.data?.id,
        username: this.data?.username,
        email: this.data?.email,
        dateOfBirth: this.data?.dateOfBirth,
      },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.userService.updateProfile(result);
      }
    });
  }
}
