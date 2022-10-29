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

namespace Scambici.Tests.EmployeeAPI
{
	public class TakeTaskControllerTestable : Scambici.EmployeeAPI.StorageMaintenance.StorageMaintenanceController
	{
		public TakeTaskControllerTestable(Scambici.Domain.ScambiciContext context)
		{	dbContext = context; }
	}

	[NUnit.Framework.TestFixture]
	public class StorageMaintenanceController_TaskTask
	{
		Scambici.Domain.ScambiciContext dbContext;
		Scambici.EmployeeAPI.StorageMaintenance.StorageMaintenanceController controller;
		[NUnit.Framework.SetUp]
		public void SetUp()
		{
			//Setup mock
			dbContext = new Scambici.Tests.ScambiciContextMock();
			dbContext.Database.EnsureDeleted();
			dbContext.Database.EnsureCreated();
			controller = new TakeTaskControllerTestable(dbContext);
			//Admin
            Scambici.Domain.Employee mazzini = new Scambici.Domain.Employee { EmployeeId = 666, Name = "Giuseppe", Surname = "Mazzini", EMailAddress = "mazzini@giovane.it", PasswordHash = "12345", Tasks = new System.Collections.Generic.List<Scambici.Domain.Task>() };
			dbContext.Add(mazzini);
			dbContext.SaveChanges();
			//Store
			Scambici.Domain.Store miliani = new Scambici.Domain.Store { StoreId = 123, Address = "Via Pietro Miliani 7, Bologna", Administrator = mazzini, Employees = new System.Collections.Generic.List<Scambici.Domain.Employee>(), Users =  new System.Collections.Generic.List<Scambici.Domain.User>() };
            dbContext.Add(miliani);
			dbContext.SaveChanges();
			//Bike
			var bike1 = new Scambici.Domain.Bike { BikeId = 5566, StoreId = 123 };
			var bike2 = new Scambici.Domain.Bike { BikeId = 7788, StoreId = 123 };
			dbContext.Add(bike1);
			dbContext.Add(bike2);
			dbContext.SaveChanges();
			//Maintenance
            Scambici.Domain.StorageMaintenance maintenance1 = new Scambici.Domain.StorageMaintenance { TaskId = 7, Completed = false, Bike = bike1 };
            Scambici.Domain.StorageMaintenance maintenance2 = new Scambici.Domain.StorageMaintenance { TaskId = 98, Completed = false, Bike = bike2 };
            dbContext.Add(maintenance1);
			dbContext.Add(maintenance2);
			dbContext.SaveChanges();
		}

		[NUnit.Framework.Test]
		public void StorageMaintenanceController_DBStoreHasStorageMaintenance_TakeMaintenance()
		{
			NUnit.Framework.Assert.That(() => controller.TakeTask(dbContext.Employees.First(), 98), NUnit.Framework.Throws.Nothing);
			var storage = controller.GetStorageMaintenances(dbContext.Stores.First());
			NUnit.Framework.Assert.That(storage.Count, NUnit.Framework.Is.EqualTo(2));
			NUnit.Framework.Assert.That(dbContext.Employees.First().CurrentTask.TaskId, NUnit.Framework.Is.EqualTo(98));
			NUnit.Framework.Assert.That(storage[1].AssignedTo, NUnit.Framework.Is.EqualTo(dbContext.Employees.First()));
		}
	}
}