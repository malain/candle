<%@ Page Language="C#"  AspCompat="true" %>
<%@ Import Namespace ="DSLFactory.Candle.SystemModel.Dependencies" %>
<%@ Import Namespace ="DSLFactory.Candle.SystemModel.Repository"%>
<%@ Import Namespace ="DSLFactory.Candle.SystemModel"%>
<%@ Import Namespace ="System.Collections.Generic" %>
<%@ Import Namespace ="DSLFactory.Candle.Repository" %>

<script runat="server">
    /// <summary>
    /// Magouille pour pouvoir executer un object com en STA (aspCompat)
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {
        try
        {
            Context.Response.ContentType = "image/svg+xml";

            List<DependencyGraphVisitor.RelationShip> relations = CandleRepositoryController.Instance.GetAllRelations();
            DotGraphGenerator generator = new DotGraphGenerator(relations);

            string id = Context.Request.QueryString["id"];
            string version = Context.Request.QueryString["version"];
            if (id != null && version != null)
            {
                ComponentModelMetadata metadata = CandleRepositoryController.Instance.GetMetadata(id, version);
                if (metadata != null)
                {
                    generator.Generate( new ComponentSig(metadata));
                }
            }
            else
                generator.Generate();
            
            generator.SaveGraph(Context);
        }
        catch (Exception ex)
        {
            Context.Trace.Warn("Repository", "DependenciesGraph", ex);
        }
        Response.End();
    }
</script>

