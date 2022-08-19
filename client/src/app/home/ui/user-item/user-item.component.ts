import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProfileModel } from '../../Models/profile.model';

@Component({
  selector: 'app-user-item',
  templateUrl: './user-item.component.html',
  styleUrls: ['./user-item.component.css'],
})
export class UserItemComponent implements OnInit {
  @Input() profile?: ProfileModel;

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute
  ) { }

  ngOnInit(): void { }

  getProfileImage(): string {
    return 'https://img.icons8.com/fluency/344/fox.png';
  }

  profileClicked() {
    this.router.navigate(['./', 'chat', this.profile?.userId], { relativeTo: this.activatedRoute })
  }
}
