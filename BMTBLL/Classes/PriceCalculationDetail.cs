using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BMTBLL
{
    public class PriceCalculationDetail
    {
        #region PROPERTIES
        // Price calculator
        public string Quantity
        { get; set; }

        public string Type
        { get; set; }

        public string Amount
        { get; set; }

        public string PaymentMethod
        { get; set; }

        // Price comparison Summary
        public int ID
        { get; set; }

        public string SystemSequence
        { get; set; }

        public string SystemName
        { get; set; }

        public string PurchaseModel
        { get; set; }

        public double YearOne
        { get; set; }

        public double YearTwoToFive
        { get; set; }

        public double TOCYearTwo
        { get; set; }

        public double TOCYearThree
        { get; set; }

        public double TOCYearFour
        { get; set; }

        public double TOCYearFive
        { get; set; }

        public string ProviderInfo
        { get; set; }

        // Char comparison detail
        public string Year
        { get; set; }

        public double Value
        { get; set; }

        #endregion

        #region CONSTRUCTOR
        public PriceCalculationDetail(string quantity, string type, string amount, string paymentMethod)
        {
            this.Quantity = quantity;
            this.Type = type;
            this.Amount = amount;
            this.PaymentMethod = paymentMethod;
        }

        public PriceCalculationDetail(int id, string systemSequence, string systemName, string purchaseModel, double yearOne, double yearTwoToFive, double tOCYearTwo,
            double tOCYearThree, double tOCYearFour, double tOCYearFive, string providerInfo)
        {
            this.ID = id;
            this.SystemSequence = systemSequence;
            this.SystemName = systemName;
            this.PurchaseModel = purchaseModel;
            this.YearOne = yearOne;
            this.YearTwoToFive = yearTwoToFive;
            this.TOCYearTwo = tOCYearTwo;
            this.TOCYearThree = tOCYearThree;
            this.TOCYearFour = tOCYearFour;
            this.TOCYearFive = tOCYearFive;
            this.ProviderInfo = providerInfo;
        }

        public PriceCalculationDetail(string systemSequence, string systemName, string purchaseModel, string year, double value)
        {
            this.SystemSequence = systemSequence;
            this.SystemName = systemName;
            this.PurchaseModel = purchaseModel;
            this.Year = year;
            this.Value = value;
        }

        #endregion
    }
}
