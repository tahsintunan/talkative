import { TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { GenerateImagePipe } from '../../pipes/generate-image.pipe';
import { AvatarComponent } from './avatar.component';

describe('Avatar Component', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      declarations: [AvatarComponent, GenerateImagePipe],
    }).compileComponents();
  });

  it('should create the avatar component', () => {
    const fixture = TestBed.createComponent(AvatarComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });
});
