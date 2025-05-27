import { Component, OnInit } from '@angular/core';
import { DepartmentListComponent } from '../../components/department-list/department-list.component';
import { DepartmentService } from '../../services/department.service';
import { DepartmentDTO } from '../../models/department.model';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { TranslateModule } from '@ngx-translate/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { Filter } from '../../../../core/core.model';
import { MatIconModule } from '@angular/material/icon';

@Component({
    selector: 'app-department-search',
    imports: [DepartmentListComponent,
        MatFormFieldModule,
        MatInputModule,
        TranslateModule,
        ReactiveFormsModule,
        MatIconModule,
        MatButtonModule,
        CommonModule],
    templateUrl: './department-search.component.html',
    styleUrl: './department-search.component.scss'
})

export class DepartmentSearchComponent implements OnInit {
    public filterForm!: FormGroup;
    public departments: DepartmentDTO[] = [];
    public filteredDepartments: DepartmentDTO[] = [];
    public showExtraFilters = false;

    constructor(
        public departmentService: DepartmentService,
        private router: Router,
        private fb: FormBuilder,
        private route: ActivatedRoute
    ) { }

    ngOnInit(): void {
        this.filterForm = this.fb.group({
            generic: [''],
            name: [''],
            university: [''],
        });

        this.departmentService.getDepartments().subscribe(departments => {
            this.departments = departments;
            this.filteredDepartments = [...this.departments];
        });
    }
    
    onCreate(): void {
        this.router.navigate(['new'], { relativeTo: this.route });
    }

    onSearch(): void {
        const formValues = this.filterForm.value;
        let filters: Filter[] = [];

        Object.keys(formValues).forEach(key => {
            const value = formValues[key];
            if (value !== null && value !== undefined && value !== '') {
                filters.push({ key, value });
            }
        });

        this.departmentService.searchDepartments(filters).subscribe(departments =>{
            this.filteredDepartments = departments;
        });
    }

    onClearFilters(): void {
        this.filterForm.reset(); // Reiniciar el formulario
        this.filteredDepartments = [...this.departments]; // Restaurar la lista completa
    }

    deleteDepartment(departmentId: number): void {
        this.departmentService.deleteDepartment(departmentId).subscribe({
            next: () => {},
            error: (err) => console.error(err),
            complete: () => {
                this.departments = this.departments.filter((item) => item.id !== departmentId);
                this.filteredDepartments = this.filteredDepartments.filter((item) => item.id !== departmentId);
            }
        });
    }

    onShowExtraFilters(): void {
        this.showExtraFilters = !this.showExtraFilters;
        if (!this.showExtraFilters) {
            this.filterForm.get('university')?.setValue(''); // Limpiar el campo de universidad
            this.filterForm.get('name')?.setValue(''); // Limpiar el campo de nombre
        }
    }
}