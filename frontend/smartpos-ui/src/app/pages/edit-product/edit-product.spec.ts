import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { EditProduct } from './edit-product';

describe('EditProduct', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditProduct],
      providers: [
        provideRouter([]),
        provideHttpClient()
      ]
    }).compileComponents();
  });

  it('should create', () => {
    const fixture = TestBed.createComponent(EditProduct);
    const component = fixture.componentInstance;
    expect(component).toBeTruthy();
  });
});