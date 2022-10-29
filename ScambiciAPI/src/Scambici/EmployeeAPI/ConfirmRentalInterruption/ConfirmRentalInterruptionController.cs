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

namespace Scambici.EmployeeAPI.ConfirmRentalInterruption
{
	public abstract class ConfirmRentalInterruptionController : Scambici.API.Controller
	{
		//Get all the rental interruption requests 
		public System.Collections.Generic.List<Scambici.Domain.RentalInterruption> GetRentalInterruptionRequests(Scambici.Domain.Store currentStore)
		{
			var pendingRentalInterruption = new System.Collections.Generic.List<Scambici.Domain.RentalInterruption>();
			//Get the current store
			var dbStore = dbContext.Stores.Where(s => s.StoreId == currentStore.StoreId).First();
			//Get all the users related to the store
			var users = dbStore.Users;
			//Iterate to get all the rental interruption
			foreach (Scambici.Domain.User u in users)
			{
				foreach (Scambici.Domain.RentalInterruption i in u.RentalInterruptions)
				{
					//Check if not completed
					if(i.Confirmed == false)
						pendingRentalInterruption.Add(i);
				}
			}
			//Return the correct list of pending rental interruption
			return pendingRentalInterruption;
		}
		//Confirm the rental interruption
		public void ConfirmRentalInterruption(int rentalInterruptionId)
		{
			//Get the specified rental interruption
			var dbRentalInterruption = dbContext.RentalInterruptions.Where(i => i.RentalInterruptionId == rentalInterruptionId).First();
			//Update the user
			var user = dbContext.Users.Where(u => u.UserId == dbRentalInterruption.CreatedBy.UserId).First();
			user.RentedBike = null;
			user.RentedBikeId = null;
			//Set the task to completed
			dbRentalInterruption.Confirmed = true;
			dbContext.SaveChanges();
		}
	}
}
