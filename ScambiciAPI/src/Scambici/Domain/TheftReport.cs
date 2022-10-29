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
	public class TheftReport
	{
		public int TheftReportId
		{ get; set; }
		// Does not actually represent whether
		// the user really had the key
		// Controllers must offer an option
		// to make the user pay the full fine
		// even if he initially declared 
		// to have the key
		[System.ComponentModel.DataAnnotations.Required]
		public bool Confirmed 
		{ get; set; }
		public virtual User CreatedBy
		{ get; set; }
	}
}
