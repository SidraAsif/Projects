#region Modification History

//  ******************************************************************************
//  Module        : AddressDetails
//  Created By    : Mirza Fahad Ali Baig
//  When Created  : 12/01/2011
//  Description   : 
//
//  ********************************** Modification History **********************
//  Who                     Date            Description
//  *******************************************************************************
//  Mirza Fahad Ali Baig    02-27-2012      Remove Un-nessary code
//  Mirza Fahad Ali Baig    02-27-2012      Optimize the current code
//  *******************************************************************************

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BMTBLL
{
    public class AddressBO : BMTConnection
    {
        #region CONSTANTS
        protected const string DEFAULT_ADDRESS = "";
        protected const string DEFAULT_CITY = "";
        protected const string DEFAULT_STATE = "";
        protected const string DEFAULT_ZIPCODE = "";

        #endregion

        #region VARIABLE
        protected Address _address { get; set; }

        protected int _addressId;
        public int AddressId
        {
            get { return _addressId; }
            set { _addressId = value; }
        }

        protected string _primaryAddress;
        public string PrimaryAddress
        {
            get { return _primaryAddress; }
            set { _primaryAddress = value; }
        }

        protected string _secondaryAddress;
        public string SecondaryAddress
        {
            get { return _secondaryAddress; }
            set { _secondaryAddress = value; }
        }

        protected string _city;
        public string City
        {
            get { return _city; }
            set { _city = value; }
        }

        protected string _state;
        public string State
        {
            get { return _state; }
            set { _state = value; }
        }

        protected string _zipCode;
        public string ZipCode
        {
            get { return _zipCode; }
            set { _zipCode = value; }
        }

        protected string _phone;
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }

        protected string _mobile;
        public string Mobile
        {
            get { return _mobile; }
            set { _mobile = value; }
        }

        protected string _fax;
        public string Fax
        {
            get { return _fax; }
            set { _fax = value; }
        }

        protected string _email;
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        #endregion

        #region CONSTRUCTOR
        public AddressBO()
        {
            _primaryAddress = DEFAULT_ADDRESS;
            _city = DEFAULT_CITY;
            _state = DEFAULT_STATE;
            _zipCode = DEFAULT_ZIPCODE;

        }

        #endregion

        #region FUNCTIONS
        public int SaveAddress()
        {
            try
            {
                _address = new Address();

                if (this.AddressId == 0)
                    return AddNewAddress();
                else
                    return UpdateAddress();

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private int AddNewAddress()
        {
            try
            {
                _address.PrimaryAddress = this.PrimaryAddress;
                _address.SecondaryAddress = this.SecondaryAddress;
                _address.State = this.State;
                _address.City = this.City;
                _address.ZipCode = this.ZipCode;
                _address.Telephone = this.Phone;
                _address.Mobile = this.Mobile;
                _address.Fax = this.Fax;
                _address.Email = this.Email;

                BMTDataContext.Addresses.InsertOnSubmit(_address);
                BMTDataContext.SubmitChanges();

                // return Address Id
                return _address.AddressId;
            }
            catch (System.Exception exception)
            {
                throw exception;
            }
        }

        private int UpdateAddress()
        {
            try
            {

                var AddressInfo = (from AddressRecord in BMTDataContext.Addresses
                                   where AddressRecord.AddressId == this.AddressId
                                   select AddressRecord).First();

                AddressInfo.PrimaryAddress = this.PrimaryAddress;
                AddressInfo.SecondaryAddress = this.SecondaryAddress;
                AddressInfo.State = this.State;
                AddressInfo.City = this.City;
                AddressInfo.ZipCode = this.ZipCode;
                AddressInfo.Telephone = this.Phone;
                AddressInfo.Mobile = this.Mobile;
                AddressInfo.Fax = this.Fax;
                AddressInfo.Email = this.Email;

                BMTDataContext.SubmitChanges();
                return this.AddressId;

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        #endregion
    }
}
