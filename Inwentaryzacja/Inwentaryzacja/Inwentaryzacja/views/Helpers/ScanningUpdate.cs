using Inwentaryzacja.Controllers.Api;
using Inwentaryzacja.Models;
using System.Collections.Generic;

namespace Inwentaryzacja.views.Helpers
{
    /// <summary>
    /// Kontroller zapisywania aktualnego stanu skanowania
    /// </summary>
    public class ScanningUpdate
    {
        private APIController Api;
        private RoomEntity ThisRoom;
        private int Scanid;


        /// <summary>
        /// Konstruktor klasy
        /// </summary>
        /// <param name="api">obiekt api do interakcji</param>
        /// <param name="room">pokoj w ktorym odbywa sie skanowanie</param>
        /// <param name="scanid">id skanowania</param>
        public ScanningUpdate(APIController api, RoomEntity room, int scanid)
        {
            ThisRoom = room;
            Scanid = scanid;
            Api = api;
        }

        /// <summary>
        /// Updateuje skanowanie
        /// </summary>
        /// <param name="allPositions">Aktualny stan skanowania</param>
        public async void Update(List<ScanPosition> allPositions)
        {
            var scannedItems = allPositions.FindAll((el) => el.Approved || el.AssetRoomId != ThisRoom.id);

            var positions = new List<ScanPositionPropotype>();

            foreach (var position in scannedItems)
            {
                positions.Add(new ScanPositionPropotype(position.AssetEntity.id, position.state));
            }

            await Api.updateScan(new ScanUpdatePropotype(Scanid, positions.ToArray()));
        }

        /// <summary>
        /// Usuwa skanowanie
        /// </summary>
        public async void Delete()
        {
            await Api.deleteScan(Scanid);
        }
    }
}
