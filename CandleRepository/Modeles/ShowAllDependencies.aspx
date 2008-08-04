<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ShowAllDependencies.aspx.cs" Inherits="Modeles_ShowAllDependencies" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Body" Runat="Server">
<script language="JavaScript" type="text/javascript" src="<%=ResolveUrl("~/scripts/svgcheck.js")%>"></script>
<script language="VBScript" src="<%=ResolveUrl("~/scripts/svgcheck.vbs")%>" type="text/vbscript"></script>
<script language="JavaScript"><!--
checkAndGetSVGViewer();
emitSVG('src="DependenciesGraph.aspx" name="SVGEmbed" height="1000" width="1000" type="image/svg-xml"');
// -->
</script>
<noscript>
<embed src="DependenciesGraph.aspx" name="SVGEmbed" height="1000" width="1000" type="image/svg-xml"
pluginspage="http://www.adobe.com/svg/viewer/install/">
</noscript>
</asp:Content>

