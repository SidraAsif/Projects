using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Xml;
using System.Xml.Linq;
using System.Text;

using BMTBLL;
using BMTBLL.Enumeration;

public partial class ExpressAssessment : System.Web.UI.UserControl
{
    #region CONSTANTS
    private const string CONTROL_NAME = "Form";

    #endregion

    #region PROPERTIES
    enQuestionnaireType _questionnaireType { get; set; }
    //public int ProjectId { get; set; }
    public int ProjectUsageId { get; set; }
    public int UserId { get; set; }
    public int SiteId { get; set; }
    public int SectionId { get; set; }
    public string SiteName { get; set; }

    #endregion

    #region VARIABLES
    private QuestionBO _questionBO;
    private ProjectBO project;
    private int questionnaireId;

    private XDocument questionnaireDocument;

    private Table _questionTitleTable;
    private Table _questionTable;
    private Table _formSubmitTable;
    private TableRow _questionRow;
    private TableCell _questioncell;

    private Label _label;
    private RadioButtonList _rbChoice;
    private CheckBoxList _chkBoxChoice;

    private RequiredFieldValidator _requiredFieldvalidator;
    private CustomValidator _customeValidator;

    private Button _button;

    #endregion

    #region EVENTS
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!this.Visible)
                return;

            if (Request.QueryString["NodeContentType"] != null && Request.QueryString["NodeContentType"] == CONTROL_NAME)
            {
                if (Session["UserApplicationId"] == null || Session["UserType"] == null || Session["PracticeId"] == null)
                    Response.Redirect("~/Account/Login.aspx?Authentication=failedOrTimeOut");

                // configure Questionnaire Type
                _questionnaireType = enQuestionnaireType.SimpleQuestionnaire;

                // get Questionnaire
                GetQuestionnaire(_questionnaireType);

                if (ProjectUsageId != 0)
                {
                    project = new ProjectBO();
                    lblSiteName.Text = SiteName;
                }
            }
        }
        catch (Exception exception)
        {
            string exceptionMessage = exception.InnerException.Message.ToString();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {

        try
        {
            SaveFilledQuestionnaire();

        }
        catch (Exception exception)
        {
            string exceptionMessage = exception.Message.ToString();
            message.Error("Your answers couldn't be submitted. Please try again.");
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {

            XElement RootElement = questionnaireDocument.Root;

            #region SaveSimpleQuestionnaire

            if (_questionnaireType == enQuestionnaireType.SimpleQuestionnaire)
            {

                IEnumerable<XElement> _listOfQuestions = from element in RootElement.Descendants("Question")
                                                         select element;

                string QuestionChoiceId = string.Empty;
                string ChoiceType = string.Empty;
                string ControlId = string.Empty;
                string value = string.Empty;
                string NewElement = string.Empty;

                foreach (XElement selectedQuestion in _listOfQuestions)
                {

                    #region ADDING_USER_CURRENT_ANSWER
                    QuestionChoiceId = selectedQuestion.Attribute("sequence").Value.ToString();
                    ChoiceType = selectedQuestion.Attribute("type").Value.ToString();
                    ControlId = "";


                    #region Clear_SINGLE-CHOICE_OPTION

                    if (ChoiceType == enQuestionChoiceType.SingleChoice.ToString())
                    {
                        ControlId = "rbQuestionChoice" + QuestionChoiceId + SiteId.ToString();
                        RadioButtonList rbl = (RadioButtonList)pnlQuestion.FindControl(ControlId);
                        rbl.SelectedIndex = -1;
                    }

                    #endregion

                    #region Clear_MULTI-CHOICE_OPTION
                    if (ChoiceType == enQuestionChoiceType.MultiChoice.ToString())
                    {
                        ControlId = "chkBoxQuestionChoice" + QuestionChoiceId + SiteId.ToString();
                        CheckBoxList chkBoxList = (CheckBoxList)pnlQuestion.FindControl(ControlId);

                        for (int index = 0; index < chkBoxList.Items.Count; index++)
                        {
                            if (chkBoxList.Items[index].Selected)
                            {
                                chkBoxList.Items[index].Selected = false;
                            }
                        }

                    }

                    #endregion

                    #endregion

                }


            }

            #endregion

        }
        catch (Exception exception)
        {
            string exceptionMessage = exception.Message.ToString();
            message.Error("Fields couldn't be clear. please try again.");
        }

    }

    protected void btnCalculateResult_Click(object sender, EventArgs e)
    {

        try
        {
            SaveFilledQuestionnaire();
            Session[enSessionKey.currentControl.ToString()] = "ExpressAssessmentResult";
            Session["QueryString"] = "ExpressAssessment";

            string Active = Request.QueryString["Active"] != null ? Request.QueryString["Active"] : string.Empty;
            string Path = Request.QueryString["Path"] != null ? Request.QueryString["Path"] : string.Empty;

            Response.Redirect("~/Webforms/Projects.aspx?report=expressAssessment&puid=" + ProjectUsageId + "&NodeSectionID=" + SectionId +  "&SiteId=" + SiteId + "&Active=" + Active + "&Path=" + Path);

        }
        catch (Exception exception)
        {

            throw exception;
        }
    }

    #endregion

    #region FUNCTIONS

    protected void GetQuestionnaire(enQuestionnaireType questionnaireType)
    {
        try
        {
            _questionTitleTable = new Table();
            _questionBO = new QuestionBO();

            // Clear existing controls (if any)
            pnlQuestionTitle.Controls.Clear();
            string receivedQuestion = "";

            if (questionnaireType == enQuestionnaireType.SimpleQuestionnaire)
            {
                _questionBO.ProjectUsageId = ProjectUsageId;
                _questionBO.QuestionnaireId = questionnaireId = (int)enQuestionnaireType.SimpleQuestionnaire;
                _questionBO.SiteId = SiteId;
                // get questionnaire
                receivedQuestion = _questionBO.GetQuestionnaireByType();

                if (receivedQuestion != string.Empty)
                    questionnaireDocument = XDocument.Parse(receivedQuestion);// Create XML object using string
                else
                    return;
            }

            // Generate Questionnaire
            GenerateQuestionnaire(questionnaireDocument);


        }
        catch (Exception exception)
        {
            throw exception;
        }

    }

    protected void GenerateQuestionnaire(XDocument questionXML)
    {

        try
        {
            _questionTable = new Table();

            /*Clear existing controls (if any)*/
            pnlQuestion.Controls.Clear();
            message.Clear("");

            /*Create and add tablle*/
            pnlQuestion.Controls.Add(_questionTable);

            /* ROOT Element*/
            XElement rootElelement = questionXML.Root;

            string questionnaireType = rootElelement.Name.ToString();

            #region SimpleQuestionnaire
            if (questionnaireType == enQuestionnaireType.SimpleQuestionnaire.ToString())
            {
                CreateQuestionnaireTitle(enQuestionnaireType.SimpleQuestionnaire, rootElelement);

                /* To Read the child Elements of Root Element*/
                foreach (XElement childElement in rootElelement.Elements())
                {
                    /* Fetching the choice type*/
                    string lastChoiceType = childElement.Attribute("type").Value.ToString();

                    #region SimpleQuestionnaireControlInitialization
                    /*Initialize Input controls */
                    //1.RadioButtonList
                    _rbChoice = new RadioButtonList();
                    _rbChoice.ID = "rbQuestionChoice" + childElement.Attribute("sequence").Value.ToString() + SiteId.ToString();
                    _rbChoice.ClientIDMode = ClientIDMode.Static;

                    //2.CheckBoxList
                    _chkBoxChoice = new CheckBoxList();
                    _chkBoxChoice.ID = "chkBoxQuestionChoice" + childElement.Attribute("sequence").Value.ToString() + SiteId.ToString();
                    _chkBoxChoice.ClientIDMode = ClientIDMode.Static;

                    /*Intitilize Validation Controls*/
                    _requiredFieldvalidator = new RequiredFieldValidator();
                    _requiredFieldvalidator.ID = "rfv" + lastChoiceType +
                        childElement.Attribute("sequence").Value.ToString() + SiteId.ToString();
                    _requiredFieldvalidator.Display = ValidatorDisplay.Dynamic;
                    _requiredFieldvalidator.Text = " Required";
                    _requiredFieldvalidator.SetFocusOnError = true;

                    _customeValidator = new CustomValidator();
                    _customeValidator.ID = "cv" + lastChoiceType +
                        childElement.Attribute("sequence").Value.ToString() + SiteId.ToString();
                    _customeValidator.Display = ValidatorDisplay.Dynamic;
                    _customeValidator.Text = " Required";
                    _customeValidator.SetFocusOnError = true;

                    #region ControlToValidate
                    if (lastChoiceType == enQuestionChoiceType.SingleChoice.ToString())
                    {
                        _requiredFieldvalidator.ControlToValidate = _rbChoice.ID;
                    }
                    else if (lastChoiceType == enQuestionChoiceType.MultiChoice.ToString())
                    {
                        _customeValidator.ClientValidationFunction = _chkBoxChoice.ID + "Validate";
                        string validationScript = string.Empty;
                        validationScript = "<script type='text/javascript'> function " + _chkBoxChoice.ID + "Validate(sender, args) { args.IsValid = false;  jQuery('#" + _chkBoxChoice.ID + "').find(':checkbox').each(function() {if (jQuery (this).attr('checked')) { args.IsValid = true; return; } }); }  </script>";
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), _chkBoxChoice.ID, validationScript, false);

                    }

                    #endregion


                    #endregion

                    string childElementName = childElement.Name.ToString();

                    #region CreateQuestion

                    if (childElementName == enQuestionnaireElements.Question.ToString()
                        && lastChoiceType == enQuestionChoiceType.SingleChoice.ToString())
                    {
                        CreateQuestion(childElement, enQuestionChoiceType.SingleChoice);
                    }
                    else if (childElementName == enQuestionnaireElements.Question.ToString()
                        && lastChoiceType == enQuestionChoiceType.MultiChoice.ToString())
                    {
                        CreateQuestion(childElement, enQuestionChoiceType.MultiChoice);
                    }

                    #endregion

                    /*Create Variable to capture the Answers (if any)*/
                    string selectedChoiceValue = string.Empty;
                    List<string> MultiSelectedChoiceValue = new List<string>();
                    /* Add controls A/C to question type*/
                    foreach (XElement nestedChildElement in childElement.Elements())
                    {
                        string nestedChildElementName = nestedChildElement.Name.ToString();

                        #region SINGLE_CHOICE
                        if (lastChoiceType == enQuestionChoiceType.SingleChoice.ToString()
                            && nestedChildElementName != enQuestionnaireElements.Answer.ToString())
                        {
                            /* Add New choice*/
                            string value = nestedChildElement.Attribute("points").Value;
                            string text = nestedChildElement.Attribute("option").Value;
                            _rbChoice.Items.Add(new ListItem(text, value)); continue;
                        }
                        else if (lastChoiceType == enQuestionChoiceType.SingleChoice.ToString()
                            && nestedChildElementName == enQuestionnaireElements.Answer.ToString())
                        {
                            foreach (XElement AnswerElement in nestedChildElement.Elements())
                            {
                                selectedChoiceValue = AnswerElement.Attribute("sequence").Value.ToString();
                                break;
                            }

                        }

                        #endregion

                        #region MULTI_CHOICE

                        /*If Questionnaire Apprear first time for the selected Project*/
                        else if (lastChoiceType == enQuestionChoiceType.MultiChoice.ToString()
                             && nestedChildElementName != enQuestionnaireElements.Answer.ToString())
                        {
                            string value = nestedChildElement.Attribute("points").Value;
                            string text = nestedChildElement.Attribute("option").Value;
                            _chkBoxChoice.Items.Add(new ListItem(text, value)); continue;
                        }
                        /*If Questionnaire have submitted record*/
                        else if (lastChoiceType == enQuestionChoiceType.MultiChoice.ToString()
                            && nestedChildElementName == enQuestionnaireElements.Answer.ToString())
                        {
                            foreach (XElement AnswerElement in nestedChildElement.Elements())
                            {
                                MultiSelectedChoiceValue.Add(AnswerElement.Attribute("sequence").Value.ToString());

                            }

                        }

                        #endregion

                    }

                    #region SINGLE_CHOICE_CONTROL_ADD
                    /* Add Choices for above question*/
                    if (lastChoiceType == enQuestionChoiceType.SingleChoice.ToString())
                    {
                        AddQuestionChoices(enQuestionChoiceType.SingleChoice);
                        /*Check if answer of Question is available or not*/
                        if (selectedChoiceValue != string.Empty)
                        {
                            _rbChoice.SelectedIndex = Convert.ToInt32(selectedChoiceValue) - 1;
                        }
                        selectedChoiceValue = string.Empty;
                        lastChoiceType = "";
                    }
                    #endregion

                    #region MULTI_CHOICE_CONTROL_ADD
                    /* Add MULTI-Choices for above question*/
                    if (lastChoiceType == enQuestionChoiceType.MultiChoice.ToString())
                    {
                        AddQuestionChoices(enQuestionChoiceType.MultiChoice);
                        /*Check if answer of Question is available or not*/
                        if (MultiSelectedChoiceValue.Count() > 0)
                        {
                            foreach (string selectedchoices in MultiSelectedChoiceValue)
                            {
                                int selecteditem = Convert.ToInt32(selectedchoices) - 1;
                                _chkBoxChoice.Items[selecteditem].Selected = true;
                            }
                        }
                        MultiSelectedChoiceValue.Clear();
                        lastChoiceType = "";
                    }
                    #endregion

                }

            #endregion

            }

            CreateButton();
        }
        catch (Exception exception)
        {

            throw exception;
        }
    }

    protected void CreateQuestionnaireTitle(enQuestionnaireType questionnaireType, XElement rootElement)
    {

        try
        {
            _questionRow = new TableRow();
            _questioncell = new TableCell();
            _label = new Label();

            switch (questionnaireType)
            {
                case enQuestionnaireType.SimpleQuestionnaire:
                    _questionRow.CssClass = "QuestionnaireTitle";
                    break;
                default:
                    break;
            }

            _label.ID = "lblQuestionnaireTitle";
            _label.Text = rootElement.Attribute("title").Value;

            // Creating New Row
            _questioncell.Controls.Add(_label);
            _questionRow.Cells.Add(_questioncell);
            _questionTitleTable.Rows.Add(_questionRow);


        }
        catch (Exception exception)
        {

            throw exception;
        }
    }

    protected void CreateQuestion(XElement childElement, enQuestionChoiceType questionnaireChoiceType)
    {

        try
        {
            _questionRow = new TableRow();
            _questioncell = new TableCell();
            _label = new Label();

            _label.ID = "lblQuestion" + childElement.Attribute("sequence").Value.ToString();
            _label.Text = childElement.Attribute("sequence").Value + ". " + childElement.Attribute("title").Value;
            _label.CssClass = "QuestionTitle";

            _questioncell.Controls.Add(_label);

            if (questionnaireChoiceType == enQuestionChoiceType.SingleChoice)
                _questioncell.Controls.Add(_requiredFieldvalidator);
            else if (questionnaireChoiceType == enQuestionChoiceType.MultiChoice)
                _questioncell.Controls.Add(_customeValidator);

            _questionRow.Cells.Add(_questioncell);
            _questionTable.Rows.Add(_questionRow);

        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void AddQuestionChoices(enQuestionChoiceType questionnaireChoiceType)
    {
        try
        {
            _questionRow = new TableRow();
            _questioncell = new TableCell();

            // Adding Control A/C to Question Type
            switch (questionnaireChoiceType)
            {
                case enQuestionChoiceType.SingleChoice:
                    _questioncell.Controls.Add(_rbChoice);
                    _questionRow.Cells.Add(_questioncell);
                    _questionTable.Rows.Add(_questionRow);
                    break;
                case enQuestionChoiceType.MultiChoice:
                    _questioncell.Controls.Add(_chkBoxChoice);
                    _questionRow.Cells.Add(_questioncell);
                    _questionTable.Rows.Add(_questionRow);
                    break;

                default:
                    break;
            }

        }
        catch (Exception exception)
        {
            throw exception;
        }

    }

    protected void CreateButton()
    {

        try
        {
            _formSubmitTable = new Table();

            pnlQuestionSubmit.Controls.Clear();
            pnlQuestionSubmit.Controls.Add(_formSubmitTable);

            _questionRow = new TableRow();

            string[] _buttonNames = new string[3] { "btnSave", "btnClear", "btnCalculateResult" };

            // Create Cell for each Button
            foreach (string value in _buttonNames)
            {
                _button = new Button();
                _questioncell = new TableCell();

                if (value == "btnSave")
                {
                    _button.ID = "btnSave";
                    _button.Text = "Save";
                    _button.Click += new System.EventHandler(this.btnSave_Click);

                    //button.Attributes.Add("onClick", "scrollTop();");

                    _questioncell.Controls.Add(_button);
                    _button.CausesValidation = false;
                    _questionRow.Cells.Add(_questioncell); continue;
                }
                else if (value == "btnClear")
                {
                    _button.ID = "btnClear";
                    _button.Text = "Clear";
                    _button.Click += new System.EventHandler(this.btnClear_Click);
                    _button.CausesValidation = false;
                    _questioncell.Controls.Add(_button);
                    _questionRow.Cells.Add(_questioncell); continue;
                }
                else if (value == "btnCalculateResult")
                {
                    _button.ID = "btnCalculateResult";
                    _button.Text = "Calculate Result";
                    _button.Click += new System.EventHandler(this.btnCalculateResult_Click);
                    _questioncell.Controls.Add(_button);
                    _questionRow.Cells.Add(_questioncell); continue;
                }
                else { break; }

            }

            // Creating new row
            _formSubmitTable.Rows.Add(_questionRow);

        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void SaveFilledQuestionnaire()
    {
        try
        {

            XElement RootElelement = questionnaireDocument.Root;

            #region SaveSimpleQuestionnaire

            if (_questionnaireType == enQuestionnaireType.SimpleQuestionnaire)
            {

                IEnumerable<XElement> ListofQuestion = from element in RootElelement.Descendants("Question")
                                                       select element;

                string QuestionChoiceId = string.Empty;
                string ChoiceType = string.Empty;
                string ControlId = string.Empty;
                string value = string.Empty;
                string NewElement = string.Empty;

                foreach (XElement selectedQuestion in ListofQuestion)
                {

                    #region REMOVE_EXISTING_ANSWERS
                    IEnumerable<XElement> ListofAnswer = from answerelement in selectedQuestion.Descendants("Answer")
                                                         select answerelement;
                    if (ListofAnswer.Count() > 0)
                    {
                        foreach (XElement answerElement in ListofAnswer)
                        {
                            answerElement.Remove();
                            break;
                        }
                    }

                    #endregion
                }

                foreach (XElement selectedQuestion in ListofQuestion)
                {

                    #region ADDING_USER_CURRENT_ANSWER
                    QuestionChoiceId = selectedQuestion.Attribute("sequence").Value.ToString();
                    ChoiceType = selectedQuestion.Attribute("type").Value.ToString();
                    ControlId = "";
                    value = "";
                    NewElement = "";

                    #region INSERT_SINGLE-CHOICE_OPTION


                    if (ChoiceType == enQuestionChoiceType.SingleChoice.ToString())
                    {
                        ControlId = "rbQuestionChoice" + QuestionChoiceId + SiteId.ToString();
                        RadioButtonList rbl = (RadioButtonList)pnlQuestion.FindControl(ControlId);
                        value = rbl.SelectedValue;

                        int selectedIndex = rbl.SelectedIndex + 1;
                        NewElement = @"<Answer points='" + value + "'><SelectedChoice sequence='" + selectedIndex + "'/></Answer>";

                        /*Add Answer Element*/
                        XElement AnswerElement = XElement.Parse(NewElement);
                        selectedQuestion.Add(AnswerElement);

                    }

                    #endregion

                    #region INSERT_MULTI-CHOICE_OPTION
                    if (ChoiceType == enQuestionChoiceType.MultiChoice.ToString())
                    {
                        ControlId = "chkBoxQuestionChoice" + QuestionChoiceId + SiteId.ToString();
                        CheckBoxList chkBoxList = (CheckBoxList)pnlQuestion.FindControl(ControlId);
                        List<string> optionList = new List<string>();
                        int totalPoints = 0;
                        for (int index = 0; index < chkBoxList.Items.Count; index++)
                        {

                            if (chkBoxList.Items[index].Selected)
                            {
                                totalPoints = totalPoints + Convert.ToInt32((chkBoxList.Items[index].Value));
                                int sequence = index + 1;
                                optionList.Add(Convert.ToString(sequence));
                            }
                        }

                        value = Convert.ToString(totalPoints);
                        NewElement = @"<Answer points='" + value + "'>";
                        foreach (string option in optionList)
                        {

                            NewElement += "<SelectedChoice sequence='" + option + "'/>";
                        }
                        NewElement += "</Answer>";
                        /*Add Answer Element*/
                        XElement AnswerElement = XElement.Parse(NewElement);
                        selectedQuestion.Add(AnswerElement);

                    }

                    #endregion

                    #endregion

                }

                _questionBO.SaveFilledQuestionnaire(questionnaireId, ProjectUsageId,SiteId, questionnaireDocument.Root, UserId);
                message.Success("Your Answers have been submitted.");
                /* pnlQuestion.Controls.Clear();
                 pnlQuestionTitle.Controls.Clear();
                 pnlQuestionSubmit.Controls.Clear();*/
                lblSiteName.Text = "";

            }

            #endregion

        }
        catch (Exception exception)
        {
            throw exception;
        }

    }


    #endregion
}
