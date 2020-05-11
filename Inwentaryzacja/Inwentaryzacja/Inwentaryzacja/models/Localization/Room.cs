using System;

namespace Inwentaryzacja.Models
{
	public class Room
	{
		public int Id { get; private set; }
		public string Name { get; private set; }
		public Building Building { get; private set; }

		public Room(int id, string name, Building building)
		{
			Id = id;
			Name = name;
			Building = building;
		}

		public bool SameAs(Room other)
		{
			return Id == other.Id;
		}
	}
}
