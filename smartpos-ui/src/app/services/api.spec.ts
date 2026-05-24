import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ApiService } from './api';

describe('ApiService', () => {
  let service: ApiService;
  let httpMock: HttpTestingController;

  const baseUrl = 'https://localhost:7247/api';

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [ApiService]
    });

    service = TestBed.inject(ApiService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  // ✅ Service create test
  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  // 🔐 Login test
  it('should call login API', () => {
    const mockResponse = { token: 'test-token' };

    service.login({ username: 'admin', password: '1234' })
      .subscribe(res => {
        expect(res).toEqual(mockResponse);
      });

    const req = httpMock.expectOne(`${baseUrl}/Auth/login`);
    expect(req.request.method).toBe('POST');

    req.flush(mockResponse);
  });

  // 📦 Get Products test
  it('should fetch products', () => {
    const mockProducts = [
      { productId: 1, productName: 'Rice', price: 300 }
    ];

    localStorage.setItem('token', 'test-token');

    service.getProducts().subscribe(res => {
      expect(res).toEqual(mockProducts);
    });

    const req = httpMock.expectOne(`${baseUrl}/Product`);
    expect(req.request.method).toBe('GET');
    expect(req.request.headers.get('Authorization')).toBe('Bearer test-token');

    req.flush(mockProducts);
  });

  // 🛒 Create Sale test
  it('should create sale', () => {
    const saleData = {
      items: [
        { productId: 1, quantity: 2 }
      ]
    };

    localStorage.setItem('token', 'test-token');

    service.createSale(saleData).subscribe(res => {
      expect(res).toBeTruthy();
    });

    const req = httpMock.expectOne(`${baseUrl}/Sale`);
    expect(req.request.method).toBe('POST');
    expect(req.request.headers.get('Authorization')).toBe('Bearer test-token');

    req.flush({ success: true });
  });

});
