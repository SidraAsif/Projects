#region Modification History

//  ******************************************************************************
//  Module        : Credential Details
//  Created By    : Waqas Amin
//  When Created  : 03/21/2012
//  Description   : 
//
//  ********************************** Modification History **********************
//  Who                     Date            Description
//  *******************************************************************************
//  Mirza Fahad Ali Baig    03/21/2012      Add Error Handling
//  *******************************************************************************

#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BMTBLL.Enumeration;
using System.Text.RegularExpressions;

namespace BMTBLL
{
    public class SystemInfoBO : BMTConnection
    {   
        #region FUNCTIONS
        public string GetSystemInfoByKey(int systemInfoId)
        {
            try
            {
                return (from systemInfoRecord in BMTDataContext.SystemInfos
                        where systemInfoRecord.SystemInfoId == systemInfoId
                        select (string)systemInfoRecord.Value.ToString()).FirstOrDefault();
            }
            catch (Exception exception)
            {
                throw exception;
            }

        }
        public string GetSystemInfoByKey(int enterpriseId, enSystemInfo systemInfoKey)
        {
            try
            {
                return (from systemInfoRecord in BMTDataContext.SystemInfos
                        where systemInfoRecord.Key == systemInfoKey.ToString()
                        && systemInfoRecord.EnterpriseId == enterpriseId
                        select (string)systemInfoRecord.Value.ToString()).FirstOrDefault();
            }
            catch (Exception exception)
            {
                throw exception;
            }

        }
        public IQueryable GetSystemAdRotator(int enterpriseId)
        {
            try
            {
                return (from systemInfoRecord in BMTDataContext.SystemInfos.ToList()
                        where systemInfoRecord.Key == enSystemInfo.adBannerLink.ToString()
                        && systemInfoRecord.EnterpriseId == enterpriseId
                        select new
                        {
                            NavUrl = string.Format("{0}", systemInfoRecord.Value.ToString().Split('|')[1]),
                            ImgUrl = string.Format("{0}", systemInfoRecord.Value.ToString().Split('|')[0]), 
                            AltText = string.Empty,
                            systemInfoRecord.SystemInfoId,
                            systemInfoRecord.Key,
                            systemInfoRecord.Value
                              
                        }).AsQueryable();
            }
            catch (Exception exception)
            {
                throw exception;
            }

        }


        public string GetSystemAdRotatorList(int enterpriseId)
        {
            try
            {
                string adRotatorList = string.Empty;
                var systemInfoRow = (from systemInfoRecord in BMTDataContext.SystemInfos.ToList()
                                     where systemInfoRecord.Key == enSystemInfo.adBannerLink.ToString()
                                      && systemInfoRecord.EnterpriseId == enterpriseId
                                     select new
                                     {
                                         systemInfoRecord.Value
                                     });

                foreach (var adro in systemInfoRow)
                {
                    adRotatorList = adRotatorList + adro.Value + ";";                
                }

                return adRotatorList;
            }
            catch (Exception exception)
            {
                throw exception;
            }

        }
        #endregion
    }
}

