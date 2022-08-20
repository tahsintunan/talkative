import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { ProfileModel } from '../../models/profile.model';

@Component({
  selector: 'app-user-item',
  templateUrl: './user-item.component.html',
  styleUrls: ['./user-item.component.css'],
})
export class UserItemComponent implements OnInit {
  @Input() profile?: ProfileModel;

  constructor(private router: Router) {}

  ngOnInit(): void {}

  onClick() {
    this.router.navigate(['/home', this.profile?.userId]);
  }
}
