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

namespace Scambici.EmployeeAPI.ConfirmTheftReport
{
	public abstract class ConfirmTheftReportController : Scambici.API.Controller
	{
		//Get all the theft reports which need confirmation 
		public System.Collections.Generic.List<Scambici.Domain.TheftReport> GetTheftRequests(Scambici.Domain.Store currentStore)
		{
			var pendingTheftReport = new System.Collections.Generic.List<Scambici.Domain.TheftReport>();
			//Get the current store
			var dbStore = dbContext.Stores.Where(s => s.StoreId == currentStore.StoreId).First();
			//Get all the users related to the store
			var users = dbStore.Users;
			//Iterate to get all the theft report
			foreach (Scambici.Domain.User u in users)
			{
				foreach (Scambici.Domain.TheftReport t in u.TheftReports)
				{
					//Check if not completed
					if(t.Confirmed == false)
						pendingTheftReport.Add(t);
				}
			}
			//Return the correct list of pending theft reports
			return pendingTheftReport;
		}
		//Confirm the theft report
		public void ConfirmTheftReport(Scambici.Domain.User user, Scambici.Domain.Store currentStore, int newBikeId)
		{
			//Get the specified user 
			var dbUser = dbContext.Users.Where(u => u.UserId == user.UserId).First();
			//Get his theft report
			var dbTheftReport = dbUser.TheftReports.Where(t => t.Confirmed == false).First();
			//Set the task to completed
			dbTheftReport.Confirmed = true; 
			//Provide the user with a new bike
			var newBike = dbContext.Stores.Where(s => s.StoreId == currentStore.StoreId).First().Bikes.Where(b => b.BikeId == newBikeId).First();
			dbUser.RentedBike = newBike;
			dbContext.SaveChanges();
		}
	}
}
