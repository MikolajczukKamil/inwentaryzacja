using System;
using System.Collections.Generic;
using System.Linq;
using Inwentaryzacja.Controllers.Api;
using Inwentaryzacja.Models;

namespace Inwentaryzacja.Services
{
	/// <summary>
	/// Klasa odpowiadajaca za obsluge pokojow i budynkow
	/// </summary>
	public class LocalizationService
	{

		private APIController api;

		/// <summary>
		/// Konstruktor klasy
		/// </summary>
		/// <param name="apiController">obiekt do interakcji z api</param>
		public LocalizationService(APIController apiController)
		{
			api = apiController;
		}
		
		public Building[] GetBuildings()
		{
			// return api.getAllBuildings().Result; // map to Building[]
			throw new System.Exception("Not implemented");
		}

		public Room[] GetRoomsInBuilding(Building building)
		{
			// return api.getAllRooms(building.Id).Result; // map to Room[]
			throw new System.Exception("Not implemented");
		}

		/// <summary>
		/// Funkcja dodajaca nowy pokoj
		/// </summary>
		/// <param name="newRoom">Pokoj ktory chcemy dodac</param>
		/// <returns></returns>
		public int AddNewRoom(RoomPropotype newRoom)
		{
			return api.createRoom(newRoom).Result;
		}
		/// <summary>
		/// Funkcja dodajaca nowy budynek
		/// </summary>
		/// <param name="newBuilding">budynek ktory chcemy dodac</param>
		/// <returns></returns>
		public int AddNewBuilding(BuildingPrototype newBuilding)
		{
			return api.createBuilding(newBuilding).Result;
		}

#if false // Old

	public Room GetRoomById(int id) {

		RoomEntity roomentity = api.GetRoomByID(id).Result;
		Room room = new Room(roomentity.id, roomentity.name, );
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

	public bool AddNewRoom(RoomPropotype newRoom) {
		bool addnew = api.createRoom(newRoom).Result;
		return addnew;
	}

	public bool DeleteRoom(int id) {
		bool delated = api.deleteRoomByID(id).Result;
		return delated;
	}
	
#endif
	} 
}
