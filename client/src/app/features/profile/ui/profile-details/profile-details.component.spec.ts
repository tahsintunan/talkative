import { HttpClientModule } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { GenerateImagePipe } from 'src/app/shared/pipes/generate-image.pipe';
import { ShortNumberPipe } from 'src/app/shared/pipes/short-number.pipe';
import { TimeAgoPipe } from 'src/app/shared/pipes/time-ago.pipe';
import { ProfileDetailsComponent } from './profile-details.component';

describe('Profile details component', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RouterTestingModule, HttpClientModule],
      declarations: [
        ProfileDetailsComponent,
        GenerateImagePipe,
        ShortNumberPipe,
      ],
    }).compileComponents();
  });

  it('should create the profile details component', () => {
    const fixture = TestBed.createComponent(ProfileDetailsComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });
});
