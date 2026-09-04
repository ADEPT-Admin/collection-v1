import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TeamAssignmentModalComponent } from './team-assignment-modal.component';

describe('TeamAssignmentModalComponent', () => {
  let component: TeamAssignmentModalComponent;
  let fixture: ComponentFixture<TeamAssignmentModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TeamAssignmentModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TeamAssignmentModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
