import { Component, OnInit } from '@angular/core';
import { UniversityListComponent } from '../../components/university-list/university-list.component';
import { UniversityService } from '../../services/university.service';
import { UniversityBase } from '../../models/university.model';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { TranslateModule } from '@ngx-translate/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-university-search',
    imports: [UniversityListComponent,
        MatFormFieldModule,
        MatInputModule,
        TranslateModule,
        ReactiveFormsModule,
        MatButtonModule,
        CommonModule],
    templateUrl: './university-search.component.html',
    styleUrl: './university-search.component.scss'
})

export class UniversitySearchComponent implements OnInit {
    public filterForm!: FormGroup;
    public universities: UniversityBase[] = [];
    public filteredUniversities: UniversityBase[] = [];

    constructor(
        public universityService: UniversityService,
        private router: Router,
        private fb: FormBuilder,
        private route: ActivatedRoute
    ) { }

    ngOnInit(): void {
        this.filterForm = this.fb.group({
            filter: [''] 
        });

        this.universityService.getUniversities().subscribe(universities => {
            this.universities = universities;
            this.filteredUniversities = [...this.universities];
        });
    }
    
    onCreate(): void {
        this.router.navigate(['new'], { relativeTo: this.route });
    }

    onSearch(): void {
        const filterValue = this.filterForm.get('filter')?.value.toLowerCase();
        this.filteredUniversities = this.universities.filter(university =>
            university.name.toLowerCase().includes(filterValue) ||
            university.addess.toLowerCase().includes(filterValue)
        );
    }

    onClearFilters(): void {
        this.filterForm.reset(); // Reiniciar el formulario
        this.filteredUniversities = [...this.universities]; // Restaurar la lista completa
    }

    deleteUniversity(universityId: number): void {
        this.universityService.deleteUniversity(universityId).subscribe({
            next: () => {},
            error: (err) => console.error(err),
            complete: () => {
                this.universities = this.universities.filter((item) => item.id !== universityId);
                this.filteredUniversities = this.filteredUniversities.filter((item) => item.id !== universityId);
            }
        });
    }
}