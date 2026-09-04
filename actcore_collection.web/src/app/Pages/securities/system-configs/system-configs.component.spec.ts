import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SystemConfigsComponent } from './system-configs.component';

describe('SystemConfigComponent', () => {
  let component: SystemConfigsComponent;
  let fixture: ComponentFixture<SystemConfigsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SystemConfigsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SystemConfigsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
