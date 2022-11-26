import { HttpClientModule } from '@angular/common/http';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {
  MatDialogModule,
  MAT_DIALOG_DATA,
  MatDialogRef,
} from '@angular/material/dialog';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { RouterTestingModule } from '@angular/router/testing';
import { PostMakerDialogComponent } from './post-maker-dialog.component';

describe('Post maker dialog component', () => {
  let component: PostMakerDialogComponent;
  let fixture: ComponentFixture<PostMakerDialogComponent>;
  let dialogCloseSpy: any;
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        FormsModule,
        ReactiveFormsModule,
        HttpClientModule,
        MatSnackBarModule,
        MatDialogModule,
      ],
      declarations: [PostMakerDialogComponent],
      providers: [
        { provide: MAT_DIALOG_DATA, useValue: {} },
        { provide: MatDialogRef, useValue: { close: () => {} } },
      ],
      schemas: [NO_ERRORS_SCHEMA],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PostMakerDialogComponent);
    component = fixture.componentInstance;
    dialogCloseSpy = spyOn(component.dialogRef, 'close');
    fixture.detectChanges();
  });

  it('should create the post maker dialog', () => {
    expect(PostMakerDialogComponent).toBeTruthy();
  });

  it('should have a valid form', () => {
    component.formData.setValue({
      id: '2',
      text: 'random text',
      hashtags: [],
      isQuoteRetweet: false,
      originalTweetId: '',
    });

    expect(!component.formData.get('text')?.errors).toBe(true);
  });

  it('should close post maker dialog on form submit', () => {
    component.onSubmit();
    expect(dialogCloseSpy).toHaveBeenCalledWith(
      component.formData.getRawValue()
    );
  });
});
