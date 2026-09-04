import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ContractPhoneComponent } from './contract-phone.component';

describe('ContractPhoneComponent', () => {
  let component: ContractPhoneComponent;
  let fixture: ComponentFixture<ContractPhoneComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ContractPhoneComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ContractPhoneComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
