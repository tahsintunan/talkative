import { HttpClientModule } from '@angular/common/http';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { By } from '@angular/platform-browser';
import { RouterTestingModule } from '@angular/router/testing';
import { BlockServiceMock } from 'src/app/core/mock-services/block.service.mock';
import { FollowServiceMock } from 'src/app/core/mock-services/follow.service.mock';
import { BlockService } from 'src/app/core/services/block.service';
import { FollowService } from 'src/app/core/services/follow.service';
import { ProfilePeopleItemComponent } from './profile-people-item.component';

describe('Profile people component', () => {
  let component: ProfilePeopleItemComponent;
  let fixture: ComponentFixture<ProfilePeopleItemComponent>;
  let serviceStub: any;
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,
        FormsModule,
        ReactiveFormsModule,
        HttpClientModule,
      ],
      declarations: [ProfilePeopleItemComponent],
      providers: [
        { provide: FollowService, useClass: FollowServiceMock },
        { provide: BlockService, useClass: BlockServiceMock },
      ],
      schemas: [NO_ERRORS_SCHEMA],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProfilePeopleItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the profile people component', () => {
    expect(ProfilePeopleItemComponent).toBeTruthy();
  });

  it('should follow a user', () => {
    component.isFollowing = false;
    fixture.detectChanges();
    spyOn(component, 'follow');

    fixture.debugElement
      .query(
        (debugEl) =>
          debugEl.name === 'button' &&
          (debugEl.nativeElement.textContent as string).includes('Follow')
      )
      .triggerEventHandler('click', null);
    expect(component.follow).toHaveBeenCalled();
  });

  it('should unfollow a user', () => {
    component.isFollowing = true;
    fixture.detectChanges();
    spyOn(component, 'unfollow');

    fixture.debugElement
      .query(
        (debugEl) =>
          debugEl.name === 'button' &&
          (debugEl.nativeElement.textContent as string).includes('Unfollow')
      )
      .triggerEventHandler('click', null);
    expect(component.unfollow).toHaveBeenCalled();
  });

  it('should block a user', () => {
    component.isBlocked = false;
    component.showBlockButton = true;
    fixture.detectChanges();
    spyOn(component, 'block');

    fixture.debugElement
      .query(
        (debugEl) =>
          debugEl.name === 'button' &&
          (debugEl.nativeElement.textContent as string).includes('Block')
      )
      .triggerEventHandler('click', null);
    expect(component.block).toHaveBeenCalled();
  });

  it('should unblock a user', () => {
    component.isBlocked = true;
    component.showBlockButton = true;
    fixture.detectChanges();
    spyOn(component, 'unblock');

    fixture.debugElement
      .query(
        (debugEl) =>
          debugEl.name === 'button' &&
          (debugEl.nativeElement.textContent as string).includes('Unblock')
      )
      .triggerEventHandler('click', null);
    expect(component.unblock).toHaveBeenCalled();
  });
});
