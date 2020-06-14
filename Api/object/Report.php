<?php


class Report
{
    private $report_header;
    private $report_assets;

    public function __construct($header, $assets)
    {
        $this->report_header = $header;
        $this->report_assets = $assets;
    }

    /**
     * @return ReportHeader
     */
    public function getReportHeader()
    {
        return $this->report_header;
    }

    /**
     * @param ReportHeader $report_header
     */
    public function setReportHeader(ReportHeader $report_header): void
    {
        $this->report_header = $report_header;
    }

    /**
     * @return array
     */
    public function getReportAssets()
    {
        return $this->report_assets;
    }

    /**
     * @param array $report_assets
     */
    public function setReportAssets($report_assets): void
    {
        $this->report_assets = $report_assets;
    }

}