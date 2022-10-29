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

namespace Scambici.API
{
	// Implement IDisposable to allow using(controller) in routes
	public abstract class Controller : System.IDisposable
	{
		// Concrete definitions should define a constructor that
		// assigns a value to dbContext
		protected Scambici.Domain.ScambiciContext dbContext;
		// The Dispose() method can exist independently from the constructor
		// since ScambiciContext implements IDisposable already
		public void Dispose()
		{
			dbContext.Dispose();
		}

		private string CalculateSignature(byte[] passHash, byte[] timestamp)
		{
			string result;
			using (var hmac = new System.Security.Cryptography.HMACSHA512(passHash))
			{
				result = hmac.ComputeHash(timestamp).Aggregate("", (s, e) => s + System.String.Format("{0:x2}",e), s => s );
			}
			return result;
		}
		private readonly int _MAX_TIMESTAMP_DISCREPANCY = 300; // Five minutes
		protected bool CheckSignature(Scambici.Domain.User user, string timestamp, string signature)
		{
			int unixTimestamp = (int)(System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds;
			if (System.Math.Abs(System.Int32.Parse(timestamp) - unixTimestamp) > _MAX_TIMESTAMP_DISCREPANCY) { return false ; }
			Scambici.Domain.User dbUser = dbContext.Users.Where(u => (u.UserId == user.UserId || u.EMailAddress == user.EMailAddress)).First();
			if (user.PasswordHash != null && user.PasswordHash != dbUser.PasswordHash)
			{
				return false;
			}
			byte[] passHash = System.Text.Encoding.ASCII.GetBytes(dbUser.PasswordHash);
			return CalculateSignature(passHash, System.Text.Encoding.ASCII.GetBytes(timestamp)).Equals(signature);
		}
		protected bool CheckSignature(Scambici.Domain.Employee employee, string timestamp, string signature)
		{
			int unixTimestamp = (int)(System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds;
			if (System.Math.Abs(System.Int32.Parse(timestamp) - unixTimestamp) > _MAX_TIMESTAMP_DISCREPANCY) { return false ; }
			Scambici.Domain.Employee dbEmployee = dbContext.Employees.Where(u => (u.EmployeeId == employee.EmployeeId || u.EMailAddress == employee.EMailAddress)).First();
			if (employee.PasswordHash != null && employee.PasswordHash != dbEmployee.PasswordHash)
			{
				return false;
			}
			byte[] passHash = System.Text.Encoding.ASCII.GetBytes(dbEmployee.PasswordHash);
			var computedSignature = CalculateSignature(passHash, System.Text.Encoding.ASCII.GetBytes(timestamp));
			return computedSignature.Equals(signature);
		}
	}
}
