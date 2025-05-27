import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TfgService } from '../../services/tfg.service';
import { TFGLineDTO } from '../../models/tfg.model';
import { TranslateModule } from '@ngx-translate/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { CommonModule, Location } from '@angular/common';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { DepartmentService } from '../../../admin/services/department.service';
import { DepartmentDTO } from '../../../admin/models/department.model';
import { CareerService } from '../../../admin/services/career.service';
import { CareerDTO } from '../../../admin/models/career.model';
import { fork } from 'child_process';
import { forkJoin } from 'rxjs';
import { RoleId } from '../../../admin/models/role.model';
import { ProfessorDTO } from '../../../professor/models/professor.model';
import { ProfessorSearchComponent } from '../../../professor/pages/profesor-search/professor-search.component';
import { ProfessorService } from '../../../professor/services/professor.service';
import { MatDialog } from '@angular/material/dialog';
import { StartTfgDialogComponent } from '../../components/start-tfg-dialog/start-tfg-dialog.component';
import { ConfigurationService } from '../../../../core/services/configuration.service';

@Component({
    selector: 'tfg-detail',
    standalone: true,
    imports: [
        TranslateModule,
        ReactiveFormsModule,
        MatCheckboxModule,
        MatFormFieldModule,
        MatInputModule,
        MatSelectModule,
        MatButtonModule,
        CommonModule],
    templateUrl: './tfg-detail.component.html',
    styleUrls: ['./tfg-detail.component.scss']
})
export class TfgDetailComponent implements OnInit {
    id: string | null = null;
    tfg: TFGLineDTO | null = null;
    creation: boolean = false;
    tfgForm!: FormGroup;
    departments: DepartmentDTO[] = [];
    careers: CareerDTO[] = [];
    professors: ProfessorDTO[] = [];
    isAdmin: boolean = false;
    isStudent: boolean = false;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private tfgService: TfgService,
        private fb: FormBuilder,
        private departmentService: DepartmentService,
        private careerService: CareerService,
        private professorService: ProfessorService,
        private dialog: MatDialog,
        private location: Location,
        private configurationService: ConfigurationService
    ) { }

    ngOnInit(): void {
        let role = this.configurationService.getRole();
        this.isAdmin = role === RoleId.Admin;
        this.isStudent = role === RoleId.Student;
        this.id = this.route.snapshot.paramMap.get('id');
        if (this.id !== "new" && isNaN(Number(this.id))) {
            this.router.navigate(['/']);
        }
        this.creation = this.id == "new";

        this.tfgForm = this.fb.group({
            id: [this.creation ? null : this.id],
            name: ['', Validators.required],
            description: ['', Validators.required],
            departmentId: ['', Validators.required],
            slots: [1, [Validators.required, Validators.min(1)]],
            group: [false],
            careers: [[]],
            professors: [[]],
        });

        if (!this.isAdmin) {
            this.tfgForm.disable();
        }

        let universityId = this.configurationService.getSelectedUniversity()!;
        if (universityId === undefined) {
            universityId = localStorage.getItem('selectedUniversity') ? parseInt(localStorage.getItem('selectedUniversity')!) : 0;
        }
        const departmentRequest = this.departmentService.getDepartmentsByUniversityId(universityId);
        const careerRequest = this.careerService.getCareers();
        const professorRequest = this.professorService.getProfessors();

        if (!this.creation) {
            const tfgRequest = this.tfgService.getTfg(+this.id!);

            forkJoin([departmentRequest, careerRequest, tfgRequest, professorRequest]).subscribe(([departments, careers, tfg, professors]) => {
                this.departments = departments;
                this.careers = careers;
                this.tfg = tfg;
                this.professors = professors;
                this.tfgForm.patchValue(tfg);
                this.tfgForm.get('departmentId')?.setValue(tfg.department?.id);
                this.tfgForm.get('careers')?.setValue(tfg.careers?.map((career) => career.id));
                this.tfgForm.get('professors')?.setValue(tfg.professors?.map((professor) => professor.id));
            });
        } else {
            forkJoin([departmentRequest, careerRequest]).subscribe(([departments, careers]) => {
                this.departments = departments;
                this.careers = careers;
            });
        }

    }

    onSubmit(): void {
        if (this.tfgForm.valid) {
            const tfgData = this.tfgForm.value;
            if (this.creation) {
                this.tfgService.createTfg(tfgData).subscribe((data) => this.location.back());
            } else {
                forkJoin([
                    this.tfgService.updateTfg(tfgData),
                    this.tfgService.addCareersToTfg(tfgData.id, tfgData.careers),
                    this.tfgService.addProfessorsToTfg(tfgData.id, tfgData.professors)
                ]).subscribe(() => this.location.back());
            }
        }
    }

    onCancel(): void {
        this.location.back();
    }

    onRequest() {
        const dialogRef = this.dialog.open(StartTfgDialogComponent, {
            data: {
                professors: this.professors.filter((professor) => this.tfgForm.get('professors')?.value.includes(professor.id)),
                tfgLineId: this.tfg?.id,
            }
        });
        
          dialogRef.afterClosed().subscribe((result) => {
            if (result) {
              this.router.navigate(['/tfg']);
            }
          });
    }
}