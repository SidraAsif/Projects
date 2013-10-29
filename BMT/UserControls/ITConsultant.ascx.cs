using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Xml;
using BMT.WEB;
using BMTBLL;
using BMTBLL.Enumeration;

public partial class ITConsultant : System.Web.UI.UserControl
{
    #region PROPERTIES & VARIABLES

    public int PracticeId
    {
        get;
        set;
    }


    ITconsultanBO consultant = new ITconsultanBO();
    Email email = new Email();

    string oldPractice = "";
    string oldConsultant = "";
    string oldRelation = "";
    string oldXml = "";    

    #endregion

    #region EVENTS

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.Visible)
                return;

            GetDataForFeaturedConsultant();

            if (!IsPostBack)
            {
                if (consultant.CheckNullData(PracticeId))
                {
                    RBNone.Checked = true;
                }

                string data = consultant.CheckXMLData(PracticeId);
                if (data != null)
                {
                    LoadXml(data);
                    RBForm.Checked = true;
                    PnlEntryForm.Visible = true;
                }
            }
        }
        catch (Exception)
        {            
            throw;
        }
    }

    protected void Btn_Submit_Click(object sender, EventArgs e)
    {

        try
        {
            message.Clear("");
            PracticeConsultant consltnt = consultant.GetPracticeConsultantByPracticeId(PracticeId);
            if (consltnt != null)
            {
                oldPractice = consultant.GetPracticeName(PracticeId);
                oldConsultant = consultant.GetUserName(Convert.ToInt32(consltnt.ConsultantId));
                oldRelation = consultant.GetRelationship(Convert.ToInt32(consltnt.RelationshipId));
                oldXml = consultant.CheckXMLData(PracticeId);
            }

            if (RBForm.Checked)
            {
                if (TxtCompany.Text == "Enter Text" || TxtFullName.Text == "Enter Text" || TxtAddress.Text == "Enter Text"
                    || TxtEmail.Text == "Enter Text" || TxtPhone.Text == "Enter Text" || TxtState.Text == "Enter Text"
                    || TxtZip.Text == "Enter Text" || TxtCity.Text == "Enter Text"

                    || TxtCompany.Text == "" || TxtFullName.Text == "" || TxtAddress.Text == ""
                    || TxtEmail.Text == "" || TxtPhone.Text == "" || TxtState.Text == ""
                    || TxtZip.Text == "" || TxtCity.Text == "")
                {
                    //ClearControls();
                    Page.Validate("submit2");
                    message.Error("Please fill all required fields.");
                    return;
                }

                XDocument xml = CreateXML();

                if (oldConsultant == null && oldRelation == null && oldXml == xml.ToString())
                {
                    message.Error("Record already exists.");
                    return;
                }

                else
                {
                    if (consltnt != null)
                    {
                        consultant.UpdatePracticeConsultant(consltnt.PracticeConsultantId, 0, 0, xml.Root);
                        SendMail(oldPractice, oldConsultant, oldRelation, oldXml, null, null, xml.ToString());
                        message.Success("Record has been updated successfully.");
                        PnlEntryForm.Visible = false;
                        ClearControls();
                        if (HdnChangedUserId.Value != "")
                        {
                            DropDownList ddl = (DropDownList)this.FindControl(HdnChangedUserId.Value + "DDL");
                            ddl.SelectedValue = "0";
                            HdnChangedUserId.Value = "";
                        }
                        return;
                    }
                    else
                    {
                        consultant.InsertPracticeConsultant(PracticeId, 0, 0, xml.Root);
                        SendMail(oldPractice, oldConsultant, oldRelation, oldXml, null, null, xml.ToString());
                        message.Success("Record has been inserted successfully.");
                        PnlEntryForm.Visible = false;
                        ClearControls();
                        return;
                    }

                }
            }

            else if (RBNone.Checked)
            {


                if (oldConsultant == null && oldRelation == null && oldXml == null)
                {
                    message.Error("Record already exists.");
                    return;
                }

                else
                {
                    if (consltnt != null)
                    {
                        consultant.UpdatePracticeConsultant(consltnt.PracticeConsultantId, 0, 0, null);
                        SendMail(oldPractice, oldConsultant, oldRelation, oldXml, null, null, null);
                        message.Success("Record has been updated successfully.");
                        if (HdnChangedUserId.Value != "")
                        {
                            DropDownList ddl = (DropDownList)this.FindControl(HdnChangedUserId.Value + "DDL");
                            ddl.SelectedValue = "0";
                            HdnChangedUserId.Value = "";
                        }
                        return;
                    }
                    else
                    {
                        consultant.InsertPracticeConsultant(PracticeId, 0, 0, null);
                        SendMail(oldPractice, oldConsultant, oldRelation, oldXml, null, null, null);
                        message.Success("Record has been inserted successfully.");
                        return;
                    }


                }

            }
            else
            {
                if (HdnUserId.Value != "")
                {
                    DropDownList ddl = (DropDownList)this.FindControl(HdnUserId.Value + "DDL");
                    string DdlValue = ddl.SelectedValue.ToString();

                    if (DdlValue == "0")
                    {
                        message.Error("Please select relation.");
                        return;
                    }

                    if (oldConsultant == consultant.GetNewUserName(Convert.ToInt32(HdnUserId.Value)) && oldRelation == consultant.GetRelationship(Convert.ToInt32(DdlValue))
                        && oldXml == null)
                    {
                        message.Error("Record already exists.");
                        return;
                    }

                    else
                    {
                        if (consltnt != null)
                        {
                            consultant.UpdatePracticeConsultant(consltnt.PracticeConsultantId, Convert.ToInt32(HdnUserId.Value), Convert.ToInt32(DdlValue), null);

                            SendMail(oldPractice, oldConsultant, oldRelation, oldXml, consultant.GetUserName(Convert.ToInt32(HdnUserId.Value)),
                           consultant.GetRelationship(Convert.ToInt32(DdlValue)), null);

                            message.Success("Record has been updated successfully.");

                            if (HdnChangedUserId.Value != "")
                            {
                                DropDownList ddlist = (DropDownList)this.FindControl(HdnChangedUserId.Value + "DDL");
                                ddlist.SelectedValue = "0";
                                HdnChangedUserId.Value = "";
                            }
                            return;

                        }
                        else
                        {
                            consultant.InsertPracticeConsultant(PracticeId, Convert.ToInt32(HdnUserId.Value), Convert.ToInt32(DdlValue), null);

                            SendMail(oldPractice, oldConsultant, oldRelation, oldXml, consultant.GetUserName(Convert.ToInt32(HdnUserId.Value)),
                           consultant.GetRelationship(Convert.ToInt32(DdlValue)), null);

                            message.Success("Record has been inserted successfully.");
                            return;
                        }


                    }


                }

                else
                {

                    message.Error("Record already exists.");

                }
            }


            ClearControls();

        }
        catch (Exception)
        {

            message.Error("Your record cannot be inserted. Please try again.");
        }

    }

    protected void Btn_Cancel_Click(object sender, EventArgs e)
    {

        ClearControls();
    }

    protected void CheckedChanged(object sender, EventArgs e)
    {
        HdnUserId.Value = ((RadioButton)sender).ID; //consultant.GetUserIdByPracticeId(Convert.ToInt32(((RadioButton)sender).ID)).ToString();
        PnlEntryForm.Visible = false;
        RadioButton rButton = (RadioButton)this.FindControl(HdnUserId.Value);
        rButton.Checked = true;
        RBNone.Checked = false;
        HdnIsChanged.Value = "true";
    }

    protected void IndexChanged(object sender, EventArgs e)
    {
        string pId = ((DropDownList)sender).ID.Replace("DDL", "");
        HdnUserId.Value = pId;  //consultant.GetUserIdByPracticeId(Convert.ToInt32(pId)).ToString();
        RadioButton rButton = (RadioButton)this.FindControl(HdnUserId.Value);
        rButton.Checked = true;
        RBNone.Checked = false;
        HdnIsChanged.Value = "true";
    }

    protected void RBForm_CheckedChanged(object sender, EventArgs e)
    {

        if (RBForm.Checked)
        {
            PnlEntryForm.Visible = true;
            RBNone.Checked = false;
        }
        else
            PnlEntryForm.Visible = false;
    }

    protected void RBNone_CheckedChanged(object sender, EventArgs e)
    {

        PnlEntryForm.Visible = false;
    }


    #endregion

    #region FUNCTIONS
    protected void GetDataForFeaturedConsultant()
    {

        try
        {
            int enterpriseId = Convert.ToInt32(Session[enSessionKey.EnterpriseId.ToString()]);

            Table TblData = new Table();
            TblData.Attributes.Add("Width", "100%");
            TblData.Attributes.Add("table-layout", "fixed");
            List<ConsultantDetails> data = consultant.GetFeaturedConsultants(enterpriseId);

            PracticeConsultant pCnst = consultant.GetPracticeAndRelationship(PracticeId);

            foreach (ConsultantDetails dt in data)
            {
                TableRow tr = new TableRow();

                TableCell tc1 = new TableCell();
                RadioButton RBid = new RadioButton();
                RBid.ID = dt.UserID.ToString();
                RBid.AutoPostBack = true;
                RBid.CheckedChanged += new EventHandler(CheckedChanged);
                RBid.GroupName = "RButton";

                if (PracticeId != 0)
                {
                    if (pCnst == null)
                    {
                        if (HdnIsChanged.Value == "false" || HdnIsChanged.Value == "")
                            RBNone.Checked = true;
                    }
                    else
                    {

                        if (pCnst.PracticeId == PracticeId && pCnst.RelationshipId != 0 && pCnst.ConsultantId == dt.UserID)
                        {
                            HdnChangedUserId.Value = dt.UserID.ToString();
                            RBid.Checked = true;
                        }
                        else
                            RBid.Checked = false;
                    }

                }

                tc1.Controls.Add(RBid);
                tc1.Width = System.Web.UI.WebControls.Unit.Pixel(5);

                TableCell tc2 = new TableCell();
                tc2.Width = 90;
                tc2.Height = 60;

                if (dt.Website != string.Empty && dt.LogoPath != null)
                {
                    Image logo = new Image();                    
                    logo.ImageUrl = dt.LogoPath;
                    logo.Width = 90;
                    logo.Height = 60;

                    Panel imagePanel = new Panel();
                    imagePanel.Controls.Add(logo);

                    HyperLink _hyperLink = new HyperLink();
                    _hyperLink.NavigateUrl = "https://" + dt.Website;
                    _hyperLink.Target = "_blank";

                    _hyperLink.Controls.Add(imagePanel);

                    tc2.Controls.Add(_hyperLink);
                    tc2.Width = System.Web.UI.WebControls.Unit.Pixel(95);
                }
                else
                {
                    tc2.Width = System.Web.UI.WebControls.Unit.Pixel(95);
                }

                TableCell tc3 = new TableCell();
                tc3.Text = dt.CompanyName;
                tc3.Width = System.Web.UI.WebControls.Unit.Pixel(95);
                tc3.Attributes.Add("white-space", "normal");
                tc3.HorizontalAlign = HorizontalAlign.Center;

                TableCell tc4 = new TableCell();
                tc4.Text = dt.City;
                tc4.Width = System.Web.UI.WebControls.Unit.Pixel(80);
                tc4.Attributes.Add("white-space", "normal");
                tc4.HorizontalAlign = HorizontalAlign.Center;

                TableCell tc5 = new TableCell();
                tc5.Text = dt.State;
                tc5.Width = System.Web.UI.WebControls.Unit.Pixel(90);
                tc5.Attributes.Add("white-space", "normal");
                tc5.HorizontalAlign = HorizontalAlign.Center;

                TableCell tc6 = new TableCell();
                tc6.Text = dt.ServiceArea;
                tc6.Width = System.Web.UI.WebControls.Unit.Pixel(85);
                tc6.Attributes.Add("white-space", "normal");
                tc6.HorizontalAlign = HorizontalAlign.Center;


                TableCell tc7 = new TableCell();
                tc7.Text = dt.Phone;
                tc7.Width = System.Web.UI.WebControls.Unit.Pixel(85);
                tc7.Attributes.Add("white-space", "normal");
                tc7.HorizontalAlign = HorizontalAlign.Center;


                TableCell tc8 = new TableCell();
                DropDownList relation = new DropDownList();
                relation.ID = dt.UserID.ToString() + "DDL";
                relation.DataSource = consultant.GetRelationship();
                relation.DataValueField = "relationshipId";
                relation.DataTextField = "Name";
                relation.AutoPostBack = true;
                relation.SelectedIndexChanged += new EventHandler(IndexChanged);
                relation.DataBind();
                relation.Items.Insert(0, new ListItem("--- Select ---", "0"));

                if (pCnst != null)
                {
                    if (pCnst.RelationshipId != null && pCnst.ConsultantId == dt.UserID)
                        relation.SelectedValue = pCnst.RelationshipId.ToString();
                }
                relation.Width = System.Web.UI.WebControls.Unit.Pixel(85);
                tc8.Controls.Add(relation);
                tc8.Width = System.Web.UI.WebControls.Unit.Pixel(95);

                TableCell tc9 = new TableCell();
                HyperLink link = new HyperLink();
                link.NavigateUrl = "mailto:" + dt.Email;
                link.ImageUrl = "../Themes/Images/email-icon.png";
                link.ToolTip = "Send Email";
                tc9.HorizontalAlign = HorizontalAlign.Left;
                tc9.Controls.Add(link);

                tr.Cells.Add(tc1);
                tr.Cells.Add(tc2);
                tr.Cells.Add(tc3);
                tr.Cells.Add(tc4);
                tr.Cells.Add(tc5);
                tr.Cells.Add(tc6);
                tr.Cells.Add(tc7);
                tr.Cells.Add(tc8);
                tr.Cells.Add(tc9);

                TblData.Controls.Add(tr);

            }

            PnlGrid.Controls.Add(TblData);


        }
        catch (Exception ex)
        {

            ex.InnerException.Message.ToString();
        }
    }


    protected XDocument CreateXML()
    {

        try
        {
            XmlDocument xmldoc = new XmlDocument();
            XmlElement root = xmldoc.CreateElement("ITConsultant");
            xmldoc.AppendChild(root);

            XmlElement data1 = xmldoc.CreateElement("Data");
            data1.SetAttribute("Name", "ConsultingCompany");
            data1.SetAttribute("Value", TxtCompany.Text);
            root.AppendChild(data1);

            XmlElement data2 = xmldoc.CreateElement("Data");
            data2.SetAttribute("Name", "ConsultantFullName");
            data2.SetAttribute("Value", TxtFullName.Text);
            root.AppendChild(data2);

            XmlElement data3 = xmldoc.CreateElement("Data");
            data3.SetAttribute("Name", "Address");
            data3.SetAttribute("Value", TxtAddress.Text);
            root.AppendChild(data3);

            XmlElement data4 = xmldoc.CreateElement("Data");
            data4.SetAttribute("Name", "Email");
            data4.SetAttribute("Value", TxtEmail.Text);
            root.AppendChild(data4);

            XmlElement data5 = xmldoc.CreateElement("Data");
            data5.SetAttribute("Name", "City");
            data5.SetAttribute("Value", TxtCity.Text);
            root.AppendChild(data5);

            XmlElement data6 = xmldoc.CreateElement("Data");
            data6.SetAttribute("Name", "Phone");
            data6.SetAttribute("Value", TxtPhone.Text);
            root.AppendChild(data6);

            XmlElement data7 = xmldoc.CreateElement("Data");
            data7.SetAttribute("Name", "State");
            data7.SetAttribute("Value", TxtState.Text);
            root.AppendChild(data7);

            XmlElement data8 = xmldoc.CreateElement("Data");
            data8.SetAttribute("Name", "Zip");
            data8.SetAttribute("Value", TxtZip.Text);
            root.AppendChild(data8);

            return XDocument.Parse(xmldoc.OuterXml);

        }
        catch (Exception)
        {

            return null;
        }
    }

    protected void ClearControls()
    {

        TxtCompany.Text = "";
        TxtFullName.Text = "";
        TxtAddress.Text = "";
        TxtEmail.Text = "";
        TxtCity.Text = "";
        TxtPhone.Text = "";
        TxtState.Text = "";
        TxtZip.Text = "";

    }

    protected void LoadXml(string xml)
    {

        try
        {

            var data = XDocument.Parse(xml);
            var recordList = new List<string>();
            foreach (var section in data.Root.Elements("Data"))
            {

                recordList.Add(section.Attribute("Value").Value);
            }

            TxtCompany.Text = recordList.ElementAt(0);
            TxtFullName.Text = recordList.ElementAt(1);
            TxtAddress.Text = recordList.ElementAt(2);
            TxtEmail.Text = recordList.ElementAt(3);
            TxtCity.Text = recordList.ElementAt(4);
            TxtPhone.Text = recordList.ElementAt(5);
            TxtState.Text = recordList.ElementAt(6);
            TxtZip.Text = recordList.ElementAt(7);

        }
        catch (Exception ex)
        {

            throw ex;
        }

    }

    protected List<string> ReadXml(string xml)
    {

        try
        {

            var data = XDocument.Parse(xml);
            var recordList = new List<string>();
            foreach (var section in data.Root.Elements("Data"))
            {

                recordList.Add(section.Attribute("Value").Value);
            }

            return recordList;

        }
        catch
        {

            return null;
        }

    }

    protected void SendMail(string oldPractice,
            string oldConsultant,
            string oldRelation,
            string oldXml,
            string newConsultant,
            string newRelation,
            string newXml)
    {

        try
        {

            string subject = "Change of user's consultant";
            string userName = consultant.UserName(PracticeId);
            string loginName = consultant.LoginName(PracticeId);
            string body = "Hello Administrator,<br/><br/>";

            body += userName + " [" + loginName + "] has changed consultants. Details are given below:<br/>";

            if (oldPractice != "")
            {

                body += "<br/>Previous consultant:<br/><br/>";

                if (oldConsultant == null && oldRelation == null && oldXml != null)
                {

                    List<string> list = ReadXml(oldXml);
                    body += "Consulting Company : " + list.ElementAt(0) + "<br/>";
                    body += "Consultant Full Name : " + list.ElementAt(1) + "<br/>";
                    body += "Address : " + list.ElementAt(2) + "<br/>";
                    body += "Email : " + list.ElementAt(3) + "<br/>";
                    body += "City : " + list.ElementAt(4) + "<br/>";
                    body += "Phone : " + list.ElementAt(5) + "<br/>";
                    body += "State : " + list.ElementAt(6) + "<br/>";
                    body += "Zip : " + list.ElementAt(7) + "<br/>";
                }

                else if (oldConsultant == null && oldRelation == null && oldXml == null)
                {
                    body += "The user was using its internal resources for this assessment.<br/>";
                }

                else if (oldConsultant != null && oldRelation != null && oldXml == null)
                {

                    body += "Consultant : " + oldConsultant + "<br/>";
                    body += "Relation : " + oldRelation + "<br/>";
                }

                body += "<br/>New consultant:<br/><br/>";

                if (newConsultant == null && newRelation == null && newXml != null)
                {

                    List<string> lists = ReadXml(newXml);
                    body += "Consulting Company : " + lists.ElementAt(0) + "<br/>";
                    body += "Consultant Full Name : " + lists.ElementAt(1) + "<br/>";
                    body += "Address : " + lists.ElementAt(2) + "<br/>";
                    body += "Email : " + lists.ElementAt(3) + "<br/>";
                    body += "City : " + lists.ElementAt(4) + "<br/>";
                    body += "Phone : " + lists.ElementAt(5) + "<br/>";
                    body += "State : " + lists.ElementAt(6) + "<br/>";
                    body += "Zip : " + lists.ElementAt(7) + "<br/>";
                }

                else if (newConsultant == null && newRelation == null && newXml == null)
                {
                    body += "The user is using its internal resources for this assessment now.<br/>";
                }

                else if (newConsultant != null && newRelation != null && newXml == null)
                {

                    body += "Consultant : " + newConsultant + "<br/>";
                    body += "Relation : " + newRelation + "<br/>";
                }

            }
            else
            {

                body += "<br/>New consultant:<br/><br/>";

                if (newConsultant == null && newRelation == null && newXml != null)
                {

                    List<string> lists = ReadXml(newXml);
                    body += "Consulting Company : " + lists.ElementAt(0) + "<br/>";
                    body += "Consultant Full Name : " + lists.ElementAt(1) + "<br/>";
                    body += "Address : " + lists.ElementAt(2) + "<br/>";
                    body += "Email : " + lists.ElementAt(3) + "<br/>";
                    body += "City : " + lists.ElementAt(4) + "<br/>";
                    body += "Phone : " + lists.ElementAt(5) + "<br/>";
                    body += "State : " + lists.ElementAt(6) + "<br/>";
                    body += "Zip : " + lists.ElementAt(7) + "<br/>";
                }

                else if (newConsultant == null && newRelation == null && newXml == null)
                {
                    body += "The user is using its internal resources for this assessment now.<br/>";
                }

                else if (newConsultant != null && newRelation != null && newXml == null)
                {

                    body += "Consultant : " + newConsultant + "<br/>";
                    body += "Relation : " + newRelation + "<br/>";
                }

            }

            email.SendEmail(System.Configuration.ConfigurationManager.AppSettings["EmailToAddress"], subject, body);

        }
        catch (Exception ex)
        {

            ex.InnerException.Message.ToString();
        }

    }

    #endregion
}