import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddMultipleCollectorProflieComponent } from './add-multiple-collector-proflie.component';

describe('AddMultipleCollectorProflieComponent', () => {
  let component: AddMultipleCollectorProflieComponent;
  let fixture: ComponentFixture<AddMultipleCollectorProflieComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddMultipleCollectorProflieComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddMultipleCollectorProflieComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
