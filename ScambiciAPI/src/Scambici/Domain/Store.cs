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

namespace Scambici.Domain
{
	public class Store
	{
		public int StoreId
		{ get; set; }
		[System.ComponentModel.DataAnnotations.Required]
		public string Address
		{ get; set; }
		[System.ComponentModel.DataAnnotations.Schema.ForeignKey("AdministratorId")]
		public virtual Employee Administrator
		{ get; set; }
		// FIXME: Nullable foreign key. Dangerous.
		public int? AdministratorId
		{ get; set; }
		[Newtonsoft.Json.JsonIgnore]
		[System.ComponentModel.DataAnnotations.Schema.InverseProperty("Store")]
		public virtual System.Collections.Generic.List<User> Users
		{ get; set; }
		[Newtonsoft.Json.JsonIgnore]
		[System.ComponentModel.DataAnnotations.Schema.InverseProperty("Store")]
		public virtual System.Collections.Generic.List<Employee> Employees
		{ get; set; }
		[Newtonsoft.Json.JsonIgnore]
		[System.ComponentModel.DataAnnotations.Schema.InverseProperty("Store")]
		public virtual System.Collections.Generic.List<Bike> Bikes
		{ get; set; }
	}
}
