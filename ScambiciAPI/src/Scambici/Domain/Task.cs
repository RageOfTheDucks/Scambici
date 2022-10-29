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
	public abstract class Task
	{
		public int TaskId
		{ get; set; }
		public bool Completed
		{ get; set; }
		[System.ComponentModel.DataAnnotations.Schema.ForeignKey("AssigneeId")]
		public virtual Employee AssignedTo
		{ get; set; }
		public int? AssigneeId
		{ get; set; }
		public virtual Employee CompletedBy
		{ get; set; }
	}
	public abstract class Maintenance : Task
	{
		[System.ComponentModel.DataAnnotations.Required]
		public string FaultDescription
		{ get; set; }
	}
	public class UserMaintenance : Maintenance
	{
		public virtual User CreatedBy
		{ get; set; }
	}
	public class StorageMaintenance : Maintenance
	{
		public virtual Bike Bike
		{ get; set; }
	}
	public class Delivery : Task
	{
		[System.ComponentModel.DataAnnotations.Required]
		public virtual System.DateTime DesiredDateTime
		{ get; set; }
		public virtual User CreatedBy
		{ get; set; }
	}
}


