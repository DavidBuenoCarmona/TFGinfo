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
import { MatIconModule } from '@angular/material/icon';
import { ConfigurationService } from '../../../../core/services/configuration.service';

@Component({
    selector: 'app-tfg-search',
    imports: [TfgListComponent,
        MatFormFieldModule,
        MatInputModule,
        TranslateModule,
        ReactiveFormsModule,
        MatButtonModule,
        MatIconModule,
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
    public showExtraFilters: boolean = false;

    constructor(
        public tfgService: TfgService,
        private router: Router,
        private fb: FormBuilder,
        private route: ActivatedRoute,
        private configurationService: ConfigurationService
    ) { }

    ngOnInit(): void {
        let role = this.configurationService.getRole();
        this.isAdmin = role === RoleId.Admin;
        this.filterForm = this.fb.group({
            generic: [''],
            name: [''],
            description: [''],
            departmentName: [''],
            slots: [''],
        });
        const user = this.configurationService.getUser();
        switch (role) {
            case RoleId.Student:
                if (user) {
                    this.filters.push({ key: 'career', value: user.career.toString() });
                }
                break;
            case RoleId.Professor:
                if (user) {
                    this.filters.push({ key: 'department', value: user.department.toString() });
                }
                break;
            case RoleId.Admin:
                this.filters.push({ key: 'university', value: this.configurationService.getSelectedUniversity()!.toString() });
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
        const formValues = this.filterForm.value;

        Object.keys(formValues).forEach(key => {
            const value = formValues[key];
            if (value !== null && value !== undefined && value !== '') {
                this.filters.push({ key, value });
            }
        });

        this.tfgService.searchTfgs(this.filters).subscribe(tfgs => {
            this.filteredTfgs = tfgs;
        });
    }

    onClearFilters(): void {
        this.filterForm.reset(); // Reiniciar el formulario
        this.filteredTfgs = [...this.tfgs]; // Restaurar la lista completa

        const formValues = this.filterForm.value;
        Object.keys(formValues).forEach(key => {
            this.filters = this.filters.filter(filter => filter.key !== key);
        });

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

    onShowExtraFilters(): void {
        this.showExtraFilters = !this.showExtraFilters;
        if (!this.showExtraFilters) {
            this.filterForm.get('name')?.setValue('');
            this.filterForm.get('description')?.setValue('');
            this.filterForm.get('slots')?.setValue('');
            this.filterForm.get('departmentName')?.setValue('');
        const formValues = this.filterForm.value;
            Object.keys(formValues).forEach(key => {
                if (key != "generic") this.filters = this.filters.filter(filter => filter.key !== key);
            });
        }
    }

}
