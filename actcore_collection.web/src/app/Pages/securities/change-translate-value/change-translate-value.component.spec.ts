import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChangeTranslateValueComponent } from './change-translate-value.component';

describe('ChangeTranslateValueComponent', () => {
  let component: ChangeTranslateValueComponent;
  let fixture: ComponentFixture<ChangeTranslateValueComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ChangeTranslateValueComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ChangeTranslateValueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
