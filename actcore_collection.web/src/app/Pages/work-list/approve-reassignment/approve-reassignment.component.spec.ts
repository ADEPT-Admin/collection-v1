import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ApproveReassignmentComponent } from './approve-reassignment.component';

describe('ApproveReassignmentComponent', () => {
  let component: ApproveReassignmentComponent;
  let fixture: ComponentFixture<ApproveReassignmentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ApproveReassignmentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ApproveReassignmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
