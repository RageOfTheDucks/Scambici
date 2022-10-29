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

namespace Scambici.UserAPI.Stores
{
	public abstract class StoresController : Scambici.API.Controller
	{
		public System.Collections.Generic.List<Scambici.Domain.Store> GetStores()
		{
			return dbContext.Stores.ToList();
		}
		public Scambici.Domain.Store GetStoreByEmployee(Scambici.Domain.Employee employee)
		{
			foreach (var store in dbContext.Stores.ToList()) {
				if (store.Employees.Where(e => e.EmployeeId == employee.EmployeeId).Any()) {
					return store;
				}
			}
			return null;
		}
	}
}


