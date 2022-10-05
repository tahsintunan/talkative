import { Component, Input, OnInit } from '@angular/core';
import { UserModel } from '../../models/user.model';

@Component({
  selector: 'app-profile-details',
  templateUrl: './profile-details.component.html',
  styleUrls: ['./profile-details.component.css'],
})
export class ProfileDetailsComponent implements OnInit {
  @Input() data?: UserModel;

  constructor() {}

  ngOnInit(): void {}

  public getAge() {
    const today = new Date();
    const birthDate = new Date(this.data?.dateOfBirth || '');
    let age = today.getFullYear() - birthDate.getFullYear();
    const m = today.getMonth() - birthDate.getMonth();
    if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
      age--;
    }
    return age;
  }
}
