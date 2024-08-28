import { ComponentFixture, TestBed } from '@angular/core/testing';

import { XOPlayersComponent } from './xoplayers.component';

describe('XOPlayersComponent', () => {
  let component: XOPlayersComponent;
  let fixture: ComponentFixture<XOPlayersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [XOPlayersComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(XOPlayersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
