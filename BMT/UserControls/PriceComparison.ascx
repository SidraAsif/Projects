<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PriceComparison.ascx.cs"
    Inherits="PriceComparison" %>
<%@ Register Src="~/UserControls/DisplayMessage.ascx" TagName="DisplayMessage" TagPrefix="ucdm" %>
<link rel="Stylesheet" type="text/css" href="../Themes/priceCalculator.css" />
<script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/jgcharts.js") %>"></script>
<script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/priceComparison.js") %>"></script>
<script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/JQChart/jquery.jqChart.min.js") %>"></script>
<%--<script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/JQChart/excanvas.js") %>"></script>--%>
<div class="body-container-right">
    <asp:Panel ID="pnlmessage" runat="server" Style="margin-left: 10px;">
        <ucdm:DisplayMessage ID="message" runat="server" DisplayMessageWidth="700" />
    </asp:Panel>
    <div class="heading1">
        <asp:Label ID="lblPracticeName" runat="server" Text=""></asp:Label>
    </div>
    <div class="heading3">
        EHR/PM Total Cost of Ownership (TOC) - 5 Years Comparison
    </div>
    <table width="730" border="0">
        <tr>
            <td>
                <asp:GridView ID="gvPriceDetails" runat="server" AllowPaging="true" AllowSorting="true"
                    PageSize="8" AutoGenerateColumns="false" HeaderStyle-BackColor="#5880B3" HeaderStyle-ForeColor="#FFFFFF"
                    BorderColor="White" HeaderStyle-HorizontalAlign="Center" OnPageIndexChanging="gvPriceDetails_PageIndexChanging"
                    CssClass="gvpriceComparison">
                    <EmptyDataRowStyle BackColor="LightBlue" ForeColor="Red" Width="600" />
                    <EmptyDataTemplate>
                        <table width="700" style="background-color: #5880B3; color: #FFFFFF;">
                            <tr>
                                <td align="center">
                                    No Record Found.
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <AlternatingRowStyle BackColor="#F2F2F2" />
                    <PagerSettings FirstPageText="First Page" LastPageText="Last Page" Mode="NumericFirstLast"
                        Position="Bottom" />
                    <PagerStyle BackColor="#5880B3" ForeColor="White" BorderColor="Transparent" BorderWidth="0px"
                        BorderStyle="None" Font-Bold="true" />
                    <Columns>
                        <asp:BoundField DataField="SystemName" HeaderText="EHR/PM" ItemStyle-HorizontalAlign="Left"
                            HeaderStyle-Width="125" ItemStyle-BorderColor="White" />
                        <asp:BoundField DataField="PurchaseModel" HeaderText="Purchase Model" ItemStyle-HorizontalAlign="Left"
                            HeaderStyle-Width="125" ItemStyle-BorderColor="White" />
                        <asp:BoundField DataField="YearOne" HeaderText="Year 1" ItemStyle-HorizontalAlign="Right"
                            HeaderStyle-Width="75" ItemStyle-BorderColor="White" DataFormatString="${0:#,0.00}" />
                        <asp:BoundField DataField="YearTwoToFive" HeaderText="Year 2-5" ItemStyle-HorizontalAlign="Right"
                            HeaderStyle-Width="75" ItemStyle-BorderColor="White" DataFormatString="${0:#,0.00}" />
                        <asp:BoundField DataField="TOCYearTwo" HeaderText="TOC Year 2" ItemStyle-HorizontalAlign="Right"
                            HeaderStyle-Width="75" ItemStyle-BorderColor="White" DataFormatString="${0:#,0.00}" />
                        <asp:BoundField DataField="TOCYearThree" HeaderText="TOC Year 3" ItemStyle-HorizontalAlign="Right"
                            HeaderStyle-Width="75" ItemStyle-BorderColor="White" DataFormatString="${0:#,0.00}" />
                        <asp:BoundField DataField="TOCYearFour" HeaderText="TOC Year 4" ItemStyle-HorizontalAlign="Right"
                            HeaderStyle-Width="75" ItemStyle-BorderColor="White" DataFormatString="${0:#,0.00}" />
                        <asp:BoundField DataField="TOCYearFive" HeaderText="TOC Year 5" ItemStyle-HorizontalAlign="Right"
                            HeaderStyle-Width="75" ItemStyle-BorderColor="White" DataFormatString="${0:#,0.00}" />
                        <asp:TemplateField ItemStyle-BorderColor="White" HeaderStyle-Width="25" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Image ID="hlSiteName" runat="server" ImageUrl="~/Themes/Images/info.png" AlternateText='<%# Eval("ProviderInfo")%>'
                                    ToolTip='<%# Eval("ProviderInfo")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
    <div style="clear: both; padding-top: 25px;">
        <div id="jqChart" style="width: 720px; height: 450px;">
        </div>
    </div>
    <table>
        <tr>
            <td align="right" style="width: 40%">
                <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_Click" />
            </td>
            <td>
                <asp:Button ID="btnSaveDoc" runat="server" Text="Save to My Documents" OnClick="btnSaveDoc_Click" />
            </td>
            <td>
                <asp:Button ID="btnReturn" runat="server" Text="Return to Calculator" />
            </td>
        </tr>
    </table>
</div>
