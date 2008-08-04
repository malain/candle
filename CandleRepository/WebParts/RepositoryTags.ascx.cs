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
using System.Collections.Generic;
using DSLFactory.Candle.Repository;
using System.Text;

public partial class RepositoryCloudTagsControl : System.Web.UI.UserControl
{
    private string[] _fontScale = new string[] {"xx-small", "x-small", "small", "medium", "large", "x-large", "xx-large"};

    protected void Page_Load(object sender, EventArgs e)
    {
        int minWeight;
        int maxWeight;
        Dictionary<string, int> tags = CandleRepositoryController.Instance.GetTaggings(out minWeight, out maxWeight);
        if( tags==null)
            return;
        decimal scaleUnitLength = (maxWeight - minWeight + 1) / (decimal)_fontScale.Length;

        StringBuilder sb = new StringBuilder();
        foreach (string tag in tags.Keys)
        {
            int scaleValue = (int)Math.Truncate((tags[tag] - minWeight) / scaleUnitLength);
            sb.AppendFormat("<a href='{0}?tag={2}' style='font-size:{1};'>{2}</a> ", this.Page.ResolveUrl("~/Modeles/default.aspx"), _fontScale[scaleValue], tag);
        }
        CloudMarkup.Text = sb.ToString();
    }

    public Unit Height
    {
        get { return Container.Height; }
        set { Container.Height = value; }
    }

    public Unit Width
    {
        get { return Container.Width; }
        set { Container.Width = value; }
    }

    protected void lstSelectedTags_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
