using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using BMTBLL;
using System.Xml.Linq;

using BMT.WEB;
using BMTBLL.Enumeration;
using BMTBLL;

namespace BMT.WebServices
{
    /// <summary>
    /// Summary description for SRAServices
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class ReportService : System.Web.Services.WebService
    {
        #region VARIABLE
        private readonly string[] DEFAULT_LETTERS = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        private char[] DEFAULT_CHARACTERS = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        private string ELEMENT_SEQUENCE = "1,2,3,4,5,6,7,8,9";
        private string STANDARD_SEQUENCE = "1,2,3,4,5,6";
        #endregion

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod(EnableSession = true)]
        public List<NCQADetails> GetNCQAStandards()
        {

            List<NCQADetails> _ncqaStandarList = new List<NCQADetails>();
            try
            {
                if (Session[enSessionKey.NCQAQuestionnaire.ToString()] != null)
                {
                    string recievedQuestionnaire = Session[enSessionKey.NCQAQuestionnaire.ToString()].ToString();
                    XDocument questionnaire = XDocument.Parse(recievedQuestionnaire);

                    IEnumerable<XElement> standards = from standardsRecord in questionnaire.Descendants("Standard")
                                                      select standardsRecord;

                    _ncqaStandarList.Add(new NCQADetails("0", "All Standards"));
                    foreach (XElement standard in standards)
                    {
                        _ncqaStandarList.Add(new NCQADetails(standard.Attribute("sequence").Value.ToString(), "PCMH" + " " + standard.Attribute("sequence").Value.ToString()));
                    }
                }

            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }

            return _ncqaStandarList;

        }

        [WebMethod(EnableSession = true)]
        public List<NCQADetails> GetNCQAElements(string standardSequence)
        {
            QuestionBO questionBO = new QuestionBO();
            List<NCQADetails> _ncqaElementList = new List<NCQADetails>();
            try
            {
                //if (Session[enSessionKey.NCQAQuestionnaire.ToString()] != null)
                //{
                //    string recievedQuestionnaire = Session[enSessionKey.NCQAQuestionnaire.ToString()].ToString();
                //    XDocument questionnaire = XDocument.Parse(recievedQuestionnaire);

                _ncqaElementList.Add(new NCQADetails("0", "All Elements"));
                if (standardSequence == "0")
                {
                    standardSequence = "1,2,3,4,5,6";
                    foreach (string sequenceNo in standardSequence.Split(','))
                    {
                        var elements = questionBO.GetElements(sequenceNo);

                        foreach (NCQADetails element in elements)
                        {
                            _ncqaElementList.Add(new NCQADetails(sequenceNo + element.ElementSequence.ToString(), sequenceNo + DEFAULT_LETTERS[Convert.ToInt32(element.ElementSequence) - 1].ToString()));
                        }
                    }

                    //{
                    //    IEnumerable<XElement> standards = from standardsRecord in questionnaire.Descendants("Standard")
                    //                                      where (string)standardsRecord.Attribute("sequence").Value.ToString() == sequenceNo
                    //                                      select standardsRecord;


                    //    foreach (XElement element in standards.Elements("Element"))
                    //    {
                    //        //if (element.Descendants("DocFile").Count() > 0)
                    //        _ncqaElementList.Add(new NCQADetails(sequenceNo + element.Attribute("sequence").Value.ToString(), standards.Attributes("sequence").First().Value + DEFAULT_LETTERS[Convert.ToInt32(element.Attribute("sequence").Value) - 1].ToString()));
                    //    }
                    //}
                }
                else
                {
                    foreach (string sequenceNo in standardSequence.Split(','))
                    {
                        var elements = questionBO.GetElements(sequenceNo);

                        foreach (NCQADetails element in elements)
                        {
                            _ncqaElementList.Add(new NCQADetails(sequenceNo + element.ElementSequence.ToString(), sequenceNo + DEFAULT_LETTERS[Convert.ToInt32(element.ElementSequence) - 1].ToString()));
                        }
                        //IEnumerable<XElement> standards = from standardsRecord in questionnaire.Descendants("Standard")
                        //                                  where (string)standardsRecord.Attribute("sequence").Value.ToString() == sequenceNo
                        //                                  select standardsRecord;


                        //foreach (XElement element in standards.Elements("Element"))
                        //{
                        //    //if (element.Descendants("DocFile").Count() > 0)
                        //    _ncqaElementList.Add(new NCQADetails(sequenceNo + element.Attribute("sequence").Value.ToString(), standards.Attributes("sequence").First().Value + DEFAULT_LETTERS[Convert.ToInt32(element.Attribute("sequence").Value) - 1].ToString()));
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }

            return _ncqaElementList;

        }


        [WebMethod(EnableSession = true)]
        public List<NCQADetails> GetNCQAFactors(string standardSequence, string elementSequence, string selectedElements)
        {
            QuestionBO questionBO = new QuestionBO();
            List<NCQADetails> _ncqaFactorList = new List<NCQADetails>();
            try
            {
                List<NCQADetails> Elements = new List<NCQADetails>();
                List<NCQADetails> Factors = new List<NCQADetails>();
                _ncqaFactorList.Add(new NCQADetails("0", "All Factors"));

                if (standardSequence == "0" && (elementSequence == "00" || elementSequence == "0"))
                {
                    standardSequence = "1,2,3,4,5,6";
                    foreach (string sequenceNo in standardSequence.Split(','))
                    {
                        Elements.AddRange(questionBO.GetElements(sequenceNo));
                    }
                }


                else if (standardSequence != "0" && elementSequence == "0")
                {
                    Elements = questionBO.GetElements(standardSequence);
                }


                else if (elementSequence != "0")
                {
                    var newElement = new NCQADetails();
                    newElement.PCMHSequence = elementSequence.Substring(0, 1);
                    newElement.ElementSequence = elementSequence.Substring(1, 1);
                    Elements.Add(newElement);
                }

                foreach (NCQADetails element in Elements)
                {
                    Factors.AddRange(questionBO.GetFactors(element.PCMHSequence, element.ElementSequence));
                }

                foreach (NCQADetails factor in Factors)
                {
                    _ncqaFactorList.Add(new NCQADetails(factor.FactorSequence, factor.PCMHSequence + DEFAULT_LETTERS[Convert.ToInt32(factor.ElementSequence) - 1].ToString() + "-" + factor.FactorSequence));
                }
            }

            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }

            return _ncqaFactorList;

        }

        [WebMethod(EnableSession = true)]
        public List<ConsultingUserDetail> GetConsultant()
        {            
            List<ConsultingUserDetail> lstConsultants = new List<ConsultingUserDetail>();
            ConsultingUserBO consultingUser = new ConsultingUserBO();
            lstConsultants = consultingUser.GetConsultants(Convert.ToInt32(Session[enSessionKey.EnterpriseId.ToString()]));
            ConsultingUserDetail allConsultant = new ConsultingUserDetail(0, "All Consultants");
            lstConsultants.Insert(0, allConsultant);

            return lstConsultants;

        }

        [WebMethod(EnableSession = true)]
        public List<PracticeSizeBO> PracticeSize()
        {
            PracticeSizeBO practiceSize = new PracticeSizeBO();
            List<PracticeSizeBO> lstPracticeSize = new List<PracticeSizeBO>();
            lstPracticeSize = practiceSize.GetPracticeSizeByGroupId();
            PracticeSizeBO allConsultant = new PracticeSizeBO(0, "All Practices");
            lstPracticeSize.Insert(0, allConsultant);

            return lstPracticeSize;
        }

    }

}
