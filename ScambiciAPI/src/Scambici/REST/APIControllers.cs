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
namespace Scambici.REST
{
	internal class StoresControllerAzure : Scambici.UserAPI.Stores.StoresController
	{
		public StoresControllerAzure()
		{
			dbContext = new Scambici.Domain.ScambiciContextAzure();
		}
	}
	internal class UsersControllerAzure : Scambici.UserAPI.Users.UsersController
	{
		public UsersControllerAzure()
		{
			dbContext = new Scambici.Domain.ScambiciContextAzure();
		}
		public UsersControllerAzure(Scambici.Domain.User user, string timestamp, string signature)
		{
			dbContext = new Scambici.Domain.ScambiciContextAzure();
			if (!CheckSignature(user, timestamp, signature))
			{
				throw new System.Security.Authentication.AuthenticationException("HMAC signature mismatch.");
			}
		}
	}
	internal class UserDeletionControllerAzure : Scambici.UserAPI.UserDeletion.UserDeletionController
	{
		public UserDeletionControllerAzure()
		{
			dbContext = new Scambici.Domain.ScambiciContextAzure();
		}
		public UserDeletionControllerAzure(Scambici.Domain.User user, string timestamp, string signature)
		{
			{
				dbContext = new Scambici.Domain.ScambiciContextAzure();
				if (!CheckSignature(user, timestamp, signature))
				{
					throw new System.Security.Authentication.AuthenticationException("HMAC signature mismatch.");
				}
			}
		}
	}
	internal class MaintenanceRequestControllerAzure : Scambici.UserAPI.MaintenanceRequest.MaintenanceRequestController
	{
		public MaintenanceRequestControllerAzure()
		{
			dbContext = new Scambici.Domain.ScambiciContextAzure();
		}
		public MaintenanceRequestControllerAzure(Scambici.Domain.User user, string timestamp, string signature)
		{
			{
				dbContext = new Scambici.Domain.ScambiciContextAzure();
				if (!CheckSignature(user, timestamp, signature))
				{
					throw new System.Security.Authentication.AuthenticationException("HMAC signature mismatch.");
				}
			}
		}
	}
	internal class TheftReportControllerAzure : Scambici.UserAPI.TheftReport.TheftReportController
	{
		public TheftReportControllerAzure()
		{
			dbContext = new Scambici.Domain.ScambiciContextAzure();
		}
		public TheftReportControllerAzure(Scambici.Domain.User user, string timestamp, string signature)
		{
			{
				dbContext = new Scambici.Domain.ScambiciContextAzure();
				if (!CheckSignature(user, timestamp, signature))
				{
					throw new System.Security.Authentication.AuthenticationException("HMAC signature mismatch.");
				}
			}
		}
	}
	internal class NewBikeRequestControllerAzure : Scambici.UserAPI.NewBikeRequest.NewBikeRequestController
	{
		public NewBikeRequestControllerAzure()
		{
			dbContext = new Scambici.Domain.ScambiciContextAzure();
		}
		public NewBikeRequestControllerAzure(Scambici.Domain.User user, string timestamp, string signature)
		{
			{
				dbContext = new Scambici.Domain.ScambiciContextAzure();
				if (!CheckSignature(user, timestamp, signature))
				{
					throw new System.Security.Authentication.AuthenticationException("HMAC signature mismatch.");
				}
			}
		}
	}
	internal class RentalInterruptionControllerAzure : Scambici.UserAPI.RentalInterruption.RentalInterruptionController
	{
		public RentalInterruptionControllerAzure()
		{
			dbContext = new Scambici.Domain.ScambiciContextAzure();
		}
		public RentalInterruptionControllerAzure(Scambici.Domain.User user, string timestamp, string signature)
		{
			{
				dbContext = new Scambici.Domain.ScambiciContextAzure();
				if (!CheckSignature(user, timestamp, signature))
				{
					throw new System.Security.Authentication.AuthenticationException("HMAC signature mismatch.");
				}
			}
		}
	}
	internal class EmployeesControllerAzure : Scambici.EmployeeAPI.Employees.EmployeesController
	{
		public EmployeesControllerAzure()
		{
			dbContext = new Scambici.Domain.ScambiciContextAzure();
		}
		public EmployeesControllerAzure(Scambici.Domain.Employee employee, string timestamp, string signature)
		{
			{
				dbContext = new Scambici.Domain.ScambiciContextAzure();
				if (!CheckSignature(employee, timestamp, signature))
				{
					throw new System.Security.Authentication.AuthenticationException("HMAC signature mismatch.");
				}
			}
		}
	}
	internal class BikeDeliveryControllerAzure : Scambici.EmployeeAPI.BikeDelivery.BikeDeliveryController
	{
		public BikeDeliveryControllerAzure()
		{
			dbContext = new Scambici.Domain.ScambiciContextAzure();
		}
		public BikeDeliveryControllerAzure(Scambici.Domain.Employee employee, string timestamp, string signature)
		{
			{
				dbContext = new Scambici.Domain.ScambiciContextAzure();
				if (!CheckSignature(employee, timestamp, signature))
				{
					throw new System.Security.Authentication.AuthenticationException("HMAC signature mismatch.");
				}
			}
		}
	}
	internal class UserMaintenanceControllerAzure : Scambici.EmployeeAPI.UserMaintenance.UserMaintenanceController
	{
		public UserMaintenanceControllerAzure()
		{
			dbContext = new Scambici.Domain.ScambiciContextAzure();
		}
		public UserMaintenanceControllerAzure(Scambici.Domain.Employee employee, string timestamp, string signature)
		{
			dbContext = new Scambici.Domain.ScambiciContextAzure();
			if (!CheckSignature(employee, timestamp, signature))
			{
				throw new System.Security.Authentication.AuthenticationException("HMAC signature mismatch.");
			}
		}
	}
	internal class StorageMaintenanceControllerAzure : Scambici.EmployeeAPI.StorageMaintenance.StorageMaintenanceController
	{
		public StorageMaintenanceControllerAzure()
		{
			dbContext = new Scambici.Domain.ScambiciContextAzure();
		}
		public StorageMaintenanceControllerAzure(Scambici.Domain.Employee employee, string timestamp, string signature)
		{
			dbContext = new Scambici.Domain.ScambiciContextAzure();
			if (!CheckSignature(employee, timestamp, signature))
			{
				throw new System.Security.Authentication.AuthenticationException("HMAC signature mismatch.");
			}
		}
	}
	internal class ConfirmRentalInterruptionControllerAzure : Scambici.EmployeeAPI.ConfirmRentalInterruption.ConfirmRentalInterruptionController
	{
		public ConfirmRentalInterruptionControllerAzure()
		{
			dbContext = new Scambici.Domain.ScambiciContextAzure();
		}
		public ConfirmRentalInterruptionControllerAzure(Scambici.Domain.Employee employee, string timestamp, string signature)
		{
			dbContext = new Scambici.Domain.ScambiciContextAzure();
			if (!CheckSignature(employee, timestamp, signature))
			{
				throw new System.Security.Authentication.AuthenticationException("HMAC signature mismatch.");
			}
		}
	}
	internal class ConfirmTheftReportControllerAzure : Scambici.EmployeeAPI.ConfirmTheftReport.ConfirmTheftReportController
	{
		public ConfirmTheftReportControllerAzure()
		{
			dbContext = new Scambici.Domain.ScambiciContextAzure();
		}
		public ConfirmTheftReportControllerAzure(Scambici.Domain.Employee employee, string timestamp, string signature)
		{
			dbContext = new Scambici.Domain.ScambiciContextAzure();
			if (!CheckSignature(employee, timestamp, signature))
			{
				throw new System.Security.Authentication.AuthenticationException("HMAC signature mismatch.");
			}
		}
	}
	internal class ManageEmployeeControllerAzure : Scambici.EmployeeAPI.ManageEmployee.ManageEmployeeController
	{
		public ManageEmployeeControllerAzure(Scambici.Domain.Employee admin, string timestamp, string signature)
		{
			dbContext = new Scambici.Domain.ScambiciContextAzure();
			if (!CheckIsAdmin(admin))
			{
				throw new System.Security.Authentication.AuthenticationException("Requestant is not an administrator.");
			}
			if (!CheckSignature(admin, timestamp, signature))
			{
				throw new System.Security.Authentication.AuthenticationException("HMAC signature mismatch.");
			}
		}
		public ManageEmployeeControllerAzure()
		{
			dbContext = new Scambici.Domain.ScambiciContextAzure();
		}
	}
}
