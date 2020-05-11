using System;
using System.Collections.Generic;
using Inwentaryzacja.controllers;
using Inwentaryzacja.models;

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

        public ReportHeader GetReportHeader(int id)
        {
            // ReportHeaderEntity raportHeaderEntity = api.getReportHeader(id).Result; // To ReportHeader

            throw new System.Exception("Not implemented");
        }

        public Report GetFullReport(ReportHeader reportHeader)
        {
            throw new System.Exception("Not implemented");
        }

        public Report GetFullReport(int id)
        {
            throw new System.Exception("Not implemented");
        }

        private ReportHeader GetReportPositions(int id)
        {
            // ReportPositionEntity[] reportPositionEntity = api.getReportPositions(id).Result; // Map to ReportPosition[]

            throw new System.Exception("Not implemented");
        }

        public bool AddNewReport(ReportPrototype newReport)
        {
            return api.createReportWithAssets(newReport).Result;
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
    public bool ExportReportToPDF(int id)
    {
        throw new System.Exception("Not implemented");//Brak Danych
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
