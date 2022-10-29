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
	public class GetUserMaintenanceTestable : Scambici.EmployeeAPI.UserMaintenance.UserMaintenanceController
	{
		public GetUserMaintenanceTestable(Scambici.Domain.ScambiciContext context)
		{	dbContext = context; }
	}

	[NUnit.Framework.TestFixture]
	public class UserMaintenanceController_GetMaintenances
	{
		Scambici.Domain.ScambiciContext dbContext;
		Scambici.EmployeeAPI.UserMaintenance.UserMaintenanceController controller;
		[NUnit.Framework.SetUp]
		public void SetUp()
		{
			//Setup mock
			dbContext = new Scambici.Tests.ScambiciContextMock();
			dbContext.Database.EnsureDeleted();
			dbContext.Database.EnsureCreated();
			controller = new GetUserMaintenanceTestable(dbContext);
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
			dbContext.Add(bike1);
			dbContext.SaveChanges();
			//User 1
            Scambici.Domain.User garibaldi = new Scambici.Domain.User { Name = "Giuseppe", Surname = "Garibaldi", Address = "Via dei Mille 1000, Nizza", 
				EMailAddress = "garibaldi@italia.it", PasswordHash = "farelitalia12345", RentedBike = bike1, 
				MaintenanceRequests = new System.Collections.Generic.List<Scambici.Domain.UserMaintenance>(),
				RentalInterruptions = new System.Collections.Generic.List<Scambici.Domain.RentalInterruption>(),
				TheftReports = new System.Collections.Generic.List<Scambici.Domain.TheftReport>(),
				Deliveries = new System.Collections.Generic.List<Scambici.Domain.Delivery>(),
				Store = miliani };
			dbContext.Add(garibaldi);
			dbContext.SaveChanges();
			//Setup maintenance
			Scambici.Domain.UserMaintenance maintenance = new Scambici.Domain.UserMaintenance { TaskId = 7, Completed = false, CreatedBy = garibaldi };
			dbContext.Add(maintenance);
			dbContext.SaveChanges();			
		}

		[NUnit.Framework.Test]
		public void UserMaintenanceController_DBUserHasMaintenance_GetMaintenance()
		{
			NUnit.Framework.Assert.That(() => controller.GetUserMaintenanceRequests(dbContext.Stores.First()), NUnit.Framework.Throws.Nothing);
			var maintenance = controller.GetUserMaintenanceRequests(dbContext.Stores.First());
			NUnit.Framework.Assert.That(maintenance.Count, NUnit.Framework.Is.EqualTo(1));
			NUnit.Framework.Assert.That(maintenance[0].TaskId, NUnit.Framework.Is.EqualTo(7));
			NUnit.Framework.Assert.That(dbContext.Stores.First().Users.First().MaintenanceRequests.Count, NUnit.Framework.Is.EqualTo(1));
			NUnit.Framework.Assert.That(dbContext.Users.First().MaintenanceRequests.First(), NUnit.Framework.Is.EqualTo(maintenance[0]));
		}
	}
}
