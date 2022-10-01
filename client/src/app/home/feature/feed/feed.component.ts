import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { TweetModel } from '../../models/tweet.model';
import { TweetService } from '../../services/tweet.service';
import { PostMakerDialogComponent } from '../../ui/post-maker-dialog/post-maker-dialog.component';

@Component({
  selector: 'app-feed',
  templateUrl: './feed.component.html',
  styleUrls: ['./feed.component.css'],
})
export class FeedComponent implements OnInit {
  constructor(private tweetService: TweetService, public dialog: MatDialog) {}

  ngOnInit(): void {}

  onCreatePostClick() {
    const dialogRef = this.dialog.open(PostMakerDialogComponent, {
      width: '500px',
    });

    dialogRef.afterClosed().subscribe((result: TweetModel) => {
      if (result) {
        this.tweetService.createTweet(result).subscribe((res) => {
          console.log(res);
        });
      }
    });
  }
}
