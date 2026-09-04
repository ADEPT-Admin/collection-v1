import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ContractDescriptionComponent } from './contract-description.component';

describe('ContractDescriptionComponent', () => {
  let component: ContractDescriptionComponent;
  let fixture: ComponentFixture<ContractDescriptionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ContractDescriptionComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ContractDescriptionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
