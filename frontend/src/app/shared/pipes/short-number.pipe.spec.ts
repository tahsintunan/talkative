import { ShortNumberPipe } from './short-number.pipe';

describe('ShortNumberPipe', () => {
  const pipe = new ShortNumberPipe();

  it('transforms 20000 to "20K"', () => {
    expect(pipe.transform(20000)).toBe('20K');
  });

  it('transforms 20000000 to "20M"', () => {
    expect(pipe.transform(20000000)).toBe('20M');
  });

  it('transforms 20000000000 to "20B"', () => {
    expect(pipe.transform(20000000000)).toBe('20B');
  });

  it('transforms 20000000000000 to "20T"', () => {
    expect(pipe.transform(20000000000000)).toBe('20T');
  });

  it('transforms 20000000000000000 to "20Q"', () => {
    expect(pipe.transform(20000000000000000)).toBe('20Q');
  });
});
