using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DSLFactory.Candle.SystemModel.Repository;
using AjaxControlToolkit;
using DSLFactory.Candle.Repository;
using System.Collections.Generic;

public partial class Modeles_Default : System.Web.UI.Page
{
    private string lastCategoryString;
    private string lastComponentName;

    protected void Page_Load(object sender, EventArgs e)
    {
        string tag = Request["tag"];
        string path = Request["path"];
        List<string> tags = null;
        if (tag != null)
        {
            if (string.IsNullOrEmpty(currentTags.Value))
                currentTags.Value = tag;
            else
                currentTags.Value += "." + tag;
            tags = new List<string>( currentTags.Value.Split('.'));
        }

        metadataRepeater.DataSource = CandleRepositoryController.Instance.GetAllSortedMetadata(tags, path);
        metadataRepeater.DataBind();
    }

    protected void metadataRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        ComponentModelMetadata metadata = e.Item.DataItem as ComponentModelMetadata;
        if (metadata == null)
            return;

        Label lbDomain = e.Item.FindControl("lbDomain") as Label;
        Label lbDescription = e.Item.FindControl("lbDescription") as Label;
        HyperLink link = e.Item.FindControl("lnkModel") as HyperLink;
        link.NavigateUrl = String.Format("details.aspx?id={0}&version={1}", metadata.Id, metadata.Version);
        Label lbName = e.Item.FindControl("lbName") as Label;

        //LinkButton l = e.Item.FindControl("Option1") as LinkButton;
        //l.CommandArgument = metadata.Id.ToString();
        //l = e.Item.FindControl("Option2") as LinkButton;
        //l.CommandArgument = metadata.Id.ToString();
        //l = e.Item.FindControl("Option3") as LinkButton;
        //l.CommandArgument = metadata.Id.ToString();

        if (lastCategoryString != metadata.Path)
        {
            lbDomain.Text = metadata.Path.Replace('/', '>');
            lastCategoryString = metadata.Path;
            lastComponentName = null;
        }
        else
        {
            Panel p = e.Item.FindControl("pnlDomain") as Panel;
            p.Visible = false;
            lbDomain.Visible = false;
        }
        if (lastComponentName != metadata.Name)
        {
            lbName.Text = metadata.Name;
            lastComponentName = metadata.Name;
        }
        else
            lbName.Text = String.Empty;

        link.Text = metadata.Version.ToString();
        lbDescription.Text = metadata.Description;
    }

    protected void OnCommand(object sender, CommandEventArgs e)
    {

    }
}
