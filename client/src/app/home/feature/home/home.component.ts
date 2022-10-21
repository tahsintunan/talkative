import { Component, OnInit } from '@angular/core';
import { UserModel } from '../../models/user.model';
import { BlockService } from '../../services/block.service';
import { FollowService } from '../../services/follow.service';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  userAuth?: UserModel;

  constructor(
    private userService: UserService,
    private blockService: BlockService,
    private followService: FollowService
  ) {}

  ngOnInit(): void {
    this.userService.userAuth.subscribe((res) => {
      this.userAuth = res;
    });

    this.preloadData();
  }

  preloadData(): void {
    this.userService.init();
    this.blockService.init();
    this.followService.init();
  }
}
