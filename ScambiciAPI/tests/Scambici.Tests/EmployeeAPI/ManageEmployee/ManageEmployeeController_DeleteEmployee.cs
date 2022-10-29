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
	public class DeleteEmployeeTestable : Scambici.EmployeeAPI.ManageEmployee.ManageEmployeeController
	{
		public DeleteEmployeeTestable(Scambici.Domain.ScambiciContext context)
		{	dbContext = context; }
	}

	[NUnit.Framework.TestFixture]
	public class ManageEmployeeController_DeleteEmployee
	{
		Scambici.Domain.ScambiciContext dbContext;
		Scambici.EmployeeAPI.ManageEmployee.ManageEmployeeController controller;
		[NUnit.Framework.SetUp]
		public void SetUp()
		{
			//Setup mock
			dbContext = new Scambici.Tests.ScambiciContextMock();
			dbContext.Database.EnsureDeleted();
			dbContext.Database.EnsureCreated();
			controller = new DeleteEmployeeTestable(dbContext);
			//Admin
            Scambici.Domain.Employee mazzini = new Scambici.Domain.Employee { Name = "Giuseppe", Surname = "Mazzini", EMailAddress = "mazzini@giovane.it", PasswordHash = "12345", Tasks = new System.Collections.Generic.List<Scambici.Domain.Task>() };
			dbContext.Add(mazzini);
			dbContext.SaveChanges();
			//Store
			Scambici.Domain.Store miliani = new Scambici.Domain.Store { StoreId = 123, Address = "Via Pietro Miliani 7, Bologna", Administrator = mazzini, Employees = new System.Collections.Generic.List<Scambici.Domain.Employee>(), Users =  new System.Collections.Generic.List<Scambici.Domain.User>() };
            dbContext.Add(miliani);
			dbContext.SaveChanges();
			//Employees
			Scambici.Domain.Employee pascal = new Scambici.Domain.Employee { Name = "Mattia", Surname = "Pascal", EMailAddress = "mattia@giovane.it", PasswordHash = "67890", Store =  miliani};
            Scambici.Domain.Employee cosini = new Scambici.Domain.Employee { Name = "Zeno", Surname = "Cosini", EMailAddress = "zeno@giovane.it", PasswordHash = "09876", Store =  miliani };		
			dbContext.Add(pascal);
			dbContext.Add(cosini);
			dbContext.SaveChanges();
		}

		[NUnit.Framework.Test]
		public void ManageEmployeeController_DBStoreHasEmployees_DeleteEmployee()
		{
			NUnit.Framework.Assert.That(dbContext.Stores.First().Employees.Count(), NUnit.Framework.Is.EqualTo(2));
			NUnit.Framework.Assert.That(() => controller.DeleteEmployee(dbContext.Stores.First().Employees[0], dbContext.Stores.First()), NUnit.Framework.Throws.Nothing);
			NUnit.Framework.Assert.That(dbContext.Stores.First().Employees.Count(), NUnit.Framework.Is.EqualTo(1));
			NUnit.Framework.Assert.That(dbContext.Stores.First().Employees[0].Surname, NUnit.Framework.Is.EqualTo("Cosini"));
		}
	}
}