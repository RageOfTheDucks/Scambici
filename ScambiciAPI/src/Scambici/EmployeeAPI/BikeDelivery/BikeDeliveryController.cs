// This file is part of Scambici.
// Copyright (C) 2020 Giovanni Lucia, Stefano Fantazzini and Kevin Michael Frick
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https:// www.gnu.org/licenses/>.

using System.Linq;

namespace Scambici.EmployeeAPI.BikeDelivery
{
	public abstract class BikeDeliveryController : Scambici.API.Controller
	{
		//Get all the delivery requests 
		public System.Collections.Generic.List<Scambici.Domain.Delivery> GetDeliveryRequests(Scambici.Domain.Store currentStore)
		{
			var pendingDeliveries = new System.Collections.Generic.List<Scambici.Domain.Delivery>();
			//Get the current store
			var dbStore = dbContext.Stores.Where(s => s.StoreId == currentStore.StoreId).First();
			//Get all the users related to the store
			var users = dbStore.Users;
			//Iterate to get all the deliveries
			foreach (Scambici.Domain.User u in users)
			{
				foreach (Scambici.Domain.Delivery d in u.Deliveries)
				{
					//Check if not completed
					if(d.Completed == false)
						pendingDeliveries.Add(d);
				}
			}
			//Return the correct list of pending deliveries
			return pendingDeliveries;

		}
		//Assign the specified delivery to the employee
		public bool TakeTask(Scambici.Domain.Employee employee, int deliveryId)
		{
			//Get the employee and verify that he doesn't have an actve task
			var dbEmployee = dbContext.Employees.Where(e => e.EmployeeId == employee.EmployeeId).First();
			//Check if the employee can take the task
			if(dbEmployee.CurrentTask == null)
			{
				//Assign the task to the employee
				dbEmployee.CurrentTask = dbContext.Deliveries.Where(d => d.TaskId == deliveryId).First();
				dbContext.SaveChanges();
				return true;
			}
			//The employee has an already active task
			else
			{
				return false;
			}

		}
		//Terminate the current active task of the employee
		public void TerminateTask(Scambici.Domain.Employee employee, int bikeId)
		{
			//Get the employee and verify that he has an active task
			var dbEmployee = dbContext.Employees.Where(e => e.EmployeeId == employee.EmployeeId).First();
			//Get Current Task and User
			var completedTask = (Scambici.Domain.Delivery) dbEmployee.CurrentTask;
			var dbUser = dbContext.Users.Where(u => u.UserId == completedTask.CreatedBy.UserId).First();
			//Set the task as completed
			completedTask.Completed = true;
			//Add the completed task to the list of completed task
			dbEmployee.Tasks.Add(completedTask);
			//Remove the task from the employee
			dbEmployee.CurrentTask = null;
			//Change the user bike
			var newBike = dbContext.Stores.Where(s => s.StoreId == dbUser.Store.StoreId).First().Bikes.Where(b => b.BikeId == bikeId).First();
			dbUser.RentedBike=newBike;
			dbContext.SaveChanges();
		}
	}
}

