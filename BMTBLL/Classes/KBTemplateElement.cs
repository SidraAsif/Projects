using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BMTBLL;

namespace BMTBLL
{
    public class KBTemplateElement
    {
        #region PROPERTIES
        //public int AnswerTypeEnumId { get; set; }
        //public int AnswerTypeId { get; set; }
        //public bool Complete { get; set; }
        //public string CriticalToolTip { get; set; }
        //public string DataBoxComments { get; set; }
        //public string DefaultScore { get; set; }
        //public string EvaluationNotes { get; set; }
        //public int FilledAnswersId { get; set; }
        //public bool IsCorporateElement { get; set; }
        //public bool IsCritical { get; set; }
        //public bool IsDataBox { get; set; }
        //public bool IsInfoDocEnable { get; set; }
        //public int KnowledgeBaseId { get; set; }
        //public int KnowledgeBaseTemplateId { get; set; }
        //public int LogsOrToolsDocumentCount { get; set; }
        //public int MaxPoints { get; set; }
        //public string Name { get; set; }
        //public int OtherDocumentsCount { get; set; }
        //public int ParentKnowledgeBaseId { get; set; }
        //public int PoliciesDocumentCount { get; set; }
        //public string PrivateNotes { get; set; }
        //public int ReferencePages { get; set; }
        //public int ReportsDocumentCount { get; set; }
        //public string ReviewNotes { get; set; }
        //public int ScreenShotsDocumentCount { get; set; }
        //public int Sequence { get; set; }
        //public int TemplateId { get; set; }

        public int KnowledgeBaseTemplateId { get; set; }
        public int KnowledgeBaseId { get; set; }
        public int TemplateId { get; set; }
        public int ParentKnowledgeBaseId { get; set; }
        public int Sequence { get; set; }
        public int AnswerTypeId { get; set; }
        public int MaxPoints { get; set; }
        public bool IsCritical { get; set; }
        public string CriticalToolTip { get; set; }
        public bool IsDataBox { get; set; }
        public bool IsCorporateElement { get; set; }
        public bool IsInfoDocEnable { get; set; }
        public int ReferencePages { get; set; }
        public string Name { get; set; }
        public int FilledAnswersId { get; set; }
        public int AnswerTypeEnumId { get; set; }
        public bool Complete { get; set; }
        public string DataBoxComments { get; set; }
        public string DefaultScore { get; set; }
        public int PoliciesDocumentCount { get; set; }
        public int ReportsDocumentCount { get; set; }
        public int ScreenShotsDocumentCount { get; set; }
        public int LogsOrToolsDocumentCount { get; set; }
        public int OtherDocumentsCount { get; set; }
        public string PrivateNotes { get; set; }
        public string ReviewNotes { get; set; }
        public string EvaluationNotes { get; set; }

       // public int KnowledgeBaseTemplateId { get; set; }

        //public int KnowledgeBaseId { get; set; }

        //public int TemplateId { get; set; }

        //public Nullable<int> ParentKnowledgeBaseId { get; set; }

        //public int Sequence { get; set; }

        //public Nullable<int> AnswerTypeId { get; set; }

        //public Nullable<int> MaxPoints { get; set; }

        //public Nullable<bool> IsCritical { get; set; }

        //public string CriticalToolTip { get; set; }

        //public Nullable<bool> IsDataBox { get; set; }

        //public Nullable<bool> IsCorporateElement { get; set; }

        //public bool IsInfoDocEnable { get; set; }

        //public Nullable<int> ReferencePages { get; set; }

        //public string Name { get; set; }

        //public int FilledAnswersId { get; set; }

        //public Nullable<int> AnswerTypeEnumId { get; set; }

        //public Nullable<bool> Complete { get; set; }

        //public string DataBoxComments { get; set; }

        //public string DefaultScore { get; set; }

        //public Nullable<int> PoliciesDocumentCount { get; set; }

        //public Nullable<int> ReportsDocumentCount { get; set; }

        //public Nullable<int> ScreenShotsDocumentCount { get; set; }

        //public Nullable<int> LogsOrToolsDocumentCount { get; set; }

        //public Nullable<int> OtherDocumentsCount { get; set; }

        //public string PrivateNotes { get; set; }

        //public string ReviewNotes { get; set; }

        //public string EvaluationNotes { get; set; }
        #endregion

        #region CONSTRUCTOR
        public KBTemplateElement()
        { }
        #endregion
    }
}
