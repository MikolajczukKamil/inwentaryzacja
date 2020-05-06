using System;
using Inwentaryzacja.controllers;
using Inwentaryzacja.models;

public class ScanningService {
	private APIController ApiController;

	public ScanningService(APIController apiController)
	{
		ApiController = apiController;
	}

	public Scanning GetScanning(int id) {
		throw new System.Exception("Not implemented");//Na razie nie ma zapisu skanowania w bazie
	}
	public void UpdateScanning(ScanningPropotype scanningPropotype) {
		throw new System.Exception("Not implemented");//Na razie nie ma zapisu skanowania w bazie
	}
	public Scanning CreateScanning(int roomID) {
		RoomEntity roomEntity = ApiController.getRoomByID(roomID).Result;
		BuildingEntity buildingEntity = ApiController.getBuildingByID(roomEntity.building).Result;
		 Room room = new Room(roomEntity.name, buildingEntity.name, roomEntity.id);
		Scanning scanning = new Scanning(0, room);
		return scanning;
	}
}
