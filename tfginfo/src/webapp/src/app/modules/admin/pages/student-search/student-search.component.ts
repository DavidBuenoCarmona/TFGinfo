import { Component, OnInit } from '@angular/core';
import { StudentDTO } from '../../models/student.model';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { StudentService } from '../../services/student.service';
import { StudentListComponent } from '../../components/student-list/student-list.component';
import { Filter } from '../../../../core/core.model';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ImportDialogComponent } from '../../../../core/layout/components/import-dialog/import-dialog.component';
import { ImportSummaryComponent } from '../../../../core/layout/components/import-summary/import-summary.component';
import { error } from 'console';
import { MatTooltipModule } from '@angular/material/tooltip';

@Component({
    selector: 'app-student-search',
    imports: [StudentListComponent,
        MatFormFieldModule,
        MatInputModule,
        TranslateModule,
        ReactiveFormsModule,
        MatButtonModule,
        MatDialogModule,
        MatIconModule,
        MatTooltipModule,
        CommonModule],
    templateUrl: './student-search.component.html',
    styleUrl: './student-search.component.scss'
})

export class StudentSearchComponent implements OnInit {
    public filterForm!: FormGroup;
    public students: StudentDTO[] = [];
    public filteredStudents: StudentDTO[] = [];
    public showExtraFilters = false;

    constructor(
        public studentService: StudentService,
        public translateService: TranslateService,
        private router: Router,
        private fb: FormBuilder,
        private route: ActivatedRoute,
        private dialog: MatDialog
    ) { }

    ngOnInit(): void {
        this.filterForm = this.fb.group({
            generic: [''],
            name: [''],
            surname: [''],
            email: [''],
            career: [''],
            university: ['']
        });

        this.studentService.getStudents().subscribe(students => {
            this.students = students;
            this.filteredStudents = [...this.students];
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

        this.studentService.searchStudents(filters).subscribe(students => {
            this.filteredStudents = students;
        });
    }

    onClearFilters(): void {
        this.filterForm.reset(); // Reiniciar el formulario
        this.filteredStudents = [...this.students]; // Restaurar la lista completa
    }

    deleteStudent(studentId: number): void {
        this.studentService.deleteStudent(studentId).subscribe({
            next: () => { },
            error: (err) => console.error(err),
            complete: () => {
                this.students = this.students.filter((item) => item.id !== studentId);
                this.filteredStudents = this.filteredStudents.filter((item) => item.id !== studentId);
            }
        });
    }

    onShowExtraFilters(): void {
        this.showExtraFilters = !this.showExtraFilters;
        if (!this.showExtraFilters) {
            this.filterForm.get('university')?.setValue(''); // Limpiar el campo de universidad
            this.filterForm.get('name')?.setValue(''); // Limpiar el campo de nombre
            this.filterForm.get('surname')?.setValue(''); // Limpiar el campo de apellido
            this.filterForm.get('email')?.setValue(''); // Limpiar el campo de email
            this.filterForm.get('career')?.setValue(''); // Limpiar el campo de carrera
        }
    }

    openImportDialog(): void {
        const format = "STUDENT.CSV_FORMAT";
        const dialogRef = this.dialog.open(ImportDialogComponent, {data: { format: format}});
        dialogRef.afterClosed().subscribe((result: File) => {
            if (result) {
                const reader = new FileReader();
                reader.onload = () => {
                    const base64 = (reader.result as string).split(',')[1]; // Solo la parte base64
                    this.studentService.importFromCSV(base64).subscribe({
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