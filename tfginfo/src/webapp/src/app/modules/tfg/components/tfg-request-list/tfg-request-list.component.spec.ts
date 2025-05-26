import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TfgRequestListComponent } from './tfg-request-list.component';

describe('TfgRequestListComponent', () => {
  let component: TfgRequestListComponent;
  let fixture: ComponentFixture<TfgRequestListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TfgRequestListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TfgRequestListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
