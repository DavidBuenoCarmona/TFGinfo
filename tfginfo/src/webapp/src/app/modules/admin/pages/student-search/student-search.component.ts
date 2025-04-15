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

@Component({
    selector: 'app-student-search',
    imports: [StudentListComponent,
        MatFormFieldModule,
        MatInputModule,
        TranslateModule,
        ReactiveFormsModule,
        MatButtonModule,
        CommonModule],
    templateUrl: './student-search.component.html',
    styleUrl: './student-search.component.scss'
})

export class StudentSearchComponent implements OnInit {
    public filterForm!: FormGroup;
    public students: StudentDTO[] = [];
    public filteredStudents: StudentDTO[] = [];

    constructor(
        public studentService: StudentService,
        private router: Router,
        private fb: FormBuilder,
        private route: ActivatedRoute
    ) { }

    ngOnInit(): void {
        this.filterForm = this.fb.group({
            filter: [''] 
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
        const filterValue = this.filterForm.get('filter')?.value.toLowerCase();
        this.filteredStudents = this.students.filter(student =>
            student.name.toLowerCase().includes(filterValue)
        );
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
}