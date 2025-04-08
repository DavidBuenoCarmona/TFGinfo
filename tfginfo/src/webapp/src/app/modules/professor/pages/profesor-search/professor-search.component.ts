import { Component, OnInit } from '@angular/core';
import { ProfessorService } from '../../services/professor.service';
import { ProfessorDTO } from '../../models/professor.model';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { TranslateModule } from '@ngx-translate/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { ProfessorListComponent } from '../../components/profesor-list/professor-list.component';

@Component({
    selector: 'app-professor-search',
    imports: [ProfessorListComponent,
        MatFormFieldModule,
        MatInputModule,
        TranslateModule,
        ReactiveFormsModule,
        MatButtonModule,
        CommonModule],
    templateUrl: './professor-search.component.html',
    styleUrl: './professor-search.component.scss'
})

export class ProfessorSearchComponent implements OnInit {
    public filterForm!: FormGroup;
    public professors: ProfessorDTO[] = [];
    public filteredProfessors: ProfessorDTO[] = [];

    constructor(
        public professorService: ProfessorService,
        private router: Router,
        private fb: FormBuilder
    ) { }

    ngOnInit(): void {
        this.filterForm = this.fb.group({
            filter: [''] 
        });

        this.professorService.getProfessors().subscribe(professors => {
            this.professors = professors;
            this.filteredProfessors = [...this.professors];
        });
    }
    
    onCreate(): void {
        this.router.navigate(['/professor/new']);
    }

    onSearch(): void {
        const filterValue = this.filterForm.get('filter')?.value.toLowerCase();
        this.filteredProfessors = this.professors.filter(professor =>
            professor.name.toLowerCase().includes(filterValue) ||
            professor.surname.toLowerCase().includes(filterValue) ||
            professor.email.toLowerCase().includes(filterValue)
        );
    }

    onClearFilters(): void {
        this.filterForm.reset(); // Reiniciar el formulario
        this.filteredProfessors = [...this.professors]; // Restaurar la lista completa
    }

    deleteProfessor(id: number): void {
        this.professorService.deleteProfessor(id).subscribe({
            next: () => {},
            error: (err) => console.error(err),
            complete: () => {
                this.professors = this.professors.filter(professor => professor.id !== id);
                this.filteredProfessors = this.filteredProfessors.filter(professor => professor.id !== id);
            }
          });
       
    }
}