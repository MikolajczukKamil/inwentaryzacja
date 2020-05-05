using System;
using System.Collections.Generic;
using Inwentaryzacja.controllers;
using Inwentaryzacja.models;

public class RoomService {
	private APIController api;

	public Room GetRoomById(ref int id) {

		RoomEntity roomentity = api.GetRoomByID(id).Result;
		Room room = new Room(roomentity.name, roomentity.building, roomentity.id);
		return room;
	}
	public Room[] GetAllRooms() {

		List<RoomEntity> roomlist = new List<RoomEntity>();
		roomlist = api.getAllRooms().Result;
		Room[] roomtab = new Room[roomlist.Count];
		for (int i = 0; i < roomlist.Count; i++)
		{
			RoomEntity roomentity = roomlist[i];
			Room room = new Room(roomentity.name, roomentity.building, roomentity.id);
			roomtab[i] = room;
		}
		return roomtab;
	}
	public bool AddNewRoom(ref RoomPropotype newRoom) {
		bool addnew = api.createRoom(newRoom).Result;
		return addnew;
	}
	public bool DeleteRoom(ref int id) {
		bool delated = api.deleteRoomByID(id).Result;
		return delated;
	}



	private Room room;

}
