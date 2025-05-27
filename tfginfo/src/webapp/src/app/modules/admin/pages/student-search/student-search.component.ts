import { Component, OnInit } from '@angular/core';
import { StudentDTO } from '../../models/student.model';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { TranslateModule } from '@ngx-translate/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { StudentService } from '../../services/student.service';
import { StudentListComponent } from '../../components/student-list/student-list.component';
import { Filter } from '../../../../core/core.model';
import { MatIconModule } from '@angular/material/icon';

@Component({
    selector: 'app-student-search',
    imports: [StudentListComponent,
        MatFormFieldModule,
        MatInputModule,
        TranslateModule,
        ReactiveFormsModule,
        MatButtonModule,
        MatIconModule,
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
        private router: Router,
        private fb: FormBuilder,
        private route: ActivatedRoute
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
        
        this.studentService.searchStudents(filters).subscribe(students =>{
            this.filteredStudents = students;
        });
    }

    onClearFilters(): void {
        this.filterForm.reset(); // Reiniciar el formulario
        this.filteredStudents = [...this.students]; // Restaurar la lista completa
    }

    deleteStudent(studentId: number): void {
        this.studentService.deleteStudent(studentId).subscribe({
            next: () => {},
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
}