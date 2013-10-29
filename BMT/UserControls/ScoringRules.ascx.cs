using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BMTBLL;
using BMTBLL.Classes;
using BMT.WEB;
using BMTBLL.BusinessObjects;
using BMTBLL.Enumeration;


public partial class ScoringRules : System.Web.UI.UserControl
{
    #region VARIABLES
    TemplatesBO _Template;
    ScoringRulesBO _scoringRulesBO;
    ProjectTemplateBO _template;
    private List<string> standarSequenceList = new List<string>();
    private List<string> knowledgeBaseIdList = new List<string>();
    private List<string> standardsElementsList = new List<string>();
    int templateId;

    #endregion
    #region CONTROLS
    private Table _NCQASubmissionTable;
    private List<AnswerTypeEnum> answerTypeEnumId;
    private List<KnowledgeBase> templateHeaders;
    private List<KnowledgeBase> templateSubHeaders;
    private List<KnowledgeBase> templateQuestions;
    private List<KnowledgeBaseTemplate> knowledgeBaseTemplateList;
    private List<ScoringRule> scoringRulesList;
    private List<AnswerTypeWeightage> simpleSumList;
    private TableRow _tableRow;
    private TableCell _tableCell;

    private RadioButton _rbRulesBase;
    private TextBox _scoringRulesTextBox;
    private TextBox _simpleSumTextBox;
    private DropDownList _ddConditional;
    private DropDownList _ddAndOr;
    private ListBox _lbMustPresent;
    private Label _label;
    private Label _whiteSpaceLabel;
    private Table _elementTable;
    private Table _simpleSum;
    private Table _scoringRules;
    private Table PCMHTable;
    private Image _image;
    #endregion

    #region CONSTANTS
    private const int DEFAULT_NUMBER_CONSTANT = 1;
    private const string SCORING_RULES = "scoring-rules";
    private const string SCORING_RULES_DROPDOWN = "scoring-rules-dropdown";
    private const string SCORING_RULES_LISTBOX = "scoring-rules-listbox";
    private const string SCORING_RULES_SUB_HEADER = "scoring-rules-subh-header";
    private const string SCORING_RULES_SUB_HEADER_MUSTPASS = "scoring-rules-subh-header-mustpass";
    private const string SIMPLE_SUM = "Simple Sum";
    private const string RULES_BASE = "Rules Based";
    private const string ELEMENT_MAX = "Element Max = ";
    private const string TXT_SUM = "S Sum";
    private const string TXT_ELSE_IF = "ELSE IF:";
    private const string TXT_ELSE = "ELSE:";
    private const string TXT_IF = "IF:";
    private const string SCORING_RULES_TEXBOX = "scoring-rules-textbox";
    private const string SIMPLE_SUM_TEXBOX = "simple-sum-textbox";
    private const string DEFAULT_LABEL_TEXT_YES = "Yes =";
    private const string DEFAULT_LABEL_TEXT_NO = "No  =";
    private const string DEFAULT_LABEL_TEXT_NA = "NA  =";
    private const string REQUIRED_VALIDATOR_MESSAGE = "*";
    private const string DEFAULT_LABEL_TEXT_ZERO_PERCENT = "Total = 0%";
    private const string DEFAULT_LABEL_TEXT_TWENTYFIVE_PERCENT = "Total = 25%";
    private const string DEFAULT_LABEL_TEXT_FIFTY_PERCENT = "Total = 50%";
    private const string DEFAULT_LABEL_TEXT_SEVENTYFIVE_PERCENT = "Total = 75%";
    private const string DEFAULT_LABEL_TEXT_HUNDRED_PERCENT = "Total = 100%";
    #endregion

    #region EVENTS
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            
            if (Session["TemplateId"] != null)
            {
                    scoringRulesMessage.Clear("");
                    int userId = Session[enSessionKey.UserApplicationId.ToString()] != null ? Convert.ToInt32(Session[enSessionKey.UserApplicationId.ToString()]) : 0;
                    templateId = Convert.ToInt32(Session["TemplateId"].ToString());
                    templateTitle.Text = GetTemplateNameByTemplateId(templateId);
                    GenerateLayOut();
                    GetMOReList(_NCQASubmissionTable);
                    upnlScoringRules.Update();          
            }   
        }
        catch (Exception exception)
        {
            Logger.PrintError(exception);
        }

    }


    protected void btnsaveScoringRules_Click(object sender, EventArgs e)
    {
        /*local varibles*/
        #region local variables
        _scoringRulesBO = new ScoringRulesBO();
        int standardsequenceIndex = 0;
        int rowNumber;
        int answerTypeId;
        int simpleSumRowIndex;
        int listBoxIndex;
        int scoringRulesRowIndex;
        string selectedMustPresentSequence;
        string minYesFactors;
        string mustPresentFactor;
        string maxYesFactors;
        string abcentFactor;
        #endregion

        List<KnowledgeBaseTemplate> GetKnowledgeBase = _scoringRulesBO.GetKnowledgeBaseList(templateId);
  
        foreach (var knowledgeBase in GetKnowledgeBase)
        {            
            RadioButton rbscoringRule = (RadioButton)pnlNCQASummary.FindControl("ruleBase" + knowledgeBase.KnowledgeBaseId + knowledgeBase.ParentKnowledgeBaseId);
            RadioButton simpleSum = (RadioButton)pnlNCQASummary.FindControl("RdoSimpleSum" + knowledgeBase.KnowledgeBaseId + knowledgeBase.ParentKnowledgeBaseId);
            TextBox maxElements = (TextBox)pnlNCQASummary.FindControl("ElementMax" + knowledgeBase.KnowledgeBaseId + knowledgeBase.ParentKnowledgeBaseId);
            Label maxElementsLabel = (Label)pnlNCQASummary.FindControl("maxElementLabel" + knowledgeBase.KnowledgeBaseId + knowledgeBase.ParentKnowledgeBaseId);

            if (rbscoringRule != null)
                if (rbscoringRule.Checked == true)
                {
                    Table scoringRule = (Table)pnlNCQASummary.FindControl("scoringRules" + knowledgeBase.KnowledgeBaseId + standarSequenceList[standardsequenceIndex]);
                    if (scoringRule != null)
                        rowNumber = scoringRule.Rows.Count;
                    else
                        rowNumber = 0;

                    _scoringRulesBO.DeleteExistingScore(knowledgeBase.KnowledgeBaseTemplateId);
                    _scoringRulesBO.DeleteExistingWeightage(knowledgeBase.KnowledgeBaseTemplateId);

                    if (maxElements.Text == "")
                    {
                        scoringRulesMessage.Error("Error! Required All Fields.");
                        return;
                    }
                    else
                    {
                        maxElementsLabel.Style.Add("display", "none");
                    }
                    if (rowNumber != 0)
                    {
                        scoringRulesRowIndex = DEFAULT_NUMBER_CONSTANT;
                        while (rowNumber > 0)
                        {
                            scoringRulesList = _scoringRulesBO.GetScoringRules(templateId, knowledgeBase.KnowledgeBaseId);
                            selectedMustPresentSequence = string.Empty;
                            int knowledgeBaseTemplateId = knowledgeBase.KnowledgeBaseTemplateId;
                            DropDownList ddConditionaldd = (DropDownList)pnlNCQASummary.FindControl("ddConditional" + standarSequenceList[standardsequenceIndex] + knowledgeBase.KnowledgeBaseId + scoringRulesRowIndex);
                            TextBox scoringRulesText = (TextBox)pnlNCQASummary.FindControl("scoringRulesTextBox" + standarSequenceList[standardsequenceIndex] + knowledgeBase.KnowledgeBaseId + scoringRulesRowIndex);
                            DropDownList ddAndOrdd = (DropDownList)pnlNCQASummary.FindControl("ddAndOr" + standarSequenceList[standardsequenceIndex] + knowledgeBase.KnowledgeBaseId + scoringRulesRowIndex);
                            ListBox ddMustPresentdd = (ListBox)pnlNCQASummary.FindControl("ddMustPresent" + standarSequenceList[standardsequenceIndex] + knowledgeBase.KnowledgeBaseId + scoringRulesRowIndex);
                            if (ddMustPresentdd != null && ddMustPresentdd.Items.Count > 0)
                            {
                                for (listBoxIndex = ddMustPresentdd.Items.Count - 1; listBoxIndex >= 0; listBoxIndex--)
                                {
                                    if (ddMustPresentdd.Items[listBoxIndex].Selected)
                                    {
                                        selectedMustPresentSequence = ddMustPresentdd.Items[listBoxIndex].Text.Substring(1) + "," + selectedMustPresentSequence;                                        
                                    }
                                }
                                selectedMustPresentSequence = selectedMustPresentSequence != string.Empty ? selectedMustPresentSequence.Remove(selectedMustPresentSequence.Length - 1) : selectedMustPresentSequence;
                            }
                            Label labelTotal = (Label)pnlNCQASummary.FindControl("label" + standarSequenceList[standardsequenceIndex] + knowledgeBase.KnowledgeBaseId + scoringRulesRowIndex);
                            Label rfvlabes = (Label)pnlNCQASummary.FindControl("rfvlabel" + standarSequenceList[standardsequenceIndex] + knowledgeBase.KnowledgeBaseId + scoringRulesRowIndex);
                            Label rfvListBox = (Label)pnlNCQASummary.FindControl("rfvListBox" + standarSequenceList[standardsequenceIndex] + knowledgeBase.KnowledgeBaseId + scoringRulesRowIndex);

                            if (ddConditionaldd != null && scoringRulesText != null && ddAndOrdd != null && ddMustPresentdd != null)
                            {
                                if (ddConditionaldd.SelectedValue == ">=")
                                {
                                    minYesFactors = scoringRulesText.Text;                                   
                                    
                                    if (minYesFactors == "")
                                    {
                                        scoringRulesMessage.Error("Error! Required All Fields.");
                                        rfvlabes.Style.Add("display", "visible");
                                        return;
                                    }
                                    maxYesFactors = null;
                                }
                                else if (ddConditionaldd.SelectedValue == "<=")
                                {
                                    maxYesFactors = scoringRulesText.Text;
                                    if (maxYesFactors == "")
                                    {
                                        scoringRulesMessage.Error("Error! Required All Fields.");
                                        rfvlabes.Style.Add("display", "visible");
                                        return;
                                    }
                                    minYesFactors = null;
                                }
                                else
                                {
                                    minYesFactors = null;
                                    maxYesFactors = null;
                                }

                                if (ddAndOrdd.SelectedValue == "AND")
                                {
                                    mustPresentFactor = selectedMustPresentSequence;
                                    if (mustPresentFactor == "")
                                    {
                                        scoringRulesMessage.Error("Error! Required All Fields.");
                                        rfvListBox.Style.Add("display", "visible");
                                        return;
                                    }

                                    abcentFactor = null;
                                }
                                else if (ddAndOrdd.SelectedValue == "NOT")
                                {
                                    abcentFactor = selectedMustPresentSequence;
                                    if (abcentFactor == "")
                                    {
                                        scoringRulesMessage.Error("Error! Required All Fields.");
                                        rfvListBox.Style.Add("display", "visible");
                                        return;
                                    }
                                    mustPresentFactor = null;
                                }
                                else
                                {
                                    mustPresentFactor = null;
                                    abcentFactor = null;
                                }
                            }
                            else
                            {
                                minYesFactors = null;
                                maxYesFactors = null;
                                mustPresentFactor = null;
                                abcentFactor = null;
                            }

                            foreach (var isExistScoringRule in scoringRulesList)
                            {   
                                standardsElementsList = _scoringRulesBO.GetStandardElements(knowledgeBase.KnowledgeBaseTemplateId);    

                                int previouseMaxFactor = (isExistScoringRule.MaxYesFactor.HasValue ? (int)isExistScoringRule.MaxYesFactor : 0);
                                int previouseMinFactor = (isExistScoringRule.MinYesFactor.HasValue ? (int)isExistScoringRule.MinYesFactor : 0);

                                if ((previouseMaxFactor == Convert.ToInt32(maxYesFactors)) && (previouseMinFactor == Convert.ToInt32(minYesFactors)) && (isExistScoringRule.MustPresentFactorSequence == mustPresentFactor) && (isExistScoringRule.AbsentFactorSequence == abcentFactor))
                                {
                                    scoringRulesMessage.Error("Error! Same criteria for different percentages is not allowed. Please check " + standardsElementsList[0]);
                                   return;
                                }                                
                            }
                            string score = labelTotal.Text.Substring(8);
                            scoringRulesRowIndex++;

                            _scoringRulesBO.SaveScoringRules(knowledgeBaseTemplateId, score, minYesFactors, maxYesFactors, mustPresentFactor, abcentFactor);
                            rowNumber--;
                        }
                    }

                    Table simpleSumTable = (Table)pnlNCQASummary.FindControl("simpleSum" + knowledgeBase.KnowledgeBaseId + standarSequenceList[standardsequenceIndex]);

                    if (scoringRule != null)
                        rowNumber = simpleSumTable.Rows.Count;
                    else 
                        rowNumber = 0;

                    if (rowNumber != 0)
                    {
                        rowNumber = rowNumber - DEFAULT_NUMBER_CONSTANT;
                        int knowledgeBaseTemplateId = knowledgeBase.KnowledgeBaseTemplateId;
                        simpleSumRowIndex = DEFAULT_NUMBER_CONSTANT;
                        TextBox answerTypeCheck = (TextBox)pnlNCQASummary.FindControl("txtNAQuestion" + knowledgeBase.KnowledgeBaseId + knowledgeBase.ParentKnowledgeBaseId);
                        if (answerTypeCheck.Text != string.Empty)
                            answerTypeId = (int)enAnswerType.YesNoNA;
                        else
                            answerTypeId = (int)enAnswerType.YesNo;

                        answerTypeEnumId = _scoringRulesBO.GetAnswerTypeEnum(answerTypeId);

                        while (rowNumber > 1)
                        {
                            TextBox simpleSumWeightage = (TextBox)pnlNCQASummary.FindControl("txtQuestion" + knowledgeBase.KnowledgeBaseId + knowledgeBase.ParentKnowledgeBaseId + simpleSumRowIndex);
                            _scoringRulesBO.SaveSimpleSum(answerTypeEnumId[simpleSumRowIndex - DEFAULT_NUMBER_CONSTANT].AnswerTypeEnumId, knowledgeBaseTemplateId, Convert.ToInt32(simpleSumWeightage.Text));
                            simpleSumRowIndex++;
                            rowNumber--;
                        }
                        if (answerTypeId == DEFAULT_NUMBER_CONSTANT)
                        {
                            _scoringRulesBO.SaveSimpleSum(answerTypeEnumId[simpleSumRowIndex - DEFAULT_NUMBER_CONSTANT].AnswerTypeEnumId, knowledgeBaseTemplateId, Convert.ToInt32(answerTypeCheck.Text));
                        }

                    }
                    _scoringRulesBO.SaveMaxPoints(knowledgeBase.KnowledgeBaseTemplateId, knowledgeBase.KnowledgeBaseId, Convert.ToInt32(maxElements.Text));                    

                }
                else if (simpleSum.Checked == true)
                {
                    if (maxElements.Text == "")
                    {
                        scoringRulesMessage.Error("Error! Required All Fields.");
                        return;
                    }
                    else
                    {
                        maxElementsLabel.Style.Add("display", "none");
                    }
                    _scoringRulesBO.DeleteExistingWeightage(knowledgeBase.KnowledgeBaseTemplateId);
                    _scoringRulesBO.DeleteExistingScore(knowledgeBase.KnowledgeBaseTemplateId);
                    Table simpleSumTable = (Table)pnlNCQASummary.FindControl("simpleSum" + knowledgeBase.KnowledgeBaseId + standarSequenceList[standardsequenceIndex]);
                    rowNumber = simpleSumTable.Rows.Count;
                    if (rowNumber != 0)
                    {
                        rowNumber = rowNumber - DEFAULT_NUMBER_CONSTANT;
                        int knowledgeBaseTemplateId = knowledgeBase.KnowledgeBaseTemplateId;
                        simpleSumRowIndex = DEFAULT_NUMBER_CONSTANT;
                        TextBox answerTypeCheck = (TextBox)pnlNCQASummary.FindControl("txtNAQuestion" + knowledgeBase.KnowledgeBaseId + knowledgeBase.ParentKnowledgeBaseId);
                        if (answerTypeCheck.Text != string.Empty)
                            answerTypeId = (int)enAnswerType.YesNoNA;                          
                        else
                            answerTypeId = (int)enAnswerType.YesNo;

                        answerTypeEnumId = _scoringRulesBO.GetAnswerTypeEnum(answerTypeId);

                        while (rowNumber > DEFAULT_NUMBER_CONSTANT)
                        {
                            TextBox simpleSumWeightage = (TextBox)pnlNCQASummary.FindControl("txtQuestion" + knowledgeBase.KnowledgeBaseId + knowledgeBase.ParentKnowledgeBaseId + simpleSumRowIndex);
                            _scoringRulesBO.SaveSimpleSum(answerTypeEnumId[simpleSumRowIndex - DEFAULT_NUMBER_CONSTANT].AnswerTypeEnumId, knowledgeBaseTemplateId, Convert.ToInt32(simpleSumWeightage.Text));
                            simpleSumRowIndex++;
                            rowNumber--;
                        }
                        if (answerTypeId == DEFAULT_NUMBER_CONSTANT)
                        {
                            _scoringRulesBO.SaveSimpleSum(answerTypeEnumId[simpleSumRowIndex - DEFAULT_NUMBER_CONSTANT].AnswerTypeEnumId, knowledgeBaseTemplateId, Convert.ToInt32(answerTypeCheck.Text));
                        }

                    }
                    _scoringRulesBO.SaveMaxPoints(knowledgeBase.KnowledgeBaseTemplateId,knowledgeBase.KnowledgeBaseId, Convert.ToInt32(maxElements.Text));
                }
                standardsequenceIndex++;

            }

        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "SettingsFormSection", "SettingsFormSection('#createMORe');returnBackTemplate()", true);
        
    }

    #endregion

    #region FUNCTIONS

    public void GenerateLayOut()
    {
        _NCQASubmissionTable = new Table();
        _NCQASubmissionTable.ID = "NCQASubmissionTable";
        _NCQASubmissionTable.ClientIDMode = ClientIDMode.Static;

        pnlNCQASummary.Controls.Clear();
        pnlNCQASummary.Controls.Add(_NCQASubmissionTable);

    }

    public void GetMOReList(Table masterTable)
    {
        try
        {
        string standardSequence = string.Empty;

        _Template = new TemplatesBO();
        templateHeaders = _Template.GetAllHeaders(templateId);
        foreach (KnowledgeBase header in templateHeaders)
        {
            standardSequence = header.KnowledgeBaseId.ToString();

            _tableRow = new TableRow();
            _tableCell = new TableCell();
            _tableCell.ColumnSpan = 7;
            _image = new Image();
            _image.Attributes.Add("onclick", "javascript:PCMHtoggle('" + standardSequence + "');");
            _image.ClientIDMode = ClientIDMode.Static;
            _image.ID = "imgElement" + standardSequence;
            _image.ImageUrl = "../Themes/Images/Plus-1.png";

            _image.CssClass = "toggle-img";
            _tableCell.Controls.Add(_image);

            _whiteSpaceLabel = new Label();
            _whiteSpaceLabel.Text = "  ";
            _tableCell.Controls.Add(_whiteSpaceLabel);

            _label = new Label();
            _label.Text = header.Name;
            _label.Width = 707;
            _tableCell.Controls.Add(_label);

            _tableCell.Height = 20;
            _tableRow.Controls.Add(_tableCell);

            masterTable.Controls.Add(_tableRow);
            _NCQASubmissionTable.Controls.Add(_tableRow);

            AddPCMHTable(header.KnowledgeBaseId.ToString(), standardSequence);
        }          
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void AddPCMHTable(string parentId, string elementSequence)
    {
        try
        {
            PCMHTable = new Table();
            _tableRow = new TableRow();
            _tableCell = new TableCell();

            PCMHTable.ID = "PCMHTable" + elementSequence;
            PCMHTable.ClientIDMode = ClientIDMode.Static;

            _tableCell.ColumnSpan = 7;
            _tableCell.Controls.Add(PCMHTable);
            _tableRow.Controls.Add(_tableCell);


            _NCQASubmissionTable.Controls.Add(_tableRow);

            AddStandards(parentId, PCMHTable);

        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void AddStandards(string parentId, Table PCMHTable)
    {
        try
        {
            _Template = new TemplatesBO();
            templateSubHeaders = _Template.GetAllSubHeaders(Convert.ToInt32(parentId), templateId);

            foreach (var subHeader in templateSubHeaders)
            {
                AddElementsTable(PCMHTable, subHeader.Name, subHeader.MustPass.ToString(), Convert.ToInt32(parentId), Convert.ToInt32(subHeader.KnowledgeBaseId));

                templateQuestions = _Template.GetAllQuestions(Convert.ToInt32(subHeader.KnowledgeBaseId), templateId);
                AddStandardRow(templateQuestions, parentId, subHeader.KnowledgeBaseId.ToString());

            }

        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void AddElementsTable(Table PCMHTable, string subHeader, string mustPass, int parentId, int knowledgeBaseId)
    {
        try
        {
            int standardSequence = parentId;
            string standardTitle = subHeader;

            standarSequenceList.Add(standardSequence.ToString());
            _scoringRulesBO = new ScoringRulesBO();

            _tableRow = new TableRow();
            
            _tableRow = new TableRow();
            _tableCell = new TableCell();
            _tableCell.VerticalAlign = VerticalAlign.Top;
            _image = new Image();
            _image.Attributes.Add("onclick", "javascript:PCMHElementtoggle('" + knowledgeBaseId + "','" + standardSequence + "');");
            _image.ClientIDMode = ClientIDMode.Static;
            _image.ID = "imgStandard" + knowledgeBaseId + standardSequence;

            _image.CssClass = "toggle-img";
            _tableCell.Controls.Add(_image);

            _tableCell.Width = 12;
            _tableRow.Controls.Add(_tableCell);

            _tableCell = new TableCell();
            _tableCell.VerticalAlign = VerticalAlign.Top;

            _label = new Label();
            _label.Width = 230;
            if (mustPass == "True")
                _label.CssClass = SCORING_RULES_SUB_HEADER_MUSTPASS;
            else
                _label.CssClass = SCORING_RULES_SUB_HEADER;
            _label.Text = standardTitle;
            _tableCell.Controls.Add(_label);

            _tableRow.Controls.Add(_tableCell);

            _tableCell = new TableCell();
            _tableCell.ID = "subHeader" + knowledgeBaseId + standardSequence;
            _tableCell.Style.Add("display", "none");
            _tableCell.VerticalAlign = VerticalAlign.Top;

            RadioButton rbSimpleSum = new RadioButton();
            rbSimpleSum.CssClass = SCORING_RULES_SUB_HEADER;
            rbSimpleSum.Text = SIMPLE_SUM;
            rbSimpleSum.Width = 80;
            rbSimpleSum.ID = "RdoSimpleSum" + knowledgeBaseId + parentId;
            rbSimpleSum.GroupName = "Formula" + knowledgeBaseId;
            rbSimpleSum.Attributes.Add("onchange", "javascript:OnClickSimpleSum('" + knowledgeBaseId + "','" + standardSequence + "');");
            _tableCell.Controls.Add(rbSimpleSum);

            _tableRow.Controls.Add(_tableCell);

            _tableCell = new TableCell();
            _tableCell.ID = "subHeaderRulesBase" + knowledgeBaseId + standardSequence;
            _tableCell.Style.Add("display", "none");
            _tableCell.VerticalAlign = VerticalAlign.Top;
            _rbRulesBase = new RadioButton();
            _rbRulesBase.CssClass = SCORING_RULES_SUB_HEADER;
            _rbRulesBase.Text = RULES_BASE;
            _rbRulesBase.Width = 97;
            _rbRulesBase.ID = "ruleBase" + knowledgeBaseId + parentId;
            _scoringRulesBO = new ScoringRulesBO();
            
            scoringRulesList = _scoringRulesBO.GetScoringRules(templateId, knowledgeBaseId);
            simpleSumList = _scoringRulesBO.GetSumList(templateId, Convert.ToInt32(knowledgeBaseId));

            if(scoringRulesList.Count > 0)
            foreach(var scoringRuleIndex in scoringRulesList)
            {
                knowledgeBaseTemplateList = _scoringRulesBO.GetKnowledgeTemplateList(templateId, scoringRuleIndex.KnowledgeBaseTemplateId);
                if (knowledgeBaseId == knowledgeBaseTemplateList[0].KnowledgeBaseId)
                    _rbRulesBase.Checked = true;
            }
            else if (simpleSumList.Count > 0)
            {
                rbSimpleSum.Checked = true;
            }

            _rbRulesBase.GroupName = "Formula" + knowledgeBaseId;
            _rbRulesBase.Attributes.Add("onchange", "javascript:OnClickScoringRule('" + knowledgeBaseId + "','" + standardSequence + "');");
            _tableCell.Controls.Add(_rbRulesBase);

            _label = new Label();
            _label.CssClass = SCORING_RULES_SUB_HEADER;
            _label.Text = ELEMENT_MAX;
            _label.Width = 80;
            _tableCell.Controls.Add(_label);

            _scoringRulesTextBox = new TextBox();
            _scoringRulesTextBox.CssClass = SCORING_RULES_TEXBOX;
            _scoringRulesTextBox.Style["text-align"] = "center";
            _scoringRulesTextBox.ID = "ElementMax" + knowledgeBaseId + parentId;

            _label = new Label();
            _label.Text = " *";
            _label.Style.Add("color", "red");
            _label.ID = "maxElementLabel" + knowledgeBaseId + parentId;
            _label.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            
            knowledgeBaseTemplateList = _scoringRulesBO.GetMaxPoints(knowledgeBaseId, templateId);
            foreach (var maxPoints in knowledgeBaseTemplateList)
            {
                if (knowledgeBaseTemplateList.Count > 0)
                    if (maxPoints.MaxPoints != null)
                    {
                        _scoringRulesTextBox.Text = maxPoints.MaxPoints.ToString();
                        _label.Style.Add("display", "none");
                    }
                    else
                        _label.Style.Add("display", "visible");
            }           

            _scoringRulesTextBox.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            _scoringRulesTextBox.MaxLength = 2;
            _scoringRulesTextBox.Attributes.Add("onkeyup", "javascript:isElementMaxNumberKey('" + _scoringRulesTextBox.ClientID + "' , '" + _label.ID + "')");
            
            _tableCell.Controls.Add(_scoringRulesTextBox);
            _tableCell.Controls.Add(_label);

            _tableRow.Controls.Add(_tableCell);

            PCMHTable.Controls.Add(_tableRow);

            _elementTable = new Table();
            _tableRow = new TableRow();
            _tableCell = new TableCell();
            _tableRow.Controls.Add(_tableCell);

            _tableCell = new TableCell();
            _tableCell.Width = 180;
            _tableCell.VerticalAlign = VerticalAlign.Top;
            _elementTable.ID = "elementTable" + knowledgeBaseId + standardSequence;
            _elementTable.ClientIDMode = ClientIDMode.Static;

            _tableCell.Controls.Add(_elementTable);
            _tableRow.Controls.Add(_tableCell);

            _tableCell = new TableCell();
            _tableCell.VerticalAlign = VerticalAlign.Top;
            _tableCell.HorizontalAlign = HorizontalAlign.Left;
            _tableCell.Width = 30;
            _simpleSum = new Table();
            _simpleSum.Style.Add("Width", "100%");

            _simpleSum.ID = "simpleSum" + knowledgeBaseId + standardSequence;
            _simpleSum.ClientIDMode = ClientIDMode.Static;

            _tableCell.Controls.Add(_simpleSum);
            _tableRow.Controls.Add(_tableCell);

            _tableCell = new TableCell();
            _tableCell.ID = knowledgeBaseId.ToString() + standardSequence;
            _tableCell.VerticalAlign = VerticalAlign.Top;
            _tableCell.HorizontalAlign = HorizontalAlign.Left;
            _scoringRules = new Table();
            _scoringRules.Style.Add("Width", "100%");

            _scoringRules.ID = "scoringRules" + knowledgeBaseId + standardSequence;
            _scoringRules.ClientIDMode = ClientIDMode.Static;

            _tableCell.Controls.Add(_scoringRules);
            _tableRow.Controls.Add(_tableCell);

            PCMHTable.Controls.Add(_tableRow);
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    protected void AddStandardRow(List<KnowledgeBase> _listOfNCQADetail, string parentId, string knowledgeBaseId)
    {
        try
        {
            /*Local variable*/
            #region local varaibles
            int count;
            string rowIndex;
            int newRowIndex;
            int simpleSumRowIndex;
            #endregion

            if (_listOfNCQADetail.Count > 0)
            {
                _scoringRulesBO = new ScoringRulesBO();
                //simpleSumList = _scoringRulesBO.GetSumList(templateId, Convert.ToInt32(knowledgeBaseId));

                simpleSumRowIndex = DEFAULT_NUMBER_CONSTANT;
                _tableRow = new TableRow();
                _tableCell = new TableCell();

                _label = new Label();
                _label.Text = DEFAULT_LABEL_TEXT_YES;
                _label.CssClass = SCORING_RULES;
                _label.Width = 35;

                _tableCell = new TableCell();
                _tableCell.HorizontalAlign = HorizontalAlign.Center;
                _tableCell.Controls.Add(_label);

                _simpleSumTextBox = new TextBox();
                _simpleSumTextBox.ID = "txtQuestion" + knowledgeBaseId + parentId + simpleSumRowIndex ;
                _simpleSumTextBox.Style["text-align"] = "center";

                
                if (simpleSumList.Count > 0)
                foreach (var answerWeightage in simpleSumList)
                {
                    if (answerWeightage.AnswerTypeEnumId == 1)
                        _simpleSumTextBox.Text = answerWeightage.Weightage.ToString();
                }
                else
                    _simpleSumTextBox.Text = DEFAULT_NUMBER_CONSTANT.ToString();

                _simpleSumTextBox.CssClass = SCORING_RULES;
                _simpleSumTextBox.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                _simpleSumTextBox.Attributes.Add("onkeyup", "javascript:numberOnly('" + _simpleSumTextBox.ClientID + "')");
                _simpleSumTextBox.Style.Add("font-size", "10px");

                _simpleSumTextBox.CssClass = SIMPLE_SUM_TEXBOX;
                _simpleSumTextBox.MaxLength = DEFAULT_NUMBER_CONSTANT;
                _tableCell.Controls.Add(_simpleSumTextBox);
                
                _tableCell.Width = 70;
                _tableRow.Controls.Add(_tableCell);                

                RequiredFieldValidator rfv1 = new RequiredFieldValidator();
                rfv1.Text = REQUIRED_VALIDATOR_MESSAGE;
                rfv1.ErrorMessage = REQUIRED_VALIDATOR_MESSAGE;
                rfv1.ID = "rfv" + knowledgeBaseId + parentId + simpleSumRowIndex;
                rfv1.ControlToValidate = "txtQuestion" + knowledgeBaseId + parentId + simpleSumRowIndex;
                rfv1.ValidationGroup = "btnsaveScoringRules";
                rfv1.Display = ValidatorDisplay.Dynamic;
                                
                _tableCell = new TableCell();
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.Controls.Add(rfv1);

                _tableRow.Controls.Add(_tableCell);
                _simpleSum.Controls.Add(_tableRow);

                simpleSumRowIndex++;

                _tableRow = new TableRow();
                _tableCell = new TableCell();

                _label = new Label();
                _label.Text = DEFAULT_LABEL_TEXT_NA;
                _label.CssClass = SCORING_RULES;
                _label.Width = 35;

                _tableCell = new TableCell();
                _tableCell.HorizontalAlign = HorizontalAlign.Center;
                _tableCell.Controls.Add(_label);

                _simpleSumTextBox = new TextBox();

                _simpleSumTextBox.ID = "txtNAQuestion" + knowledgeBaseId + parentId;
                _simpleSumTextBox.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                _simpleSumTextBox.Style["text-align"] = "center";

                _simpleSumTextBox.Attributes.Add("onkeyup", "javascript:numberOnly('" + _simpleSumTextBox.ClientID + "')");
                _simpleSumTextBox.Style.Add("font-size", "10px");

                if (simpleSumList.Count > 0)
                    foreach (var answerWeightage in simpleSumList)
                    {
                        if (answerWeightage.AnswerTypeEnumId == 3)
                            _simpleSumTextBox.Text = answerWeightage.Weightage.ToString();                        
                    }
                else
                    _simpleSumTextBox.Text = DEFAULT_NUMBER_CONSTANT.ToString();

                _simpleSumTextBox.CssClass = SIMPLE_SUM_TEXBOX;
                _simpleSumTextBox.MaxLength = DEFAULT_NUMBER_CONSTANT;
                _tableCell.Controls.Add(_simpleSumTextBox);
                _tableCell.Width = 70;

                _tableRow.Controls.Add(_tableCell);

                _simpleSum.Controls.Add(_tableRow);

                _tableRow = new TableRow();
                _tableCell = new TableCell();

                _label = new Label();
                _label.Text = DEFAULT_LABEL_TEXT_NO;
                _label.CssClass = SCORING_RULES;
                _label.Width = 35;

                _tableCell = new TableCell();

                _tableCell.HorizontalAlign = HorizontalAlign.Center;
                _tableCell.Controls.Add(_label);

                _simpleSumTextBox = new TextBox();
                _simpleSumTextBox.ID = "txtQuestion" + knowledgeBaseId + parentId + simpleSumRowIndex;
                _simpleSumTextBox.Style["text-align"] = "center";
                _simpleSumTextBox.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                if (simpleSumList.Count > 0)
                    foreach (var answerWeightage in simpleSumList)
                    {
                        if (answerWeightage.AnswerTypeEnumId == 2)
                            _simpleSumTextBox.Text = answerWeightage.Weightage.ToString();
                    }
                else
                    _simpleSumTextBox.Text = "0";

                _simpleSumTextBox.Attributes.Add("onkeyup", "javascript:numberOnly('" + _simpleSumTextBox.ClientID + "')");
                _simpleSumTextBox.Style.Add("font-size", "10px");
                _simpleSumTextBox.CssClass = SIMPLE_SUM_TEXBOX;
                _simpleSumTextBox.MaxLength = DEFAULT_NUMBER_CONSTANT;
                _tableCell.Controls.Add(_simpleSumTextBox);                
                _tableCell.Width = 70;
                _tableRow.Controls.Add(_tableCell);

                _tableCell = new TableCell();
                RequiredFieldValidator rfv2 = new RequiredFieldValidator();
                rfv2.Text = REQUIRED_VALIDATOR_MESSAGE;
                rfv2.ErrorMessage = REQUIRED_VALIDATOR_MESSAGE;
                rfv2.ID = "rfv" + knowledgeBaseId + parentId + simpleSumRowIndex;
                rfv2.ControlToValidate = "txtQuestion" + knowledgeBaseId + parentId + simpleSumRowIndex;
                rfv2.ValidationGroup = "btnsaveScoringRules";
                rfv2.Display = ValidatorDisplay.Dynamic;
                _tableCell.Controls.Add(rfv2);
                _tableRow.Controls.Add(_tableCell);

                _simpleSum.Controls.Add(_tableRow);

                _tableRow = new TableRow();
                _tableCell = new TableCell();
                _tableCell.HorizontalAlign = HorizontalAlign.Center;
                _label = new Label();
                _label.CssClass = SCORING_RULES;
                _label.Style.Add("color", "red");
                //_label.Text = "Total available points";
                _tableCell.Controls.Add(_label);

                _tableRow.Controls.Add(_tableCell);
                _simpleSum.Controls.Add(_tableRow);

                // START SCORING RULES TABLE 
                newRowIndex = DEFAULT_NUMBER_CONSTANT;
                knowledgeBaseIdList.Add(parentId + knowledgeBaseId);
                rowIndex = parentId + knowledgeBaseId + newRowIndex;
                _tableRow = new TableRow();
                _tableRow.ID = rowIndex;
                _tableCell = new TableCell();

                _label = new Label();
                _label.Text = TXT_IF;
                _label.CssClass = SCORING_RULES;

                _tableCell = new TableCell();
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.HorizontalAlign = HorizontalAlign.Right;
                _tableCell.Controls.Add(_label);

                _tableRow.Controls.Add(_tableCell);

                _label = new Label();
                _label.Text = TXT_SUM;
                _label.CssClass = SCORING_RULES;

                _tableCell = new TableCell();
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.Controls.Add(_label);

                _tableRow.Controls.Add(_tableCell);

                _label = new Label();
                _label.Text = " *";
                _label.Style.Add("color", "red");
                _label.ID = "rfvlabel" + _tableRow.ID;
                _label.Style.Add("display", "none");
                _label.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                _ddConditional = new DropDownList();
                _ddConditional.ID = "ddConditional" + _tableRow.ID;
                _ddConditional.CssClass = SCORING_RULES_DROPDOWN;
                _ddConditional.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                _ddConditional.Attributes.Add("onchange", "javascript:ConditionChange('" + _ddConditional.ClientID + "','" + _tableRow.ID + "', '" + _label.ID + "');");
                _ddConditional.Items.Add("pick");
                _ddConditional.Items.Add(">=");
                _ddConditional.Items.Add("<=");          
                
                _tableCell = new TableCell();
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.Controls.Add(_ddConditional);

                _tableRow.Controls.Add(_tableCell);

                _scoringRulesTextBox = new TextBox();
                _scoringRulesTextBox.ID = "scoringRulesTextBox" + _tableRow.ID;
                _scoringRulesTextBox.Style["text-align"] = "center";
                _scoringRulesTextBox.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                _scoringRulesTextBox.MaxLength = 3;
                
                _scoringRulesTextBox.Attributes.Add("onkeyup", "javascript:isNumberKey('" + _scoringRulesTextBox.ClientID + "' ,'" + _label.ID + "')");
                _tableCell = new TableCell();
                _tableCell.Width = 35;
                _tableCell.VerticalAlign = VerticalAlign.Top;
                
                _tableCell.Controls.Add(_scoringRulesTextBox);
                _tableCell.Controls.Add(_label);

                _scoringRulesTextBox.CssClass = SCORING_RULES_TEXBOX;

                _tableRow.Controls.Add(_tableCell);

                _label = new Label();
                _label.Text = " *";
                _label.Style.Add("color", "red");
                _label.Style.Add("vertical-align", "top");
                _label.ID = "rfvListBox" + _tableRow.ID;                
                _label.Style.Add("display", "none");
                _label.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                _lbMustPresent = new ListBox();
                _lbMustPresent.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                _lbMustPresent.ID = "ddMustPresent" + _tableRow.ID;
                _lbMustPresent.Attributes.Add("onclick", "javascript:ListBoxChange('" + _label.ID + "');");

                _ddAndOr = new DropDownList();
                _ddAndOr.ID = "ddAndOr" + _tableRow.ID;
                _ddAndOr.Items.Add("pick");
                _ddAndOr.Items.Add("AND");
                _ddAndOr.Items.Add("NOT");
                _ddAndOr.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                _ddAndOr.Attributes.Add("onchange", "javascript:AndOrChange('" + _ddAndOr.ClientID + "','" + _tableRow.ID + "', '" + _label.ID + "', '" + _lbMustPresent.ID + "');");
                _ddAndOr.CssClass = SCORING_RULES_DROPDOWN;
                _tableCell = new TableCell();
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.Controls.Add(_ddAndOr);

                _tableRow.Controls.Add(_tableCell);                

                _lbMustPresent.CssClass = SCORING_RULES_LISTBOX;
                _lbMustPresent.SelectionMode = ListSelectionMode.Multiple;
                count = 1;
                foreach (KnowledgeBase ncqaDetails in _listOfNCQADetail)
                {
                    _lbMustPresent.Items.Add("#" + count);
                    count++;
                }
                _tableCell = new TableCell();
                _tableCell.Width = 60;
                _tableCell.VerticalAlign = VerticalAlign.Top;                
                _tableCell.Controls.Add(_lbMustPresent);
                _tableCell.Controls.Add(_label);

                _tableRow.Controls.Add(_tableCell);

                foreach (var score in scoringRulesList)
                {
                    if (score.Score == "100%")
                    {
                        if (score.MinYesFactor != null)
                        {
                            _ddConditional.SelectedValue = ">=";
                            _scoringRulesTextBox.Text = score.MinYesFactor.ToString();                            
                        }
                        else if (score.MaxYesFactor != null)
                        {
                            _ddConditional.SelectedValue = "<=";
                            _scoringRulesTextBox.Text = score.MaxYesFactor.ToString();                            
                        }

                        if (score.MustPresentFactorSequence != null)
                        {
                            _ddAndOr.SelectedValue = "AND";
                            string[] str = score.MustPresentFactorSequence.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var factorSequence in str)
                                _lbMustPresent.Items[Convert.ToInt32(factorSequence) - 1].Selected = true;
                        }
                        else if (score.AbsentFactorSequence != null)
                        {
                            _ddAndOr.SelectedValue = "NOT";
                            string[] str = score.AbsentFactorSequence.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string factorSequence in str)
                                _lbMustPresent.Items[Convert.ToInt32(factorSequence) - 1].Selected = true;
                        }
                    }
                }                

                _label = new Label();
                _label.Text = DEFAULT_LABEL_TEXT_HUNDRED_PERCENT;
                _label.ID = "label" + _tableRow.ID;
                _label.CssClass = SCORING_RULES;

                _tableCell = new TableCell();
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.HorizontalAlign = HorizontalAlign.Right;
                _tableCell.Controls.Add(_label);
                _tableRow.Controls.Add(_tableCell);

                _scoringRules.Controls.Add(_tableRow);
                newRowIndex++;
                rowIndex = parentId + knowledgeBaseId + newRowIndex;
                _tableRow = new TableRow();
                _tableRow.ID = rowIndex.ToString();
                _label = new Label();
                _label.Text = TXT_ELSE_IF;
                _label.CssClass = SCORING_RULES;

                _tableCell = new TableCell();
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.HorizontalAlign = HorizontalAlign.Right;
                _tableCell.Controls.Add(_label);

                _tableRow.Controls.Add(_tableCell);

                _label = new Label();
                _label.Text = TXT_SUM;
                _label.CssClass = SCORING_RULES;

                _tableCell = new TableCell();
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.HorizontalAlign = HorizontalAlign.Right;
                _tableCell.Controls.Add(_label);

                _tableRow.Controls.Add(_tableCell);

                _ddConditional = new DropDownList();
                _ddConditional.ID = "ddConditional" + _tableRow.ID;

                _label = new Label();
                _label.Text = " *";
                _label.Style.Add("color", "red");
                _label.ID = "rfvlabel" + _tableRow.ID;
                _label.Style.Add("display", "none");
                _label.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                _ddConditional.CssClass = SCORING_RULES_DROPDOWN;
                _ddConditional.Items.Add("pick");
                _ddConditional.Items.Add(">=");
                _ddConditional.Items.Add("<=");
                _ddConditional.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                _ddConditional.Attributes.Add("onchange", "javascript:ConditionChange('" + _ddConditional.ClientID + "','" + _tableRow.ID + "' , '" + _label.ID + "');");

                _tableCell = new TableCell();
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.Controls.Add(_ddConditional);

                _tableRow.Controls.Add(_tableCell);

                _scoringRulesTextBox = new TextBox();
                _scoringRulesTextBox.ID = "scoringRulesTextBox" + _tableRow.ID;
                _scoringRulesTextBox.Style["text-align"] = "center";
                _scoringRulesTextBox.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                _scoringRulesTextBox.MaxLength = 3;
                _scoringRulesTextBox.Attributes.Add("onkeyup", "javascript:isNumberKey('" + _scoringRulesTextBox.ClientID + "' , '" + _label.ID + "')");
                _tableCell = new TableCell();
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.Controls.Add(_scoringRulesTextBox);
                _tableCell.Controls.Add(_label);
                _scoringRulesTextBox.CssClass = SCORING_RULES_TEXBOX;

                _tableRow.Controls.Add(_tableCell);

                _label = new Label();
                _label.Text = " *";
                _label.Style.Add("color", "red");
                _label.Style.Add("vertical-align", "top");
                _label.ID = "rfvListBox" + _tableRow.ID;
                _label.Style.Add("display", "none");
                _label.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                _lbMustPresent = new ListBox();
                _lbMustPresent.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                _lbMustPresent.SelectionMode = ListSelectionMode.Multiple;
                _lbMustPresent.ID = "ddMustPresent" + _tableRow.ID;

                _ddAndOr = new DropDownList();
                _ddAndOr.ID = "ddAndOr" + _tableRow.ID;
                _ddAndOr.CssClass = SCORING_RULES_DROPDOWN;
                _ddAndOr.Items.Add("pick");
                _ddAndOr.Items.Add("AND");
                _ddAndOr.Items.Add("NOT");
                _ddAndOr.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                _ddAndOr.Attributes.Add("onchange", "javascript:AndOrChange('" + _ddAndOr.ClientID + "','" + _tableRow.ID + "', '" + _label.ID + "', '" + _lbMustPresent.ID + "');");

                _tableCell = new TableCell();
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.Controls.Add(_ddAndOr);

                _tableRow.Controls.Add(_tableCell);                
                
                _lbMustPresent.Attributes.Add("onclick", "javascript:ListBoxChange('" + _label.ID + "');");
                _lbMustPresent.CssClass = SCORING_RULES_LISTBOX;
                count = DEFAULT_NUMBER_CONSTANT;
                foreach (KnowledgeBase ncqaDetails in _listOfNCQADetail)
                {
                    _lbMustPresent.Items.Add("#" + count);
                    count++;
                }

                _tableCell = new TableCell();
                _tableCell.Width = 60;
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.Controls.Add(_lbMustPresent);
                _tableCell.Controls.Add(_label);

                _tableRow.Controls.Add(_tableCell);

                foreach (var score in scoringRulesList)
                {
                    if (score.Score.Length > 2)
                    {
                        if (score.Score.Substring(0, 3) == "75%")
                        {
                            if (score.MinYesFactor != null)
                            {
                                _ddConditional.SelectedValue = ">=";
                                _scoringRulesTextBox.Text = score.MinYesFactor.ToString();
                            }
                            else if (score.MaxYesFactor != null)
                            {
                                _ddConditional.SelectedValue = "<=";
                                _scoringRulesTextBox.Text = score.MaxYesFactor.ToString();
                            }
                            if (score.MustPresentFactorSequence != null)
                            {
                                _ddAndOr.SelectedValue = "AND";
                                string[] str = score.MustPresentFactorSequence.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (var factorSequence in str)
                                    _lbMustPresent.Items[Convert.ToInt32(factorSequence) - DEFAULT_NUMBER_CONSTANT].Selected = true;
                            }
                            else if (score.AbsentFactorSequence != null)
                            {
                                _ddAndOr.SelectedValue = "NOT";
                                string[] str = score.AbsentFactorSequence.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (string factorSequence in str)
                                    _lbMustPresent.Items[Convert.ToInt32(factorSequence) - 1].Selected = true;
                            }
                            break;
                        }
                    }
                }
                     
                _label = new Label();
                _label.Text = DEFAULT_LABEL_TEXT_SEVENTYFIVE_PERCENT;
                _label.ID = "label" + _tableRow.ID;
                _label.CssClass = SCORING_RULES;

                _tableCell = new TableCell();
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.HorizontalAlign = HorizontalAlign.Right;
                _tableCell.Controls.Add(_label);
                _tableRow.Controls.Add(_tableCell);

                _scoringRules.Controls.Add(_tableRow);
                newRowIndex++;
                rowIndex = parentId + knowledgeBaseId + newRowIndex;
                _tableRow = new TableRow();
                _tableRow.ID = rowIndex.ToString();

                _label = new Label();
                _label.Text = TXT_ELSE_IF;
                _label.CssClass = SCORING_RULES;

                _tableCell = new TableCell();
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.HorizontalAlign = HorizontalAlign.Right;
                _tableCell.Controls.Add(_label);

                _tableRow.Controls.Add(_tableCell);

                _label = new Label();
                _label.Text = TXT_SUM;
                _label.CssClass = SCORING_RULES;

                _tableCell = new TableCell();
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.HorizontalAlign = HorizontalAlign.Right;
                _tableCell.Controls.Add(_label);

                _tableRow.Controls.Add(_tableCell);

                _ddConditional = new DropDownList();
                _ddConditional.ID = "ddConditional" + _tableRow.ID;

                _label = new Label();
                _label.Text = " *";
                _label.Style.Add("color", "red");
                _label.ID = "rfvlabel" + _tableRow.ID;
                _label.Style.Add("display", "none");
                _label.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                _ddConditional.CssClass = SCORING_RULES_DROPDOWN;
                _ddConditional.Items.Add("pick");
                _ddConditional.Items.Add(">=");
                _ddConditional.Items.Add("<=");
                _ddConditional.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                _ddConditional.Attributes.Add("onchange", "javascript:ConditionChange('" + _ddConditional.ClientID + "','" + _tableRow.ID + "', '"+ _label.ID +"');");

                _tableCell = new TableCell();
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.Controls.Add(_ddConditional);

                _tableRow.Controls.Add(_tableCell);

                _scoringRulesTextBox = new TextBox();
                _scoringRulesTextBox.ID = "scoringRulesTextBox" + _tableRow.ID;
                _scoringRulesTextBox.Style["text-align"] = "center";
                _scoringRulesTextBox.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                _scoringRulesTextBox.MaxLength = 3;
                _scoringRulesTextBox.Attributes.Add("onkeyup", "javascript:isNumberKey('" + _scoringRulesTextBox.ClientID + "' , '"+ _label.ID +"')");
                _tableCell = new TableCell();
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.Controls.Add(_scoringRulesTextBox);
                _tableCell.Controls.Add(_label);

                _scoringRulesTextBox.CssClass = SCORING_RULES_TEXBOX;

                _tableRow.Controls.Add(_tableCell);

                _label = new Label();
                _label.Text = " *";
                _label.Style.Add("color", "red");
                _label.Style.Add("vertical-align", "top");
                _label.ID = "rfvListBox" + _tableRow.ID;
                _label.Style.Add("display", "none");
                _label.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                _lbMustPresent = new ListBox();
                _lbMustPresent.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                _lbMustPresent.ID = "ddMustPresent" + _tableRow.ID;

                _ddAndOr = new DropDownList();
                _ddAndOr.ID = "ddAndOr" + _tableRow.ID;
                _ddAndOr.CssClass = SCORING_RULES_DROPDOWN;
                _ddAndOr.Items.Add("pick");
                _ddAndOr.Items.Add("AND");
                _ddAndOr.Items.Add("NOT");
                _ddAndOr.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                _ddAndOr.Attributes.Add("onchange", "javascript:AndOrChange('" + _ddAndOr.ClientID + "','" + _tableRow.ID + "', '" + _label.ID + "', '" + _lbMustPresent.ID + "');");

                _tableCell = new TableCell();
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.Controls.Add(_ddAndOr);

                _tableRow.Controls.Add(_tableCell);                
                
                _lbMustPresent.Attributes.Add("onclick", "javascript:ListBoxChange('" + _label.ID + "');");
                _lbMustPresent.CssClass = SCORING_RULES_LISTBOX;
                _lbMustPresent.SelectionMode = ListSelectionMode.Multiple;
                count = DEFAULT_NUMBER_CONSTANT;
                foreach (KnowledgeBase ncqaDetails in _listOfNCQADetail)
                {
                    _lbMustPresent.Items.Add("#" + count);
                    count++;
                }

                _tableCell = new TableCell();
                _tableCell.Width = 60;
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.Controls.Add(_lbMustPresent);
                _tableCell.Controls.Add(_label);

                _tableRow.Controls.Add(_tableCell);

                foreach (var score in scoringRulesList)
                {
                    if (score.Score.Length > 2)
                    {
                        if (score.Score.Substring(0, 3) == "50%")
                        {
                            if (score.MinYesFactor != null)
                            {
                                _ddConditional.SelectedValue = ">=";
                                _scoringRulesTextBox.Text = score.MinYesFactor.ToString();
                            }
                            else if (score.MaxYesFactor != null)
                            {
                                _ddConditional.SelectedValue = "<=";
                                _scoringRulesTextBox.Text = score.MaxYesFactor.ToString();
                            }
                            if (score.MustPresentFactorSequence != null)
                            {
                                _ddAndOr.SelectedValue = "AND";
                                string[] str = score.MustPresentFactorSequence.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (var factorSequence in str)
                                    _lbMustPresent.Items[Convert.ToInt32(factorSequence) - DEFAULT_NUMBER_CONSTANT].Selected = true;
                            }
                            else if (score.AbsentFactorSequence != null)
                            {
                                _ddAndOr.SelectedValue = "NOT";
                                string[] str = score.AbsentFactorSequence.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (string factorSequence in str)
                                    _lbMustPresent.Items[Convert.ToInt32(factorSequence) - DEFAULT_NUMBER_CONSTANT].Selected = true;
                            }
                            break;
                        }

                    }
                }

                _label = new Label();
                _label.Text = DEFAULT_LABEL_TEXT_FIFTY_PERCENT;
                _label.ID = "label" + _tableRow.ID;
                _label.CssClass = SCORING_RULES;

                _tableCell = new TableCell();
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.HorizontalAlign = HorizontalAlign.Right;
                _tableCell.Controls.Add(_label);
                _tableRow.Controls.Add(_tableCell);

                _scoringRules.Controls.Add(_tableRow);
                newRowIndex++;
                rowIndex = parentId + knowledgeBaseId + newRowIndex;
                _tableRow = new TableRow();
                _tableRow.ID = rowIndex.ToString();
                _label = new Label();
                _label.Text = TXT_ELSE_IF;
                _label.CssClass = SCORING_RULES;

                _tableCell = new TableCell();
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.HorizontalAlign = HorizontalAlign.Right;
                _tableCell.Controls.Add(_label);

                _tableRow.Controls.Add(_tableCell);

                _label = new Label();
                _label.Text = TXT_SUM;
                _label.CssClass = SCORING_RULES;

                _tableCell = new TableCell();
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.HorizontalAlign = HorizontalAlign.Right;
                _tableCell.Controls.Add(_label);

                _tableRow.Controls.Add(_tableCell);

                _ddConditional = new DropDownList();
                _ddConditional.ID = "ddConditional" + _tableRow.ID;

                _label = new Label();
                _label.Text = " *";
                _label.Style.Add("color", "red");
                _label.ID = "rfvlabel" + _tableRow.ID;
                _label.Style.Add("display", "none");
                _label.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                _ddConditional.CssClass = SCORING_RULES_DROPDOWN;
                _ddConditional.Items.Add("pick");
                _ddConditional.Items.Add(">=");
                _ddConditional.Items.Add("<=");
                _ddConditional.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                _ddConditional.Attributes.Add("onchange", "javascript:ConditionChange('" + _ddConditional.ClientID + "','" + _tableRow.ID + "', '"+ _label.ID +"');");

                _tableCell = new TableCell();
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.Controls.Add(_ddConditional);

                _tableRow.Controls.Add(_tableCell);

                _tableCell = new TableCell();
                _tableCell.VerticalAlign = VerticalAlign.Top;
                
                _scoringRulesTextBox = new TextBox();
                _scoringRulesTextBox.ID = "scoringRulesTextBox" + _tableRow.ID;
                _scoringRulesTextBox.Style["text-align"] = "center";
                _scoringRulesTextBox.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                _scoringRulesTextBox.MaxLength = 3;
                _scoringRulesTextBox.Attributes.Add("onkeyup", "javascript:isNumberKey('" + _scoringRulesTextBox.ClientID + "', '" + _label.ID + "')");
                
                _tableCell.Controls.Add(_scoringRulesTextBox);
                _tableCell.Controls.Add(_label);

                _scoringRulesTextBox.CssClass = SCORING_RULES_TEXBOX;

                _tableRow.Controls.Add(_tableCell);

                _label = new Label();
                _label.Text = " *";
                _label.Style.Add("color", "red");
                _label.Style.Add("vertical-align", "top");
                _label.ID = "rfvListBox" + _tableRow.ID;
                _label.Style.Add("display", "none");
                _label.ClientIDMode = System.Web.UI.ClientIDMode.Static;

                _lbMustPresent = new ListBox();
                _lbMustPresent.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                _lbMustPresent.ID = "ddMustPresent" + _tableRow.ID;

                _ddAndOr = new DropDownList();
                _ddAndOr.ID = "ddAndOr" + _tableRow.ID;
                _ddAndOr.CssClass = SCORING_RULES_DROPDOWN;
                _ddAndOr.Items.Add("pick");
                _ddAndOr.Items.Add("AND");
                _ddAndOr.Items.Add("NOT");
                _ddAndOr.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                _ddAndOr.Attributes.Add("onchange", "javascript:AndOrChange('" + _ddAndOr.ClientID + "','" + _tableRow.ID + "', '" + _label.ID + "', '" + _lbMustPresent.ID + "');");

                _tableCell = new TableCell();
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.Controls.Add(_ddAndOr);

                _tableRow.Controls.Add(_tableCell);                
                
                _lbMustPresent.Attributes.Add("onclick", "javascript:ListBoxChange('" + _label.ID + "');");
                _lbMustPresent.SelectionMode = ListSelectionMode.Multiple;
                _lbMustPresent.CssClass = SCORING_RULES_LISTBOX;
                _lbMustPresent.Height = 42;
                count = DEFAULT_NUMBER_CONSTANT;
                foreach (KnowledgeBase ncqaDetails in _listOfNCQADetail)
                {
                    _lbMustPresent.Items.Add("#" + count);
                    count++;
                }

                _tableCell = new TableCell();
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.Width = 60;
                _tableCell.Controls.Add(_lbMustPresent);
                _tableCell.Controls.Add(_label);

                _tableRow.Controls.Add(_tableCell);

                 foreach (var score in scoringRulesList)
                    {
                        if (score.Score.Length > 2)
                        {
                            if (score.Score.Substring(0, 3) == "25%")
                            {
                                if (score.MinYesFactor != null)
                                {
                                    _ddConditional.SelectedValue = ">=";
                                    _scoringRulesTextBox.Text = score.MinYesFactor.ToString();
                                }
                                else if (score.MaxYesFactor != null)
                                {
                                    _ddConditional.SelectedValue = "<=";
                                    _scoringRulesTextBox.Text = score.MaxYesFactor.ToString();
                                }
                                if (score.MustPresentFactorSequence != null)
                                {
                                    _ddAndOr.SelectedValue = "AND";
                                    _lbMustPresent.Enabled = true;
                                    string[] str = score.MustPresentFactorSequence.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                    foreach (var factorSequence in str)
                                        _lbMustPresent.Items[Convert.ToInt32(factorSequence) - 1].Selected = true;
                                }
                                else if (score.AbsentFactorSequence != null)
                                {
                                    _lbMustPresent.Enabled = true;
                                    _ddAndOr.SelectedValue = "NOT";
                                    string[] str = score.AbsentFactorSequence.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                    foreach (string factorSequence in str)
                                        _lbMustPresent.Items[Convert.ToInt32(factorSequence) - 1].Selected = true;
                                }
                                break;
                            }
                        }
                    }                                

                _label = new Label();
                _label.Text = DEFAULT_LABEL_TEXT_TWENTYFIVE_PERCENT;
                _label.ID = "label" + _tableRow.ID;
                _label.CssClass = SCORING_RULES;

                _tableCell = new TableCell();
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.HorizontalAlign = HorizontalAlign.Right;
                _tableCell.Controls.Add(_label);

                _tableRow.Controls.Add(_tableCell);

                _scoringRules.Controls.Add(_tableRow);
                newRowIndex++;
                rowIndex = parentId + knowledgeBaseId + newRowIndex;
                _tableRow = new TableRow();
                _tableRow.ID = rowIndex.ToString();
                _label = new Label();
                _label.Text = TXT_ELSE;
                _label.CssClass = SCORING_RULES;

                _tableCell = new TableCell();
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.HorizontalAlign = HorizontalAlign.Right;
                _tableCell.Controls.Add(_label);

                _tableRow.Controls.Add(_tableCell);

                _tableCell = new TableCell();
                _tableRow.Controls.Add(_tableCell);
                _tableCell = new TableCell();
                _tableRow.Controls.Add(_tableCell);
                _tableCell = new TableCell();
                _tableRow.Controls.Add(_tableCell);
                _tableCell = new TableCell();
                _tableRow.Controls.Add(_tableCell);
                _tableCell = new TableCell();
                _tableRow.Controls.Add(_tableCell);

                _label = new Label();
                _label.Text = DEFAULT_LABEL_TEXT_ZERO_PERCENT;
                _label.ID = "label" + _tableRow.ID;
                _label.CssClass = SCORING_RULES;

                _tableCell = new TableCell();
                _tableCell.HorizontalAlign = HorizontalAlign.Right;
                _tableCell.Controls.Add(_label);

                _tableRow.Controls.Add(_tableCell);

                _scoringRules.Controls.Add(_tableRow);

            }
            count = DEFAULT_NUMBER_CONSTANT;
            foreach (KnowledgeBase ncqaDetails in _listOfNCQADetail)
            {
                string sequenceId = ncqaDetails.KnowledgeBaseId.ToString();
                string question = ncqaDetails.Name;

                _tableRow = new TableRow();               

                _tableCell = new TableCell();
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _label = new Label();
                _label.ID = "questionSequence" + ncqaDetails.KnowledgeBaseId + parentId;
                _label.CssClass = SCORING_RULES;
                _label.Width = 10;
                _label.Text = count + ". ";
                _tableCell.Controls.Add(_label);
                _tableRow.Controls.Add(_tableCell);

                _tableCell = new TableCell();
                _label = new Label();
                _label.ID = "lblTitle" + ncqaDetails.KnowledgeBaseId + parentId;
                _label.CssClass = SCORING_RULES;
                _label.Text = (question.Length > 48 ? question.Substring(0, 45) + "..." : question);
                _label.ToolTip = question;
                _label.Style.Add("overflow","hidden");
                _label.Style.Add("position","absolute");

                _label.ClientIDMode = ClientIDMode.Static;

                _tableCell = new TableCell();
                _tableCell.VerticalAlign = VerticalAlign.Top;
                _tableCell.Wrap = true;
                _tableCell.Controls.Add(_label);
                _tableRow.Controls.Add(_tableCell);

                _elementTable.Controls.Add(_tableRow);
                count++;
            }
        }
        catch (Exception exception)
        {
            throw exception;
        }
    }

    public string GetTemplateNameByTemplateId(int templateId)
    {
        try
        {
            _template = new ProjectTemplateBO();
            string templateName = _template.GetTemplateNameByTemplateId(templateId);
            return templateName;
        }
        catch (Exception exception)
        { throw exception; }
    }



    #endregion

}
