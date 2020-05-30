using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Inwentaryzacja.Controllers.Api;
using Inwentaryzacja.Models;

namespace Inwentaryzacja.Services
{
    public class ReportService
    {
        private APIController api;

        public ReportService(APIController apiController)
        {
            api = apiController;
        }

        public ReportHeader[] GetReportHeaders()
        {
            // ReportHeaderEntity[] raportHeaderEntities = api.getReportHeaders().Result; // Map to ReportHeader[]

            throw new System.Exception("Not implemented");
        }

        public ReportHeader GetReportHeader(int reportId)
        {
            // ReportHeaderEntity raportHeaderEntity = api.getReportHeader(reportId).Result; // To ReportHeader

            throw new System.Exception("Not implemented");
        }

        public Report GetFullReport(ReportHeader reportHeader)
        {
            throw new System.Exception("Not implemented");
        }

        public Report GetFullReport(int reportId)
        {
            throw new System.Exception("Not implemented");
        }

        public async Task<ReportPositionEntity[]> GetReportPositions(int reportId)
        {
            return await api.getReportPositions(reportId);
        }

        public bool AddNewReport(ReportPrototype newReport)
        {
            return api.createReport(newReport).Result;
        }
        
        public bool ExportReportToPDF(int reportId)
        {
            throw new System.Exception("Not implemented");
        }

        public string[] GetScannedItemsCount(ReportPositionEntity[] reportPositionEntities, RoomEntity currentRoom)
        {
            string[] result = new string[10];

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

            return result;
        }
        private string GenerateString(Dictionary<string, int> dict)
        {
            string result = "";

            if (dict.Count == 0 || dict == null) return result;

            foreach (KeyValuePair<string, int> item in dict)
            {
                string piecesText = "sztuk";
                string spacebars = "";
                int spaceCounter = 12 - item.Key.Length;

                if (item.Key == "krzes³o") spaceCounter += 3;
                if (item.Key == "monitor") spaceCounter += 3;
                if (item.Key == "stó³") spaceCounter += 7;
                if (item.Key == "tablica") spaceCounter += 4;
                if (item.Key == "projektor") spaceCounter += 3;

                for (int i = 0; i < spaceCounter; i++) spacebars += " ";

                if (item.Value == 1) piecesText = "sztuka";
                if (item.Value == 2 || item.Value == 3 || item.Value == 4) piecesText = "sztuki";

                result += item.Key + spacebars + item.Value + " " + piecesText + Environment.NewLine;
            }

            return result;
        }
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

#if false // Old
    public Report GetReportById(int id)
    {
        ReportEntity raportEntity = ApiController.getReportByID(id).Result;

        Report raport = new Report(raportEntity.name, raportEntity.id, raportEntity.room, raportEntity.create_date);
        return raport;
    }
    public ReportHeader[] GetReportsHeaders()
    {
        List<ReportEntity> listreportEntity = new List<ReportEntity>();
        listreportEntity = api.getAllReports().Result;
        int size = listreportEntity.Count;
        ReportHeader[] reportHeader = new ReportHeader[size];
        for (int i = 0; i < reportHeader.Length; i++)
        {
            reportHeader[i] = new ReportHeader(listreportEntity[i].id, listreportEntity[i].name, listreportEntity[i].room, listreportEntity[i].create_date);
        }
        return reportHeader;
    }
    public bool DeleteReport(int id)
    {
        bool delete = api.deleteReportByID(id).Result;
        return delete;
    }
    public bool AddNewReport(ReportPrototype newReport)
    {
        bool add = api.createReportWithAssets(newReport).Result;
        return add;
    } 
#endif
    } 
}
