import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-comment-input',
  templateUrl: './comment-input.component.html',
  styleUrls: ['./comment-input.component.css'],
})
export class CommentInputComponent implements OnInit {
  @Input() defaultValue = '';
  @Input() isEditMode = false;
  @Output() onCommentSubmit = new EventEmitter();
  @Output() onEditCancel = new EventEmitter();

  isFocused = false;

  formControl = new FormControl(this.defaultValue, [
    Validators.required,
    Validators.maxLength(100),
  ]);

  constructor() {}

  ngOnInit(): void {
    this.formControl.patchValue(this.defaultValue);
    this.isFocused = this.isEditMode;
  }

  onSubmit() {
    this.onCommentSubmit.emit(this.formControl.value);
    this.formControl.reset();
    this.isFocused = false;
  }
}
