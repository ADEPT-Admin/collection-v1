import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CollectorTeamComponent } from './collector-team.component';

describe('CollectorTeamComponent', () => {
  let component: CollectorTeamComponent;
  let fixture: ComponentFixture<CollectorTeamComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CollectorTeamComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CollectorTeamComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
