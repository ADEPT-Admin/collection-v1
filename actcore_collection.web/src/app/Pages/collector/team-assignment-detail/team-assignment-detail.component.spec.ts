import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TeamAssignmentDetailComponent } from './team-assignment-detail.component';

describe('TeamAssignmentDetailComponent', () => {
  let component: TeamAssignmentDetailComponent;
  let fixture: ComponentFixture<TeamAssignmentDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TeamAssignmentDetailComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TeamAssignmentDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
