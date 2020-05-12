using System;
using System.Collections.Generic;
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

        private ReportPosition[] GetReportPositions(int reportId)
        {
            // ReportPositionEntity[] reportPositionEntity = api.getReportPositions(reportId).Result; // Map to ReportPosition[]

            throw new System.Exception("Not implemented");
        }

        public bool AddNewReport(ReportPrototype newReport)
        {
            return api.createReport(newReport).Result;
        }
        
        public bool ExportReportToPDF(int reportId)
        {
            throw new System.Exception("Not implemented");
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
