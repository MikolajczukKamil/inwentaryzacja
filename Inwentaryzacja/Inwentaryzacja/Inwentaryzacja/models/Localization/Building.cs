using System;

namespace Inwentaryzacja.Models
{
	public class Building
	{
		public int Id { get; private set; }
		public string Name { get; private set; }

		public Building(int id, string name)
		{
			Id = id;
			Name = name;
		}
	}
}
