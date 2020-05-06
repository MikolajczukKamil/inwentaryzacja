using System;
using System.Collections.Generic;
using Inwentaryzacja.controllers;
using Inwentaryzacja.models;

public class ReportService {
	private APIController ApiController;

    public ReportService(APIController apiController)
    {
        ApiController = apiController;
    }

    public Report GetReportById(int id)
    {
        ReportEntity raportEntity = ApiController.getReportByID(id).Result;
        
        Report raport = new Report(raportEntity.name, raportEntity.id, raportEntity.room , raportEntity.create_date);
        return raport;
	}
	public ReportHeader[] GetReportsHeaders()
    {
        List<ReportEntity> listreportEntity = new List<ReportEntity>();
        listreportEntity = ApiController.getAllReports().Result;
        int size = listreportEntity.Count;
        ReportHeader[] reportHeader = new ReportHeader[size];
        for (int i = 0; i < reportHeader.Length; i++)
        {
             reportHeader[i] = new ReportHeader(listreportEntity[i].id, listreportEntity[i].name, listreportEntity[i].room, listreportEntity[i].create_date);
        }
        return reportHeader;
    }
	public bool ExportReportToPDF(int id) {
		throw new System.Exception("Not implemented");//Brak Danych
	}
	public bool DeleteReport(int id)
    {
        bool delete = ApiController.deleteReportByID(id).Result;
        return delete;
	}
	public bool AddNewReport(ReportPrototype newReport)
    {
        bool add = ApiController.createReportWithAssets(newReport).Result;
        return add;
    }

}
