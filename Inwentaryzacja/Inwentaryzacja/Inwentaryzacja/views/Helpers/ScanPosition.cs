using Inwentaryzacja.Controllers.Api;
using Inwentaryzacja.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inwentaryzacja.views.Helpers
{
    /// <summary>
    /// Klasa odpowiadajaca za skanowanie srodkow trwalych i rzeczy z tym zwiazane
    /// </summary>
    public class ScanPosition
    {
        public ReportPositionPrototype reportPositionPrototype;

        public AssetEntity AssetEntity;
        private RoomEntity ScanningRoom;
        public bool Approved = false;
        public int? AssetRoomId;

        public int state = 0;

        /// <summary>
        /// Konstruktor klasy
        /// </summary>
        /// <param name="scanningRoom">pokoj w ktorym odbywa sie skanowanie</param>
        /// <param name="assetEntity">zeskanowany srodek trwaly</param>
        /// <param name="assetRoom">pokoj z ktorego pochodzi srodek trwaly</param>
        public ScanPosition(AssetEntity assetEntity, RoomEntity assetRoom, RoomEntity scanningRoom)
        {
            if (assetRoom != null)
            {
                AssetRoomId = assetRoom.id;
                AssetRoomName = assetRoom.name;
            }
            else
            {
                AssetRoomId = null;
                AssetRoomName = "brak";
            }

            reportPositionPrototype = new ReportPositionPrototype(assetEntity, assetRoom, false);
            AssetEntity = assetEntity;
            ScannedId = assetEntity.id;
            ScanningRoom = scanningRoom;
        }

        /// <summary>
        /// Funkcja odpowiadajaca za przeniesienie srodka trwalego do aktualnie skanowanego pokoju
        /// </summary>
        public void ItemMoved()
        {
            state = 1;
            Approved = true;
            reportPositionPrototype.present = true;
            AssetRoomId = ScanningRoom.id;
        }

        /// <summary>
        /// Opcja nie rób nic
        /// </summary>
        public void ItemDontMove()
        {
            state = 2;
            Approved = true;
        }

        /// <summary>
        /// Funkcja odpowiadajaca za tekst wyswietlany po zeskanowaniu danego srodka trwalego
        /// </summary>
        /// <returns>tekst z nazwa srodka trwalego i jego id</returns>
        public string ScaningText
        {
            get { return $"{AssetEntity.type.name} {AssetEntity.id}"; }
        }

        /// <summary>
        /// Funkcja odpowiadajaca za ustawienie/zwrocenie ID zeskanowanego srodka trwalego
        /// </summary>
        public int ScannedId { get; set; }

        /// <summary>
        /// Funkcja odpowiadajaca za ustawienie/zwrocenie nazwy pokoju pochodzenia zeskanowanego srodka trwalego
        /// </summary>
        public string AssetRoomName { get; set; }
    }

}
