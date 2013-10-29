#region Modification History

//  ******************************************************************************
//  Title         : Enumeration
//  Created By    : Mirza Fahad Ali Baig
//  When Created  : 12/03/2011
//  Description   : 
//
//  ********************************** Modification History **********************
//  Who                     Date            Description
//  *******************************************************************************
//  Mirza Fahad ALi Baig    12-12-2011      QuestionnaireType
//  Mirza Fahad ALi Baig    12-12-2011      QuestionnaireSchema
//  Mirza Fahad ALi Baig    12-12-2011      QuestionChoiceType
//  Mirza Fahad ALi Baig    04-05-2012      enQuestionnaireType Subscription & UpfrontLicensePurchase
//  Mirza Fahad ALi Baig    04-05-2012      enEHRType
//  Mirza Fahad ALi Baig    04-05-2012      enPaymentMethod
//  Mirza Fahad ALi Baig    04-05-2012      enSessionKey
//  Mirza Fahad ALi Baig    04-25-2012      ValidationTypes

//  *******************************************************************************

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;

namespace BMTBLL.Enumeration
{
    public enum enSessionKey
    {
        [Description("Log-in user Id")]
        UserApplicationId,

        [Description("Medical Group Id of log-in user")]
        MedicalGroupId,

        [Description("Enterprise Id of log-in user")]
        EnterpriseId,

        [Description("Log-in user Role Type"), Category("Role")]
        UserType,

        [Description("Practice Id of log-in user/ selected practice")]
        PracticeId,

        [Description("To store the last loaded control in project section"), Category("Project")]
        currentControl,

        [Description("To store the NCQA Questionnaire"), Category("NCQA")]
        NCQAQuestionnaire,

        [Description("To store the Price Questionnaire"), Category("Price Calculator")]
        PriceQuestionnaire,

        [Description("To store the SRA Questionnaire"), Category("Security Risk Assessment")]
        SRAQuestionnaire,

    }

    public enum enSettingTree
    {
        Practice,
        Site,
        User,
        CreateEditProject,
        MyProject,
        CreateProject
    }

    public enum enUserRole
    {
        User = 1,
        SuperUser,
        Consultant,
        SuperAdmin

    }

    public enum enSiteCheck
    {
        Pass,
        HasParent,
        NoMultiple,
        Invalid
    }

    public enum enQuestionnaireType
    {
        [Description("Simple Questionnaire")]
        SimpleQuestionnaire = 1,

        [Description("Detailed Questionnaire")]
        DetailedQuestionnaire,

        [Description("Subscription (ASP)"), Category("Subscription")]
        Subscription,

        [Description("Upfront License Purchase"), Category("License")]
        License,

        [Description("SRA Questionnaire")]
        SRAQuestionnaire = 6
    }

    public enum enQuestionSubType
    {
        PCMH1 = 1,
        PCMH2,
        PCMH3,
        PCMH4,
        PCMH5,
        PCMH6
    }

    public enum enQuestionnaireElements
    {
        [Category("PCMH")]
        Question = 1,

        [Category("PCMH")]
        Answer,

        [Category("PCMH")]
        Element,

        [Category("PCMH")]
        Factor,

        [Category("PCMH")]
        Summary,

        [Category("PCMH")]
        SummaryItem,

        [Category("PCMH")]
        ReviewerNotes,

        [Category("PCMH")]
        EvaluationNotes,

        [Category("PCMH")]
        Calculation,

        [Category("PCMH")]
        Rule,

        [Category("PCMH")]
        Comment,

        [Category("PCMH")]
        PrivateNote,

        [Category("PCMH")]
        Info,

        [Category("PCMH")]
        Doc,

        [Category("PCMH")]
        Standard,

        [Category("PCMH")]
        SiteDocuments,

        [Category("PCMH")]
        UnAssociatedDoc,

        [Category("Price Calculator")]
        PriceCalculator,

        [Category("Price Calculator")]
        System,

        [Category("Price Calculator")]
        OngoingFees,

        [Category("Price Calculator")]
        OneTimeFees,

        [Category("Price Calculator")]
        Field,

        [Category("Price Calculator")]
        Entity,

        [Category("Price Calculator")]
        Quantity,

        [Category("Price Calculator")]
        Type,

        [Category("Price Calculator")]
        Amount,

        [Description("Payment Method"), Category("Price Calculator")]
        PaymentMethod
    }

    public enum enQuestionnaireAttr
    {
        [Description("Price Calculator")]
        name,

        [Category("Price Calculator")]
        sequence,

        [Category("Price Calculator")]
        title,

        [Category("Price Calculator")]
        required,

        [Category("Price Calculator")]
        description,

        [Category("Price Calculator")]
        add,

        [Category("Price Calculator")]
        addTitle,

        [Category("Price Calculator")]
        value,

        [Category("Price Calculator")]
        info
    }

    public enum enQuestionnaireControlType
    {
        Label,
        TextBox,
        DropDownList,
        HyperLink
    }

    public enum enQuestionChoiceType
    {
        SingleChoice = 1,
        MultiChoice
    }

    public enum enTreeType
    {
        ProjectSection,
        ToolSection,
        LibrarySection,
        ReportSection
    }

    public enum enDocType
    {

        Policies = 1,
        Reports,
        Screenshots,
        LogsOrTools,
        OtherDocs,
        UnAssociatedDoc
    }

    public enum enDbTables
    {
        ToolDocument,
        LibraryDocument,
        ProjectDocument
    }

    public enum enDocMode
    {
        New = 1,
        Existing
    }

    public enum enSortingType
    {
        Normal = 1,
        Ascending,
        Descending
    }

    public enum enDocsPageName
    {
        NCQASubmission = 1,
        NCQAUploadedDocs
    }

    public enum enDashBoardColumns
    {
        PracticeName,
        SiteName,
        Points,
        Documents,
        LastActivity,
        ContactName,
        FindingsFinalized,
        FollowupFinalized
    }

    public enum enSystemInfo
    {
        [Description("System version")]
        Version = 1,
        [Description("Email Body")]
        body = 2,
        [Description("Ad Banner Link")]
        adBannerLink = 3,
        [Description("Project Name")]
        projectName = 4,
        [Description("Company Name")]
        companyName = 5,
        [Description("Copyright")]
        copyright = 6,
        [Description("About US")]
        aboutUs = 7,
        [Description("Consulting User Mail")]
        consultingUserMail = 8,
        [Description("Invite Friend Mail")]
        inviteFriendMail = 9,
        [Description("Mail To")]
        mailTo = 10,
        [Description("Host")]
        host = 11,
        [Description("DisclaimerRequired")]
        disclaimerRequired = 12,
        [Description("DisclaimerText")]
        disclaimerText = 13,
        [Description("RequiredDocumentsEnabled")]
        requiredDocumentsEnabled = 14,
        [Description("MarkAsCompleteEnabled")]
        markAsCompleteEnabled = 15,
        [Description("SRADisclaimerRequired")]
        sraDisclaimerRequired = 16,
        [Description("SRADisclaimerText")]
        sraDisclaimerText = 17,
        [Description("NCQADashboardVisible")]
        ncqaDashboardVisible = 18,
        [Description("NCQASubmissionMail")]
        NCQASubmissionMail = 19,
        [Description("NCQASubmissionSuccessfulMail")]
        NCQASubmissionSuccessfulMail = 20
    }

    public enum enEHRProviders
    {
        [Description("Physician")]
        Physician = 1,

        [Description(" Mid-level")]
        MidLevel,

        [Description("Part Time")]
        PartTime,

        [Description("Discounted")]
        Discounted,

        [Description("Other")]
        Other
    }

    public enum enEHRInterfaces
    {
        [Description(" Reference Lab")]
        ReferenceLab = 1,

        [Description("Regional Lab")]
        RegionalLab,

        [Description("Other Lab")]
        OtherLab,

        [Description("LIS")]
        LIS,

        [Description("HIE – CDA")]
        HIECDA,

        [Description("HIE - Other")]
        HIEOther,

        [Description("Imaging")]
        Imaging,

        [Description("PACS/RIS")]
        PACSOrRIS,

        [Description("Hospital")]
        Hospital,

        [Description("Immunizations")]
        Immunizations,

        [Description("Cancer Registry")]
        CancerRegistry,

        [Description("Bio-Surveillance")]
        BioSurveillance,

        [Description("In Office Equipment")]
        InOfficeEquipment,

        [Description("Direct Exchange")]
        DirectExchange,

        [Description("Other")]
        Other

    }

    public enum enOnGoingPaymentMethod
    {

        [Description("Per Provider per month"), Category("OnGoing Fee")]
        PerProviderPerMonth = 1,

        [Description("Per Provider per year"), Category("OnGoing Fee")]
        PerProviderPerYear,

        [Description("Per Practice per month"), Category("OnGoing Fee")]
        PerPracticePerMonth,

        [Description("Per Practice per year"), Category("OnGoing Fee")]
        PerPracticePerYear,
    }

    public enum enOneTimePaymentMethod
    {
        [Description("Per Provider"), Category("One Time Fee")]
        PerProvider = 1,

        [Description("Per Practice"), Category("One Time Fee")]
        PerPractice
    }
    public enum enKBType
    {
        Header = 1,
        SubHeader = 2,
        Question = 3
    }

    public enum enProjectSectionMORe
    {
        ParentSectionId = 3,
        SectionId = 5,
    }
    public enum enAnsTypeId
    {
        AnsYesNoNA = 1,
        AnsYesNo = 2,
        AnsNone = 3
    }


    public enum ValidationTypes
    {
        PositiveNumber,
        Number,
        PositiveNonDecimal,
        NonDecimal,
        DatesDDMMYYYY,
        DatesDDMMYY,
        DatesDD_MMM_YYYY,
        Email,
        IPAddress,
        URL,
        CustomCharacter1
    }

    public enum enKnowledgeBaseType
    {

        Header = 1,
        SubHeader = 2,
        Question = 3
    }

    public enum enTemplateDocumentType
    {

        PoliciesOrProcess,
        ReportsOrLogs,
        ScreenshotsOrExamples,
        RRWB,
        Extra
    }

    public enum enAnswerType
    {
        YesNoNA = 1,
        YesNo = 2,
        None = 3
    }

    public enum enSubmissionStatus
    {
        Pending = 1,
        Fulfilled = 2,
        InProgress = 3
    }

    public enum enSubmissionType
    {
        Old = 1,
        New = 2
    }

    public enum enSubmitAction
    {
        NCQA = 1,
        None = 2
    }
    public enum enAnsTypeEnum
    {
        TypeYesNoNA_Yes = 1,
        TypeYesNoNA_No = 2,
        TypeYesNoNA_NA = 3,
        TypeYesNo_Yes = 4,
        TypeYesNo_No = 5,
    }

    public enum enTemplateAccessType
    {
        Public = 1,
        Enterprise = 2,
        Practice = 3,

    }
    public enum enProvider
    {
        PCP = 1,
        Specialist = 2,
        NonProvider = 3,

    }

    public enum enErrorMessage
    {
        [Description("Enterprise Name is incorrect.")]
        EnterpriseNameIsIncorrect,

        [Description("Email is not available.")]
        EmailIsNotAvailable,

        [Description("User Account couldn't be created.")]
        UserAccountCouldNotBeCreated,

        [Description("The Server has encounterd an error, Please try again later.")]
        TheServerHasEncounterdAnErrorPleaseTryAgainLater,

        [Description("Username/password is incorrect.")]
        UsernamePasswordIsIncorrect,
    }
    public enum enFormType
    {
        ExpressAssessment=1,
        EHRSelection=3,
        SecurityRiskAssessment=6
    }

    public enum enSectionType
    {
        Template = 1,
        Form = 2,
        Folder = 3,
        PlaceHolder=4,
        ProjectFolder=5,
        ITConsultant=6
    }
    public enum enAccessLevelId
    {
        Public = 1,
        Enterprise = 2,
        Practice = 3,
        MedicalGroup = 4,
    }
    public enum enToolLevelId
    {
        Practice = 1,
        PracticeSite = 2,
    }
    public enum enProjectId
    {
        PCMHProject = 1,
    }
}