import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { TweetModel } from '../../../models/tweet.model';
import { UserModel } from '../../../models/user.model';
import { TweetService } from '../../../services/tweet.service';
import { UserService } from '../../../services/user.service';

interface PostMakerDialogData {
  isEdit: boolean;
  id: string;
  text: string;
  isQuoteRetweet?: boolean;
  originalTweetId?: string;
}

@Component({
  selector: 'app-post-maker-dialog',
  templateUrl: './post-maker-dialog.component.html',
  styleUrls: ['./post-maker-dialog.component.css'],
})
export class PostMakerDialogComponent implements OnInit {
  userAuth?: UserModel;
  retweetContent?: TweetModel;

  formData: FormGroup = this.formBuilder.group({
    id: this.data?.id,
    text: [this.data?.isEdit ? this.data?.text : '', [Validators.required]],
    hashtags: [],
    isQuoteRetweet: !!this.data?.isQuoteRetweet,
    originalTweetId: this.data?.originalTweetId,
  });

  constructor(
    private userService: UserService,
    private tweetService: TweetService,
    private formBuilder: FormBuilder,
    private router: Router,
    public dialogRef: MatDialogRef<PostMakerDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data?: PostMakerDialogData
  ) {}

  ngOnInit(): void {
    this.userService.userAuth.subscribe((res) => {
      this.userAuth = res;
    });

    if (this.data?.isQuoteRetweet && this.data?.originalTweetId) {
      this.tweetService
        .getTweetById(this.data?.originalTweetId)
        .subscribe((res) => {
          this.retweetContent = res;
        });
    }

    this.router.events.subscribe((res) => {
      this.dialogRef.close();
    });
  }

  onSubmit(): void {
    this.dialogRef.close(this.formData.getRawValue());
  }

  onHashtagInputChanged(value: any): void {
    this.formData.patchValue({
      text: value.text,
      hashtags: value.hashtags,
    });
  }
}
