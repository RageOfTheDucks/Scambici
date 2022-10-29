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

namespace Scambici.EmployeeAPI.Employees
{
	// Still abstract: a constructor that assigns a value
	// to dbContext is not defined
	public abstract class EmployeesController : Scambici.API.Controller
	{
		public Scambici.Domain.Employee GetEmployeeById(int employeeId)
		{
			return dbContext.Employees.Where(e => e.EmployeeId == employeeId).First();
		}

		public Scambici.Domain.Employee GetEmployeeByEMailAddress(string eMailAddress)
		{
			return dbContext.Employees.Where(e => e.EMailAddress == eMailAddress).First();
		}
		public System.Collections.Generic.List<Scambici.Domain.Employee> GetStoreEmployees(Scambici.Domain.Store store)
		{
			return dbContext.Employees.Where(e => e.Store.StoreId == store.StoreId || e.Store.Address == store.Address).ToList();
		}

	}
}
