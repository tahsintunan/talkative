import { UtilityService } from './utility.service';

describe('utility service', () => {
  let service: UtilityService;
  beforeEach(() => {
    service = new UtilityService();
  });

  it('should give an age of 24', () => {
    expect(service.getAge(new Date(1998, 10, 8))).toBe(24);
  });
});
