import { TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { ProfilePeoplesComponent } from './profile-peoples.component';

describe('Profile Peoples Component', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      declarations: [ProfilePeoplesComponent],
    }).compileComponents();
  });

  it('should create the profile peoples component', () => {
    const fixture = TestBed.createComponent(ProfilePeoplesComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });
});
