import { NO_ERRORS_SCHEMA } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Router } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { RetweetContentComponent } from './retweet-content.component';

describe('Retweet content component', () => {
  let component: RetweetContentComponent;
  let fixture: ComponentFixture<RetweetContentComponent>;
  let router: Router;
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RouterTestingModule],
      declarations: [RetweetContentComponent],
      providers: [
        {
          provide: Router,
          useClass: class {
            navigate = jasmine.createSpy('navigate');
          },
        },
      ],
      schemas: [NO_ERRORS_SCHEMA],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RetweetContentComponent);
    component = fixture.componentInstance;
    router = fixture.debugElement.injector.get(Router);
    fixture.detectChanges();

    component.data = {
      text: 'random',
      hashtags: ['test'],
      id: '1',
      createdAt: new Date(),
      isQuoteRetweet: false,
      isRetweet: false,
      likes: [],
      comments: [],
      user: {
        userId: '1',
      },
      retweetUsers: [],
      quoteRetweets: [],
    };
  });

  it('should create the retweet content component', () => {
    expect(RetweetContentComponent).toBeTruthy();
  });

  it('should get the selected tweet', () => {
    component.onTweetClick();
    expect(router.navigate).toHaveBeenCalledWith([`/tweet/${1}`]);
  });

  it('should navigate to search with hashtags', () => {
    component.onTagClick('hashtagValue');
    expect(router.navigate).toHaveBeenCalledWith(['/search'], {
      queryParams: { type: 'hashtag', value: 'hashtagValue' },
    });
  });
});
