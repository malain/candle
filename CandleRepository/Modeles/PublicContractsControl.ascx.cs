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
using DSLFactory.Candle.SystemModel;
using System.Text;

public partial class Modeles_PublicContractsControl : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            StringBuilder sb = new StringBuilder();

            ComponentModelMetadata metadata = CandleRepositoryController.Instance.GetMetadata(Request["id"], Request["version"]);
            if (metadata != null)
            {
                ModelLoader loader = ModelLoader.GetLoader(metadata);
                if (loader != null && loader.Model != null)
                {
                    if (loader.Model.SoftwareComponent != null)
                    {
                        foreach (ServiceContract contract in loader.Model.SoftwareComponent.PublicContracts)
                        {
                            sb.AppendLine(@"<div class=""Contract"">");

                            // Nom du contrat
                            sb.AppendLine(String.Format(@"<div class=""ContractHeader"">{0}</div>", contract.FullName));
                            // Description
                            sb.AppendLine(String.Format(@"<div class=""ContractDescription"">{0}</div>", Server.HtmlEncode(contract.Comment)));

                            foreach (Operation op in contract.Operations)
                            {
                                // Operation
                                sb.AppendLine(@"<div class=""ContractOperation"">");
                                sb.AppendLine(String.Format(@"<div class=""ContractOperationHeader"">{0} {1}({2})</div>", Server.HtmlEncode( op.FullTypeName), op.Name, op.CreateParametersDefinition()));
                                sb.AppendLine(String.Format(@"<div class=""ContractOperationDesc"">{0}</div>", Server.HtmlEncode(op.Comment)));

                                // Arguments
                                sb.AppendLine(@"<div class=""ContractArguments"">");

                                sb.AppendLine(@"<div class=""ContractArgumentsHeader"">");
                                sb.AppendLine(@"<div class=""ContractArgumentsHeaderType"">Type</div>");
                                sb.AppendLine(@"<div class=""ContractArgumentsHeaderName"">Name</div>");
                                sb.AppendLine(@"<div class=""ContractArgumentsHeaderDesc"">Description</div>");
                                sb.AppendLine(@"</div>");
                                
                                foreach (Argument arg in op.Arguments)
                                {
                                    sb.AppendLine(@"<div class=""ContractArgumentItem"">");
                                    sb.AppendLine(String.Format(@"<div class=""ContractArgumentItemType"">{0}</div>", Server.HtmlEncode( arg.FullTypeName)));
                                    sb.AppendLine(String.Format(@"<div class=""ContractArgumentItemName"">{0}</div>", arg.Name));
                                    sb.AppendLine(String.Format(@"<div class=""ContractArgumentItemDesc"">{0}</div>", Server.HtmlEncode(arg.Comment)));
                                    sb.AppendLine("</div>");
                                }
                                sb.AppendLine(@"</div></div>");
                            }
                        }
                    }
                }
            }
            lbCode.Text = sb.ToString();
        }
    }
}
