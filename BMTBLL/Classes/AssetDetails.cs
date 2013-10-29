using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BMTBLL
{
    public class AssetDetails
    {
        #region PROPERTIES

        public int AssetTypeId { get; set; }
        public int Sequence { get; set; }
        public string Name { get; set; } 
        public int AssetParentId { get; set; }
        public int ValueKey { get; set; } 

        #endregion

        #region CONSTRUCTOR
        public AssetDetails()
        { }

        public AssetDetails(int assetTypeId, int sequence, string name, int parentAssetTypeId)
        {
            this.AssetTypeId = assetTypeId;
            this.Sequence = sequence;
            this.Name = name;
            this.AssetParentId = parentAssetTypeId;
        }
        #endregion
    }
}
