using Inwentaryzacja.Controllers.Api;
using System;

namespace Inwentaryzacja.Models
{
	/// <summary>
	/// Prototyp pokoju
	/// </summary>
	public class RoomPropotype
	{
		/// <summary>
		/// Nazwa pokoju
		/// </summary>
		public string name;
		
		/// <summary>
		/// Budynek w ktorym znajduje sie pokoj
		/// </summary>
		public int building;

		/// <summary>
		/// Konstruktor prototypu pokoju
		/// </summary>
		/// <param name="name">Nazwa pokoju</param>
		/// <param name="building">Budynek w ktorym znajduje sie pokoj</param>
		public RoomPropotype(string name, Building building)
		{
			this.name = name;
			this.building = building.Id;
		}

		/// <summary>
		/// Konstruktor prototypu pokoju
		/// </summary>
		/// <param name="name">Nazwa pokoju</param>
		/// <param name="building">Budynek w ktorym znajduje sie pokoj</param>
		public RoomPropotype(string name, BuildingEntity building)
		{
			this.name = name;
			this.building = building.id;
		}
	}
}
