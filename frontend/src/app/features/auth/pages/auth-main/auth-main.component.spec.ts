import { HttpClientModule } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { AuthMainComponent } from './auth-main.component';

describe('Auth main component', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RouterTestingModule, HttpClientModule],
      declarations: [AuthMainComponent],
    }).compileComponents();
  });

  it('should create the auth main component', () => {
    const fixture = TestBed.createComponent(AuthMainComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });
});
