import { HttpClientModule } from '@angular/common/http';
import { TestBed } from '@angular/core/testing';
import {
  MatDialogModule,
  MatDialogRef,
  MAT_DIALOG_DATA,
} from '@angular/material/dialog';
import { RouterTestingModule } from '@angular/router/testing';
import { TimeAgoPipe } from 'src/app/shared/pipes/time-ago.pipe';
import { ProfileImageUploaderDialogComponent } from './profile-image-uploader-dialog.component';

describe('Profile image uploader dialog component', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RouterTestingModule, HttpClientModule, MatDialogModule],
      providers: [
        { provide: MatDialogRef, useValue: { close: () => {} } },
        { provide: MAT_DIALOG_DATA, useValue: {} },
      ],
      declarations: [ProfileImageUploaderDialogComponent, TimeAgoPipe],
    }).compileComponents();
  });

  it('should create the profile image uploader dialog component', () => {
    const fixture = TestBed.createComponent(
      ProfileImageUploaderDialogComponent
    );
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });
});
