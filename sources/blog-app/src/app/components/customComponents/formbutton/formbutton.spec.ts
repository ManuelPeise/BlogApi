import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Formbutton } from './formbutton';

describe('Formbutton', () => {
  let component: Formbutton;
  let fixture: ComponentFixture<Formbutton>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Formbutton],
    }).compileComponents();

    fixture = TestBed.createComponent(Formbutton);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
