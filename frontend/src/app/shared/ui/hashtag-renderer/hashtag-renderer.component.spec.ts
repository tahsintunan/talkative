import { TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { HashtagPipe } from '../../pipes/hashtag.pipe';
import { HashtagRendererComponent } from './hashtag-renderer.component';

describe('Hashtag renderer Component', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      declarations: [HashtagRendererComponent, HashtagPipe],
    }).compileComponents();
  });

  it('should create the hashtag renderer component', () => {
    const fixture = TestBed.createComponent(HashtagRendererComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });
});
