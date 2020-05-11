using System;
using System.Collections.Generic;
using System.Text;

namespace Inwentaryzacja.models
{
    public class AssetInfo
    {
		public int Id { get; private set; }
		public AssetType Type { get; private set; }
		public Room Room { get; private set; }

		public AssetInfo(int id, AssetType type, Room room)
		{
			Id = id;
			Type = type;
			Room = room;
		}
	}
}
