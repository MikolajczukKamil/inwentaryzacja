using System;
using Inwentaryzacja.controllers;
using Inwentaryzacja.models;

public class ReportService {
	private APIController api;

	public Report GetReportById(ref int id) {
		throw new System.Exception("Not implemented");
	}
	public ReportHeader[] GetReportsHeaders() {
		throw new System.Exception("Not implemented");
	}
	public bool ExportReportToPDF(ref int id) {
		throw new System.Exception("Not implemented");
	}
	public bool DeleteReport(ref int id) {
		throw new System.Exception("Not implemented");
	}
	public int AddNewReport(ref ReportPrototype newReport) {
		throw new System.Exception("Not implemented");
	}

	private APIController aPIController;

}
