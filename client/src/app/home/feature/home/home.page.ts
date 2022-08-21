import { Component, OnInit } from '@angular/core';
import { ProfileModel } from '../../models/profile.model';
import jwtDecode from 'jwt-decode';
import { CookieService } from 'ngx-cookie-service';
import { ActivatedRoute, ActivationEnd, Router } from '@angular/router';
import { filter, map } from 'rxjs';
import { ActiveChatService } from '../../services/active-chat.service';
@Component({
  selector: 'app-home',
  templateUrl: './home.page.html',
  styleUrls: ['./home.page.css'],
})
export class Homepage implements OnInit {

  profileId: string = this.getUserId()
  selectedUser?: ProfileModel;
  constructor(
    private cookie: CookieService,
    private router: Router,
    private route: ActivatedRoute,
    private activeChat: ActiveChatService
  ) { }

  ngOnInit(): void {

    if (this.route.children.length > 0) {
      let childParameter = this.route.children[0].snapshot.params['userId']

      this.profileId = childParameter
    }


    this.activeChat.getActivatedChat().subscribe(res => {
      if (res == "") {
        this.profileId = this.getUserId();
      } else {
        this.profileId = res
      }
    })

  }


  getUserId(): string {
    let decodedToken: any = jwtDecode(this.cookie.get('authorization'));
    return decodedToken.user_id
  }

}
