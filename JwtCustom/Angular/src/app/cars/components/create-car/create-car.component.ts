import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CarModel } from '../../models/car.models';
import { CarsService } from '../../services/cars.service';

@Component({
  selector: 'app-create-car',
  templateUrl: './create-car.component.html',
  styleUrls: ['./create-car.component.css']
})
export class CreateCarComponent implements OnInit {

  carForm = this.fb.group({
    name: ['', Validators.required]
  });
  constructor(private fb: FormBuilder, private router: Router, private carsService: CarsService) { }

  ngOnInit(): void {
  }

  onSubmit(model: CarModel) {
    this.carsService.create(model).subscribe(result =>
    {
      this.router.navigate(['/dashboard']);
    })
  }
}
