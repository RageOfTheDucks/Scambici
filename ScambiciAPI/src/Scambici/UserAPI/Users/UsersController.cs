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

namespace Scambici.UserAPI.Users
{
	// Still abstract: a constructor that assigns a value
	// to dbContext is not defined
	public abstract class UsersController : Scambici.API.Controller
	{
		public void RegisterUser(Scambici.Domain.User user, Scambici.Domain.Store store)
		{
			dbContext.Users.Add(user);
			var userStore = dbContext.Stores.Where(s => s.StoreId == store.StoreId).First();
			if (userStore.Users == null)
			{
				userStore.Users = new System.Collections.Generic.List<Scambici.Domain.User>();
			}
			userStore.Users.Add(user);
			dbContext.SaveChanges();
		}

		public void RegisterUser(string name, string surname, string address, string eMailAddress, string passwordHash, Scambici.Domain.Store store)
		{
			var newUser = new Scambici.Domain.User { Name = name, Surname = surname, Address = address, EMailAddress = eMailAddress, PasswordHash = passwordHash};
			RegisterUser(newUser, store);
		}
		public Scambici.Domain.User GetUserById(int userId)
		{
			return dbContext.Users.Where(u => u.UserId == userId).First();
		}

		public Scambici.Domain.User GetUserByEMailAddress(string eMailAddress)
		{
			return dbContext.Users.Where(u => u.EMailAddress == eMailAddress).First();
		}
	}
}


