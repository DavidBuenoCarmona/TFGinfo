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
import { ConfigurationService } from '../../../../core/services/configuration.service';
import { MatDialog } from '@angular/material/dialog';
import { ImportDialogComponent } from '../../../../core/layout/components/import-dialog/import-dialog.component';
import { ImportSummaryComponent } from '../../../../core/layout/components/import-summary/import-summary.component';

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
    public filters: Filter[] = [];

    constructor(
        public professorService: ProfessorService,
        private router: Router,
        private fb: FormBuilder,
        private route: ActivatedRoute,
        private configurationService: ConfigurationService,
        private dialog: MatDialog
    ) { }

    ngOnInit(): void {
        let role = this.configurationService.getRole();
        this.canEdit = role === RoleId.Admin
        this.filterForm = this.fb.group({
            generic: [''],
            name: [''],
            surname: [''],
            email: [''],
            department: [''],
        });

        var selectedUniversities = this.configurationService.getSelectedUniversities();
        if (!selectedUniversities || selectedUniversities.length === 0) {
            selectedUniversities = localStorage.getItem('selectedUniversity') ? [parseInt(localStorage.getItem('selectedUniversity')!)] : [0];
            this.filters.push({ key: 'university', value: selectedUniversities.map(id => id.toString()).join(',') });
        } else {
            this.filters.push({ key: 'universities', value: selectedUniversities.map(id => id.toString()).join(',') });
        }

        this.professorService.searchProfessors(this.filters).subscribe(professors => {
            this.professors = professors;
            this.filteredProfessors = [...this.professors]; // Inicializar con todos los profesores
        });
    }
    
    onCreate(): void {
        this.router.navigate(['new'], { relativeTo: this.route });
    }

    onSearch(): void {
        const formValues = this.filterForm.value;
        Object.keys(formValues).forEach(key => {
            this.filters = this.filters.filter(filter => filter.key !== key);
        });

        Object.keys(formValues).forEach(key => {
            const value = formValues[key];
            if (value !== null && value !== undefined && value !== '') {
                this.filters.push({ key, value });
            }
        });

        this.professorService.searchProfessors(this.filters).subscribe(professors => {
            this.filteredProfessors = professors;
        });
    }

    onClearFilters(): void {
        this.filterForm.reset(); // Reiniciar el formulario
        this.filteredProfessors = [...this.professors]; // Restaurar la lista completa

        
        const formValues = this.filterForm.value;
        Object.keys(formValues).forEach(key => {
            this.filters = this.filters.filter(filter => filter.key !== key);
        });

        this.professorService.searchProfessors(this.filters).subscribe(professors => {
            this.filteredProfessors = professors;
        });
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

    openImportDialog(): void {
            const format = "PROFESSOR.CSV_FORMAT";
            const dialogRef = this.dialog.open(ImportDialogComponent, { data: { format: format } });
            dialogRef.afterClosed().subscribe((result: File) => {
                if (result) {
                    const reader = new FileReader();
                    reader.onload = () => {
                        const base64 = (reader.result as string).split(',')[1]; // Solo la parte base64
                        this.professorService.importFromCSV(base64).subscribe({
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