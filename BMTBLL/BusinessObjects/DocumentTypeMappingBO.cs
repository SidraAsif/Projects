using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BMTBLL
{
    public class DocumentTypeMappingBO : BMTConnection
    {
        public DocumentTypeMappingBO()
        { }

        public string GetOriginalDocumentType(string documentType)
        {
            return (from DocumentTypeRecord in BMTDataContext.DocumentTypeMappings
                    where DocumentTypeRecord.DocumentType == documentType
                    select DocumentTypeRecord.MappingName.ToString()).First();
        }

        public string GetDocumentType(string orignalDocumentType)
        {
            return (from DocumentTypeRecord in BMTDataContext.DocumentTypeMappings
                    where DocumentTypeRecord.MappingName == orignalDocumentType
                    select DocumentTypeRecord.DocumentType.ToString()).First();
        }
    }
}
