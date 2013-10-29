<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DocumentVista.ascx.cs" Inherits="BMT.UserControls.DocumentVista" %>

<script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/apprise-1.5.min.js") %>"></script>
<script type="text/javascript" src="<%= Page.ResolveClientUrl("~/Scripts/commonDoc.js") %>"></script>

<asp:Panel ID="pnlDocuments" runat="server">
</asp:Panel>