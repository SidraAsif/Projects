<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ITConsultant.ascx.cs"
    Inherits="ITConsultant" %>
<%@ Register Src="~/UserControls/DisplayMessage.ascx" TagName="DisplayMessage" TagPrefix="ucdm" %>
<%@ Register Src="~/UserControls/LoadingPanel.ascx" TagName="LoadingPanel" TagPrefix="ucl" %>
<%--<link rel="Stylesheet" type="text/css" href="../Themes/NCQA.css" />--%>
<%--<script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/jquery-1.7.1.min.js") %>"></script>--%>
<style type="text/css">
    .header1
    {
        background-color: #5880B3;
        color: White;
        text-align: center;
        vertical-align: middle;
        width: 60px;
    }
    
    .header2
    {
        background-color: #5880B3;
        color: White;
        text-align: center;
        vertical-align: middle;
        width: 20px;
    }
    
    .header3
    {
        background-color: #5880B3;
        color: White;
        text-align: center;
        vertical-align: middle;
        width: 80px;
    }
</style>
<script type="text/javascript">

    $(document).ready(function () {
        $('#ctl00_bodyContainer_ITConsultant_Btn_Cancel').click(function () {
            $('html,body').animate({
                scrollTop: $(".body-container-right").offset().top
            }, 0);
        });

        $('#ctl00_bodyContainer_ITConsultant_Btn_Submit').click(function () {
            $('html,body').animate({
                scrollTop: $(".body-container-right").offset().top
            }, 0);
        });
    });
</script>
<div class="body-container-right">
    <asp:Panel ID="pnlmessage" runat="server" Style="margin-left: 10px;">
        <ucdm:DisplayMessage ID="message" runat="server" DisplayMessageWidth="700"></ucdm:DisplayMessage>
    </asp:Panel>
    <asp:Table runat="server" Width="100%">
        <asp:TableRow>
            <asp:TableCell>
                <asp:Label runat="server" ID="lblHeader" Text="Register External IT Consultant for Security Risk Assessment"
                    Style="color: #C10000; font-family: Calibri; font-size: 20px; font-weight: bold;">
                </asp:Label>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2">
<div  style="font-family: Calibri; font-weight:bold;margin-top:20px; font-size:14px;">
<span>
Please select your IT Consulting Firm:
</span>
</div>
<span style="font-family: Calibri;font-size:12px;">
Note: If you do not have an existing relationship with one of the firms listed below, but would like to use their services, we STRONGLY recommend that you contact the IT company by phone or email for pricing and contractual information.
</span><br/>
<span style="font-family: Calibri;font-size:12px;">
External IT Consulting features will only be enabled if both you and your contracted IT Consultant request this service.
</span>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <br />
    <asp:Table runat="server" ID="THeader" Width="95%" HorizontalAlign="Center">
        <asp:TableRow>
            <asp:TableCell CssClass="header3">
<span style="font-family: Calibri;font-size:12px; font-weight:bold;">Website</span>
            </asp:TableCell>
            <asp:TableCell CssClass="header1">
<span style="font-family: Calibri;font-size:12px;font-weight:bold;">Company Name</span>
            </asp:TableCell>
            <asp:TableCell CssClass="header1">
<span style="font-family: Calibri;font-size:12px;font-weight:bold;">City</span>
            </asp:TableCell>
            <asp:TableCell CssClass="header1">
<span style="font-family: Calibri;font-size:12px;font-weight:bold;">State</span>
            </asp:TableCell>
            <asp:TableCell CssClass="header1">
<span style="font-family: Calibri;font-size:12px;font-weight:bold;">Service Area</span>
            </asp:TableCell>
            <asp:TableCell CssClass="header1">
<span style="font-family: Calibri;font-size:12px;font-weight:bold;">Phone</span>
            </asp:TableCell>
            <asp:TableCell CssClass="header1">
<span style="font-family: Calibri;font-size:12px;font-weight:bold;">Relationship</span>
            </asp:TableCell>
            <asp:TableCell CssClass="header2">
<span style="font-family: Calibri;font-size:12px;font-weight:bold;">Email</span>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <br />
    <asp:Panel ID="PnlGrid" runat="server" Width="100%" Visible="true">
    </asp:Panel>
    <asp:Panel ID="Pnlform" runat="server" Width="100%" Visible="true">
        <asp:Table runat="server" Width="100%">
            <asp:TableRow>
                <asp:TableCell Width="5px">
                    <asp:RadioButton ID="RBForm" runat="server" GroupName="RButton" AutoPostBack="true"
                        OnCheckedChanged="RBForm_CheckedChanged" EnableViewState="false" />
                </asp:TableCell>
                <asp:TableCell>
<span  style="font-family: Calibri;font-size:11px;">
I want to use a different IT Consulting company
</span>
                </asp:TableCell>
                <asp:TableCell>
<span  style="font-family: Calibri;font-size:11px;color:#C00000">
(your IT Consultant will be charged a standard $250 fee to use the BizMed Toolbox for this service)
</span>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <br />
        <asp:Panel ID="PnlEntryForm" runat="server" Visible="false">
            <asp:Table runat="server" Width="90%" HorizontalAlign="Center">
                <asp:TableRow>
                    <asp:TableCell>
                        <span style="font-family: Calibri; font-size: 12px;">Consulting Company: </span>
                        <span style="font-family: Calibri; font-size: 15px; color: Red;">* </span>
                        <asp:RequiredFieldValidator ID="RfvTxtCompany" runat="server" ErrorMessage="" ControlToValidate="TxtCompany"
                            ValidationGroup="submit2"></asp:RequiredFieldValidator>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox runat="server" ID="TxtCompany" Width="150px" Text="Enter Text" ForeColor="DarkSlateGray"
                            onfocus="this.value='';"></asp:TextBox>
                    </asp:TableCell>
                    <asp:TableCell>
                        <span style="font-family: Calibri; font-size: 12px;">Consultant Full Name: </span>
                        <span style="font-family: Calibri; font-size: 15px; color: Red;">* </span>
                        <asp:RequiredFieldValidator ID="RfvTxtFullName" runat="server" ErrorMessage="" ControlToValidate="TxtFullName"
                            ValidationGroup="submit2"></asp:RequiredFieldValidator>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox runat="server" ID="TxtFullName" Width="150px" Text="Enter Text" ForeColor="DarkSlateGray"
                            onfocus="this.value='';"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <span style="font-family: Calibri; font-size: 12px;">Address: </span><span style="font-family: Calibri;
                            font-size: 15px; color: Red;">* </span>
                        <asp:RequiredFieldValidator ID="RfvTxtAddress" runat="server" ErrorMessage="" ControlToValidate="TxtAddress"
                            ValidationGroup="submit2"></asp:RequiredFieldValidator>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox runat="server" ID="TxtAddress" Width="150px" Text="Enter Text" ForeColor="DarkSlateGray"
                            onfocus="this.value='';"></asp:TextBox>
                    </asp:TableCell>
                    <asp:TableCell>
                        <span style="font-family: Calibri; font-size: 12px;">Email: </span><span style="font-family: Calibri;
                            font-size: 15px; color: Red;">* </span>
                        <asp:RequiredFieldValidator ID="RfvTxtEmail" runat="server" ErrorMessage="" ControlToValidate="TxtEmail"
                            ValidationGroup="submit2"></asp:RequiredFieldValidator>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox runat="server" ID="TxtEmail" Width="150px" Text="Enter Text" ForeColor="DarkSlateGray"
                            onfocus="this.value='';"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <span style="font-family: Calibri; font-size: 12px;">City: </span><span style="font-family: Calibri;
                            font-size: 15px; color: Red;">* </span>
                        <asp:RequiredFieldValidator ID="RfvTxtCity" runat="server" ErrorMessage="" ControlToValidate="TxtCity"
                            ValidationGroup="submit2"></asp:RequiredFieldValidator>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox runat="server" ID="TxtCity" Width="150px" Text="Enter Text" ForeColor="DarkSlateGray"
                            onfocus="this.value='';"></asp:TextBox>
                    </asp:TableCell>
                    <asp:TableCell>
                        <span style="font-family: Calibri; font-size: 12px;">Phone: </span><span style="font-family: Calibri;
                            font-size: 15px; color: Red;">* </span>
                        <asp:RequiredFieldValidator ID="RfvTxtPhone" runat="server" ErrorMessage="" ControlToValidate="TxtPhone"
                            ValidationGroup="submit2"></asp:RequiredFieldValidator>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox runat="server" ID="TxtPhone" Width="150px" Text="Enter Text" ForeColor="DarkSlateGray"
                            onfocus="this.value='';"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <span style="font-family: Calibri; font-size: 12px;">State: </span><span style="font-family: Calibri;
                            font-size: 15px; color: Red;">* </span>
                        <asp:RequiredFieldValidator ID="RfvTxtState" runat="server" ErrorMessage="" ControlToValidate="TxtState"
                            ValidationGroup="submit2"></asp:RequiredFieldValidator>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox runat="server" ID="TxtState" Width="150px" Text="Enter Text" ForeColor="DarkSlateGray"
                            onfocus="this.value='';"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <span style="font-family: Calibri; font-size: 12px;">Zip: </span><span style="font-family: Calibri;
                            font-size: 15px; color: Red;">* </span>
                        <asp:RequiredFieldValidator ID="RfvTxtZip" runat="server" ErrorMessage="" ControlToValidate="TxtZip"
                            ValidationGroup="submit2"></asp:RequiredFieldValidator>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox runat="server" ID="TxtZip" Width="150px" Text="Enter Text" ForeColor="DarkSlateGray"
                            onfocus="this.value='';"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </asp:Panel>
        <br />
        <asp:Table runat="server" Width="100%">
            <asp:TableRow>
                <asp:TableCell Width="5px">
                    <asp:RadioButton ID="RBNone" runat="server" GroupName="RButton" OnCheckedChanged="RBNone_CheckedChanged"
                        AutoPostBack="true" />
                </asp:TableCell>
                <asp:TableCell>
<span  style="font-family: Calibri;font-size:11px;">
None of the above. We are using internal resources for this assessment.
</span>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </asp:Panel>
    <br />
    <asp:Table runat="server" ID="TDisclaimer" Width="100%">
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2">
<div style="margin-bottom:20px;margin-top:20px;">
<span  style="font-family: Calibri;font-size:12px;">
<b>Disclaimer:</b> Listing of IT Consulting services are provided solely for the convenience of our users. EHR Pathway does not recommend IT Consulting companies and accepts no responsibility for services provided to you by any IT Consulting company.
</span>
</div>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right" Width="50%">
                <asp:Button ID="Btn_Submit" runat="server" Text="Submit" Width="120px" OnClick="Btn_Submit_Click"
                    ValidationGroup="submit" />
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Left" Width="50%">
                <asp:Button ID="Btn_Cancel" runat="server" Text="Cancel" Width="120px" OnClick="Btn_Cancel_Click" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="2">
<span style="font-family: Calibri;font-size:11px;">
*All trademarks or logos are the property of their respective owners.
</span>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <asp:HiddenField ID="HdnUserId" runat="server" />
    <asp:HiddenField ID="HdnChangedUserId" runat="server" />
    <asp:HiddenField ID="HdnIsChanged" runat="server" />
    <ucl:LoadingPanel ID="lpnlUser" runat="server" />
</div>
