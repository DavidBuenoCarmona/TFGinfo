import { Component, Input, OnInit } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { TranslateModule } from '@ngx-translate/core';
import { WorkingGroupBase } from '../../models/group.model';
import { ConfirmDialogComponent } from '../../../../core/layout/components/confirm-dialog/confirm-dialog.component';
import { Router, ActivatedRoute } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { GroupService } from '../../services/group-service';

@Component({
  selector: 'group-list',
  standalone: true,
  imports: [
    TranslateModule,
    MatTableModule,
    MatDialogModule,
    MatIconModule,
    MatButtonModule,
    MatCardModule
  ],
  templateUrl: './groups-list.component.html',
  styleUrls: ['./groups-list.component.scss']
})
export class GroupListComponent implements OnInit {
  @Input() groups: WorkingGroupBase[] = [];
  @Input() displayedColumns: string[] = ['name', 'description', 'isPrivate', 'actions'];

  constructor(
    private dialog: MatDialog,
    private groupService: GroupService,
    private router: Router,
    private route: ActivatedRoute
  ) {
  }

  ngOnInit(): void {
    console.log('this.groups :>> ', this.groups);
  }

  onEdit(group: WorkingGroupBase) {
    this.router.navigate([group.id], { relativeTo: this.route });
  }

  onDelete(group: WorkingGroupBase) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent);

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.groupService.deleteGroup(group.id!).subscribe({
          next: () => {},
          error: (err) => console.error(err),
          complete: () => {
            this.groups = this.groups.filter((item) => item.id !== group.id);
          }
        });
      }
    });
  }
}