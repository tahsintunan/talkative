import { Component, Input, OnInit } from '@angular/core';
import { ProfileModel } from '../../models/profile.model';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
})
export class ProfileComponent implements OnInit {
  @Input() profile?: ProfileModel;

  constructor() {}

  ngOnInit(): void {}

  getProfileImage(): string {
    return 'https://img.icons8.com/fluency/344/fox.png';
  }

  getAge(): number {
    if (this.profile) {
      return (
        new Date().getFullYear() -
        new Date(this.profile?.dateOfBirth).getFullYear()
      );
    } else {
      return 0;
    }
  }
}
