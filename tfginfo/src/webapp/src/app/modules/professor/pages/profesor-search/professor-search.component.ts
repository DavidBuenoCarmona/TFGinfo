import { Component, OnInit } from '@angular/core';
import { ProfessorService } from '../../services/professor.service';
import { ProfessorDTO } from '../../models/professor.model';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { TranslateModule } from '@ngx-translate/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { ProfessorListComponent } from '../../components/profesor-list/professor-list.component';
import { RoleId } from '../../../admin/models/role.model';
import { MatIconModule } from '@angular/material/icon';
import { Filter } from '../../../../core/core.model';

@Component({
    selector: 'app-professor-search',
    imports: [ProfessorListComponent,
        MatFormFieldModule,
        MatInputModule,
        TranslateModule,
        ReactiveFormsModule,
        MatButtonModule,
        MatIconModule,
        CommonModule],
    templateUrl: './professor-search.component.html',
    styleUrl: './professor-search.component.scss'
})

export class ProfessorSearchComponent implements OnInit {
    public filterForm!: FormGroup;
    public professors: ProfessorDTO[] = [];
    public filteredProfessors: ProfessorDTO[] = [];
    public canEdit: boolean = false;
    public showExtraFilters: boolean = false;

    constructor(
        public professorService: ProfessorService,
        private router: Router,
        private fb: FormBuilder,
        private route: ActivatedRoute
    ) { }

    ngOnInit(): void {
        let role = Number.parseInt(localStorage.getItem('role')!);
        this.canEdit = role === RoleId.Admin
        this.filterForm = this.fb.group({
            generic: [''],
            name: [''],
            surname: [''],
            email: [''],
            department: [''],
        });

        this.professorService.getProfessors().subscribe(professors => {
            this.professors = professors;
            this.filteredProfessors = [...this.professors];
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

        this.professorService.searchProfessors(filters).subscribe(professors => {
            this.filteredProfessors = professors;
        });
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

    onShowExtraFilters(): void {
        this.showExtraFilters = !this.showExtraFilters;
        if (!this.showExtraFilters) {
            this.filterForm.get('name')?.setValue('');
            this.filterForm.get('surname')?.setValue('');
            this.filterForm.get('email')?.setValue('');
            this.filterForm.get('department')?.setValue('');
        }
    }
}