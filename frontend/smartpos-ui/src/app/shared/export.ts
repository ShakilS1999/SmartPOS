import { Injectable } from '@angular/core';
import * as XLSX from 'xlsx';
import { saveAs } from 'file-saver';

@Injectable({ providedIn: 'root' })
export class ExportService {

  exportToExcel(data: any[], fileName: string, sheetName: string = 'Sheet1') {
    const worksheet = XLSX.utils.json_to_sheet(data);
    const workbook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, worksheet, sheetName);

    const excelBuffer = XLSX.write(workbook, {
      bookType: 'xlsx',
      type: 'array'
    });

    const blob = new Blob([excelBuffer], {
      type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
    });

    saveAs(blob, `${fileName}_${new Date().toLocaleDateString('en-GB').replace(/\//g, '-')}.xlsx`);
  }

  exportSales(sales: any[]) {
    const data = sales.map(s => ({
      'Invoice No': s.invoiceNo,
      'Customer': s.customerName,
      'Date': new Date(s.saleDate).toLocaleDateString('en-GB'),
      'Grand Total': s.grandTotal,
      'Discount': s.discount,
      'Tax': s.tax,
      'Net Total': s.netTotal
    }));

    this.exportToExcel(data, 'Sales_Report', 'Sales');
  }

  exportPurchases(purchases: any[]) {
    const data = purchases.map(p => ({
      'Invoice No': p.invoiceNo,
      'Supplier': p.supplierName,
      'Date': new Date(p.purchaseDate).toLocaleDateString('en-GB'),
      'Grand Total': p.grandTotal
    }));

    this.exportToExcel(data, 'Purchase_Report', 'Purchases');
  }

  exportProducts(products: any[]) {
    const data = products.map(p => ({
      'ID': p.productId,
      'Product Name': p.productName,
      'Barcode': p.barcode,
      'Price': p.price,
      'Stock': p.stockQuantity
    }));

    this.exportToExcel(data, 'Products_Report', 'Products');
  }

  exportProfit(report: any) {
    const data = [{
      'Total Profit': report.totalProfit,
      'Today Profit': report.todayProfit,
      'Total Sales': report.totalSales
    }];

    this.exportToExcel(data, 'Profit_Report', 'Profit');
  }

}