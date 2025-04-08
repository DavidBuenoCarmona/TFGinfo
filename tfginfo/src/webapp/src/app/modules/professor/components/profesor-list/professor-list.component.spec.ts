import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProfesorListComponent } from './professor-list.component';

describe('ProfesorListComponent', () => {
  let component: ProfesorListComponent;
  let fixture: ComponentFixture<ProfesorListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ProfesorListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProfesorListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
