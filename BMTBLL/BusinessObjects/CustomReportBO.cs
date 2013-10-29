using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

using System.Web;
using System.Net.Mail;
using System.Web.Configuration;
using System.Net.Configuration;
using System.Xml;
using System.Xml.Linq;
using BMTBLL.Enumeration;

namespace BMTBLL
{
    public class CustomReportBO : BMTConnection
    {
        #region Variable
        private int customReportId;
        private int reportSectionId;
        private int userId;
        private XDocument xml;
        #endregion

        #region DataMember
        private CustomReport _customReport;
        private EnterpriseReportSection _enterPriseReportSection;
        private ReportSection _reportSection;
        #endregion

        #region Properties
        public int CustomReportId
        {
            get { return customReportId; }
            set { customReportId = value; }
        }
        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        public int ReportSectionId
        {
            get { return reportSectionId; }
            set { reportSectionId = value; }
        }

        public XDocument XML
        {
            get { return xml; }
            set { xml = value; }
        }
        #endregion

        #region Constructor
        public CustomReportBO()
        { }
        #endregion

        #region Function
        public string GetNewXMLCustomReport()
        {
            try
            {
                //Get Fresh XML for Custom Report
                var xmlCustomReports = (from customReport in BMTDataContext.CustomReports
                                        where customReport.CostumReportId == this.customReportId
                                        && customReport.UserId == this.userId
                                        select customReport).SingleOrDefault();
                string xmlCustomReport = string.Empty;
                if (xmlCustomReports != null)
                    xmlCustomReport = Convert.ToString(xmlCustomReports.XML);
                else
                    xmlCustomReport = string.Empty;

                return xmlCustomReport;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public string GetNewXMLCustomReport(int userId, int reportSectionId)
        {
            try
            {
                //Get Fresh XML for Custom Report
                var xmlCustomReports = (from customReport in BMTDataContext.CustomReports
                                        where customReport.ReportSectionId == reportSectionId
                                        && customReport.UserId == userId
                                        select customReport).SingleOrDefault();
                string xmlCustomReport = string.Empty;
                if (xmlCustomReports != null)
                    xmlCustomReport = Convert.ToString(xmlCustomReports.XML);
                else
                    xmlCustomReport = string.Empty;

                return xmlCustomReport;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public bool SaveCustomReport(int userId, XElement xml, int sectionId, int enterpriseId)
        {
            try
            {
                var ReportRecord = (from reportRecord in BMTDataContext.ReportSections
                                    join customReport in BMTDataContext.CustomReports
                                    on reportRecord.ReportSectionId equals customReport.ReportSectionId
                                    join enterpriseReportSection in BMTDataContext.EnterpriseReportSections
                                    on reportRecord.ReportSectionId equals enterpriseReportSection.ReportSectionId
                                    where customReport.ReportSectionId == sectionId
                                    select new { reportRecord, customReport , enterpriseReportSection }).SingleOrDefault();




                _reportSection = new ReportSection();
                _enterPriseReportSection = new EnterpriseReportSection();
                _customReport = new CustomReport();

                _reportSection.ParentReportSectionId = 3;
                _reportSection.MedicalGroupId = null;
                if (xml.Attribute("Id").Value == "0")
                {
                    _reportSection.Name = xml.Attribute("TitleLine1").Value;
                    _reportSection.ContentType = xml.Attribute("TitleLine1").Value;

                    BMTDataContext.ReportSections.InsertOnSubmit(_reportSection);
                    BMTDataContext.SubmitChanges();

                    _customReport.ReportSectionId = _reportSection.ReportSectionId;
                    _customReport.UserId = userId;
                    _customReport.XML = xml;
                    BMTDataContext.CustomReports.InsertOnSubmit(_customReport);
                    BMTDataContext.SubmitChanges();

                    _enterPriseReportSection.EnterpriseId = enterpriseId;
                    _enterPriseReportSection.ReportSectionId = _reportSection.ReportSectionId;
                    BMTDataContext.EnterpriseReportSections.InsertOnSubmit(_enterPriseReportSection);
                    BMTDataContext.SubmitChanges();
                }
                else if (xml.Attribute("Id").Value == "1")
                {
                     string name = xml.Attribute("TitleLine1").Value;
                    string contentType = xml.Attribute("TitleLine1").Value;

                    ReportRecord.reportRecord.Name = name;
                    ReportRecord.reportRecord.ParentReportSectionId = 3;
                    ReportRecord.reportRecord.ContentType = contentType;
                    BMTDataContext.SubmitChanges();

                    ReportRecord.customReport.ReportSectionId = sectionId;
                    ReportRecord.customReport.UserId = userId;
                    ReportRecord.customReport.XML = xml;
                    BMTDataContext.SubmitChanges();

                    ReportRecord.enterpriseReportSection.EnterpriseId = enterpriseId;
                    ReportRecord.enterpriseReportSection.ReportSectionId = sectionId;
                    BMTDataContext.SubmitChanges();


                }
                return true;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        #endregion
    }
}