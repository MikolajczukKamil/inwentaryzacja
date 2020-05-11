using System;

namespace Inwentaryzacja.models
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
