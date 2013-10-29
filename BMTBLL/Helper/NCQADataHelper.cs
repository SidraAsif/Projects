using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Xml.Linq;
using System.Xml;

using BMTBLL;
using BMTBLL.Enumeration;

namespace BMTBLL.Helper
{
    public static class NCQADataHelper
    {
        #region CONSTANTS
        private const string DEFAULT_QUESTIONNAIRE_TYPE = "NCQA";

        private static string[] DEFAULT_LETTERS = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L"
    ,"M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"};

        #endregion

        #region VARIABLES

        private static decimal TOTAL_NCQA_EARNER_POINTS;

        #endregion

        #region FUNCTIONS
        public static decimal GetPoints(List<NCQASummaryDetail> ncqadocument)
        {

            decimal totalPoints = 0.0M;
            decimal scoredPoints = 0.0M;
            //IEnumerable<XElement> standards = (from standardRecord in document.Descendants("Standard") select standardRecord);
            //foreach (XElement standardElement in standards)
            //{
            //    totalPoints = totalPoints + Convert.ToDecimal(standardElement.Attribute("maxPoints").Value);
            //}
            foreach (NCQASummaryDetail element in ncqadocument)
            {
                totalPoints = totalPoints + Convert.ToDecimal(element.MaxPoints);

                decimal elementMaxPoints = 0.0M;
                elementMaxPoints = Convert.ToDecimal(element.MaxPoints);
                scoredPoints = scoredPoints + ((Convert.ToDecimal((element.EarnedPoints / 100) * elementMaxPoints)));
            }

            if (scoredPoints != 0 && totalPoints != 0)
            {
                return ((scoredPoints * 100) / totalPoints);
            }
            else
                return 0;
        }

        public static decimal GetDocuments(List<NCQAFullDetail> ncqaReqDocument, int ncqaUploadedDocument)
        {
            decimal totalRequiredDoc = 0.0M;

            decimal totalUploadedDocs = ncqaUploadedDocument;


            foreach (NCQAFullDetail factor in ncqaReqDocument)
            {
                totalRequiredDoc = totalRequiredDoc + Convert.ToInt32(factor.Policies) + Convert.ToInt32(factor.Report) + Convert.ToInt32(factor.RRWB)
                    + Convert.ToInt32(factor.ScreenShot) + Convert.ToInt32(factor.Others);
            }

            if (totalRequiredDoc != 0 && totalUploadedDocs != 0)
            {
                if (totalUploadedDocs <= totalRequiredDoc)
                {
                    return ((totalUploadedDocs * 100) / totalRequiredDoc);
                }
                else
                {
                    return 100.0M;
                }
            }
            else
                return 0M;
        }

        public static List<NCQADetails> GetDocsByElementId(string pcmhSequenceId, string elementSequenceId, int practiceId, int siteId, int projectId)
        {
            try
            {
                List<NCQADetails> _listOfNCQADoc = new List<NCQADetails>();

                #region VARIABLES
                string factorSequence = string.Empty;
                string location = string.Empty;
                string type = string.Empty;
                string title = string.Empty;
                DateTime? lastUploadedDate = null;
                string docLinkedTo = string.Empty;

                string docName = string.Empty;
                string referencePage = string.Empty;
                string relevancyLevel = string.Empty;
                string docType = string.Empty;

                string factorTitle = string.Empty;

                #endregion

                //get questionniare from session
                XDocument questionnaire = XDocument.Parse(HttpContext.Current.Session["NCQAQuestionnaire"].ToString());

                //Evaluation comment of current element
                string evaluationComments = (from commentRecord in questionnaire.Descendants("Element").Descendants("EvaluationNotes")
                                             where (string)commentRecord.Parent.Attribute("sequence") == elementSequenceId
                                                && (string)commentRecord.Parent.Parent.Attribute("sequence") == pcmhSequenceId
                                             select commentRecord.Value.Trim()).FirstOrDefault();

                #region EVALUATION_COMMENT
                type = "Evaluation Comment";
                title = "<p><a href='#' class='tt'>Evaluation Comment for PCMH " + pcmhSequenceId + " - " + "Element " + DEFAULT_LETTERS[Convert.ToInt32(elementSequenceId) - 1];
                title += "<span class='tooltip'><span class='top'></span><span class='middle'>";
                title += evaluationComments;
                title += "</span><span class='bottom'></span></span></a></p>";

                if (evaluationComments != string.Empty && evaluationComments != null)
                    _listOfNCQADoc.Add(new NCQADetails(pcmhSequenceId, elementSequenceId, string.Empty, string.Empty, type, title, null, string.Empty,
                        practiceId, siteId, projectId, docName, referencePage, relevancyLevel, docType, factorTitle));

                #endregion

                IEnumerable<XElement> docs = (from docRecord in questionnaire.Descendants("Element").Descendants("DocFile")
                                              where (string)docRecord.Parent.Parent.Parent.Attribute("sequence") == elementSequenceId
                                              && (string)docRecord.Parent.Parent.Parent.Parent.Attribute("sequence") == pcmhSequenceId
                                              select docRecord);
                #region ADD_DOCS_WITH_DETAILS
                foreach (XElement document in docs)
                {
                    factorSequence = document.Parent.Parent.Attribute("sequence").Value;
                    location = document.Attribute("location").Value;
                    string[] splitPath = location.Split('/');
                    int lastIndex = splitPath.Count() - 1;
                    string fileName = splitPath[lastIndex];
                    string timeString = fileName.Substring(0, 14);
                    lastUploadedDate = DateTime.ParseExact(timeString, "yyyyMMddHHmmss", null);

                    title = document.Attribute("name").Value.Replace("Apostrophe", "'").Replace("circumflex", "^").Replace("plussign", "+").Replace("hashsign", "#").Replace("squarebraketopen", "[").Replace("squarebraketclose", "]").Replace("curlybraketopen", "{").Replace("curlybraketclose", "{").Replace("dotsign", ".");
                    docLinkedTo = string.Empty;

                    docName = document.Attribute("name").Value.Trim();
                    referencePage = document.Attribute("referencePages").Value.Trim();
                    relevancyLevel = document.Attribute("relevancyLevel").Value.Trim();
                    type = docType = document.Parent.Name.ToString();
                    factorTitle = document.Parent.Parent.Attribute("title").Value;

                    //TODO: To found all relations of current Doc
                    IEnumerable<XElement> docRelation = (from docRecord in questionnaire.Descendants("DocFile")
                                                         where (string)docRecord.Attribute("location") == location
                                                         select docRecord);
                    foreach (XElement relation in docRelation)
                    {
                        docLinkedTo += relation.Parent.Parent.Parent.Parent.Attribute("sequence").Value +
                           DEFAULT_LETTERS[Convert.ToInt32(relation.Parent.Parent.Parent.Attribute("sequence").Value) - 1] +
                            relation.Parent.Parent.Attribute("sequence").Value + ",";
                    }

                    if (docLinkedTo.Length > 1)
                        docLinkedTo = docLinkedTo.Remove(docLinkedTo.Length - 1, 1);

                    title = "<a href='" + location + "' target='_blank'>" + title + "</a>";

                    location = location.Substring(location.LastIndexOf('/') + 1);
                    location = HttpUtility.JavaScriptStringEncode(location); // to handle single quotes while passing it as parameter

                    //Get Original Document Type
                    if (docType == enDocType.Policies.ToString() || docType == enDocType.Reports.ToString() || docType == enDocType.Screenshots.ToString() ||
                        docType == enDocType.LogsOrTools.ToString() || docType == enDocType.OtherDocs.ToString())
                    {
                        DocumentTypeMappingBO documentTypeMappingBO = new DocumentTypeMappingBO();
                        type = docType = documentTypeMappingBO.GetDocumentType(docType);
                        type = docType = docType.Replace("Or", "/");
                    }

                    _listOfNCQADoc.Add(new NCQADetails(pcmhSequenceId, elementSequenceId, factorSequence, location, type, title, lastUploadedDate, docLinkedTo,
                        practiceId, siteId, projectId, docName, referencePage, relevancyLevel, docType, factorTitle));
                }

                #endregion

                //All Facotor Comment in current element                
                IEnumerable<XElement> factorComments = (from factorCommment in questionnaire.Descendants("Element").Descendants("Comment")
                                                        where (string)factorCommment.Parent.Parent.Attribute("sequence") == elementSequenceId
                                                           && (string)factorCommment.Parent.Parent.Parent.Attribute("sequence") == pcmhSequenceId
                                                           && (string)factorCommment.Parent.Attribute("comment") == "Yes"
                                                           && (string)factorCommment.Value.Trim() != string.Empty
                                                        select factorCommment);

                #region FACTOR_COMMENT
                foreach (XElement comment in factorComments)
                {
                    factorSequence = comment.Parent.Attribute("sequence").Value;
                    type = "Factor Comment";
                    title = "<p><a href='#' class='tt'>" + comment.Parent.Attribute("note").Value;
                    title += "<span class='tooltip'><span class='top'></span><span class='middle'>";
                    title += comment.Value;
                    title += "</span><span class='bottom'></span></span></a></p>";

                    _listOfNCQADoc.Add(new NCQADetails(pcmhSequenceId, elementSequenceId, factorSequence, string.Empty, type, title, null, string.Empty,
                        practiceId, siteId, projectId, docName, referencePage, relevancyLevel, docType, factorTitle));
                }

                #endregion

                //All users comments in current element against each factor
                IEnumerable<XElement> userCommments = (from userNotes in questionnaire.Descendants("Element").Descendants("PrivateNote")
                                                       where (string)userNotes.Parent.Parent.Attribute("sequence") == elementSequenceId
                                                          && (string)userNotes.Parent.Parent.Parent.Attribute("sequence") == pcmhSequenceId
                                                       select userNotes);

                #region USERS_COMMENTS
                foreach (XElement privateNoteElement in userCommments)
                {
                    int count = 0;
                    factorSequence = privateNoteElement.Parent.Attribute("sequence").Value;
                    type = "Note for PCMH " + pcmhSequenceId + " - " + "Element " + DEFAULT_LETTERS[Convert.ToInt32(elementSequenceId) - 1] + " - " + "Factor " + factorSequence;

                    title = "<p><a href='#' class='tt'>" + "Note";
                    title += "<span class='tooltip'><span class='top'></span><span class='middle'>";

                    foreach (XElement comment in privateNoteElement.Elements())
                    {
                        count = count + 1;
                        title += count.ToString() + ". " + comment.Value;
                        title += "<span class='notice'>(" + comment.Attribute("user").Value + " on " + comment.Attribute("date").Value + ")</span><br />";
                    }

                    title += "</span><span class='bottom'></span></span></a></p>";

                    _listOfNCQADoc.Add(new NCQADetails(pcmhSequenceId, elementSequenceId, factorSequence, string.Empty, type, title, null, string.Empty,
                        practiceId, siteId, projectId, docName, referencePage, relevancyLevel, docType, factorTitle));
                }

                #endregion

                //Reviewer Notes 
                string reviewerNotes = (from commentRecord in questionnaire.Descendants("Element").Descendants("ReviewerNotes")
                                        where (string)commentRecord.Parent.Attribute("sequence") == elementSequenceId
                                           && (string)commentRecord.Parent.Parent.Attribute("sequence") == pcmhSequenceId
                                        select commentRecord.Value.Trim()).FirstOrDefault();

                #region REVIEWER_NOTES
                type = "Reviewer Notes";
                title = "<p><a href='#' class='tt'>Reviewer Notes for PCMH " + pcmhSequenceId + " - " + "Element " + DEFAULT_LETTERS[Convert.ToInt32(elementSequenceId) - 1];
                title += "<span class='tooltip'><span class='top'></span><span class='middle'>";
                title += reviewerNotes;
                title += "</span><span class='bottom'></span></span></a></p>";

                if (reviewerNotes != string.Empty && reviewerNotes != null)
                    _listOfNCQADoc.Add(new NCQADetails(pcmhSequenceId, elementSequenceId, string.Empty, string.Empty, type, title, null, string.Empty,
                        practiceId, siteId, projectId, docName, referencePage, relevancyLevel, docType, factorTitle));

                #endregion

                return _listOfNCQADoc.ToList();

            }
            catch (Exception exception)
            {
                throw exception;
            }

        }


        public static List<NCQADetails> GetDocsByElementId(string pcmhSequenceId, string elementSequenceId, int practiceId, int siteId, int projectUsageId, int templateId)
        {
            try
            {
                List<NCQADetails> _listOfNCQADoc = new List<NCQADetails>();


                MOReBO MORe = new MOReBO();


                #region VARIABLES
                string factorSequence = string.Empty;
                string location = string.Empty;
                string type = string.Empty;
                string title = string.Empty;
                DateTime? lastUploadedDate = null;
                string docLinkedTo = string.Empty;
                string docName = string.Empty;
                string referencePage = string.Empty;
                string relevancyLevel = string.Empty;
                string docType = string.Empty;

                string factorTitle = string.Empty;

                #endregion

                List<KnowledgeBase> headers = MORe.GetKnowledgeBaseHeadersByTemplateId(templateId);

                List<KnowledgeBase> SubHeaders = MORe.GetKnowledgeBaseSubHeadersByTemplateId(templateId, headers[Convert.ToInt32(pcmhSequenceId) - 1].KnowledgeBaseId);

                string evaluationComments = MORe.GetEvaluationNotesBySubHeaderId(SubHeaders[Convert.ToInt32(elementSequenceId) - 1].KnowledgeBaseId, templateId, projectUsageId, siteId);

                #region EVALUATION_COMMENT
                type = "Evaluation Comment";
                title = "<p><a href='#' class='tt'>Evaluation Comment for PCMH " + pcmhSequenceId + " - " + "Element " + DEFAULT_LETTERS[Convert.ToInt32(elementSequenceId) - 1];
                title += "<span class='tooltip'><span class='top'></span><span class='middle'>";
                title += evaluationComments;
                title += "</span><span class='bottom'></span></span></a></p>";

                if (evaluationComments != string.Empty && evaluationComments != null)
                    _listOfNCQADoc.Add(new NCQADetails(pcmhSequenceId, elementSequenceId, string.Empty, string.Empty, type, title, null, string.Empty,
                        practiceId, siteId, projectUsageId, docName, referencePage, relevancyLevel, docType, factorTitle));

                #endregion


                List<TemplateDocument> docs = MORe.GetDocumentBySubHeaderId(SubHeaders[Convert.ToInt32(elementSequenceId) - 1].KnowledgeBaseId, templateId, projectUsageId, siteId);

                foreach (TemplateDocument doc in docs)
                {
                    location = doc.Path.Replace("Apostrophe", "'").Replace("circumflex", "^").Replace("plussign", "+").Replace("hashsign", "#").Replace("squarebraketopen", "[").Replace("squarebraketclose", "]").Replace("curlybraketopen", "{").Replace("curlybraketclose", "{").Replace("dotsign", ".");
                    string[] splitPath = location.Split('/');
                    int lastIndex = splitPath.Count() - 1;
                    string fileName = splitPath[lastIndex];
                    string timeString = fileName.Substring(0, 14);
                    lastUploadedDate = DateTime.ParseExact(timeString, "yyyyMMddHHmmss", null);

                    if (doc.Name == "" || doc.Name == null)
                    {
                        int startIndex = doc.Path.LastIndexOf('/') + 1;
                        int length = doc.Path.LastIndexOf('.') - startIndex;
                        docName = doc.Path.Substring(startIndex, length);
                    }
                    else
                        docName = doc.Name;

                    docName = docName.Replace("Apostrophe", "'").Replace("circumflex", "^").Replace("plussign", "+").Replace("hashsign", "#").Replace("squarebraketopen", "[").Replace("squarebraketclose", "]").Replace("curlybraketopen", "{").Replace("curlybraketclose", "{").Replace("dotsign", ".");

                    title = "<a href='" + location + "' target='_blank'>" + docName + "</a>";
                    docType = doc.DocumentType;

                    string temp = MORe.GetQuestionNameAndSequence(doc.DocumentId, templateId, SubHeaders[Convert.ToInt32(elementSequenceId) - 1].KnowledgeBaseId, projectUsageId, siteId);

                    string[] sequenceTitles = temp.Split(new char[] { '~' }, StringSplitOptions.RemoveEmptyEntries);

                    factorSequence = Convert.ToString(Convert.ToInt32(sequenceTitles[0]) + 1);
                    factorTitle = sequenceTitles[1];

                    docLinkedTo = MORe.GetDocumentLinks(doc.Path, templateId, pcmhSequenceId, elementSequenceId, factorSequence, projectUsageId, siteId);//string.Empty;


                    if (docType == enTemplateDocumentType.PoliciesOrProcess.ToString() || docType == enTemplateDocumentType.ReportsOrLogs.ToString() || docType == enTemplateDocumentType.ScreenshotsOrExamples.ToString() ||
                            docType == enTemplateDocumentType.RRWB.ToString() || docType == enTemplateDocumentType.Extra.ToString())
                    {
                        //DocumentTypeMappingBO documentTypeMappingBO = new DocumentTypeMappingBO();
                        //type = docType = documentTypeMappingBO.GetDocumentType(docType);
                        type = docType = docType.Replace("Or", "/");
                    }
                    else if (docType == enDocType.Policies.ToString() || docType == enDocType.Reports.ToString() || docType == enDocType.Screenshots.ToString() ||
                            docType == enDocType.LogsOrTools.ToString() || docType == enDocType.OtherDocs.ToString())
                    {
                        DocumentTypeMappingBO documentTypeMappingBO = new DocumentTypeMappingBO();
                        type = docType = documentTypeMappingBO.GetDocumentType(docType);
                        type = docType = docType.Replace("Or", "/");

                    }

                    relevancyLevel = doc.RelevencyLevel;
                    referencePage = doc.ReferencePages;

                    _listOfNCQADoc.Add(new NCQADetails(pcmhSequenceId, elementSequenceId, factorSequence, location, type, title, lastUploadedDate, docLinkedTo,
                            practiceId, siteId, projectUsageId, docName, referencePage, relevancyLevel, docType, factorTitle));
                }

                List<KnowledgeBase> Question = MORe.GetQuestionCriticalComments(Convert.ToInt32(pcmhSequenceId), Convert.ToInt32(elementSequenceId), templateId);

                int index = 1;
                foreach (KnowledgeBase kb in Question)
                {
                    factorSequence = index.ToString();

                    FilledAnswer fAnswer = MORe.GetFilledAnswersByKnowledgeBase(kb.KnowledgeBaseId, templateId, projectUsageId, siteId);

                    KnowledgeBaseTemplate kbtemplate = MORe.GetKnowledgeBaseTemplate(kb.KnowledgeBaseId, templateId);

                    if (fAnswer.DataBoxComments != null)
                    {
                        type = "Factor Comment";
                        title = "<p><a href='#' class='tt'>" + kbtemplate.DataBoxHeader;
                        title += "<span class='tooltip'><span class='top'></span><span class='middle'>";
                        title += fAnswer.DataBoxComments;
                        title += "</span><span class='bottom'></span></span></a></p>";

                        if (fAnswer.DataBoxComments != string.Empty)
                        {
                            _listOfNCQADoc.Add(new NCQADetails(pcmhSequenceId, elementSequenceId, factorSequence, string.Empty, type, title, null, string.Empty,
                                practiceId, siteId, projectUsageId, docName, referencePage, relevancyLevel, docType, factorTitle));
                        }
                    }
                    index++;
                }

                List<KnowledgeBase> _question = MORe.GetKnowledgeBaseQuestionsBySubHeader(SubHeaders[Convert.ToInt32(elementSequenceId) - 1].KnowledgeBaseId, templateId);

                int count = 1;
                foreach (KnowledgeBase question in _question)
                {
                    string privateNotes = MORe.GetPrivateNotes(templateId, headers[Convert.ToInt32(pcmhSequenceId) - 1].KnowledgeBaseId,
                    Convert.ToInt32(elementSequenceId) - 1, count - 1, projectUsageId, siteId);

                    factorSequence = count.ToString();
                    type = "Note for PCMH " + pcmhSequenceId + " - " + "Element " + DEFAULT_LETTERS[Convert.ToInt32(elementSequenceId) - 1] + " - " + "Factor " + factorSequence;

                    title = "<p><a href='#' class='tt'>" + "Note";
                    title += "<span class='tooltip'><span class='top'></span><span class='middle'>";

                    if (privateNotes != null)
                    {

                        string[] History = privateNotes.Split(new char[] { '~' }, StringSplitOptions.RemoveEmptyEntries);

                        int notesCount = 1;
                        foreach (string comment in History)
                        {
                            string[] PrivateNotes = comment.Split(new char[] { '@' }, StringSplitOptions.None);

                            title += notesCount.ToString() + ". " + PrivateNotes[2];
                            title += "<span class='notice'>(" + PrivateNotes[1] + " on " + PrivateNotes[0] + ")</span><br />";
                            notesCount++;
                        }

                        title += "</span><span class='bottom'></span></span></a></p>";

                        if (privateNotes != string.Empty)
                            _listOfNCQADoc.Add(new NCQADetails(pcmhSequenceId, elementSequenceId, factorSequence, string.Empty, type, title, null, string.Empty,
                                practiceId, siteId, projectUsageId, docName, referencePage, relevancyLevel, docType, factorTitle));

                    }

                    count++;

                }

                string reviewNotes = MORe.GetReviewNotesBySubHeaderId(SubHeaders[Convert.ToInt32(elementSequenceId) - 1].KnowledgeBaseId, templateId, projectUsageId, siteId);

                #region REVIEWER_NOTES
                type = "Reviewer Notes";
                title = "<p><a href='#' class='tt'>Reviewer Notes for PCMH " + pcmhSequenceId + " - " + "Element " + DEFAULT_LETTERS[Convert.ToInt32(elementSequenceId) - 1];
                title += "<span class='tooltip'><span class='top'></span><span class='middle'>";
                title += reviewNotes;
                title += "</span><span class='bottom'></span></span></a></p>";

                if (reviewNotes != string.Empty && reviewNotes != null)
                    _listOfNCQADoc.Add(new NCQADetails(pcmhSequenceId, elementSequenceId, string.Empty, string.Empty, type, title, null, string.Empty,
                        practiceId, siteId, projectUsageId, docName, referencePage, relevancyLevel, docType, factorTitle));

                #endregion


                return _listOfNCQADoc.ToList();

            }
            catch (Exception exception)
            {
                throw exception;
            }

        }


        public static List<NCQADetails> GetDocsByElementId(string pcmhSequenceId, string elementSequenceId, int practiceId, int siteId, int projectId, string ncqaQuestionnaire)
        {
            try
            {
                List<NCQADetails> _listOfNCQADoc = new List<NCQADetails>();

                #region VARIABLES
                string factorSequence = string.Empty;
                string location = string.Empty;
                string type = string.Empty;
                string title = string.Empty;
                string docLinkedTo = string.Empty;

                string docName = string.Empty;
                string referencePage = string.Empty;
                string relevancyLevel = string.Empty;
                string docType = string.Empty;

                string factorTitle = string.Empty;

                #endregion

                XDocument questionnaire = XDocument.Parse(ncqaQuestionnaire);

                // Evaluation comment of current element
                string evaluationComments = (from commentRecord in questionnaire.Descendants("Element").Descendants("EvaluationNotes")
                                             where (string)commentRecord.Parent.Attribute("sequence") == elementSequenceId
                                                && (string)commentRecord.Parent.Parent.Attribute("sequence") == pcmhSequenceId
                                             select commentRecord.Value.Trim()).FirstOrDefault();

                #region EVALUATION_COMMENT
                type = "Evaluation Comment";
                title = "<p><a href='#' class='tt'>Evaluation Comment for PCMH " + pcmhSequenceId + " - " + "Element " + DEFAULT_LETTERS[Convert.ToInt32(elementSequenceId) - 1];
                title += "<span class='tooltip'><span class='top'></span><span class='middle'>";
                title += evaluationComments;
                title += "</span><span class='bottom'></span></span></a></p>";

                if (evaluationComments != string.Empty && evaluationComments != null)
                    _listOfNCQADoc.Add(new NCQADetails(pcmhSequenceId, elementSequenceId, string.Empty, string.Empty, type, title, string.Empty,
                        practiceId, siteId, projectId, docName, referencePage, relevancyLevel, docType, factorTitle));

                #endregion

                IEnumerable<XElement> docs = (from docRecord in questionnaire.Descendants("Element").Descendants("DocFile")
                                              where (string)docRecord.Parent.Parent.Parent.Attribute("sequence") == elementSequenceId
                                              && (string)docRecord.Parent.Parent.Parent.Parent.Attribute("sequence") == pcmhSequenceId
                                              select docRecord);
                #region ADD_DOCS_WITH_DETAILS
                foreach (XElement document in docs)
                {
                    factorSequence = document.Parent.Parent.Attribute("sequence").Value;
                    location = document.Attribute("location").Value;
                    title = document.Attribute("name").Value;
                    docLinkedTo = string.Empty;

                    docName = document.Attribute("name").Value.Trim();
                    referencePage = document.Attribute("referencePages").Value.Trim();
                    relevancyLevel = document.Attribute("relevancyLevel").Value.Trim();
                    type = docType = document.Parent.Name.ToString();
                    factorTitle = document.Parent.Parent.Attribute("title").Value;

                    // TODO: To found all relations of current Doc
                    IEnumerable<XElement> docRelation = (from docRecord in questionnaire.Descendants("DocFile")
                                                         where (string)docRecord.Attribute("location") == location
                                                         select docRecord);
                    foreach (XElement relation in docRelation)
                    {
                        docLinkedTo += relation.Parent.Parent.Parent.Parent.Attribute("sequence").Value +
                           DEFAULT_LETTERS[Convert.ToInt32(relation.Parent.Parent.Parent.Attribute("sequence").Value) - 1] +
                            relation.Parent.Parent.Attribute("sequence").Value + ",";
                    }

                    if (docLinkedTo.Length > 1)
                        docLinkedTo = docLinkedTo.Remove(docLinkedTo.Length - 1, 1);

                    title = "<a href='" + location + "' target='_blank'>" + title + "</a>";

                    location = location.Substring(location.LastIndexOf('/') + 1);
                    location = HttpUtility.JavaScriptStringEncode(location); // to handle single quotes while passing it as parameter

                    //Get Original Document Type
                    if (docType == enDocType.Policies.ToString() || docType == enDocType.Reports.ToString() || docType == enDocType.Screenshots.ToString() ||
                        docType == enDocType.LogsOrTools.ToString() || docType == enDocType.OtherDocs.ToString())
                    {
                        DocumentTypeMappingBO documentTypeMappingBO = new DocumentTypeMappingBO();
                        type = docType = documentTypeMappingBO.GetDocumentType(docType);
                        type = docType = docType.Replace("Or", "/");
                    }

                    _listOfNCQADoc.Add(new NCQADetails(pcmhSequenceId, elementSequenceId, factorSequence, location, type, title, docLinkedTo,
                        practiceId, siteId, projectId, docName, referencePage, relevancyLevel, docType, factorTitle));
                }

                #endregion

                // All Facotor Comment in current element                
                IEnumerable<XElement> factorComments = (from factorCommment in questionnaire.Descendants("Element").Descendants("Comment")
                                                        where (string)factorCommment.Parent.Parent.Attribute("sequence") == elementSequenceId
                                                           && (string)factorCommment.Parent.Parent.Parent.Attribute("sequence") == pcmhSequenceId
                                                           && (string)factorCommment.Parent.Attribute("comment") == "Yes"
                                                           && (string)factorCommment.Value.Trim() != string.Empty
                                                        select factorCommment);

                #region FACTOR_COMMENT
                foreach (XElement comment in factorComments)
                {
                    factorSequence = comment.Parent.Attribute("sequence").Value;
                    type = "Factor Comment";
                    title = "<p><a href='#' class='tt'>" + comment.Parent.Attribute("note").Value;
                    title += "<span class='tooltip'><span class='top'></span><span class='middle'>";
                    title += comment.Value;
                    title += "</span><span class='bottom'></span></span></a></p>";

                    _listOfNCQADoc.Add(new NCQADetails(pcmhSequenceId, elementSequenceId, factorSequence, string.Empty, type, title, string.Empty,
                        practiceId, siteId, projectId, docName, referencePage, relevancyLevel, docType, factorTitle));
                }

                #endregion

                // All users comments in current element against each factor
                IEnumerable<XElement> userCommments = (from userNotes in questionnaire.Descendants("Element").Descendants("PrivateNote")
                                                       where (string)userNotes.Parent.Parent.Attribute("sequence") == elementSequenceId
                                                          && (string)userNotes.Parent.Parent.Parent.Attribute("sequence") == pcmhSequenceId
                                                       select userNotes);

                #region USERS_COMMENTS
                foreach (XElement privateNoteElement in userCommments)
                {
                    int count = 0;
                    factorSequence = privateNoteElement.Parent.Attribute("sequence").Value;
                    type = "Note for PCMH " + pcmhSequenceId + " - " + "Element " + DEFAULT_LETTERS[Convert.ToInt32(elementSequenceId) - 1] + " - " + "Factor " + factorSequence;

                    title = "<p><a href='#' class='tt'>" + "Note";
                    title += "<span class='tooltip'><span class='top'></span><span class='middle'>";

                    foreach (XElement comment in privateNoteElement.Elements())
                    {
                        count = count + 1;
                        title += count.ToString() + ". " + comment.Value;
                        title += "<span class='notice'>(" + comment.Attribute("user").Value + " on " + comment.Attribute("date").Value + ")</span><br />";
                    }

                    title += "</span><span class='bottom'></span></span></a></p>";

                    _listOfNCQADoc.Add(new NCQADetails(pcmhSequenceId, elementSequenceId, factorSequence, string.Empty, type, title, string.Empty,
                        practiceId, siteId, projectId, docName, referencePage, relevancyLevel, docType, factorTitle));
                }

                #endregion

                // Reviewer Notes 
                string reviewerNotes = (from commentRecord in questionnaire.Descendants("Element").Descendants("ReviewerNotes")
                                        where (string)commentRecord.Parent.Attribute("sequence") == elementSequenceId
                                           && (string)commentRecord.Parent.Parent.Attribute("sequence") == pcmhSequenceId
                                        select commentRecord.Value.Trim()).FirstOrDefault();

                #region REVIEWER_NOTES
                type = "Reviewer Notes";
                title = "<p><a href='#' class='tt'>Reviewer Notes for PCMH " + pcmhSequenceId + " - " + "Element " + DEFAULT_LETTERS[Convert.ToInt32(elementSequenceId) - 1];
                title += "<span class='tooltip'><span class='top'></span><span class='middle'>";
                title += reviewerNotes;
                title += "</span><span class='bottom'></span></span></a></p>";

                if (reviewerNotes != string.Empty && reviewerNotes != null)
                    _listOfNCQADoc.Add(new NCQADetails(pcmhSequenceId, elementSequenceId, string.Empty, string.Empty, type, title, string.Empty,
                        practiceId, siteId, projectId, docName, referencePage, relevancyLevel, docType, factorTitle));

                #endregion

                return _listOfNCQADoc.ToList();

            }
            catch (Exception exception)
            {
                throw exception;
            }

        }

        #endregion
    }
}