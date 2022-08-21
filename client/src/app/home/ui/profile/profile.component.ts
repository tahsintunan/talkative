import {
  Component,
  Input,
  OnChanges,
  OnInit,
  SimpleChanges,
} from '@angular/core';
import { ProfileModel } from '../../models/profile.model';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
})
export class ProfileComponent implements OnInit, OnChanges {
  @Input() profileId: string = '';

  profile: ProfileModel | undefined;

  constructor(private userService: UserService) {}
  ngOnChanges(changes: SimpleChanges): void {
    let currentProfileId = changes['profileId'].currentValue;

    if (currentProfileId != null) {
      this.getCurrentUserProfile(currentProfileId);
    }
  }

  getCurrentUserProfile(profileId: string) {
    this.userService.getUser(profileId).subscribe({
      next: (res) => {
        this.profile = {
          ...res,
        };
      },
      error: (err) => {
        console.log(err);
      },
    });
  }

  ngOnInit(): void {}

  getAge(): number {
    if (this.profile) {
      return (
        new Date().getFullYear() -
        new Date(this.profile.dateOfBirth!).getFullYear()
      );
    } else {
      return 0;
    }
  }
}
