import { TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { EmptyResultOverlayComponent } from './empty-result-overlay.component';

describe('Empty result overlay Component', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      declarations: [EmptyResultOverlayComponent],
    }).compileComponents();
  });

  it('should create the empty result overlay component', () => {
    const fixture = TestBed.createComponent(EmptyResultOverlayComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });
});
