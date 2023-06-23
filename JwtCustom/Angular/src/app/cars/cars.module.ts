import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreateCarComponent } from './components/create-car/create-car.component';
import { ReactiveFormsModule } from '@angular/forms';



@NgModule({
  declarations: [
    CreateCarComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule
  ]
})
export class CarsModule { }
