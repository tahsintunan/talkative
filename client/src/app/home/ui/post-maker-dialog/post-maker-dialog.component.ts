import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { UserModel } from '../../models/user.model';
import { UserService } from '../../services/user.service';

interface PostMakerDialogData {
  text: string;
  dialogTitle: string;
  isRetweet?: boolean;
  retweetId?: string;
}

@Component({
  selector: 'app-post-maker-dialog',
  templateUrl: './post-maker-dialog.component.html',
  styleUrls: ['./post-maker-dialog.component.css'],
})
export class PostMakerDialogComponent implements OnInit {
  userAuth?: UserModel;

  postData: FormGroup = this.formBuilder.group({
    text: ['', Validators.required],
    hashtags: [],
    isRetweet: !!this.data?.isRetweet,
    retweetId: this.data?.retweetId,
  });

  constructor(
    private userService: UserService,
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<PostMakerDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data?: PostMakerDialogData
  ) {}

  ngOnInit(): void {
    this.userService.userAuth.subscribe((res) => {
      this.userAuth = res;
    });
  }

  onPostClick(): void {
    this.dialogRef.close(this.postData.getRawValue());
  }

  onHashtagInputChanged(value: any): void {
    this.postData.get('text')?.setValue(value.text);
    this.postData.get('hashtags')?.setValue(value.hashtags);
  }
}
