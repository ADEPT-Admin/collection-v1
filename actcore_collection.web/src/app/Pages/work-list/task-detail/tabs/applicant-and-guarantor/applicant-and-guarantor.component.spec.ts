import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApplicantAndGuarantorComponent } from './applicant-and-guarantor.component';

describe('ApplicantAndGuarantorComponent', () => {
  let component: ApplicantAndGuarantorComponent;
  let fixture: ComponentFixture<ApplicantAndGuarantorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ApplicantAndGuarantorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ApplicantAndGuarantorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
