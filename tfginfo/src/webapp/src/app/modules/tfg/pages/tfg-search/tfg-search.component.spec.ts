import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TfgSearchComponent } from './tfg-search.component';

describe('TfgSearchComponent', () => {
  let component: TfgSearchComponent;
  let fixture: ComponentFixture<TfgSearchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TfgSearchComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TfgSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
