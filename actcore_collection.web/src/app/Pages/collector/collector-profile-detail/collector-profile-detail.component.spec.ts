import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CollectorProfileDetailComponent } from './collector-profile-detail.component';

describe('CollectorProfileDetailComponent', () => {
  let component: CollectorProfileDetailComponent;
  let fixture: ComponentFixture<CollectorProfileDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CollectorProfileDetailComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CollectorProfileDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
