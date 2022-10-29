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
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace Scambici.REST
{
	public class FormEndTask
	{
		public int EmployeeId
		{ get; set; }
		public string Signature
		{ get; set; }
		public string Timestamp
		{ get; set; }
	}
	public class FormEndBikeDelivery : FormEndTask
	{
		public int BikeId
		{ get; set; }
	}
	public static class TaskEnding
	{
		[FunctionName("TerminateUserMaintenance")]
		public static async Task<System.Net.Http.HttpResponseMessage> RunUserMaintenance(
				[HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
				ILogger log)
		{
			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			log.LogInformation(requestBody);
			var requestData = JsonConvert.DeserializeObject<FormEndTask>(requestBody);
			try
			{
				using var employeesController = new EmployeesControllerAzure();
				var employee = employeesController.GetEmployeeById(requestData.EmployeeId);
				using var controller = new UserMaintenanceControllerAzure(employee, requestData.Timestamp, requestData.Signature);
				controller.TerminateTask(employee);
				return new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.OK) {
					Content = new System.Net.Http.StringContent("{}", System.Text.Encoding.UTF8, "application/json")
				};
			} 
			catch (System.Security.Authentication.AuthenticationException)
			{
				return new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
			}
		}

		[FunctionName("TerminateStoreMaintenance")]
		public static async Task<System.Net.Http.HttpResponseMessage> RunStoreMaintenance(
				[HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
				ILogger log)
		{
			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			log.LogInformation(requestBody);
			var requestData = JsonConvert.DeserializeObject<FormEndTask>(requestBody);
			try
			{
				using var employeesController = new EmployeesControllerAzure();
				var employee = employeesController.GetEmployeeById(requestData.EmployeeId);
				using var controller = new StorageMaintenanceControllerAzure(employee, requestData.Timestamp, requestData.Signature);
				controller.TerminateTask(employee);
				return new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.OK) {
					Content = new System.Net.Http.StringContent("{}", System.Text.Encoding.UTF8, "application/json")
				};
			} 
			catch (System.Security.Authentication.AuthenticationException)
			{
				return new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
			}
		}

		[FunctionName("TerminateBikeDelivery")]
		public static async Task<System.Net.Http.HttpResponseMessage> RunBikeDelivery(
				[HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
				ILogger log)
		{
			string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			log.LogInformation(requestBody);
			var requestData = JsonConvert.DeserializeObject<FormEndBikeDelivery>(requestBody);
			try
			{
				using var employeesController = new EmployeesControllerAzure();
				var employee = employeesController.GetEmployeeById(requestData.EmployeeId);
				using var controller = new BikeDeliveryControllerAzure(employee, requestData.Timestamp, requestData.Signature);
				controller.TerminateTask(employee, requestData.BikeId);
				return new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.OK) {
					Content = new System.Net.Http.StringContent("{}", System.Text.Encoding.UTF8, "application/json")
				};
			} 
			catch (System.Security.Authentication.AuthenticationException)
			{
				return new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
			}
		}
		

	}
}