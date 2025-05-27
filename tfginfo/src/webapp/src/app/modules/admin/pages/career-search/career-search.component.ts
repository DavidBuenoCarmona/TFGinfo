import { Component, OnInit } from '@angular/core';
import { CareerListComponent } from '../../components/career-list/career-list.component';
import { CareerService } from '../../services/career.service';
import { CareerDTO } from '../../models/career.model';
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
    selector: 'app-career-search',
    imports: [CareerListComponent,
        MatFormFieldModule,
        MatInputModule,
        TranslateModule,
        ReactiveFormsModule,
        MatButtonModule,
        MatIconModule,
        CommonModule],
    templateUrl: './career-search.component.html',
    styleUrl: './career-search.component.scss'
})

export class CareerSearchComponent implements OnInit {
    public filterForm!: FormGroup;
    public careers: CareerDTO[] = [];
    public filteredCareers: CareerDTO[] = [];
    public showExtraFilters = false;

    constructor(
        public careerService: CareerService,
        private router: Router,
        private fb: FormBuilder,
        private route: ActivatedRoute
    ) { }

    ngOnInit(): void {
        this.filterForm = this.fb.group({
            generic: [''],
            name: [''],
            university: ['']
        });

        this.careerService.getCareers().subscribe(careers => {
            this.careers = careers;
            this.filteredCareers = [...this.careers];
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

        this.careerService.searchCarrers(filters).subscribe(careers =>{
            this.filteredCareers = careers;
        });
    }

    onClearFilters(): void {
        this.filterForm.reset();
        this.filteredCareers = [...this.careers];
    }

    deleteCareer(careerId: number): void {
        this.careerService.deleteCareer(careerId).subscribe({
            next: () => {},
            error: (err) => console.error(err),
            complete: () => {
                this.careers = this.careers.filter((item) => item.id !== careerId);
                this.filteredCareers = this.filteredCareers.filter((item) => item.id !== careerId);
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