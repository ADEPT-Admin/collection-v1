import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CollectorModalComponent } from './collector-modal.component';

describe('SearchModalComponent', () => {
  let component: CollectorModalComponent;
  let fixture: ComponentFixture<CollectorModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CollectorModalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CollectorModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
