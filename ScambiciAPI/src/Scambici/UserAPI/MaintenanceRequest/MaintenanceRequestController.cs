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

namespace Scambici.UserAPI.MaintenanceRequest
{
	public abstract class MaintenanceRequestController : Scambici.API.Controller
	{
		public void RequestMaintenance(Scambici.Domain.User user, string faultDescription)
		{
			var dbUser = dbContext.Users.Where(u => (u.EMailAddress == user.EMailAddress || u.UserId == user.UserId)).Where(u => u.RentedBike != null).First();
			if(dbUser.MaintenanceRequests == null)
			{
				dbUser.MaintenanceRequests = new System.Collections.Generic.List<Scambici.Domain.UserMaintenance>();
			}
			dbUser.MaintenanceRequests.Add(new Scambici.Domain.UserMaintenance { FaultDescription = faultDescription });
			dbContext.SaveChanges();
		}
	}
}


