<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RepositoryTags.ascx.cs" Inherits="RepositoryCloudTagsControl" %>
<div class="CloudHeader">Tags</div>
<asp:Panel runat="server" ID="Container" CssClass="CloudBox">
<asp:Literal ID="CloudMarkup" runat="server" />
<asp:CheckBoxList ID="lstSelectedTags" Visible="false" runat="server" AutoPostBack="True" RepeatDirection="Horizontal" OnSelectedIndexChanged="lstSelectedTags_SelectedIndexChanged">
</asp:CheckBoxList></asp:Panel>