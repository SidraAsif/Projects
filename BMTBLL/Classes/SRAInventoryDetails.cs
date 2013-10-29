using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BMTBLL
{
    public class SRAInventoryDetails
    {
        #region properties
        public int AssetCategoryId { get; set; }
        public string AssetCategory { get; set; }
        public string AssetType { get; set; }
        public string AssetDescription { get; set; }
        public string PracticeNotes { get; set; }
        public string ReviewNotes { get; set; }

        #endregion


        #region Constructor

        public SRAInventoryDetails()
        { }


        public SRAInventoryDetails(int _assetCategoryId, string _assetCategory, string _assetType, string _assetDescription, string _practiceNotes, string _reviewNotes)
        {
            this.AssetCategoryId = _assetCategoryId;
            this.AssetCategory = _assetCategory;
            this.AssetType = _assetType;
            this.AssetDescription = _assetDescription;
            this.PracticeNotes = _practiceNotes;
            this.ReviewNotes = _reviewNotes;
        }

        #endregion
    }
}
