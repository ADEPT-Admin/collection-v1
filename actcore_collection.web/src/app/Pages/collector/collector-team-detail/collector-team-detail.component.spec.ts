import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CollectorTeamDetailComponent } from './collector-team-detail.component';

describe('CollectorTeamDetailComponent', () => {
  let component: CollectorTeamDetailComponent;
  let fixture: ComponentFixture<CollectorTeamDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CollectorTeamDetailComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CollectorTeamDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
