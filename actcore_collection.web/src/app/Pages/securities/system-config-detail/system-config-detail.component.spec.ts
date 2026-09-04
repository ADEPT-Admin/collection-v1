import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SystemConfigDetailComponent } from './system-config-detail.component';

describe('SystemConfigDetailComponent', () => {
  let component: SystemConfigDetailComponent;
  let fixture: ComponentFixture<SystemConfigDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SystemConfigDetailComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SystemConfigDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
