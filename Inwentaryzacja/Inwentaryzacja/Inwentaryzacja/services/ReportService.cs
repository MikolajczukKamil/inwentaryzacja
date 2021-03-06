using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Inwentaryzacja.Controllers.Api;
using Inwentaryzacja.Models;

namespace Inwentaryzacja.Services
{
    /// <summary>
    /// Klasa odpowiadajaca za obsluge raportow
    /// </summary>
    public class ReportService
    {
        private APIController api;

        /// <summary>
        /// Konstruktor klasy
        /// </summary>
        /// <param name="apiController">obiekt do interakcji z api</param>
        public ReportService(APIController apiController)
        {
            api = apiController;
        }

        /// <summary>
        /// Funkcja zwracajaca pozycje (srodki trwale) z raportu 
        /// </summary>
        /// <param name="reportId">ID Raportu z ktorego chcemy zwrocic srodki trwale</param>
        /// <returns>srodki trwale z danego raportu</returns>
        public async Task<ReportPositionEntity[]> GetReportPositions(int reportId)
        {
            return await api.getReportPositions(reportId);
        }

        /// <summary>
        /// Funkcja liczaca ilosci srodkow trwalych z pokoju z raportu
        /// </summary>
        /// <param name="reportPositionEntities">srodki trwale z raportu</param>
        /// <param name="currentRoom">pokoj ktorego dotyczy raport</param>
        /// <returns>tablica licznosci zeskanowanych srodkow trwalych</returns>
        public string[] GetScannedItemsCount(ReportPositionEntity[] reportPositionEntities, RoomEntity currentRoom)
        {
            string[] result = new string[15];

            Dictionary<string, int> inThisRoomCount = new Dictionary<string, int>();
            Dictionary<string, int> movedToRoomCount = new Dictionary<string, int>();
            Dictionary<string, int> movedFromRoomCount = new Dictionary<string, int>();
            Dictionary<string, int> inAnotherRoomCount = new Dictionary<string, int>();
            Dictionary<string, int> scannedAll = new Dictionary<string, int>();

            Dictionary<string, string> scannedAllDetails = new Dictionary<string, string>();
            Dictionary<string, string> inThisRoomDetails = new Dictionary<string, string>();
            Dictionary<string, string> movedToRoomDetails = new Dictionary<string, string>();
            Dictionary<string, string> movedFromRoomDetails = new Dictionary<string, string>();
            Dictionary<string, string> inAnotherRoomDetails = new Dictionary<string, string>();

            foreach (ReportPositionEntity item in reportPositionEntities)
            {
                string typeName = item.asset.type.name;
                if (item.present == true)
                {
                    if (!scannedAll.ContainsKey(typeName))
                    {
                        scannedAll.Add(typeName, 1);
                        scannedAllDetails.Add(typeName, item.asset.id + "");
                    }
                    else
                    {
                        scannedAll[typeName]++;
                        scannedAllDetails[typeName] += ", " + item.asset.id;
                    }
                }
                if (item.present == true)
                {
                    if (item.previous_room == null || item.previous_room.id != currentRoom.id)
                    {
                        if (!movedToRoomCount.ContainsKey(typeName))
                        {
                            movedToRoomCount.Add(typeName, 1);
                            if(item.previous_room == null) movedToRoomDetails.Add(typeName, item.asset.id + " (z magazynu)");
                            if(item.previous_room != null) movedToRoomDetails.Add(typeName, item.asset.id + " (z sali " + item.previous_room.name + ")");

                        }
                        else
                        {
                            movedToRoomCount[typeName]++;
                            movedToRoomDetails[typeName] += ", " + item.asset.id;
                        }
                    }
                    else
                    {
                        if (!inThisRoomCount.ContainsKey(typeName))
                        {
                            inThisRoomCount.Add(typeName, 1);
                            inThisRoomDetails.Add(typeName, item.asset.id + "");
                        }
                        else
                        {
                            inThisRoomCount[typeName]++;
                            inThisRoomDetails[typeName] += ", " + item.asset.id;
                        }
                    }

                }
                else if (item.present == false)
                {
                    if (item.previous_room == null || item.previous_room.id != currentRoom.id)
                    {
                        if (!inAnotherRoomCount.ContainsKey(typeName))
                        {
                            inAnotherRoomCount.Add(typeName, 1);
                            if (item.previous_room == null) inAnotherRoomDetails.Add(typeName, item.asset.id + " (z magazynu)");
                            if (item.previous_room != null) inAnotherRoomDetails.Add(typeName, item.asset.id + " (z sali " + item.previous_room.name + ")");

                        }
                        else
                        {
                            inAnotherRoomCount[typeName]++;
                            if (item.previous_room == null) inAnotherRoomDetails[typeName] += ", " + item.asset.id + " (z magazynu)";
                            if (item.previous_room != null) inAnotherRoomDetails[typeName] += ", " + item.asset.id + " (z sali " + item.previous_room.name + ")";
                        }
                    }
                    else
                    {
                        if (!movedFromRoomCount.ContainsKey(typeName))
                        {
                            movedFromRoomCount.Add(typeName, 1);
                            movedFromRoomDetails.Add(typeName, item.asset.id + "");
                        }
                        else
                        {
                            movedFromRoomCount[typeName]++;
                            movedFromRoomDetails[typeName] += ", " + item.asset.id;
                        }
                    }

                    
                }
            }

            result[0] = GenerateString(inThisRoomCount);
            result[1] = GenerateString(movedToRoomCount);
            result[2] = GenerateString(movedFromRoomCount);
            result[3] = GenerateString(inAnotherRoomCount);
            result[4] = GenerateString(scannedAll);

            result[5] = GenerateString(scannedAllDetails);
            result[6] = GenerateString(inThisRoomDetails);
            result[7] = GenerateString(movedToRoomDetails);
            result[8] = GenerateString(movedFromRoomDetails);
            result[9] = GenerateString(inAnotherRoomDetails);

            result[10] = GenerateStringLabel(scannedAll);
            result[11] = GenerateStringLabel(inThisRoomCount);
            result[12] = GenerateStringLabel(movedToRoomCount);
            result[13] = GenerateStringLabel(inAnotherRoomCount);
            result[14] = GenerateStringLabel(movedFromRoomCount);

            return result;
        }

        /// <summary>
        /// Funkcja tworzaca string opisujacy ilosci srodkow trwalych ze slownika srodkow trwalych
        /// </summary>
        /// <param name="dict">slownik <string,int> z ilosciami srodkow trwalych</param>
        /// <returns>string opisujacy ilosci srodkow trwalych ze slownika srodkow trwalych</returns>
        private string GenerateString(Dictionary<string, int> dict)
        {
            string result = "";

            if (dict.Count == 0 || dict == null) return result;

            foreach (KeyValuePair<string, int> item in dict)
            {
                string piecesText = "sztuk";

                if (item.Value == 1) piecesText = "sztuka";
                if (item.Value == 2 || item.Value == 3 || item.Value == 4) piecesText = "sztuki";

                result += item.Value + " " + piecesText + Environment.NewLine;
            }

            return result;
        }

        /// <summary>
        /// Funkcja tworzaca etykiete stringa opisujacy ilosci srodkow trwalych ze slownika srodkow trwalych
        /// </summary>
        /// <param name="dict">slownik <string,int> z ilosciami srodkow trwalych</param>
        /// <returns>etykiete stringa opisujacego ilosci srodkow trwalych ze slownika srodkow trwalych</returns>
        private string GenerateStringLabel(Dictionary<string, int> dict)
        {
            string result = "";

            if (dict.Count == 0 || dict == null) return result;

            foreach (KeyValuePair<string, int> item in dict)
            {
                result += item.Key + Environment.NewLine;
            }

            return result;
        }

        /// <summary>
        /// Funkcja tworzaca string opisujacy ilosci srodkow trwalych ze slownika srodkow trwalych
        /// </summary>
        /// <param name="dict">slownik <string,string> z ilosciami srodkow trwalych</param>
        /// <returns>string opisujacy ilosci srodkow trwalych ze slownika srodkow trwalych</returns>
        private string GenerateString(Dictionary<string, string> dict)
        {
            string result = "";

            if (dict.Count == 0 || dict == null) return result;

            foreach (KeyValuePair<string, string> item in dict)
            {
                result += item.Key + " numer: " + item.Value + Environment.NewLine;
            }
            return result;
        }
    } 
}
