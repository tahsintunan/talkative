import { NO_ERRORS_SCHEMA } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { By } from '@angular/platform-browser';
import { RouterTestingModule } from '@angular/router/testing';
import { TimeAgoPipe } from 'src/app/shared/pipes/time-ago.pipe';
import { CommentInputComponent } from 'src/app/shared/ui/comment-input/comment-input.component';
import { CommentItemComponent } from './comment-item.component';

describe('Comment item component', () => {
  let component: CommentItemComponent;
  let fixture: ComponentFixture<CommentItemComponent>;
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RouterTestingModule, FormsModule, ReactiveFormsModule],
      declarations: [CommentItemComponent, TimeAgoPipe, CommentInputComponent],
      providers: [],
      schemas: [NO_ERRORS_SCHEMA],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CommentItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the comment item component', () => {
    expect(CommentItemComponent).toBeTruthy();
  });
});
