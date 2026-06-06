import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReceiptModal } from './receipt-modal';

describe('ReceiptModal', () => {
  let component: ReceiptModal;
  let fixture: ComponentFixture<ReceiptModal>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReceiptModal],
    }).compileComponents();

    fixture = TestBed.createComponent(ReceiptModal);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});