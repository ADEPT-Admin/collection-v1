import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TeamAssignmentDetailModalComponent } from './team-assignment-detail-modal.component';

describe('TeamAssignmentDetailModalComponent', () => {
  let component: TeamAssignmentDetailModalComponent;
  let fixture: ComponentFixture<TeamAssignmentDetailModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TeamAssignmentDetailModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TeamAssignmentDetailModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
