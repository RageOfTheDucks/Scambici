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
	public class UsersControllerTestable : Scambici.UserAPI.Users.UsersController 
	{
		public UsersControllerTestable(Scambici.Domain.ScambiciContext context)
		{	dbContext = context; }
	}

	[NUnit.Framework.TestFixture]
	public class UsersController_CreateNewUser
	{
		Scambici.Domain.ScambiciContext dbContext;
		Scambici.UserAPI.Users.UsersController controller;
		[NUnit.Framework.SetUp]
		public void SetUp()
		{
			dbContext = new Scambici.Tests.ScambiciContextMock();
			dbContext.Database.EnsureDeleted();
			dbContext.Database.EnsureCreated();
			controller = new UsersControllerTestable(dbContext);
			Scambici.Domain.Employee mazzini = new Scambici.Domain.Employee { Name = "Giuseppe", Surname = "Mazzini", EMailAddress = "mazzini@giovane.it", PasswordHash = "12345" };
			dbContext.Add(mazzini);
			dbContext.SaveChanges();
			Scambici.Domain.Store miliani = new Scambici.Domain.Store { Address = "Via Pietro Miliani 7, Bologna", Administrator = mazzini, Employees = new System.Collections.Generic.List<Scambici.Domain.Employee>(), Users =  new System.Collections.Generic.List<Scambici.Domain.User>() };
			miliani.Employees.Add(mazzini);
			dbContext.Add(miliani);
			dbContext.SaveChanges();
		}

		[NUnit.Framework.Test]
		public void UsersController_DBHasNoUsers_AddUser()
		{
			NUnit.Framework.Assert.That(() => controller.RegisterUser("Giuseppe", "Garibaldi", "Via dei Mille 1000, Nizza", "garibaldi@italia.it", "farelitalia12345", dbContext.Stores.First()), NUnit.Framework.Throws.Nothing);
			var user = dbContext.Users.OrderBy(i => i.UserId).First();
			NUnit.Framework.Assert.That(user.Name, NUnit.Framework.Is.EqualTo("Giuseppe"));
			NUnit.Framework.Assert.That(user.Surname, NUnit.Framework.Is.EqualTo("Garibaldi"));
			NUnit.Framework.Assert.That(user.Address, NUnit.Framework.Is.EqualTo("Via dei Mille 1000, Nizza"));
			NUnit.Framework.Assert.That(user.EMailAddress, NUnit.Framework.Is.EqualTo("garibaldi@italia.it"));
			NUnit.Framework.Assert.That(user.PasswordHash, NUnit.Framework.Is.EqualTo("farelitalia12345"));
			NUnit.Framework.Assert.That(dbContext.Stores.First().Users.First(), NUnit.Framework.Is.EqualTo(user));
			NUnit.Framework.Assert.That(controller.GetUserById(1) == dbContext.Users.First());
		}

	}
}
