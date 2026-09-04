import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SecuritiesComponent } from './securities.component';

describe('SettingsComponent', () => {
  let component: SecuritiesComponent;
  let fixture: ComponentFixture<SecuritiesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SecuritiesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SecuritiesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
