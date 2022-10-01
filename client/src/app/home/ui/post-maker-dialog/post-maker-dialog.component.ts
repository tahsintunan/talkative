import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

interface PostMakerDialogData {
  text: string;
  dialogTitle: string;
}

@Component({
  selector: 'app-post-maker-dialog',
  templateUrl: './post-maker-dialog.component.html',
  styleUrls: ['./post-maker-dialog.component.css'],
})
export class PostMakerDialogComponent implements OnInit {
  postData: FormGroup = this.formBuilder.group({
    text: ['', Validators.required],
    tags: [[], Validators.required],
  });

  constructor(
    private formBuilder: FormBuilder,
    public dialogRef: MatDialogRef<PostMakerDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data?: PostMakerDialogData
  ) {}

  ngOnInit(): void {}

  onCancelClick(): void {
    this.dialogRef.close();
  }

  onPostClick(): void {
    this.dialogRef.close(this.postData.getRawValue());
  }

  onHashtagInputChanged(value: any): void {
    this.postData.get('text')?.setValue(value.text);
    this.postData.get('tags')?.setValue(value.tags);
  }
}
