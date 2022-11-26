import { TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { CommentInputComponent } from './comment-input.component';

describe('Comment input Component', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      declarations: [CommentInputComponent],
    }).compileComponents();
  });

  it('should create the comment input component', () => {
    const fixture = TestBed.createComponent(CommentInputComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });
});
