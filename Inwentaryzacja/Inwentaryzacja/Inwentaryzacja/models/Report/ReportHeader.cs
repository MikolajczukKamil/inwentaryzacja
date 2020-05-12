using System;

namespace Inwentaryzacja.Models
{ 
	public class ReportHeader
	{
		public int Id { get; private set; }
		public string Name { get; private set; }
		public string RoomName { get; private set; }
		public string BuildingName { get; private set; }
		public string OwnerName { get; private set; }
		public DateTime CreateDate { get; private set; }
		public int OwnerId { get; private set; }

		public ReportHeader(int id, string name, string roomName, string buildingName, DateTime date, int ownerId, string ownerName)
		{
			Id = id;
			Name = name;
			RoomName = roomName;
			BuildingName = buildingName;
			OwnerName = ownerName;
			OwnerId = ownerId;
			CreateDate = date;
		}
	}
}
