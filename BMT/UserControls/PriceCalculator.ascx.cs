#region Modification History

//  ******************************************************************************
//  Module        : EHR Price Calculator
//  Created By    : Mirza Fahad Ali Baig
//  When Created  : 
//  Description   : EHR/PM Total Cost of Ownership (TOC) - 5 Years Comparison
//
//  ********************************** Modification History **********************
//  Who                     Date            Description
//  *******************************************************************************
//  *******************************************************************************

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Xml;
using System.Xml.Linq;

using BMT.Validation;
using BMTBLL.Enumeration;
using BMT.WEB;
using BMTBLL;
namespace BMT.UserControls
{
    public partial class PriceCalculator : System.Web.UI.UserControl
    {
        #region Data Memeber
        private QuestionBO _questionBO;
        private ProjectBO _projectBO;

        #endregion

        #region PROPERTIES
        public bool IsReturn { get; set; }
        public bool IsReset
        {
            get
            {
                if (Session["EHRIsReset"] == null)
                    return false;
                return (bool)Session["EHRIsReset"];
            }
            set
            {
                Session["EHRIsReset"] = value;
            }
        }
        public int ProjectUsageId { get; set; }
        public int SectionId { get; set; }
        #endregion

        #region CONSTANTS
        // Default values
        private const string DEFAULT_PRACTICE_PROJECT_NAME = "Practice Project";
        private const string DEFAULT_PRACTICE_PROJECT_DESCRIPTION = "Created for Practice level questionnaire";
        private const int DEFAULT_SYSTEM_ID = 1;
        private const string CONTROL_NAME = "Form";

        // Default css styles
        private const string DEFAULT_FEES_TITLE_LABLE_TITLE = "label-fee-title";
        private const string DEFAULT_FEES_TABLE_HEADER_MAIN_CLASS = "table-headerMain";
        private const string DEFAULT_FEES_TABLE_CLASS = "table-fees";
        private const string DEFAULT_NORMAL_TEXTBOX_CLASS = "txt-normal";
        private const string DEFAULT_FEES_TEXTBOX_CLASS = "txt-fee";
        private const string DEFAULT_FEES_DROPDOWN_CLASS = "ddl-fee";
        private const string DEFAULT_FEES_HYPERLINK_CLASS = "link-fee";
        private const string DEFAULT_HIDDEN_CLASS = "hidden";

        private const char DEFAULT_TEXT_SEPERATOR = ':';
        private const string DEFAULT_REQ_TEXT_SEPERATOR = "<span class='asterik'>*</span>";

        // Default control ids prefixes
        private const string DEFAULT_HIDDEN_FIELD_ID_PREFIX = "hdnMultiValue";
        private const string DEFAULT_TABLE_ID_PREFIX = "table";
        private const string DEFAULT_SYSTEM_TEXT_PREFIX = "ControlSystem";
        private const string DEFAULT_ONGOING_TEXT_PREFIX = "ControlOnGoing";
        private const string DEFAULT_ONETIME_TEXT_PREFIX = "ControlOneTime";

        // Default control id to create pattern
        private const string DEFAULT_QUANTITY_CONTROL_SEQUENCE = "1";
        private const string DEFAULT_TYPE_CONTROL_SEQUENCE = "2";
        private const string DEFAULT_AMOUNT_CONTROL_SEQUENCE = "3";
        private const string DEFAULT_PAYMENT_METHOD_CONTROL_SEQUENCE = "4";
        private const string DEFAULT_GENERIC_CONTROL_SEQUENCE = "5";

        // Answers
        private const string DEFAULT_APPROVED_ANSWER = "Yes";
        private const string DEFAULT_REJECTED_ANSWER = "No";

        // intial selected text & values
        private const string DEFAULT_EMPTY_TEXT = "Select";
        private const string DEFAULT_EMPTY_VALUE = "0";

        // TYPE VALUES
        private const string DEFAULT_EHR_INTERFACE_TYPE_TEXT = "Interface";
        private const string DEFAULT_EHR_OTHER_TYPE_TEXT = "Other";

        // Max lenght of textBoxes
        private const int DEFAULT_QNTY_MAX_LENGTH = 4;
        private const int DEFAULT_AMOUNT_MAX_LENGTH = 10;
        private const int DEFAULT_OTHER_MAX_LENGTH = 50;

        // Validation css class
        private const string DEFAULT_CELL1_CLASS = "cell1";
        private const string DEFAULT_CELL2_CLASS = "cell2";
        private const string DEFAULT_CELL3_CLASS = "cell3";
        private const string DEFAULT_CELL4_CLASS = "cell4";
        private const string DEFAULT_CELL5_CLASS = "cell5";
        private const string DEFAULT_COMMON_CELL1_CLASS = "common-cell1"; // colspan 2
        private const string DEFAULT_COMMON_CELL2_CLASS = "common-cell2"; // colspan 3

        private const string DEFAULT_QNTY_VALIDATION_CLASS = "txt-quantity";
        private const string DEFAULT_AMOUNT_VALIDATION_CLASS = "txt-Amount";
        private const string DEFAULT_OTHER_VALIDATION_CLASS = "txt-Other";

        // Image URL
        private const string DEFAULT_INFO_IMAGE_URL = "~/Themes/Images/info.png";

        // System Prefix
        private const string DEFAULT_ACTIVE_SYSTEM_PREFIX = "System #";

        // Control name
        private const string DEFAULT_CONTROL_NAME = "PriceCalculator";

        #endregion

        #region CONTROLS
        private Label _label;
        private TextBox _textbox;
        private DropDownList _dropDownList;
        private Table _table;
        private TableRow _tableRow;
        private TableCell _tableCell;
        private HyperLink _hyperlink;
        private HiddenField _hiddenField;
        private Image _image;
        private RequiredFieldValidator _requiredFieldValidator;
        private RegularExpressionValidator _regularExpressionValidator;

        #endregion

        #region VARIABLES
        private int practiceId;
        //private int projectUsageId
        //{
        //    get
        //    {
        //        if (Session["EHRprojectId"] == null)
        //            return 0;
        //        return (int)Session["EHRprojectId"];
        //    }
        //    set
        //    {
        //        Session["EHRprojectId"] = value;
        //    }
        //}
        private int questionnaireId
        {
            get
            {
                if (Session["EHRquestionnaireId"] == null)
                    return 0;
                return (int)Session["EHRquestionnaireId"];
            }
            set
            {
                Session["EHRquestionnaireId"] = value;
            }
        }

        private int systemId
        {
            get
            {
                if (Session["EHRsystemId"] == null)
                    return 0;
                return (int)Session["EHRsystemId"];
            }
            set
            {
                Session["EHRsystemId"] = value;
            }
        }

        private int totalAllowedSystems
        {
            get
            {
                if (Session["EHRTotalAllowedSystems"] == null)
                    return 0;
                return (int)Session["EHRTotalAllowedSystems"];
            }
            set
            {
                Session["EHRTotalAllowedSystems"] = value;
            }
        }
        private int activeSystems
        {
            get
            {
                if (Session["EHRActiveSystems"] == null)
                    return 0;
                return (int)Session["EHRActiveSystems"];
            }
            set
            {
                Session["EHRActiveSystems"] = value;
            }
        }
        private string tempSystemName = string.Empty;

        private int userId;

        private XDocument filledQuestionnaire
        {
            get
            {
                if (Session["EHRFilledQuestionnaire"] == null)
                    return null;
                return (XDocument)Session["EHRFilledQuestionnaire"];
            }
            set
            {
                Session["EHRFilledQuestionnaire"] = value;
            }
        }
        private XDocument subscriptionQuestionnaire
        {
            get
            {
                if (Session["subscriptionQuestionnaire"] == null)
                    return null;
                return (XDocument)Session["subscriptionQuestionnaire"];
            }
            set
            {
                Session["subscriptionQuestionnaire"] = value;
            }
        }
        private XDocument licenseQuestionnaire
        {
            get
            {
                if (Session["licenseQuestionnaire"] == null)
                    return null;
                return (XDocument)Session["licenseQuestionnaire"];
            }
            set
            {
                Session["licenseQuestionnaire"] = value;
            }
        }
        private XDocument backUpQuestionnaire
        {
            get
            {
                if (Session["EHRBackUpQuestionnaire"] == null)
                    return null;
                return (XDocument)Session["EHRBackUpQuestionnaire"];
            }
            set
            {
                Session["EHRBackUpQuestionnaire"] = value;
            }
        }

        protected bool _isOnGoing
        {
            get
            {
                if (Session["EHRisOnGoing"] == null)
                    return false;
                return (bool)Session["EHRisOnGoing"];
            }
            set
            {
                Session["EHRisOnGoing"] = value;
            }
        }

        protected bool _isSaved
        {
            get
            {
                if (Session["EHRisSaved"] == null)
                    return false;
                return (bool)Session["EHRisSaved"];
            }
            set
            {
                Session["EHRisSaved"] = value;
            }
        }

        protected bool _isDeleted
        {
            get
            {
                if (Session["EHRisDeleted"] == null)
                    return false;
                return (bool)Session["EHRisDeleted"];
            }
            set
            {
                Session["EHRisDeleted"] = value;
            }
        }

        protected bool _isSystemChanged
        {
            get
            {
                if (Session["EHRisSystemChanged"] == null)
                    return false;
                return (bool)Session["EHRisSystemChanged"];
            }
            set
            {
                Session["EHRisSystemChanged"] = value;
            }
        }

        #endregion

        #region EVENTS
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // TODO: Reset Control when practice changed
            if (IsReset)
            {
                filledQuestionnaire = subscriptionQuestionnaire = licenseQuestionnaire = backUpQuestionnaire = null;
                ProjectUsageId = questionnaireId = systemId = totalAllowedSystems = activeSystems = userId = 0;
                IsReset = false;
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.Visible)
                    return;
                PageLoadingProcess();
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }

        protected void rbQuestionnaire_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (divSystem.Controls.Count > 1)
                {
                    TextBox txtSystem = (TextBox)divSystem.FindControl(DEFAULT_SYSTEM_TEXT_PREFIX + (int)questionnaireId + systemId.ToString());
                    tempSystemName = txtSystem.Text;
                }

                if (activeSystems > 0)
                {
                    if (filledQuestionnaire != null)
                    {
                        XElement elements = (from elementRecord in filledQuestionnaire.Root.Descendants(enQuestionnaireElements.System.ToString())
                                             where (string)elementRecord.Attribute(enQuestionnaireAttr.sequence.ToString()).Value == systemId.ToString()
                                             select elementRecord).FirstOrDefault();

                        if (elements != null)
                        {
                            if (backUpQuestionnaire == null)
                            {
                                backUpQuestionnaire = XDocument.Parse("<root></root>");
                                backUpQuestionnaire.Root.Add(elements);
                            }
                            else
                            {
                                XElement elementBackup = (from elementRecord in backUpQuestionnaire.Descendants(enQuestionnaireElements.System.ToString())
                                                          where (string)elementRecord.Attribute(enQuestionnaireAttr.sequence.ToString()).Value == systemId.ToString()
                                                          select elementRecord).FirstOrDefault();
                                if (elementBackup == null)
                                    backUpQuestionnaire.Root.Add(elements);
                                else
                                {
                                    IEnumerable<XElement> removeChangedElement = (from elementRecord in filledQuestionnaire.Root.Descendants(enQuestionnaireElements.System.ToString())
                                                                                  where (string)elementRecord.Attribute(enQuestionnaireAttr.sequence.ToString()).Value == systemId.ToString()
                                                                                  select elementRecord);
                                    removeChangedElement.Remove();
                                    filledQuestionnaire.Root.Add(elementBackup);
                                }
                            }
                        }

                        GetSystemQuestionnaire();
                    }
                    else
                        GetSystemQuestionnaire();

                    if (rbQuestionnaire.SelectedValue == ((int)enQuestionnaireType.License).ToString())
                        Response.Redirect(Request.RawUrl);
                }
                else
                {
                    rbQuestionnaire.ClearSelection();
                    message.Error("No system exists. Please Add new system and try again.");
                }

            }
            catch (Exception exception)
            {
                pnlFee.Visible = pnlOperation.Visible = false;
                Logger.PrintError(exception);
            }

        }

        protected void btnSaveAndContinue_Click(object sender, EventArgs e)
        {
            try
            {
                SaveQuestionnaire(true);
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }

        protected void btnTriggerSwitchSystem_Click(object sender, EventArgs e)
        {
            try
            {
                systemId = hdnSystemId.Value.Trim() != string.Empty ? Convert.ToInt32(hdnSystemId.Value.Trim()) : DEFAULT_SYSTEM_ID;

                lblSystemInfo.Text = DEFAULT_ACTIVE_SYSTEM_PREFIX + systemId.ToString();

                if (filledQuestionnaire != null)
                {
                    IEnumerable<XElement> systemElement = (from systemRecord in filledQuestionnaire.Descendants(enQuestionnaireElements.System.ToString())
                                                           where (string)systemRecord.Attribute(enQuestionnaireAttr.sequence.ToString()).Value == systemId.ToString()
                                                           select systemRecord);
                    if (systemElement.Count() != 0)
                    {
                        // Hack: Temporary solution to re-generate the questionnaire for next system
                        _isSystemChanged = true;
                        Response.Redirect(Request.RawUrl);
                    }
                    else
                    {
                        rbQuestionnaire.ClearSelection();
                        DisablePanels();

                        // auto select the purchase model for new system
                        if (sender == null && e == null)
                        {
                            rbQuestionnaire.SelectedValue = ((int)enQuestionnaireType.Subscription).ToString();
                            GetSystemQuestionnaire();
                        }
                    }
                }

            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }

        protected void btnTriggerClearContents_Click(object sender, EventArgs e)
        {
            try
            {
                Logger.PrintInfo("Clearing Contents against UserId +" + userId);

                IEnumerable<XElement> systemElement;

                if (filledQuestionnaire != null)
                {
                    // Remove questionniare from filledQuestionnaire
                    systemElement = (from systemRecord in filledQuestionnaire.Descendants(enQuestionnaireElements.System.ToString())
                                     where (string)systemRecord.Attribute(enQuestionnaireAttr.sequence.ToString()).Value == systemId.ToString()
                                     select systemRecord);

                    if (systemElement.Count() > 0)
                        systemElement.Remove();
                }

                if (backUpQuestionnaire != null)
                {
                    // Remove questionniare from backUpquestionnaire
                    systemElement = (from systemRecord in backUpQuestionnaire.Descendants(enQuestionnaireElements.System.ToString())
                                     where (string)systemRecord.Attribute(enQuestionnaireAttr.sequence.ToString()).Value == systemId.ToString()
                                     select systemRecord);

                    if (systemElement.Count() > 0)
                        systemElement.Remove();
                }

                // To re-create empty voucher
                questionnaireId = (int)enQuestionnaireType.Subscription;
                AddSystem();


                // Save New questionniare to Db
                rbQuestionnaire.SelectedValue = ((int)enQuestionnaireType.Subscription).ToString();
                SaveQuestionnaireToDB(false);
                Response.Redirect(Request.RawUrl);

            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }

        protected void btnDiscardChanges_Click(object sender, EventArgs e)
        {
            try
            {
                rbQuestionnaire.ClearSelection();
                GetSystemQuestionnaire();
                Response.Redirect(Request.RawUrl);
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }

        protected void btnTriggerAddEHR_Click(object sender, EventArgs e)
        {
            try
            {
                if (activeSystems == totalAllowedSystems)
                {
                    message.Warning("System(EHR/PM) limit reached.");
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), " SwapRow();DisbaleSystems();", true);

                    return;
                }
                else
                {
                    if (filledQuestionnaire != null && activeSystems > 0)
                    {
                        IEnumerable<XElement> availableSystems = (from systemRecords in filledQuestionnaire.Descendants(enQuestionnaireElements.System.ToString())
                                                                  where (string)systemRecords.Attribute(enQuestionnaireAttr.sequence.ToString()).Value == activeSystems.ToString()
                                                                  select systemRecords);
                        if (availableSystems.Count() == 0)
                        {
                            message.Error("Your request can't be proceed because data against the new System is missing.");
                            return;
                        }
                        else
                            EnableNewSystem();
                    }
                    else if (activeSystems == 0)
                    {
                        EnableNewSystem();
                        systemId = DEFAULT_SYSTEM_ID;
                    }
                    else
                    {
                        message.Error("Your request cann't be proceed because data against the System #" + systemId + " is missing.");
                        return;
                    }
                }

                // Select new system automatically
                if (sender != null && e != null)
                {
                    SaveQuestionnaire(false);
                    hdnSystemId.Value = activeSystems.ToString();
                    btnTriggerSwitchSystem_Click(null, null);
                    Response.Redirect(Request.RawUrl);
                }

            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }

        }

        protected void btnTriggerCalculatePrice_Click(object sender, EventArgs e)
        {
            try
            {
                SaveQuestionnaire(false);
                Session[enSessionKey.currentControl.ToString()] = DEFAULT_CONTROL_NAME;

                string Active = Request.QueryString["Active"] != null ? Request.QueryString["Active"] : string.Empty;
                string Path = Request.QueryString["Path"] != null ? Request.QueryString["Path"] : string.Empty;

                Response.Redirect("~/Webforms/Projects.aspx?report=" + DEFAULT_CONTROL_NAME + "&puid=" + ProjectUsageId + "&NodeSectionID=" + SectionId + "&Active=" + Active + "&Path=" + Path + "&system=" + systemId + "&activeVoucherId=" + rbQuestionnaire.SelectedValue);
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }
        }

        protected void btnTriggerDeleteSystem_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteSystem();
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), Guid.NewGuid().ToString(), " EnabledSystems();", true);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        #endregion

        #region FUNCTIONS
        private void PageLoadingProcess()
        {
            if (Request.QueryString["NodeContentType"] != null && Request.QueryString["NodeContentType"].ToString() == CONTROL_NAME)
            {
                // Check if session expire
                if (Session[enSessionKey.UserApplicationId.ToString()] != null && Session[enSessionKey.PracticeId.ToString()] != null)
                {
                    userId = Convert.ToInt32(Session[enSessionKey.UserApplicationId.ToString()]);
                    practiceId = Convert.ToInt32(Session[enSessionKey.PracticeId.ToString()]);
                }

                // clear message on each postback
                message.Clear("");

                // when it load from price comparison control
                if (IsReturn)
                {
                    systemId = Request.QueryString["systemId"] != null ? Convert.ToInt32(Request.QueryString["systemId"]) : 0;
                    hdnSystemId.Value = systemId.ToString();
                    rbQuestionnaire.SelectedValue = Request.QueryString["activeVoucherId"] != null ? Request.QueryString["activeVoucherId"] : string.Empty;

                    if (rbQuestionnaire.SelectedIndex != -1)
                        questionnaireId = Convert.ToInt32(rbQuestionnaire.SelectedValue);

                    lblSystemInfo.Text = DEFAULT_ACTIVE_SYSTEM_PREFIX + systemId.ToString();

                    IsReturn = false;
                }

                // Hack; Temporary solutoin to show the success message until not proper solution found.
                if (_isSaved)
                {
                    message.Success("Information saved successfully.");
                    rbQuestionnaire.SelectedValue = questionnaireId.ToString();
                    _isSaved = false;
                }
                else if (_isDeleted)
                {
                    message.Success("System Deleted Successfully.");
                    _isDeleted = false;
                }
                else if (_isSystemChanged) // Hack; Temporary solutoin to reload the questionnaire for next system.
                {
                    // clear previous system selection
                    rbQuestionnaire.ClearSelection();

                    // get and generate questionnaire against current system
                    GetSystemQuestionnaire();

                    lblSystemInfo.Text = DEFAULT_ACTIVE_SYSTEM_PREFIX + systemId.ToString();
                    _isSystemChanged = false;
                }

                // keep current system tab selected
                if (hdnSystemId.Value.Trim() == string.Empty && systemId > 0)
                    hdnSystemId.Value = systemId.ToString();

                GenerateQuestionnaire();

                // Generate system                
                GenerateSystemsTab();

                // select current system if filled questionnaire exist against current practice
                if (rbQuestionnaire.SelectedIndex == -1)
                    GetSystemQuestionnaire();

                // Add first system by default (if required)
                if (activeSystems == 0)
                    btnTriggerAddEHR_Click(null, null);
            }
            else
                return;

        }

        private void GenerateSystemsTab()
        {
            // Set Default System Id
            if (systemId == 0)
                systemId = DEFAULT_SYSTEM_ID;

            totalAllowedSystems = Convert.ToInt32(Util.GetMaxAllowedEHR());
            int count = 0;

            literalSystem.Text = "<ul class='tabs'>";

            while (count < totalAllowedSystems)
            {
                count += 1;
                GenerateSystemList(count);
            }

            literalSystem.Text += "</ul>";
        }

        private void GenerateSystemList(int systemNumber)
        {
            if (systemNumber <= activeSystems)
                literalSystem.Text += "<li id='" + systemNumber + "'>";
            else
                literalSystem.Text += "<li id='" + systemNumber + "' class='inactiveTab'>";

            literalSystem.Text += "<a href=\"javascript:SwitchSystem(" + systemNumber + ");\">System " + systemNumber + " </a>";
            literalSystem.Text += "</li>";
        }

        private void EnablePanels()
        {
            // clear existing controls         
            divSystem.Controls.Clear();
            divSubsOneTimeFees.Controls.Clear();
            divSubsOngoingFees.Controls.Clear();
            hdnTablesId.Value = string.Empty;
            pnlFee.Visible = pnlOperation.Visible = divSystem.Visible = divSubsOngoingFees.Visible = divSubsOneTimeFees.Visible = true;
        }

        private void DisablePanels()
        {
            // clear existing controls
            divSystem.Controls.Clear();
            divSubsOneTimeFees.Controls.Clear();
            divSubsOngoingFees.Controls.Clear();
            hdnTablesId.Value = string.Empty;
            pnlFee.Visible = pnlOperation.Visible = divSystem.Visible = divSubsOngoingFees.Visible = divSubsOneTimeFees.Visible = false;
        }

        private void GenerateQuestionnaire()
        {

            if (questionnaireId > (int)enQuestionnaireType.DetailedQuestionnaire && rbQuestionnaire.SelectedIndex != -1)
            {
                // get questionnaire
                GetQuestionnaire();

                // store static data to access later on client side
                StoreDataToClient();

                // Render Questionnaire
                RenderQuestionnaire(filledQuestionnaire.Root);

            }
            else
                GetQuestionnaire(); // get questionnaire

        }

        private void GetQuestionnaire()
        {

            if (ProjectUsageId == 0)
            {
                _projectBO = new ProjectBO();
                _projectBO.PracticeId = practiceId;
                _projectBO.Name = DEFAULT_PRACTICE_PROJECT_NAME;
                _projectBO.Description = DEFAULT_PRACTICE_PROJECT_DESCRIPTION;
                _projectBO.CreatedDate = System.DateTime.Now;
                _projectBO.CreatedBy = userId;

                // get project id by pracitce Id
                //projectUsageId = _projectBO.GetPracticeProjectUsageId();
            }

            _questionBO = new QuestionBO();
            _questionBO.ProjectUsageId = ProjectUsageId;

            // Get Only FilledQuestionnaire

            string receivedQuestionnaire = _questionBO.GetEHRQuestionnaire();

            if (filledQuestionnaire == null)
            {
                if (receivedQuestionnaire.Trim() != string.Empty)
                {
                    filledQuestionnaire = XDocument.Parse(receivedQuestionnaire);

                    // total active systems
                    IEnumerable<XElement> listOfSystem = (from systems in filledQuestionnaire.Root.Descendants(enQuestionnaireElements.System.ToString())
                                                          select systems);
                    activeSystems = listOfSystem.Count();
                }
            }


            // get fresh Questionnaire for both questionnaire
            _questionBO.ProjectId = 0;

            if (subscriptionQuestionnaire == null || licenseQuestionnaire == null)
            {
                _questionBO.QuestionnaireId = (int)enQuestionnaireType.Subscription;
                subscriptionQuestionnaire = XDocument.Parse(_questionBO.GetQuestionnaireByTypeEHR());

                _questionBO.QuestionnaireId = (int)enQuestionnaireType.License;
                licenseQuestionnaire = XDocument.Parse(_questionBO.GetQuestionnaireByTypeEHR());
            }

        }

        private void EnableNewSystem()
        {
            activeSystems += 1;

            //generate systems
            GenerateSystemsTab();

            lblSystemInfo.Text = DEFAULT_ACTIVE_SYSTEM_PREFIX + systemId.ToString();

            if (activeSystems == 1)
                systemId = 1;
        }

        private void AddSystem()
        {
            XDocument newQuestionnaire = questionnaireId == (int)enQuestionnaireType.Subscription ? subscriptionQuestionnaire : licenseQuestionnaire;


            foreach (XElement system in newQuestionnaire.Root.Elements())
            {
                system.Attribute(enQuestionnaireAttr.sequence.ToString()).Value = systemId.ToString();
                filledQuestionnaire.Root.Add(system);
                break;
            }

        }

        private void AddSystem(string systemName)
        {
            XDocument newQuestionnaire = questionnaireId == (int)enQuestionnaireType.Subscription ? subscriptionQuestionnaire : licenseQuestionnaire;


            foreach (XElement system in newQuestionnaire.Root.Elements())
            {
                system.Attribute(enQuestionnaireAttr.sequence.ToString()).Value = systemId.ToString();
                system.Attribute(enQuestionnaireAttr.name.ToString()).Value = systemName;
                filledQuestionnaire.Root.Add(system);
                break;
            }

        }

        private void GetSystemQuestionnaire()
        {
            int currentQuestionnaireId = 0;
            string questionType = string.Empty;

            if (rbQuestionnaire.SelectedIndex != -1)
                questionnaireId = Convert.ToInt32(rbQuestionnaire.SelectedValue);
            else
            {
                if (filledQuestionnaire == null)
                    return;
                else
                {
                    questionType = (from systemRecords in filledQuestionnaire.Descendants(enQuestionnaireElements.System.ToString())
                                    where (string)systemRecords.Attribute(enQuestionnaireAttr.sequence.ToString()).Value == systemId.ToString()
                                    select systemRecords.Attribute(enQuestionnaireAttr.description.ToString()).Value.Trim()).FirstOrDefault();

                    if (questionType != null)
                    {
                        questionnaireId = (int)Util.GetValueFromCategory<enQuestionnaireType>(questionType);
                        rbQuestionnaire.SelectedValue = questionnaireId.ToString();
                    }
                    else
                        return;
                }
            }


            string tempSubscription = subscriptionQuestionnaire.ToString();
            string tempLicense = licenseQuestionnaire.ToString();

            if (filledQuestionnaire == null)
                filledQuestionnaire = questionnaireId == (int)enQuestionnaireType.Subscription ? XDocument.Parse(tempSubscription) : XDocument.Parse(tempLicense);
            else
            {
                IEnumerable<XElement> availableSystems = (from systemRecords in filledQuestionnaire.Descendants(enQuestionnaireElements.System.ToString())
                                                          where (string)systemRecords.Attribute(enQuestionnaireAttr.sequence.ToString()).Value == systemId.ToString()
                                                          select systemRecords);
                if (availableSystems.Count() == 0)
                    AddSystem(tempSystemName);
                else
                {
                    questionType = (from systemRecords in filledQuestionnaire.Descendants(enQuestionnaireElements.System.ToString())
                                    where (string)systemRecords.Attribute(enQuestionnaireAttr.sequence.ToString()).Value == systemId.ToString()
                                    select systemRecords.Attribute(enQuestionnaireAttr.description.ToString()).Value.Trim()).FirstOrDefault();


                    currentQuestionnaireId = (int)Util.GetValueFromCategory<enQuestionnaireType>(questionType);
                    if (questionnaireId != currentQuestionnaireId)
                    {
                        availableSystems.Remove();
                        AddSystem(tempSystemName);
                    }
                }
            }

            // Dont need to save the questionnaire where user switch to another system
            /*if (!_isSystemChanged)
                SaveQuestionnaireToDB(false);*/

            GenerateQuestionnaire();

            lblSystemInfo.Text = DEFAULT_ACTIVE_SYSTEM_PREFIX + systemId.ToString();
        }

        private void InitializeControl()
        {
            _tableCell = new TableCell();
            _label = new Label();
            _textbox = new TextBox();
            _dropDownList = new DropDownList();
            _hyperlink = new HyperLink();
            _hiddenField = new HiddenField();
            _image = new Image();

            _requiredFieldValidator = new RequiredFieldValidator();
            _regularExpressionValidator = new RegularExpressionValidator();

            _requiredFieldValidator.Display = ValidatorDisplay.None;
            _regularExpressionValidator.Display = ValidatorDisplay.None;
            _requiredFieldValidator.Text = _regularExpressionValidator.Text = DEFAULT_REQ_TEXT_SEPERATOR;

            _textbox.ClientIDMode = _dropDownList.ClientIDMode = _hiddenField.ClientIDMode = ClientIDMode.Static;
        }

        private void RenderQuestionnaire(XElement element)
        {

            foreach (XElement currentElement in element.Elements())
            {
                if (currentElement.Name == enQuestionnaireElements.System.ToString() && currentElement.Attribute(enQuestionnaireAttr.sequence.ToString()).Value != systemId.ToString())
                    continue;
                else if (currentElement.Name == enQuestionnaireElements.System.ToString() && currentElement.Attribute(enQuestionnaireAttr.sequence.ToString()).Value == systemId.ToString())
                    EnablePanels();

                // Initialize the controls
                _tableRow = new TableRow();

                // discard nested elements
                if (currentElement.Name == enQuestionnaireElements.Quantity.ToString() || currentElement.Name == enQuestionnaireElements.Type.ToString() || currentElement.Name == enQuestionnaireElements.Amount.ToString() || currentElement.Name == enQuestionnaireElements.PaymentMethod.ToString())
                    continue;

                if (currentElement.Name == enQuestionnaireElements.System.ToString() || currentElement.Name == enQuestionnaireElements.OngoingFees.ToString() || currentElement.Name == enQuestionnaireElements.OneTimeFees.ToString())
                    _table = new Table();

                // add initialized table into its proper place
                if (currentElement.Name == enQuestionnaireElements.System.ToString())
                    divSystem.Controls.Add(_table);
                else if (currentElement.Name == enQuestionnaireElements.OngoingFees.ToString())
                {
                    _isOnGoing = true;
                    divSubsOngoingFees.Controls.Add(_table);

                }
                else if (currentElement.Name == enQuestionnaireElements.OneTimeFees.ToString())
                {
                    _isOnGoing = false;
                    divSubsOneTimeFees.Controls.Add(_table);
                }

                // filter current element
                FilterElement(currentElement);

                // call recursive
                RenderQuestionnaire(currentElement);
            }
        }

        private void FilterElement(XElement element)
        {
            int count = 0;
            string value = string.Empty;
            string genericValues = string.Empty;
            bool required = false;

            if (element.Name == enQuestionnaireElements.System.ToString())
            {
                // *************************************************************************************
                // (XElement element, enQuestionnaireControlType controlType, string controlPrefix,
                //    int colSpan, bool isNormalCssClass, bool isNewTable, string dataType, string value)
                // *************************************************************************************

                value = element.Attribute(enQuestionnaireAttr.name.ToString()).Value;
                required = element.Attribute(enQuestionnaireAttr.required.ToString()).Value.Trim() == DEFAULT_APPROVED_ANSWER ? true : false;

                // add label
                AddControl(element, enQuestionnaireControlType.Label, "", 0, true, false, string.Empty, string.Empty, required);

                // add TextBox
                AddControl(element, enQuestionnaireControlType.TextBox, DEFAULT_SYSTEM_TEXT_PREFIX + (int)questionnaireId, 0, true, false,
                    string.Empty, value, required);

                // add row in table
                _table.Controls.Add(_tableRow);
            }
            else if (element.Name == enQuestionnaireElements.Field.ToString())
            {
                // *************************************************************************************
                // (XElement element, enQuestionnaireControlType controlType, string controlPrefix,
                //    int colSpan, bool isNormalCssClass, bool isNewTable, string dataType, string value)
                // *************************************************************************************

                // Add Label
                required = element.Attribute(enQuestionnaireAttr.required.ToString()).Value.Trim() == DEFAULT_APPROVED_ANSWER ? true : false;

                AddControl(element, enQuestionnaireControlType.Label, "", 0, false, false, string.Empty, string.Empty, required);

                string feeControlPrefix = string.Empty;
                if (_isOnGoing)
                    feeControlPrefix = DEFAULT_ONGOING_TEXT_PREFIX + (int)questionnaireId;
                else
                    feeControlPrefix = DEFAULT_ONETIME_TEXT_PREFIX + (int)questionnaireId;


                // Add other controls against current field
                foreach (XElement control in element.Elements().Elements())
                {
                    count += 1;

                    value = control.Attribute(enQuestionnaireAttr.value.ToString()).Value;
                    required = control.Attribute(enQuestionnaireAttr.required.ToString()).Value.Trim() == DEFAULT_APPROVED_ANSWER ? true : false;

                    genericValues = genericValues + value + "@";

                    // *************************************************************************************
                    // (XElement element, enQuestionnaireControlType controlType, string controlPrefix,
                    //    int colSpan, bool isNormalCssClass, bool isNewTable, string dataType, string value)
                    // *************************************************************************************

                    if (control.Name == enQuestionnaireElements.Quantity.ToString())
                        AddControl(element, enQuestionnaireControlType.TextBox, feeControlPrefix + DEFAULT_QUANTITY_CONTROL_SEQUENCE, 0, false, false,
                            control.Name.ToString(), value, required);

                    else if (control.Name == enQuestionnaireElements.Type.ToString() && count == 2)
                        AddControl(element, enQuestionnaireControlType.DropDownList, feeControlPrefix + DEFAULT_TYPE_CONTROL_SEQUENCE, 0, false, false,
                            control.Name.ToString(), value, required);

                    else if (control.Name == enQuestionnaireElements.Amount.ToString() && (count == 3 || count == 2))
                        AddControl(element, enQuestionnaireControlType.TextBox, feeControlPrefix + DEFAULT_AMOUNT_CONTROL_SEQUENCE, 0, false, false,
                            control.Name.ToString(), value, required);

                    else if (control.Name == enQuestionnaireElements.PaymentMethod.ToString())
                        AddControl(element, enQuestionnaireControlType.DropDownList, feeControlPrefix + DEFAULT_PAYMENT_METHOD_CONTROL_SEQUENCE, 0, false,
                            false, control.Name.ToString(), value, required);

                    else if (control.Name == enQuestionnaireElements.Type.ToString() && count == 1) // Add column span 2
                        AddControl(element, enQuestionnaireControlType.DropDownList, feeControlPrefix + DEFAULT_TYPE_CONTROL_SEQUENCE, 2, false, false,
                            control.Name.ToString(), value, required);

                    else if (control.Name == enQuestionnaireElements.Amount.ToString() && count == 1) // Add column span 3
                        AddControl(element, enQuestionnaireControlType.TextBox, feeControlPrefix + DEFAULT_AMOUNT_CONTROL_SEQUENCE, 3, false, false,
                            control.Name.ToString(), value, required);

                    if (control.Name == enQuestionnaireElements.Amount.ToString())
                        _table.Controls.Add(_tableRow); // Add row in table
                }

                // Add generic handler for unlimited fields if required
                string newTagRequired = element.Attribute(enQuestionnaireAttr.add.ToString()).Value;
                if (newTagRequired == DEFAULT_APPROVED_ANSWER)
                {
                    genericValues = genericValues.Substring(0, genericValues.LastIndexOf('@'));
                    AddControl(element, enQuestionnaireControlType.HyperLink, feeControlPrefix + DEFAULT_GENERIC_CONTROL_SEQUENCE, count, false,
                        true, string.Empty, genericValues, required);
                }

            }

        }

        private void AddControl(XElement element, enQuestionnaireControlType controlType, string controlPrefix,
            int colSpan, bool isNormalCssClass, bool isNewTable, string dataType, string value, bool validationRequired)
        {
            // cotains the value of more than one row
            string[] multiFieldsValue = value.Split('@');

            string[] multivalues = value.Split(',');
            value = multivalues[0];

            // Field label Text
            string fieldTitle = element.Attribute(enQuestionnaireAttr.title.ToString()).Value.Trim();

            // Initialize control
            InitializeControl();

            // set column settings
            _tableCell.ColumnSpan = colSpan;

            #region CSS_CLASS_FOR_CONTROLS
            if (!isNormalCssClass)
            {
                if (dataType == enQuestionnaireElements.Quantity.ToString())
                    _tableCell.CssClass = DEFAULT_CELL2_CLASS;
                else if (dataType == enQuestionnaireElements.Type.ToString() && colSpan == 0)
                    _tableCell.CssClass = DEFAULT_CELL3_CLASS;
                else if (dataType == enQuestionnaireElements.Amount.ToString() && colSpan == 0)
                    _tableCell.CssClass = DEFAULT_CELL4_CLASS;
                else if (dataType == enQuestionnaireElements.PaymentMethod.ToString() && colSpan == 0)
                    _tableCell.CssClass = DEFAULT_CELL5_CLASS;
                else if (colSpan == 2)
                    _tableCell.CssClass = DEFAULT_COMMON_CELL1_CLASS;
                else if (colSpan == 3)
                    _tableCell.CssClass = DEFAULT_COMMON_CELL2_CLASS;
            }

            if (dataType == enQuestionnaireElements.Quantity.ToString())
            {
                _label.Text = "Qty:";
                _label.CssClass = DEFAULT_FEES_TITLE_LABLE_TITLE;
                _tableCell.Controls.Add(_label);
            }
            else if (dataType == enQuestionnaireElements.Type.ToString())
            {
                _label.Text = "Type:";
                _label.CssClass = DEFAULT_FEES_TITLE_LABLE_TITLE;
                _tableCell.Controls.Add(_label);
            }

            #endregion

            switch (controlType)
            {
                case enQuestionnaireControlType.Label:
                    //info 
                    if (element.Name == enQuestionnaireElements.Field.ToString())
                    {
                        if (element.Attribute(enQuestionnaireAttr.info.ToString()).Value == DEFAULT_APPROVED_ANSWER)
                        {
                            _image.ImageUrl = DEFAULT_INFO_IMAGE_URL;
                            _image.AlternateText = element.Attribute(enQuestionnaireAttr.description.ToString()).Value;
                            _image.ToolTip = element.Attribute(enQuestionnaireAttr.description.ToString()).Value;

                            // add info
                            _tableCell.Controls.Add(_image);
                        }
                    }

                    // Title

                    // set Label css class for fields
                    if (!isNormalCssClass)
                        _tableCell.CssClass = DEFAULT_CELL1_CLASS;

                    if (validationRequired)
                        _label.Text = fieldTitle + DEFAULT_TEXT_SEPERATOR + DEFAULT_REQ_TEXT_SEPERATOR;
                    else
                        _label.Text = fieldTitle + DEFAULT_TEXT_SEPERATOR;

                    // Add title
                    _tableCell.Controls.Add(_label);
                    break;

                case enQuestionnaireControlType.TextBox:
                    // Cell Align
                    _tableCell.HorizontalAlign = HorizontalAlign.Right;


                    // TextBox settings
                    _textbox.ID = controlPrefix + element.Attribute(enQuestionnaireAttr.sequence.ToString()).Value;
                    _textbox.Text = value;

                    // add different restriction on the basis of data type
                    if (dataType == enQuestionnaireElements.Quantity.ToString())
                    {
                        _textbox.MaxLength = DEFAULT_QNTY_MAX_LENGTH;
                        _textbox.CssClass = DEFAULT_QNTY_VALIDATION_CLASS;
                    }
                    else if (dataType == enQuestionnaireElements.Amount.ToString())
                    {
                        _textbox.MaxLength = DEFAULT_AMOUNT_MAX_LENGTH;
                        _textbox.CssClass = DEFAULT_AMOUNT_VALIDATION_CLASS;
                    }
                    else
                    {
                        _textbox.CssClass = isNormalCssClass ? DEFAULT_NORMAL_TEXTBOX_CLASS : DEFAULT_OTHER_VALIDATION_CLASS;
                        _textbox.MaxLength = DEFAULT_OTHER_MAX_LENGTH;
                        if (tempSystemName != string.Empty)
                            _textbox.Text = tempSystemName;
                    }

                    if (validationRequired)
                    {
                        _requiredFieldValidator.ControlToValidate = _textbox.ID;

                        if (!isNormalCssClass) // if not system than its value will be false
                            _requiredFieldValidator.ErrorMessage = dataType + " of  <b>" + fieldTitle + "</b> field is required."; // hardcoded: is ok/not?
                        else
                        {
                            _requiredFieldValidator.ErrorMessage = "EHR/PM Name is required.";
                            _regularExpressionValidator.ControlToValidate = _textbox.ID;
                            _regularExpressionValidator.ValidationExpression = RegExValidate.CUSTOM_CHARACTER1;
                            _regularExpressionValidator.ErrorMessage = "EHR/PM Name is invalid.";
                            _tableCell.Controls.Add(_regularExpressionValidator);

                        }

                        _tableCell.Controls.Add(_requiredFieldValidator);

                    }

                    // Add TextBox
                    _tableCell.Controls.Add(_textbox);
                    break;

                case enQuestionnaireControlType.DropDownList:
                    // Cell Align
                    _tableCell.HorizontalAlign = HorizontalAlign.Right;

                    // DropDown settings
                    _dropDownList.ID = controlPrefix + element.Attribute(enQuestionnaireAttr.sequence.ToString()).Value;
                    _dropDownList.CssClass = DEFAULT_FEES_DROPDOWN_CLASS;

                    // Populate items
                    if (dataType == enQuestionnaireElements.Type.ToString())
                    {
                        if (fieldTitle.Contains(DEFAULT_EHR_INTERFACE_TYPE_TEXT))
                        {
                            GetEHRInterfaceType();
                            _dropDownList.SelectedIndex = _dropDownList.Items.IndexOf(_dropDownList.Items.FindByValue(value));

                            // Add Drop Down
                            _tableCell.Controls.Add(_dropDownList);
                        }
                        else if (fieldTitle.Contains(DEFAULT_EHR_OTHER_TYPE_TEXT))
                        {
                            _textbox.ID = controlPrefix + element.Attribute(enQuestionnaireAttr.sequence.ToString()).Value;
                            _textbox.Text = value;
                            _textbox.CssClass = DEFAULT_FEES_DROPDOWN_CLASS;

                            // Add TextBox
                            _tableCell.Controls.Add(_textbox);

                        }
                        else
                        {
                            GetEHRType();
                            _dropDownList.SelectedIndex = _dropDownList.Items.IndexOf(_dropDownList.Items.FindByValue(value));

                            // Add Drop Down
                            _tableCell.Controls.Add(_dropDownList);
                        }

                        _requiredFieldValidator.ControlToValidate = _textbox.ID;

                        // validation
                        if (validationRequired && !fieldTitle.Contains(DEFAULT_EHR_OTHER_TYPE_TEXT))
                        {
                            _requiredFieldValidator.ControlToValidate = _dropDownList.ID;
                            _requiredFieldValidator.InitialValue = DEFAULT_EMPTY_VALUE;
                            _requiredFieldValidator.ErrorMessage = dataType + " of  <b>" + fieldTitle + "</b> field is required."; // hardcoded: is ok/not?
                            _tableCell.Controls.Add(_requiredFieldValidator);
                        }
                        else if (fieldTitle.Contains(DEFAULT_EHR_OTHER_TYPE_TEXT))
                        {
                            _regularExpressionValidator.ControlToValidate = _textbox.ID;
                            _regularExpressionValidator.ValidationExpression = RegExValidate.CUSTOM_CHARACTER1;
                            _regularExpressionValidator.ErrorMessage = dataType + " of  <b>" + fieldTitle + "</b> field is invalid."; // hardcoded: is ok/not?;                            
                            _tableCell.Controls.Add(_regularExpressionValidator);
                        }


                    }
                    else if (dataType == enQuestionnaireElements.PaymentMethod.ToString())
                    {
                        if (_isOnGoing)
                            GetOnGoingPaymentMethods();
                        else
                            GetOneTimePaymentMethods();

                        _dropDownList.SelectedIndex = _dropDownList.Items.IndexOf(_dropDownList.Items.FindByValue(value));


                        // *****************************************************
                        // 1. get rid of per practice per year and per month for subscriptions in the subscription model
                        // 2. get rid of drop down altogether for the subcription in the upfront model
                        // 3. get rid of per practice per year and per month for maintenance in the up front model - reucrring fee
                        // *****************************************************
                        #region REMOVE_VALUES_FROM_SOME_FIELDS
                        if (fieldTitle == "Subscription" || fieldTitle == "Maintenance")
                        {
                            _dropDownList.Items.RemoveAt(4);
                            _dropDownList.Items.RemoveAt(3);

                        }
                        else if (fieldTitle == "License")
                        {
                            _tableCell.HorizontalAlign = HorizontalAlign.Left;
                            _label.Text = "Per Provider";
                            _label.CssClass = "lbl-payment";
                            _dropDownList.CssClass = DEFAULT_HIDDEN_CLASS;
                            _tableCell.Controls.Add(_label);
                            _dropDownList.SelectedIndex = _dropDownList.Items.IndexOf(_dropDownList.Items.FindByValue(((int)enOneTimePaymentMethod.PerProvider).ToString()));
                        }
                        #endregion


                        // Add Drop Down
                        _tableCell.Controls.Add(_dropDownList);

                        // validation
                        if (validationRequired)
                        {
                            _requiredFieldValidator.ControlToValidate = _dropDownList.ID;
                            _requiredFieldValidator.InitialValue = DEFAULT_EMPTY_VALUE;
                            _requiredFieldValidator.ErrorMessage = Util.GetEnumDescription(enQuestionnaireElements.PaymentMethod).ToString() + " of  <b>" + fieldTitle + "</b> field is required."; // hardcoded: is ok/not?
                            _tableCell.Controls.Add(_requiredFieldValidator);
                        }
                    }

                    break;

                case enQuestionnaireControlType.HyperLink:
                    if (isNewTable)
                    {
                        // Add new table for generic control
                        Table _genericTable = new Table();
                        _genericTable.ID = DEFAULT_TABLE_ID_PREFIX + controlPrefix + element.Attribute(enQuestionnaireAttr.sequence.ToString()).Value;
                        _genericTable.ClientIDMode = ClientIDMode.Static;
                        _genericTable.Width = Unit.Percentage(128);

                        _tableRow = new TableRow();
                        _tableCell = new TableCell();
                        _tableCell.ColumnSpan = 4; // To remove the columns difference for generic controls

                        _tableCell.Controls.Add(_genericTable);
                        _tableRow.Controls.Add(_tableCell);
                        _table.Controls.Add(_tableRow);

                        // Add hyper link control to add controls
                        _tableRow = new TableRow();
                        _tableCell = new TableCell();
                        _tableCell.ColumnSpan = 4; // To remove the columns difference from other rows which carry the generic control

                        _hiddenField.ID = controlPrefix + DEFAULT_HIDDEN_FIELD_ID_PREFIX + element.Attribute(enQuestionnaireAttr.sequence.ToString()).Value;
                        _hyperlink.ID = controlPrefix + element.Attribute(enQuestionnaireAttr.sequence.ToString()).Value;
                        _hyperlink.CssClass = DEFAULT_FEES_HYPERLINK_CLASS;
                        _hyperlink.Text = element.Attribute(enQuestionnaireAttr.addTitle.ToString()).Value;
                        _hyperlink.NavigateUrl = "javascript:GenerateControl('" + _genericTable.ID + "','" + colSpan.ToString() + "','" +
                            element.Attribute(enQuestionnaireAttr.title.ToString()).Value + DEFAULT_TEXT_SEPERATOR + "','" + _hiddenField.ID + "','','','','')";

                        // store table names for generic controls to get the values on post back
                        hdnTablesId.Value += _genericTable.ID + "," + _hiddenField.ID + "@";

                        // To store in system div
                        divSystem.Controls.Add(_hiddenField);

                        _tableCell.Controls.Add(_hyperlink);
                        _tableRow.Controls.Add(_tableCell);

                        _genericTable.Controls.Add(_tableRow);

                        #region ADD_GENERIC_ROWS

                        multivalues = multiFieldsValue[0].Split(',');

                        if (multivalues.Count() > 1)
                        {
                            // to store the multiple call of javscript function
                            string functionLines = string.Empty;

                            for (int index = 1; index < multivalues.Length; index++)
                            {
                                string param = string.Empty;
                                functionLines += "GenerateControl('" + _genericTable.ID + "','" + colSpan.ToString() + "','" +
                            element.Attribute(enQuestionnaireAttr.title.ToString()).Value + DEFAULT_TEXT_SEPERATOR + "','" + _hiddenField.ID + "',";

                                // Quantity
                                multivalues = multiFieldsValue[0].Split(',');
                                if (multiFieldsValue.Count() == 4)
                                    param = "'" + multivalues[index] + "',";
                                else
                                    param = "'',";

                                // Type
                                if (multiFieldsValue.Count() == 4)
                                    multivalues = multiFieldsValue[1].Split(',');
                                else
                                    multivalues = multiFieldsValue[0].Split(',');

                                param += "'" + multivalues[index] + "',";

                                // Amount
                                if (multiFieldsValue.Count() == 4)
                                    multivalues = multiFieldsValue[2].Split(',');
                                else
                                    multivalues = multiFieldsValue[1].Split(',');

                                param += "'" + multivalues[index] + "',";

                                // Paymeny Method
                                if (multiFieldsValue.Count() == 4)
                                    multivalues = multiFieldsValue[3].Split(',');
                                else
                                    multivalues = multiFieldsValue[2].Split(',');

                                param += "'" + multivalues[index] + "'";
                                functionLines += param + ");";
                            }


                            // to remove the alignment issue generate dynamic rows on client side
                            //Response.Write("<script language='javascript'>" + functionLines + "</script>");
                            //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), _genericTable.ID, functionLines, true);
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), _genericTable.ID, functionLines, true);

                        }


                        #endregion

                        return;


                    }
                    else
                    {
                        _hyperlink.ID = controlPrefix + element.Attribute(enQuestionnaireAttr.sequence.ToString()).Value;
                        _hyperlink.CssClass = DEFAULT_FEES_HYPERLINK_CLASS;
                        _tableCell.Controls.Add(_hyperlink);
                    }
                    break;
                default:
                    return;

            }
            _tableRow.Controls.Add(_tableCell);

        }

        private void GetEHRType()
        {
            string itemText = string.Empty;
            string itemValue = string.Empty;

            // Initial selected text
            _dropDownList.Items.Add(new ListItem(DEFAULT_EMPTY_TEXT, DEFAULT_EMPTY_VALUE));

            foreach (enEHRProviders type in Util.EnumToList<enEHRProviders>())
            {
                itemText = Util.GetEnumDescription(type);
                itemValue = Convert.ToString((int)type);
                _dropDownList.Items.Add(new ListItem(itemText, itemValue));
            }
        }

        private void GetEHRInterfaceType()
        {
            string itemText = string.Empty;
            string itemValue = string.Empty;

            // Initial selected text
            _dropDownList.Items.Add(new ListItem(DEFAULT_EMPTY_TEXT, DEFAULT_EMPTY_VALUE));

            foreach (enEHRInterfaces type in Util.EnumToList<enEHRInterfaces>())
            {
                itemText = Util.GetEnumDescription(type);
                itemValue = Convert.ToString((int)type);
                _dropDownList.Items.Add(new ListItem(itemText, itemValue));
            }

        }

        private void GetOnGoingPaymentMethods()
        {
            string itemText = string.Empty;
            string itemValue = string.Empty;

            // Initial selected text
            _dropDownList.Items.Add(new ListItem(DEFAULT_EMPTY_TEXT, DEFAULT_EMPTY_VALUE));

            foreach (enOnGoingPaymentMethod paymentMethod in Util.EnumToList<enOnGoingPaymentMethod>())
            {
                itemText = Util.GetEnumDescription(paymentMethod);
                itemValue = Convert.ToString((int)paymentMethod);
                _dropDownList.Items.Add(new ListItem(itemText, itemValue));
            }
        }

        private void GetOneTimePaymentMethods()
        {
            string itemText = string.Empty;
            string itemValue = string.Empty;

            // Initial selected text
            _dropDownList.Items.Add(new ListItem(DEFAULT_EMPTY_TEXT, DEFAULT_EMPTY_VALUE));

            foreach (enOneTimePaymentMethod paymentMethod in Util.EnumToList<enOneTimePaymentMethod>())
            {
                itemText = Util.GetEnumDescription(paymentMethod);
                itemValue = Convert.ToString((int)paymentMethod);
                _dropDownList.Items.Add(new ListItem(itemText, itemValue));
            }
        }

        private void StoreDataToClient()
        {
            string itemText = string.Empty;
            string itemValue = string.Empty;
            hdnType.Value = hdnInterfaceType.Value = hdnOngoingPaymentMethod.Value = hdnOneTimePaymentMethod.Value = string.Empty;

            // default selected item
            hdnType.Value = hdnInterfaceType.Value = hdnOngoingPaymentMethod.Value = hdnOneTimePaymentMethod.Value =
                DEFAULT_EMPTY_VALUE + "," + DEFAULT_EMPTY_TEXT + "@";

            // Store Type and payment method data for generic controls
            foreach (enEHRProviders type in Util.EnumToList<enEHRProviders>())
            {
                itemText = Util.GetEnumDescription(type);
                itemValue = Convert.ToString((int)type);
                hdnType.Value += itemValue + "," + itemText + "@";
            }

            foreach (enEHRInterfaces type in Util.EnumToList<enEHRInterfaces>())
            {
                itemText = Util.GetEnumDescription(type);
                itemValue = Convert.ToString((int)type);
                hdnInterfaceType.Value += itemValue + "," + itemText + "@";
            }

            foreach (enOnGoingPaymentMethod paymentMethod in Util.EnumToList<enOnGoingPaymentMethod>())
            {
                itemText = Util.GetEnumDescription(paymentMethod);
                itemValue = Convert.ToString((int)paymentMethod);
                hdnOngoingPaymentMethod.Value += itemValue + "," + itemText + "@";
            }

            foreach (enOneTimePaymentMethod paymentMethod in Util.EnumToList<enOneTimePaymentMethod>())
            {
                itemText = Util.GetEnumDescription(paymentMethod);
                itemValue = Convert.ToString((int)paymentMethod);
                hdnOneTimePaymentMethod.Value += itemValue + "," + itemText + "@";
            }

        }

        private void DeleteSystem()
        {
            if (activeSystems > 1)
            {
                XElement element = (from systemRecord in filledQuestionnaire.Root.Descendants(enQuestionnaireElements.System.ToString())
                                    where (string)systemRecord.Attribute(enQuestionnaireAttr.sequence.ToString()).Value == systemId.ToString()
                                    select systemRecord).FirstOrDefault();

                element.Remove();

                int resetSystemSequence = 1;
                while (resetSystemSequence < activeSystems)
                {
                    foreach (XElement systemElement in filledQuestionnaire.Root.Descendants(enQuestionnaireElements.System.ToString()))
                    {
                        systemElement.Attribute(enQuestionnaireAttr.sequence.ToString()).Value = resetSystemSequence.ToString();
                        resetSystemSequence += 1;
                    }
                }

                // save questionnaire
                SaveQuestionnaireToDB(false);

                // On Reset Flag
                OnResetFlag();

                // set delete flag in case of deletion
                _isDeleted = true;

                // Hack: Temporary solution to re-render the client side control and its updated value {Need proper solution}"
                Response.Redirect(Request.RawUrl);
            }
            else
                message.Error("You must have more than one system to delete the selected system!");

        }

        private void SaveQuestionnaire(bool referesh)
        {
            if (filledQuestionnaire != null)
            {
                XElement root = filledQuestionnaire.Root;

                string fieldSequence = string.Empty;
                string feeType = string.Empty;

                string controlId = string.Empty;

                string multiValues = string.Empty;
                string quantity = string.Empty;
                string type = string.Empty;
                string amount = string.Empty;
                string paymentMehod = string.Empty;

                foreach (XElement system in root.Elements())
                {
                    if (system.Name == enQuestionnaireElements.System.ToString() && system.Attribute(enQuestionnaireAttr.sequence.ToString()).Value != systemId.ToString())
                        continue;

                    TextBox txtSystem = (TextBox)divSystem.FindControl(DEFAULT_SYSTEM_TEXT_PREFIX + (int)questionnaireId + system.Attribute(enQuestionnaireAttr.sequence.ToString()).Value);
                    system.Attribute(enQuestionnaireAttr.name.ToString()).Value = txtSystem.Text;

                    foreach (XElement control in system.Elements().Elements().Elements().Elements())
                    {
                        fieldSequence = control.Parent.Parent.Attribute(enQuestionnaireAttr.sequence.ToString()).Value;
                        feeType = control.Parent.Parent.Parent.Name.ToString();
                        List<PriceCalculationDetail> _ListOfPriceCalculatorDetail = new List<PriceCalculationDetail>();

                        // if field is generic
                        if (control.Parent.Parent.Attribute(enQuestionnaireAttr.add.ToString()).Value == DEFAULT_APPROVED_ANSWER)
                        {
                            if (feeType == enQuestionnaireElements.OngoingFees.ToString())
                            {
                                HiddenField hdnMultiValues = (HiddenField)divSystem.FindControl(DEFAULT_ONGOING_TEXT_PREFIX + (int)questionnaireId + DEFAULT_GENERIC_CONTROL_SEQUENCE + DEFAULT_HIDDEN_FIELD_ID_PREFIX + fieldSequence);
                                multiValues = hdnMultiValues != null ? hdnMultiValues.Value : string.Empty;
                            }
                            else if (feeType == enQuestionnaireElements.OneTimeFees.ToString())
                            {
                                HiddenField hdnMultiValues2 = (HiddenField)divSystem.FindControl(DEFAULT_ONETIME_TEXT_PREFIX + (int)questionnaireId + DEFAULT_GENERIC_CONTROL_SEQUENCE + DEFAULT_HIDDEN_FIELD_ID_PREFIX + fieldSequence);
                                multiValues = hdnMultiValues2 != null ? hdnMultiValues2.Value : string.Empty;
                            }
                            else
                                multiValues = string.Empty;


                            #region EXTRACT_MULTIVALUES
                            if (multiValues != string.Empty && multiValues.Length > 2)
                            {
                                string[] rows = multiValues.Split('@');

                                foreach (string row in rows)
                                {
                                    string[] columns = row.Split(',');

                                    int colmnsCount = columns.Count();

                                    if (colmnsCount == 4)
                                        _ListOfPriceCalculatorDetail.Add(new PriceCalculationDetail(columns[0].Trim(), columns[2].Trim(), columns[1].Trim(), columns[3].Trim()));
                                    else if (colmnsCount == 3) // Hanlde Ohter Type Text field
                                        if (control.Parent.Parent.Attribute(enQuestionnaireAttr.title.ToString()).Value.Contains(DEFAULT_EHR_OTHER_TYPE_TEXT))
                                            _ListOfPriceCalculatorDetail.Add(new PriceCalculationDetail(string.Empty, columns[0].Trim(), columns[1].Trim(), columns[2].Trim()));
                                        else
                                            _ListOfPriceCalculatorDetail.Add(new PriceCalculationDetail(string.Empty, columns[1].Trim(), columns[0].Trim(), columns[2].Trim()));
                                }
                            }

                            #endregion

                            #region ADD_PARSED_VALUES_INTO_LISTED

                            if (_ListOfPriceCalculatorDetail.Count() > 0)
                            {

                                foreach (PriceCalculationDetail priceDetail in _ListOfPriceCalculatorDetail)
                                {
                                    type += "," + priceDetail.Type;
                                    quantity += "," + priceDetail.Quantity;
                                    amount += "," + priceDetail.Amount;
                                    paymentMehod += "," + priceDetail.PaymentMethod;
                                }
                            }
                            #endregion
                        }

                        #region NORMAL_VALUES
                        if (control.Name == enQuestionnaireElements.Quantity.ToString())
                        {
                            if (feeType == enQuestionnaireElements.OngoingFees.ToString())
                                controlId = DEFAULT_ONGOING_TEXT_PREFIX + (int)questionnaireId + DEFAULT_QUANTITY_CONTROL_SEQUENCE + fieldSequence;
                            else
                                controlId = DEFAULT_ONETIME_TEXT_PREFIX + (int)questionnaireId + DEFAULT_QUANTITY_CONTROL_SEQUENCE + fieldSequence;

                            TextBox txtQuantity = (TextBox)divSubsOngoingFees.FindControl(controlId);
                            control.Attribute(enQuestionnaireAttr.value.ToString()).Value = txtQuantity.Text.Trim() + quantity;
                        }
                        else if (control.Name == enQuestionnaireElements.Type.ToString())
                        {
                            if (feeType == enQuestionnaireElements.OngoingFees.ToString())
                                controlId = DEFAULT_ONGOING_TEXT_PREFIX + (int)questionnaireId + DEFAULT_TYPE_CONTROL_SEQUENCE + fieldSequence;
                            else
                                controlId = DEFAULT_ONETIME_TEXT_PREFIX + (int)questionnaireId + DEFAULT_TYPE_CONTROL_SEQUENCE + fieldSequence;

                            if (control.Parent.Parent.Attribute(enQuestionnaireAttr.title.ToString()).Value.Contains(DEFAULT_EHR_OTHER_TYPE_TEXT))
                            {
                                TextBox ddlType = (TextBox)divSubsOngoingFees.FindControl(controlId);
                                control.Attribute(enQuestionnaireAttr.value.ToString()).Value = ddlType.Text.Trim() + type;
                            }
                            else
                            {
                                DropDownList ddlType = (DropDownList)divSubsOngoingFees.FindControl(controlId);
                                control.Attribute(enQuestionnaireAttr.value.ToString()).Value = ddlType.SelectedValue + type;
                            }
                        }
                        else if (control.Name == enQuestionnaireElements.Amount.ToString())
                        {
                            if (feeType == enQuestionnaireElements.OngoingFees.ToString())
                                controlId = DEFAULT_ONGOING_TEXT_PREFIX + (int)questionnaireId + DEFAULT_AMOUNT_CONTROL_SEQUENCE + fieldSequence;
                            else
                                controlId = DEFAULT_ONETIME_TEXT_PREFIX + (int)questionnaireId + DEFAULT_AMOUNT_CONTROL_SEQUENCE + fieldSequence;

                            TextBox txtAmount = (TextBox)divSubsOngoingFees.FindControl(controlId);
                            control.Attribute(enQuestionnaireAttr.value.ToString()).Value = txtAmount.Text.Trim() + amount;
                        }
                        else if (control.Name == enQuestionnaireElements.PaymentMethod.ToString())
                        {
                            if (feeType == enQuestionnaireElements.OngoingFees.ToString())
                                controlId = DEFAULT_ONGOING_TEXT_PREFIX + (int)questionnaireId + DEFAULT_PAYMENT_METHOD_CONTROL_SEQUENCE + fieldSequence;
                            else
                                controlId = DEFAULT_ONETIME_TEXT_PREFIX + (int)questionnaireId + DEFAULT_PAYMENT_METHOD_CONTROL_SEQUENCE + fieldSequence;

                            DropDownList ddlPaymentMethod = (DropDownList)divSubsOngoingFees.FindControl(controlId);
                            control.Attribute(enQuestionnaireAttr.value.ToString()).Value = ddlPaymentMethod.SelectedValue + paymentMehod;
                        }

                        #endregion

                        // clear values
                        quantity = type = amount = paymentMehod = string.Empty;
                    }
                }

                // save questionnaire into database
                SaveQuestionnaireToDB(referesh);
            }

        }

        private void SaveQuestionnaireToDB(bool referesh)
        {
            _questionBO = new QuestionBO();
            _questionBO.ProjectUsageId = ProjectUsageId;
            _questionBO.SaveFilledQuestionnaire((int)enQuestionnaireType.Subscription, ProjectUsageId, filledQuestionnaire.Root, userId);

            // remove the backup of questionnaire if exists
            backUpQuestionnaire = null;

            if (referesh)
            {
                _isSaved = true;

                // Hack: Temporary solution to re-render the client side control and its updated value {Need proper solution}"
                Response.Redirect(Request.RawUrl);
            }

        }

        public void OnResetFlag()
        {
            IsReset = true;
        }

        #endregion
    }
}