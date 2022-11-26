import { TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { HashtagInputComponent } from './hashtag-input.component';

describe('Hashtag input Component', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      declarations: [HashtagInputComponent],
    }).compileComponents();
  });

  it('should create the hashtag input component', () => {
    const fixture = TestBed.createComponent(HashtagInputComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });
});
