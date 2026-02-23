import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { environment } from '../../../../enviroment/enviroment';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseUrl = environment.URL_BASE;
  private jwtHelper = new JwtHelperService();

  constructor(private http: HttpClient) { }

  login(username: string, password: string): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/usuario/login`, {
      username,
      password
    }).pipe(
      tap(response => {
        localStorage.setItem('token', response.token);
      })
    );
  }

  logout() {
    localStorage.removeItem('token');
  }

  obtenerToken(): string | null {
    return localStorage.getItem('token');
  }

  estaLoggeado(): boolean {
    const token = this.obtenerToken();
    if (!token) return false;

    const payload = this.obtenerPayload();
    if (!payload) return false;

    const exp = payload.exp;
    const now = Math.floor(Date.now() / 1000);

    return exp > now;
  }

  private obtenerPayload(): any {
    const token = this.obtenerToken();
    if (!token) return null;

    return this.jwtHelper.decodeToken(token);
  }

  esAdmin(): boolean {
    const payload = this.obtenerPayload();
    if (!payload) return false;

    return payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] === "Admin";
  }

  obtenerUserId(): string | null {
    const payload = this.obtenerPayload();
    if (!payload) return null;

    return payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"] || null;
  }
}