import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MasterMenuDetailComponent } from './master-menu-detail.component';

describe('MasterMenuDetailComponent', () => {
  let component: MasterMenuDetailComponent;
  let fixture: ComponentFixture<MasterMenuDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MasterMenuDetailComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MasterMenuDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
