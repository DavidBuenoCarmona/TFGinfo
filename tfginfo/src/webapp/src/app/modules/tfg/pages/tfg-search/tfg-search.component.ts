import { Component, OnInit } from '@angular/core';
import { TfgListComponent } from '../../components/tfg-list/tfg-list.component';
import { TfgService } from '../../services/tfg.service';
import { TFGLineDTO } from '../../models/tfg.model';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { TranslateModule } from '@ngx-translate/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { Filter } from '../../../../core/core.model';
import { RoleId } from '../../../admin/models/role.model';

@Component({
    selector: 'app-tfg-search',
    imports: [TfgListComponent,
        MatFormFieldModule,
        MatInputModule,
        TranslateModule,
        ReactiveFormsModule,
        MatButtonModule,
        CommonModule],
    templateUrl: './tfg-search.component.html',
    styleUrl: './tfg-search.component.scss'
})

export class TfgSearchComponent implements OnInit {
    public filterForm!: FormGroup;
    public tfgs: TFGLineDTO[] = [];
    public filteredTfgs: TFGLineDTO[] = [];
    public filters: Filter[] = [];
    public isAdmin: boolean = false;

    constructor(
        public tfgService: TfgService,
        private router: Router,
        private fb: FormBuilder,
        private route: ActivatedRoute
    ) { }

    ngOnInit(): void {
        let role = Number.parseInt(localStorage.getItem('role')!);
        this.isAdmin = role === RoleId.Admin;
        this.filterForm = this.fb.group({
            filter: [''] 
        });
        const user = localStorage.getItem('user');
        switch (role) {
            case RoleId.Student:
                if (user) {
                    this.filters.push({ key: 'career', value: JSON.parse(user).career.toString() });
                }
                break;
            case RoleId.Professor:
                if (user) {
                    this.filters.push({ key: 'professor', value: JSON.parse(user).department.toString() });
                }
                break;
            case RoleId.Admin:
                this.filters.push({ key: 'university', value: localStorage.getItem('selectedUniversity')! });
                break;
        }
        this.tfgService.searchTfgs(this.filters).subscribe(tfgs => {
            this.tfgs = tfgs;
            this.filteredTfgs = [...this.tfgs];
        });
    }
    
    onCreate(): void {
        this.router.navigate(['new'], { relativeTo: this.route });
    }

    onSearch(): void {
        const filterValue = this.filterForm.get('filter')?.value.toLowerCase();
        this.filters = this.filters.filter(filter => filter.key !== 'generic');
        this.filters.push({ key: 'generic', value: filterValue });
        this.tfgService.searchTfgs(this.filters).subscribe(tfgs => {
            this.filteredTfgs = tfgs;
        });
    }

    onClearFilters(): void {
        this.filterForm.reset(); // Reiniciar el formulario
        this.filteredTfgs = [...this.tfgs]; // Restaurar la lista completa
        this.filters = this.filters.filter(filter => filter.key !== 'generic'); // Limpiar el filtro genérico
        this.tfgService.searchTfgs(this.filters).subscribe(tfgs => {
            this.filteredTfgs = tfgs;
        });
    }

    deleteTfg(tfgId: number): void {
        this.tfgService.deleteTfg(tfgId).subscribe({
            next: () => {},
            error: (err) => console.error(err),
            complete: () => {
              this.tfgs = this.tfgs.filter((item) => item.id !== tfgId);
              this.filteredTfgs = this.filteredTfgs.filter((item) => item.id !== tfgId);
            }
          });
        }

}
