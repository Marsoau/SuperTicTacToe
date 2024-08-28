import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SuperGameComponent } from './super-game.component';

describe('SuperGameComponent', () => {
  let component: SuperGameComponent;
  let fixture: ComponentFixture<SuperGameComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SuperGameComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SuperGameComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
