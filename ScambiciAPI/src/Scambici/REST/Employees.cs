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

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Scambici.REST
{
	public class FormAdmin : Scambici.Domain.Employee
	{
		public string Timestamp
		{ get; set; }
		public string Signature
		{ get; set; }
	}
	public static class Employees
	{
		[FunctionName("Employees")] public static async Task<System.Net.Http.HttpResponseMessage> Run(
				[HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
				ILogger log)
		{
			var requestData = new FormAdmin{ EmployeeId = System.Int32.Parse(req.Query["AdminId"]),
				Signature = req.Query["Signature"], Timestamp = req.Query["Timestamp"] };
			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			using var storesController = new StoresControllerAzure();
			var store = storesController.GetStoreByEmployee(requestData);
			log.LogInformation(requestBody);
			try 
			{
				using var employeesController = new EmployeesControllerAzure(requestData, requestData.Timestamp, requestData.Signature);
				var employees = employeesController.GetStoreEmployees(store);
				string response = JsonConvert.SerializeObject(employees, new JsonSerializerSettings {
						ReferenceLoopHandling = ReferenceLoopHandling.Ignore
						});
				return new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.OK) {
					Content = new System.Net.Http.StringContent(response, System.Text.Encoding.UTF8, "application/json") };
			}
			catch (System.Security.Authentication.AuthenticationException)
			{
				return new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
			}


		}
	}
}
