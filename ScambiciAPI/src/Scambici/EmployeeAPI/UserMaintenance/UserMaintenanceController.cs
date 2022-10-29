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

namespace Scambici.EmployeeAPI.UserMaintenance
{
	public abstract class UserMaintenanceController : Scambici.API.Controller
	{
		//Get all the user maintanance requests
		public System.Collections.Generic.List<Scambici.Domain.UserMaintenance> GetUserMaintenanceRequests(Scambici.Domain.Store currentStore)
		{
			var pendingUserMaintenanceRequests = new System.Collections.Generic.List<Scambici.Domain.UserMaintenance>();
			//Get the current store
			var dbStore = dbContext.Stores.Where(s => s.StoreId == currentStore.StoreId).First();
			//Get all the users related to the store
			var users = dbStore.Users;
			//Iterate to get all the theft report
			foreach (Scambici.Domain.User u in users)
			{
				foreach (Scambici.Domain.UserMaintenance m in u.MaintenanceRequests)
				{
					//Check if not completed
					if(m.Completed == false)
						pendingUserMaintenanceRequests.Add(m);
				}
			}
			//Return the correct list of pending user maintenance requests
			return pendingUserMaintenanceRequests;
		}
		//Assign the specified maintenance to the employee
		public bool TakeTask(Scambici.Domain.Employee employee, int taskId)
		{
			//Get the employee and verify that he doesn't have an actve task
			var dbEmployee = dbContext.Employees.Where(e => e.EmployeeId == employee.EmployeeId).First();
			//The employe can take the task
			if(dbEmployee.CurrentTask == null)
			{
				//Assign the task to the employee
				dbEmployee.CurrentTask = dbContext.UserMaintenances.Where(d => d.TaskId == taskId).First();
				dbContext.SaveChanges();
				return true;
			}
			//The employee has a task already active
			else
			{
				return false;
			}

		}
		//Terminate the current active task of the employee
		public void TerminateTask(Scambici.Domain.Employee employee)
		{
			//Get the employee and verify that he doesn't have an actve task
			var dbEmployee = dbContext.Employees.Where(e => e.EmployeeId == employee.EmployeeId).Where(e => e.CurrentTask != null).First();
			//Set the task as completed
			var completedTask = (Scambici.Domain.UserMaintenance) dbEmployee.CurrentTask;
			completedTask.Completed = true;
			//Add the completed task to the list of completed task
			dbEmployee.Tasks.Add(completedTask);
			//Remove the task from the employee
			dbEmployee.CurrentTask = null;
			dbContext.SaveChanges();
		}
		//Replace the user bike
		public void ReplaceBike(int replaceBikeId, Scambici.Domain.User user, Scambici.Domain.Store currentStore)
		{
			//Get the specified user
			var dbUser = dbContext.Users.Where(u => u.UserId == user.UserId).First();
			//Create a warehouse maintenance
			dbContext.StorageMaintenances.Add(new Scambici.Domain.StorageMaintenance { Bike = dbUser.RentedBike });
			//Change his bike
			var newBike = dbContext.Stores.Where(s => s.StoreId == currentStore.StoreId).First().Bikes.Where(b => b.BikeId == replaceBikeId).First();
			dbUser.RentedBike = newBike;
			dbContext.SaveChanges();
		}
	}
}
