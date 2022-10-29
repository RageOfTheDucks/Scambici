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

namespace Scambici.Tests.API
{
	public class ControllerTestable : Scambici.API.Controller 
	{
		public ControllerTestable(Scambici.Domain.ScambiciContext context, Scambici.Domain.User user, string timestamp, string signature)
		{ 
			dbContext = context;
			if (!CheckSignature(user, timestamp, signature))
			{
				throw new System.Security.Authentication.AuthenticationException("HMAC signature mismatch or invalid timestamp given.");
			}

		}
	}

	[NUnit.Framework.TestFixture]
	public class Controller_CheckSignature
	{
		Scambici.Domain.ScambiciContext dbContext;
		Scambici.Domain.User garibaldi;
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
			garibaldi = new Scambici.Domain.User { Name = "Giuseppe", Surname = "Garibaldi", Address = "Via dei Mille 1000, Nizza",
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
		public void Controller_ValidSignatureIsGiven_Authorize()
		{
			byte[] passHash = System.Text.Encoding.ASCII.GetBytes(garibaldi.PasswordHash);
			using (var hmac = new System.Security.Cryptography.HMACSHA512(passHash))
			{
				var rand = new System.Random();
				string timestamp = ((int)(System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds + rand.Next(200)).ToString();
				byte[] timestampBytes = System.Text.Encoding.ASCII.GetBytes(timestamp);
				string signature = hmac.ComputeHash(timestampBytes).Aggregate("", (s, e) => s + System.String.Format("{0:x2}",e), s => s );
				NUnit.Framework.Assert.That(() => new ControllerTestable(dbContext, garibaldi, timestamp, signature), 
						NUnit.Framework.Throws.Nothing);
			}
		}
		[NUnit.Framework.Test]
		public void Controller_OldTimestampIsGiven_Throw()
		{
			byte[] passHash = System.Text.Encoding.ASCII.GetBytes(garibaldi.PasswordHash);
			using (var hmac = new System.Security.Cryptography.HMACSHA512(passHash))
			{
				string timestamp = "10000";
				byte[] timestampBytes = System.Text.Encoding.ASCII.GetBytes(timestamp);
				string signature = hmac.ComputeHash(timestampBytes).Aggregate("", (s, e) => s + System.String.Format("{0:x2}",e), s => s );
				NUnit.Framework.Assert.That(() => new ControllerTestable(dbContext, garibaldi, timestamp, signature), 
						NUnit.Framework.Throws.TypeOf<System.Security.Authentication.AuthenticationException>());
			}
		}
		[NUnit.Framework.Test]
		public void Controller_WrongSignatureIsGiven_Throw()
		{
			byte[] passHash = System.Text.Encoding.ASCII.GetBytes(garibaldi.PasswordHash);
			using (var hmac = new System.Security.Cryptography.HMACSHA512(passHash))
			{
				string timestamp = ((int)(System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds).ToString();
				string signature = "WrongSignature";
				NUnit.Framework.Assert.That(() => new ControllerTestable(dbContext, garibaldi, timestamp, signature), 
						NUnit.Framework.Throws.TypeOf<System.Security.Authentication.AuthenticationException>());
			}
		}
	}
}
