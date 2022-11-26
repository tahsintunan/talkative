import { HashtagPipe } from './hashtag.pipe';

describe('Hashtag pipe', () => {
  const pipe = new HashtagPipe();

  it('should transform #random to a hashtag with span tag and proper class', () => {
    expect(pipe.transform('#random')).toBe(
      '<span class="hashtag">#random</span>'
    );
  });
});
