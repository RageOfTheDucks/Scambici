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


// This using directive is needed because otherwise an error
// is thrown when invoking DbContextOptionsBuilder.
// TODO: Research this issue
using Microsoft.EntityFrameworkCore;  

namespace Scambici.Domain
{
	public abstract class ScambiciContext : Microsoft.EntityFrameworkCore.DbContext 
	{
		public Microsoft.EntityFrameworkCore.DbSet<User> Users 
		{ get; set; }
		public Microsoft.EntityFrameworkCore.DbSet<Store> Stores 
		{ get; set; }
		public Microsoft.EntityFrameworkCore.DbSet<Employee> Employees 
		{ get; set; }
		public Microsoft.EntityFrameworkCore.DbSet<TheftReport> TheftReports
		{ get; set; }
		public Microsoft.EntityFrameworkCore.DbSet<Task> Tasks
		{ get; set; }
		public Microsoft.EntityFrameworkCore.DbSet<Maintenance> Maintenances
		{ get; set; }
		public Microsoft.EntityFrameworkCore.DbSet<UserMaintenance> UserMaintenances
		{ get; set; }
		public Microsoft.EntityFrameworkCore.DbSet<StorageMaintenance> StorageMaintenances
		{ get; set; }
		public Microsoft.EntityFrameworkCore.DbSet<Delivery> Deliveries
		{ get; set; }
		public Microsoft.EntityFrameworkCore.DbSet<Bike> Bikes
		{ get; set; }
		public Microsoft.EntityFrameworkCore.DbSet<RentalInterruption> RentalInterruptions
		{ get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Employee>()
				.HasIndex(u => u.EMailAddress)
				.IsUnique();
			modelBuilder.Entity<User>()
				.HasIndex(u => u.EMailAddress)
				.IsUnique();
			modelBuilder.Entity<User>()
				.HasIndex(u => u.RentedBikeId)
				.IsUnique()
				.HasFilter("[RentedBikeId] IS NOT NULL");
			modelBuilder.Entity<StorageMaintenance>()
				.HasOne(u => u.Bike)
				.WithMany()
				.OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.SetNull);
		}
	}

	public class ScambiciContextAzure : ScambiciContext
	{
		protected override void OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder options)
			=> options
			.UseLazyLoadingProxies()
			.UseSqlServer(System.Environment.GetEnvironmentVariable("SQLAZURECONNSTR_Scambici"));
	}

}

