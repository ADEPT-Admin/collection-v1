import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChangeTranslateValueDetailComponent } from './change-translate-value-detail.component';

describe('ChangeTranslateValueDetailComponent', () => {
  let component: ChangeTranslateValueDetailComponent;
  let fixture: ComponentFixture<ChangeTranslateValueDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ChangeTranslateValueDetailComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ChangeTranslateValueDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
