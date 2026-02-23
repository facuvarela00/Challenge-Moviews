import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../servicios/authService/auth.service';

export const loginGuard: CanActivateFn = () => {

  const auth = inject(AuthService);
  const router = inject(Router);

  if (auth.estaLoggeado()) {
    router.navigate(['/inicio']);
    return false;
  }


  return true;
};
