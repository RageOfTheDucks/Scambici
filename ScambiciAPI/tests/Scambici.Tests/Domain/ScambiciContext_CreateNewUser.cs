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

// This using directive is needed to operate on databases
// LINQ methods will not resolve otherwise
// And this, kids, is a symptom of a deep problem in C#
using System.Linq;

namespace Scambici.Tests.Domain
{

	[NUnit.Framework.TestFixture]
	public class ScambiciContext_CreateNewUser
	{
		Scambici.Domain.Employee mazzini;
		Scambici.Domain.Store miliani;
		Scambici.Domain.ScambiciContext ctx;

		[NUnit.Framework.SetUp]
		public void SetUp()
		{
			mazzini = new Scambici.Domain.Employee { Name = "Giuseppe", Surname = "Mazzini", EMailAddress = "mazzini@giovane.it", PasswordHash = "12345"};
			miliani = new Scambici.Domain.Store { Address = "Via Pietro Miliani 7, Bologna", Administrator = mazzini,
				Employees = new System.Collections.Generic.List<Scambici.Domain.Employee>(), Users =  new System.Collections.Generic.List<Scambici.Domain.User>() };
			ctx = new Scambici.Tests.ScambiciContextMock();
			ctx.Database.EnsureDeleted();
			ctx.Database.EnsureCreated();
			ctx.Add(mazzini);
			ctx.Add(miliani);
		}

		[NUnit.Framework.Test]
		public void ScambiciContext_IsEmpty_AddUser()
		{

			ctx.Add(new Scambici.Domain.User { Name = "Giuseppe", Surname = "Garibaldi", Address = "Via dei Mille 1000, Nizza", EMailAddress = "garibaldi@italia.it", PasswordHash = "12345", Store = miliani});
			ctx.SaveChanges();
			var user = ctx.Users.OrderBy(i => i.UserId).First();
			NUnit.Framework.Assert.That(user.Name, NUnit.Framework.Is.EqualTo("Giuseppe"));
			NUnit.Framework.Assert.That(user.Surname, NUnit.Framework.Is.EqualTo("Garibaldi"));
			NUnit.Framework.Assert.That(user.Address, NUnit.Framework.Is.EqualTo("Via dei Mille 1000, Nizza"));
			NUnit.Framework.Assert.That(user.EMailAddress, NUnit.Framework.Is.EqualTo("garibaldi@italia.it"));
		}
		[NUnit.Framework.Test]
		public void ScambiciContext_IsEmpty_FailsAddingDuplicateEmail()
		{
			using (var ctx = new ScambiciContextMock())
			{
				ctx.Add(new Scambici.Domain.User { Name = "Giuseppe", Surname = "Garibaldi", Address = "Via dei Mille 1000, Nizza", EMailAddress = "garibaldi@italia.it", PasswordHash = "12345", Store = miliani});
				ctx.SaveChanges();
				var user = ctx.Users.OrderBy(i => i.UserId).First();
				NUnit.Framework.Assert.That(user.Name, NUnit.Framework.Is.EqualTo("Giuseppe"));
				NUnit.Framework.Assert.That(user.Surname, NUnit.Framework.Is.EqualTo("Garibaldi"));
				NUnit.Framework.Assert.That(user.Address, NUnit.Framework.Is.EqualTo("Via dei Mille 1000, Nizza"));
				NUnit.Framework.Assert.That(user.EMailAddress, NUnit.Framework.Is.EqualTo("garibaldi@italia.it"));
				ctx.Add(new Scambici.Domain.User { Name = "GiuseppeFalso", Surname = "GaribaldiFalso", Address = "Via dei Falsi 1000, Nizza", EMailAddress = "garibaldi@italia.it", PasswordHash = "12345", Store = miliani});
				NUnit.Framework.Assert.That(ctx.SaveChanges, NUnit.Framework.Throws.TypeOf<Microsoft.EntityFrameworkCore.DbUpdateException>());
			}
		}
	}
}
