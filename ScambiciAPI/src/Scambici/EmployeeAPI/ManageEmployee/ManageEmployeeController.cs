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

namespace Scambici.EmployeeAPI.ManageEmployee
{
	public abstract class ManageEmployeeController : Scambici.API.Controller
	{
		public bool CheckIsAdmin(Scambici.Domain.Employee employee)
		{
			return dbContext.Employees.Where(e => e.EmployeeId == employee.EmployeeId || e.EMailAddress == employee.EMailAddress)
				            .Where(e => e.ManagedStore != null).Any();
		}

		//Get all the employee of a store
		public System.Collections.Generic.List<Scambici.Domain.Employee> GetStoreEmployees(Scambici.Domain.Store currentStore)
		{
			//Get the current store
			var dbStore = dbContext.Stores.Where(s => s.StoreId == currentStore.StoreId).First();
			//Get all the related employees
			return dbStore.Employees;
		}
		//Create and register the employee
		public void RegisterEmployee(string name, string surname, string eMailAddress, string passwordHash, Scambici.Domain.Store currentStore)
		{
			var newEmployee = new Scambici.Domain.Employee { Name = name, Surname = surname, EMailAddress = eMailAddress, PasswordHash = passwordHash};
			RegisterEmployee(newEmployee, currentStore);

		}
		public void RegisterEmployee(Scambici.Domain.Employee employee, Scambici.Domain.Store store)
		{
			dbContext.Employees.Add(employee);
			var employeeStore = dbContext.Stores.Where(s => s.StoreId == store.StoreId).First();
			if (employeeStore.Users == null)
			{
				employeeStore.Users = new System.Collections.Generic.List<Scambici.Domain.User>();
			}
			employeeStore.Employees.Add(employee);
			dbContext.SaveChanges();
		}
		//Delete the employee from the system
		public void DeleteEmployee(Scambici.Domain.Employee employee, Scambici.Domain.Store store)
		{
			//Remove the selected employee
			dbContext.Remove(dbContext.Employees.Where(e => e.EmployeeId == employee.EmployeeId).First());
			dbContext.SaveChanges();
		}

	}
}
