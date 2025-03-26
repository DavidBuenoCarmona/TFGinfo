import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TfgListComponent } from './tfg-list.component';

describe('TfgListComponent', () => {
  let component: TfgListComponent;
  let fixture: ComponentFixture<TfgListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TfgListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TfgListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
