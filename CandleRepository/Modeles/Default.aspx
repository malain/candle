<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Modeles_Default" Title="Untitled Page" %>

<%@ Register Src="../WebParts/RepositoryTags.ascx" TagName="RepositoryTags" TagPrefix="uc1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Body" Runat="Server">
 <asp:HiddenField ID="currentTags" runat="server">
</asp:HiddenField>
   <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </cc1:ToolkitScriptManager>
    <uc1:RepositoryTags ID="RepositoryTags1" runat="server" width="100%" EnableViewState="false"/>
    <asp:Repeater ID="metadataRepeater" runat="server" EnableViewState="True" OnItemDataBound="metadataRepeater_ItemDataBound">
    <ItemTemplate>
        <div class="Item">
        <asp:Panel CssClass="ItemDomain" runat="server" id="pnlDomain"><asp:Label runat="server" ID="lbDomain" /></asp:Panel>
    <div class="ItemHeader"><div class="ItemHeaderName"><asp:Label runat="server" ID="lbName" /></div><div class="ItemHeaderVersion"><asp:HyperLink runat="server" ID="lnkModel" /></div></div>
    <div class="ItemDescription"><asp:Label runat="server" ID="lbDescription" />
    </div></div>
    </ItemTemplate>
    </asp:Repeater>
    &nbsp;&nbsp;
</asp:Content>

