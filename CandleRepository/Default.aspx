<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" Title="Untitled Page" %>

<%@ Register Src="WebParts/LastModelsPublication.ascx" TagName="LastModelsPublication"
    TagPrefix="uc1" %>
<%@ Register Src="WebParts/Actualites.ascx" TagName="Actualites" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" Runat="Server">
    <asp:WebPartManager ID="WebPartManager1" runat="server" Personalization-Enabled="false">
    </asp:WebPartManager>
    <div class="Home_Zone1">
    <asp:WebPartZone ID="Zone1" runat="server" BorderColor="#CCCCCC" Font-Names="Verdana"
        Padding="6">
        <PartChromeStyle BackColor="#EFF3FB" BorderColor="#D1DDF1" Font-Names="Verdana" ForeColor="#333333" />
        <MenuLabelHoverStyle ForeColor="#D1DDF1" />
        <EmptyZoneTextStyle Font-Size="0.8em" />
        <MenuLabelStyle ForeColor="White" />
        <MenuVerbHoverStyle BackColor="#EFF3FB" BorderColor="#CCCCCC" BorderStyle="Solid"
            BorderWidth="1px" ForeColor="#333333" />
        <HeaderStyle Font-Size="0.7em" ForeColor="#CCCCCC" HorizontalAlign="Center" />
        <MenuVerbStyle BorderColor="#507CD1" BorderStyle="Solid" BorderWidth="1px" ForeColor="White" />
        <PartStyle Font-Size="0.8em" ForeColor="#333333" />
        <TitleBarVerbStyle Font-Size="0.6em" Font-Underline="False" ForeColor="White" />
        <MenuPopupStyle BackColor="#507CD1" BorderColor="#CCCCCC" BorderWidth="1px" Font-Names="Verdana"
            Font-Size="0.6em" />
        <PartTitleStyle BackColor="#507CD1" Font-Bold="True" Font-Size="0.8em" ForeColor="White" />
    </asp:WebPartZone>
    </div>
    <div class="Home_Zone2">
    <asp:WebPartZone ID="Zone2" runat="server" BorderColor="#CCCCCC" Font-Names="Verdana"
        Padding="6">
        <PartChromeStyle BackColor="#EFF3FB" BorderColor="#D1DDF1" Font-Names="Verdana" ForeColor="#333333" />
        <MenuLabelHoverStyle ForeColor="#D1DDF1" />
        <EmptyZoneTextStyle Font-Size="0.8em" />
        <MenuLabelStyle ForeColor="White" />
        <MenuVerbHoverStyle BackColor="#EFF3FB" BorderColor="#CCCCCC" BorderStyle="Solid"
            BorderWidth="1px" ForeColor="#333333" />
        <HeaderStyle Font-Size="0.7em" ForeColor="#CCCCCC" HorizontalAlign="Center" />
        <MenuVerbStyle BorderColor="#507CD1" BorderStyle="Solid" BorderWidth="1px" ForeColor="White" />
        <PartStyle Font-Size="0.8em" ForeColor="#333333" />
        <TitleBarVerbStyle Font-Size="0.6em" Font-Underline="False" ForeColor="White" />
        <MenuPopupStyle BackColor="#507CD1" BorderColor="#CCCCCC" BorderWidth="1px" Font-Names="Verdana"
            Font-Size="0.6em" />
        <PartTitleStyle BackColor="#507CD1" Font-Bold="True" Font-Size="0.8em" ForeColor="White" />
    </asp:WebPartZone>
    </div>
    <div class="Home_Zone3">
    <asp:WebPartZone ID="Zone3" runat="server" BorderColor="#CCCCCC" Font-Names="Verdana"
        Padding="6">
        <PartChromeStyle BackColor="#EFF3FB" BorderColor="#D1DDF1" Font-Names="Verdana" ForeColor="#333333" />
        <MenuLabelHoverStyle ForeColor="#D1DDF1" />
        <EmptyZoneTextStyle Font-Size="0.8em" />
        <MenuLabelStyle ForeColor="White" />
        <MenuVerbHoverStyle BackColor="#EFF3FB" BorderColor="#CCCCCC" BorderStyle="Solid"
            BorderWidth="1px" ForeColor="#333333" />
        <HeaderStyle Font-Size="0.7em" ForeColor="#CCCCCC" HorizontalAlign="Center" />
        <MenuVerbStyle BorderColor="#507CD1" BorderStyle="Solid" BorderWidth="1px" ForeColor="White" />
        <PartStyle Font-Size="0.8em" ForeColor="#333333" />
        <TitleBarVerbStyle Font-Size="0.6em" Font-Underline="False" ForeColor="White" />
        <MenuPopupStyle BackColor="#507CD1" BorderColor="#CCCCCC" BorderWidth="1px" Font-Names="Verdana"
            Font-Size="0.6em" />
        <PartTitleStyle BackColor="#507CD1" Font-Bold="True" Font-Size="0.8em" ForeColor="White" />
        <ZoneTemplate>
        <uc1:LastModelsPublication ID="LastModelsPublication1" runat="server" />
        </ZoneTemplate>
    </asp:WebPartZone>
        &nbsp;
    </div>
</asp:Content>

