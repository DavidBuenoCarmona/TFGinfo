import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TfgDetailComponent } from './tfg-detail.component';

describe('TfgDetailComponent', () => {
  let component: TfgDetailComponent;
  let fixture: ComponentFixture<TfgDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TfgDetailComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TfgDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
