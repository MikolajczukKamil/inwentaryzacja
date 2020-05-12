using System;

namespace Inwentaryzacja.Models
{ 
	public class ReportHeader
	{
		public int Id { get; private set; }
		public string Name { get; private set; }
		public Room Room { get; private set; }
		public User Owner { get; private set; }
		public DateTime CreateDate { get; private set; }

		public ReportHeader(int id, string name, string roomName, string buildingName, DateTime date, int ownerId, string ownerName)
		{
			Id = id;
			Name = name;
			Room = new Room(-1, roomName, new Building(-1, buildingName));
			Owner = new User(ownerId, ownerName);
			CreateDate = date;
		}

		public ReportHeader(int id, string name, Room room, User owner, DateTime date)
		{
			Id = id;
			Name = name;
			Room = room;
			Owner = owner;
			CreateDate = date;
		}
	}
}
