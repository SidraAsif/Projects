using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BMT.WEB;
namespace BMT.UserControls
{
    public partial class DisplayMessage : System.Web.UI.UserControl
    {
        #region Data_Member

        #endregion

        #region Properties
        public bool ShowCloseButton { get; set; }
        public string validationGroup { get; set; }
        public int DisplayMessageWidth { get; set; }
        public string ValidationSummaryHeaderText { get; set; }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (ValidationSummaryHeaderText != string.Empty)
                    vSummary.HeaderText = ValidationSummaryHeaderText;

                vSummary.ValidationGroup = validationGroup;

                if (ShowCloseButton)
                    CloseButton.Attributes.Add("onclick", "document.getElementById('" + MessageBox.ClientID + "').style.display = 'none'");

                Image1.Visible = ShowCloseButton;

                if (!IsPostBack)
                {
                    if (DisplayMessageWidth != 0)
                        MessageBox.Width = (Unit)DisplayMessageWidth;
                }
            }
            catch (Exception exception)
            {
                Logger.PrintError(exception);
            }

        }

        #endregion

        #region Wrapper methods
        public void Error(string message)
        {
            Show(MessageType.Error, message);
        }

        public void Info(string message)
        {
            Show(MessageType.Info, message);
        }

        public void Success(string message)
        {
            Show(MessageType.Success, message);
        }

        public void Warning(string message)
        {
            Show(MessageType.Warning, message);
        }

        public void Clear(string message)
        {
            Show(MessageType.Clear, message);
        }
        #endregion

        #region Show control

        public void Show(MessageType messageType, string message)
        {
            CloseButton.Visible = ShowCloseButton;
            litMessage.Text = message;

            MessageBox.CssClass = messageType.ToString().ToLower();
            this.Visible = true;
        }

        #endregion

        #region Enum

        public enum MessageType
        {
            Error = 1,
            Info = 2,
            Success = 3,
            Warning = 4,
            Clear = 5
        }

        #endregion
    }
}