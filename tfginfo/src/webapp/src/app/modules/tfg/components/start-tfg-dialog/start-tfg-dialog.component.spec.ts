import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StartTfgDialogComponent } from './start-tfg-dialog.component';

describe('StartTfgDialogComponent', () => {
  let component: StartTfgDialogComponent;
  let fixture: ComponentFixture<StartTfgDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StartTfgDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StartTfgDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
