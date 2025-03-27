import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProfesorSearchComponent } from './profesor-search.component';

describe('ProfesorSearchComponent', () => {
  let component: ProfesorSearchComponent;
  let fixture: ComponentFixture<ProfesorSearchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProfesorSearchComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProfesorSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
