import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-admin-timetable',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './admin-timetable.component.html',
  styles: ``
})
export class AdminTimetableComponent implements OnInit {
  timetableForm!: FormGroup;

  // Predefined time slots, week days, activities, and staff members
  timeSlots = ['9:00 AM - 10:00 AM', '10:00 AM - 11:00 AM', '11:00 AM - 12:00 PM'];
  weekDays = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'];
  activities = ['Math Class', 'Science Lab', 'History Class'];
  staffMembers = ['Mr. A', 'Ms. B', 'Mr. C'];

  constructor(private fb: FormBuilder) {}

  ngOnInit(): void {
    this.timetableForm = this.fb.group({
      timetableName: ['', [Validators.required, Validators.maxLength(50)]],
      dateRange: this.fb.group({
        startDate: ['', Validators.required],
        endDate: ['', Validators.required]
      }),
      timeSlots: [[], Validators.required],
      weekDays: this.fb.array([]),
      assignedActivity: ['', Validators.required],
      assignedStaff: [[], Validators.required]
    });
  }

  // Add or remove days from the weekDays FormArray
  toggleWeekDay(day: string, event: Event): void {
    const isChecked = (event.target as HTMLInputElement).checked; // Cast event.target to HTMLInputElement
    const weekDaysArray = this.timetableForm.get('weekDays') as FormArray;
    if (isChecked) {
      weekDaysArray.push(this.fb.control(day));
    } else {
      const index = weekDaysArray.controls.findIndex(ctrl => ctrl.value === day);
      weekDaysArray.removeAt(index);
    }
  }

  onSave(): void {
    if (this.timetableForm.valid) {
      console.log('Timetable Data:', this.timetableForm.value);
      alert('Timetable saved successfully!');
    } else {
      alert('Please fill in all required fields!');
    }
  }

  onReset(): void {
    this.timetableForm.reset();
  }

  onCancel(): void {
    console.log('Cancel button clicked');
    alert('Action canceled');
  }
}