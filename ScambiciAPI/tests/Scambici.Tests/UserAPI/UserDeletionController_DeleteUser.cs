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
	public class UserDeletionControllerTestable : Scambici.UserAPI.UserDeletion.UserDeletionController 
	{
		public UserDeletionControllerTestable(Scambici.Domain.ScambiciContext context)
		{ dbContext = context; }
	}
	[NUnit.Framework.TestFixture]
	public class UserDeletionController_DeleteUser
	{
		Scambici.Domain.ScambiciContext dbContext;
		string garibaldiEMailAddress = "garibaldi@italia.it";
		[NUnit.Framework.SetUp]
		public void SetUp()
		{
			var bike = new Scambici.Domain.Bike();
			var miliani = new Scambici.Domain.Store { Address = "Via Pietro Miliani 7, Bologna",
				Employees = new System.Collections.Generic.List<Scambici.Domain.Employee>(), 
				Users =  new System.Collections.Generic.List<Scambici.Domain.User>(),
				Bikes =  new System.Collections.Generic.List<Scambici.Domain.Bike>(),
			};
			var mazzini = new Scambici.Domain.Employee { Name = "Giuseppe", Surname = "Mazzini", EMailAddress = "mazzini@giovane.it", PasswordHash = "12345" };
			var garibaldi = new Scambici.Domain.User { Name = "Giuseppe", Surname = "Garibaldi", Address = "Via dei Mille 1000, Nizza", 
				EMailAddress = garibaldiEMailAddress, PasswordHash = "farelitalia12345", RentedBike = bike };
			dbContext = new Scambici.Tests.ScambiciContextMock();
			dbContext.Database.EnsureDeleted();
			dbContext.Database.EnsureCreated();
			miliani.Bikes.Add(bike);
			dbContext.Add(miliani);
			dbContext.SaveChanges();
			miliani.Administrator = mazzini;
			mazzini.Store = miliani;
			mazzini.ManagedStore = miliani;
			garibaldi.Store = miliani;
			dbContext.Add(mazzini);
			dbContext.Add(garibaldi);
			dbContext.SaveChanges();
		}

		[NUnit.Framework.Test]
		public void UserDeletionController_UserIsRegistered_DeleteUser()
		{
			Scambici.Domain.User garibaldi = dbContext.Users.Where(u => u.EMailAddress == garibaldiEMailAddress).First();
			var controller = new UserDeletionControllerTestable(dbContext);
			NUnit.Framework.Assert.That(() => controller.DeleteUser(garibaldi), NUnit.Framework.Throws.Nothing);
			NUnit.Framework.Assert.That(dbContext.Users.Where(u => u.EMailAddress == garibaldiEMailAddress), NUnit.Framework.Is.Empty);
		}
	}
}
