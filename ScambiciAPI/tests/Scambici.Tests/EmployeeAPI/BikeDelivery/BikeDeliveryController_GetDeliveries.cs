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
	public class BikeDeliveryControllerTestable : Scambici.EmployeeAPI.BikeDelivery.BikeDeliveryController 
	{
		public BikeDeliveryControllerTestable(Scambici.Domain.ScambiciContext context)
		{	dbContext = context; }
	}

	[NUnit.Framework.TestFixture]
	public class BikeDeliveryController_GetDeliveries
	{
		Scambici.Domain.ScambiciContext dbContext;
		Scambici.EmployeeAPI.BikeDelivery.BikeDeliveryController controller;
		[NUnit.Framework.SetUp]
		public void SetUp()
		{
			//Setup mock
			dbContext = new Scambici.Tests.ScambiciContextMock();
			dbContext.Database.EnsureDeleted();
			dbContext.Database.EnsureCreated();
			controller = new BikeDeliveryControllerTestable(dbContext);
			//Employee
            Scambici.Domain.Employee mazzini = new Scambici.Domain.Employee { Name = "Giuseppe", Surname = "Mazzini", EMailAddress = "mazzini@giovane.it", PasswordHash = "12345" };
			dbContext.Add(mazzini);
			dbContext.SaveChanges();
			//Store
			Scambici.Domain.Store miliani = new Scambici.Domain.Store { Address = "Via Pietro Miliani 7, Bologna", Administrator = mazzini, Employees = new System.Collections.Generic.List<Scambici.Domain.Employee>(), Users =  new System.Collections.Generic.List<Scambici.Domain.User>() };
            dbContext.Add(miliani);
			dbContext.SaveChanges();
			//User 1
            Scambici.Domain.User garibaldi = new Scambici.Domain.User { Name = "Giuseppe", Surname = "Garibaldi", Address = "Via dei Mille 1000, Nizza", 
				EMailAddress = "garibaldi@italia.it", PasswordHash = "farelitalia12345", RentedBike = null, 
				MaintenanceRequests = new System.Collections.Generic.List<Scambici.Domain.UserMaintenance>(),
				RentalInterruptions = new System.Collections.Generic.List<Scambici.Domain.RentalInterruption>(),
				TheftReports = new System.Collections.Generic.List<Scambici.Domain.TheftReport>(),
				Deliveries = new System.Collections.Generic.List<Scambici.Domain.Delivery>(),
				Store = miliani };
			dbContext.Add(garibaldi);
			dbContext.SaveChanges();
			//User 2
            Scambici.Domain.User carboni = new Scambici.Domain.User { Name = "Luca", Surname = "Carboni", Address = "Via dei Duemila 2000, Nizza", 
				EMailAddress = "Carboni@italia.it", PasswordHash = "sonobrabo1!1!", RentedBike = null, 
				MaintenanceRequests = new System.Collections.Generic.List<Scambici.Domain.UserMaintenance>(),
				RentalInterruptions = new System.Collections.Generic.List<Scambici.Domain.RentalInterruption>(),
				TheftReports = new System.Collections.Generic.List<Scambici.Domain.TheftReport>(),
				Deliveries = new System.Collections.Generic.List<Scambici.Domain.Delivery>(),
				Store = miliani };
			//Deliveries
			Scambici.Domain.Delivery delivery1 = new Scambici.Domain.Delivery { TaskId = 1, Completed = true, CreatedBy = garibaldi, CompletedBy = mazzini};
            Scambici.Domain.Delivery delivery2 = new Scambici.Domain.Delivery { TaskId = 2, Completed = false, CreatedBy = garibaldi};
            Scambici.Domain.Delivery delivery3 = new Scambici.Domain.Delivery { TaskId = 3, Completed = false, CreatedBy = carboni };
			dbContext.Add(delivery1);
			dbContext.Add(delivery2);
			dbContext.Add(delivery3);
			dbContext.SaveChanges();
		}

		[NUnit.Framework.Test]
		public void BikeDeliveryController_DBHasOperativeStore_GetDeliveries()
		{
			NUnit.Framework.Assert.That(() => controller.GetDeliveryRequests(dbContext.Stores.First()), NUnit.Framework.Throws.Nothing);
			var deliveries = controller.GetDeliveryRequests(dbContext.Stores.First());
			var carboni = dbContext.Users.Where(u => u.Surname == "Carboni").First();
			NUnit.Framework.Assert.That(deliveries.Count, NUnit.Framework.Is.EqualTo(2));
			NUnit.Framework.Assert.That(deliveries[0].TaskId, NUnit.Framework.Is.EqualTo(2));
			NUnit.Framework.Assert.That(deliveries[1].CreatedBy, NUnit.Framework.Is.EqualTo(carboni));	
		}
	}
}