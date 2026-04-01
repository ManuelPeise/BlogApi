import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImressumPage } from './imressum-page';

describe('ImressumPage', () => {
  let component: ImressumPage;
  let fixture: ComponentFixture<ImressumPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ImressumPage],
    }).compileComponents();

    fixture = TestBed.createComponent(ImressumPage);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
