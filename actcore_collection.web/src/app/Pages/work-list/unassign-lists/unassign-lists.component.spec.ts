import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UnassignListsComponent } from './unassign-lists.component';

describe('UnassignListsComponent', () => {
  let component: UnassignListsComponent;
  let fixture: ComponentFixture<UnassignListsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UnassignListsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UnassignListsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
