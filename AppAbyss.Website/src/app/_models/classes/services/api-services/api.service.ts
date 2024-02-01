import { environment } from '../../../../../environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})

/**
 * Service for making API requests.
 */
export class ApiService {
  protected readonly apiBaseUrl;
  protected readonly http: HttpClient;

  /**
   * Constructs an instance of the ApiService.
   * @param http - The HttpClient instance used for making HTTP requests.
   */
  constructor(http: HttpClient) {
    this.http = http;
    this.apiBaseUrl = environment.API_BASE_URL;
  }
}
