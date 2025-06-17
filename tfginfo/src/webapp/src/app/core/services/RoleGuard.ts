import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { ConfigurationService } from '../services/configuration.service';
import { Location } from '@angular/common';
import { RoleId } from '../../modules/admin/models/role.model';

@Injectable({ providedIn: 'root' })
export class RoleGuard implements CanActivate {
  constructor(private configService: ConfigurationService, private router: Router, private location: Location) {}

  canActivate(route: any): boolean {
    const role = this.configService.getRole();
    const allowedRoles = route.data['roles'] as number[];
    if (role && allowedRoles.includes(role)) {
      return true;
    }
    this.router.navigate(['/not-found']);
    return false;
  }
}