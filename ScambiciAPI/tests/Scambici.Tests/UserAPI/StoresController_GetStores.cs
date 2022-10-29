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

// See ScambiciContext_CreateNewUser for explanation of this using directive
using System.Linq;

namespace Scambici.Tests.UserAPI
{
	public class StoresControllerTestable : Scambici.UserAPI.Stores.StoresController 
	{
		public StoresControllerTestable(Scambici.Domain.ScambiciContext context)
		{	dbContext = context; }
	}

	[NUnit.Framework.TestFixture]
	public class StoresController_GetStores
	{
		Scambici.Domain.ScambiciContext dbContext;
		Scambici.UserAPI.Stores.StoresController controller;
		System.Collections.Generic.List<Scambici.Domain.Store> stores;
		[NUnit.Framework.SetUp]
		public void SetUp()
		{
			dbContext = new Scambici.Tests.ScambiciContextMock();
			dbContext.Database.EnsureDeleted();
			dbContext.Database.EnsureCreated();
			var mazzini = new Scambici.Domain.Employee { Name = "Giuseppe", Surname = "Mazzini", EMailAddress = "mazzini@giovane.it", PasswordHash = "12345" };
			var zamboni = new Scambici.Domain.Employee { Name = "Luigi", Surname = "Zamboni", EMailAddress = "zamboni@unibo.it", PasswordHash = "12345" };
			var manzoni = new Scambici.Domain.Employee { Name = "Alessandro", Surname = "Manzoni", EMailAddress = "manzoni@governo.it", PasswordHash = "12345" };
			var leopardi = new Scambici.Domain.Employee { Name = "Giacomo", Surname = "leopardi", EMailAddress = "leopardi@forumdeibrutti.it", PasswordHash = "12345" };
			var noEmployees = new System.Collections.Generic.List<Scambici.Domain.Employee>();
			var noUsers = new System.Collections.Generic.List<Scambici.Domain.User>();
			stores = 
				new System.Collections.Generic.List<Scambici.Domain.Store> {
					new Scambici.Domain.Store { Address = "Via Pietro Miliani 7, Bologna", Administrator = mazzini, Employees = noEmployees, Users =  noUsers },
					new Scambici.Domain.Store { Address = "Viale del Risorgimento 2, Bologna", Administrator = zamboni, Employees = noEmployees, Users =  noUsers },
					new Scambici.Domain.Store { Address = "Via San Damiano 20, Milano", Administrator = manzoni, Employees = noEmployees, Users =  noUsers },
					new Scambici.Domain.Store { Address = "Via dei Mille 107, Nizza", Administrator = leopardi, Employees = noEmployees, Users =  noUsers }
				};
			foreach (var store in stores)
			{
				dbContext.Add(store);
			}
			dbContext.SaveChanges();
			controller = new StoresControllerTestable(dbContext);
		}

		[NUnit.Framework.Test]
		public void StoresController_DBHasStores_GetAllStores()
		{
			var storesQuery = controller.GetStores();
			NUnit.Framework.Assert.That(storesQuery, NUnit.Framework.Is.EqualTo(stores));
		}
	}
}
