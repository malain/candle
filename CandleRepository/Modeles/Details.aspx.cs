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
using DSLFactory.Candle.Repository;

public partial class Modeles_Details : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ComponentModelMetadata metadata = CandleRepositoryController.Instance.GetMetadata(Request["id"], Request["version"]);
        if (metadata != null)
        {
            if (!String.IsNullOrEmpty(metadata.DocUrl))
            {
                pnlDoc.Text = String.Format(@"<iframe scrolling=""auto"" src=""{0}""/>", metadata.DocUrl);
            }
            else
                tabDoc.Visible = false;

            lstArtifacts.DataSource = CandleRepositoryController.Instance.GetArtifacts(metadata);
            lstArtifacts.DataBind();
        }
    }
}
