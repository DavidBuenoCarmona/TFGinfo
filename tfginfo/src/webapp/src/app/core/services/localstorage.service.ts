import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class UniversitySelectionService {
    private universityIdSubject = new BehaviorSubject<number | null>(null);
    universityId$ = this.universityIdSubject.asObservable();

    setUniversityId(id: number | null) {
        localStorage.setItem('selectedUniversity', id ? id.toString() : '');
        this.universityIdSubject.next(id);
    }

    getUniversityId(): number | null {
        const id = localStorage.getItem('selectedUniversity');
        return id ? Number(id) : null;
    }
}