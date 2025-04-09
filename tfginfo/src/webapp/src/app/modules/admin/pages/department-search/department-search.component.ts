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

@Component({
    selector: 'app-department-search',
    imports: [DepartmentListComponent,
        MatFormFieldModule,
        MatInputModule,
        TranslateModule,
        ReactiveFormsModule,
        MatButtonModule,
        CommonModule],
    templateUrl: './department-search.component.html',
    styleUrl: './department-search.component.scss'
})

export class DepartmentSearchComponent implements OnInit {
    public filterForm!: FormGroup;
    public departments: DepartmentDTO[] = [];
    public filteredDepartments: DepartmentDTO[] = [];

    constructor(
        public departmentService: DepartmentService,
        private router: Router,
        private fb: FormBuilder,
        private route: ActivatedRoute
    ) { }

    ngOnInit(): void {
        this.filterForm = this.fb.group({
            filter: [''] 
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
        const filterValue = this.filterForm.get('filter')?.value.toLowerCase();
        this.filteredDepartments = this.departments.filter(department =>
            department.name.toLowerCase().includes(filterValue)
        );
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
}