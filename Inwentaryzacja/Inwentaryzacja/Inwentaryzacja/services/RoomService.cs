using System;
using Inwentaryzacja.controllers;
using Inwentaryzacja.models;

public class RoomService {
	private APIController api;

	public Room GetRoomById(ref int id) {
		throw new System.Exception("Not implemented");
	}
	public Room[] GetAllRooms() {
		throw new System.Exception("Not implemented");
	}
	public bool AddNewRoom(ref RoomPropotype newRoom) {
		throw new System.Exception("Not implemented");
	}
	public bool DeleteRoom(ref int id) {
		throw new System.Exception("Not implemented");
	}

	private APIController aPIController;

	private Room room;

}
