import { Component, OnInit } from '@angular/core';
import { DepartmentListComponent } from '../../components/department-list/department-list.component';
import { DepartmentService } from '../../services/department.service';
import { DepartmentDTO } from '../../models/department.model';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { Filter } from '../../../../core/core.model';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDialog } from '@angular/material/dialog';
import { ImportSummaryComponent } from '../../../../core/layout/components/import-summary/import-summary.component';
import { ImportDialogComponent } from '../../../../core/layout/components/import-dialog/import-dialog.component';

@Component({
    selector: 'app-department-search',
    imports: [DepartmentListComponent,
        MatFormFieldModule,
        MatInputModule,
        TranslateModule,
        ReactiveFormsModule,
        MatIconModule,
        MatTooltipModule,
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
        private route: ActivatedRoute,
        public translateService: TranslateService,
        private dialog: MatDialog
    ) { }

    ngOnInit(): void {
        this.filterForm = this.fb.group({
            generic: [''],
            name: [''],
            acronym: [''],
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
            this.filterForm.get('acronym')?.setValue(''); // Limpiar el campo de acrónimo
        }
    }

    openImportDialog(): void {
            const format = "DEPARTMENT.CSV_FORMAT";
            const dialogRef = this.dialog.open(ImportDialogComponent, {data: { format: format}});
            dialogRef.afterClosed().subscribe((result: File) => {
                if (result) {
                    const reader = new FileReader();
                    reader.onload = () => {
                        const base64 = (reader.result as string).split(',')[1]; // Solo la parte base64
                        this.departmentService.importFromCSV(base64).subscribe({
                            next: (res) => {
                                if (res.errorItems.length > 0) {
                                    this.dialog.open(ImportSummaryComponent, {
                                        data: {
                                            success: res.success,
                                            items: res.errorItems
                                        }
                                    });
                                }
                                this.onSearch();
                            }
                            , error: (err) => console.error(err)
                        });
                    };
                    reader.readAsDataURL(result);
                }
            });
        }
}