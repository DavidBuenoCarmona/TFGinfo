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
import { MatIconModule } from '@angular/material/icon';
import { Filter } from '../../../../core/core.model';

@Component({
    selector: 'app-university-search',
    imports: [UniversityListComponent,
        MatFormFieldModule,
        MatInputModule,
        TranslateModule,
        MatIconModule,
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
    public showExtraFilters = false;

    constructor(
        public universityService: UniversityService,
        private router: Router,
        private fb: FormBuilder,
        private route: ActivatedRoute
    ) { }

    ngOnInit(): void {
        this.filterForm = this.fb.group({
            generic: [''],
            name: [''],
            address: ['']
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
        const formValues = this.filterForm.value;
        let filters: Filter[] = [];

        Object.keys(formValues).forEach(key => {
            const value = formValues[key];
            if (value !== null && value !== undefined && value !== '') {
                filters.push({ key, value });
            }
        });

        this.universityService.searchUniversities(filters).subscribe(universities => {
            this.filteredUniversities = universities;
        });
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

        onShowExtraFilters(): void {
        this.showExtraFilters = !this.showExtraFilters;
        if (!this.showExtraFilters) {
            this.filterForm.get('address')?.setValue(''); // Limpiar el campo de universidad
            this.filterForm.get('name')?.setValue(''); // Limpiar el campo de nombre
        }
    }
}