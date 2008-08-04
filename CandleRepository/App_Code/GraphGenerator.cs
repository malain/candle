using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using DSLFactory.Candle.SystemModel.Dependencies;
using System.IO;
using DSLFactory.Candle.SystemModel;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// Summary description for GraphGenerator
/// </summary>
public class DotGraphGenerator : IGraphGenerator
{
    public enum GraphRelationSide
    {
        Source,
        Target
    }

    private List<DependencyGraphVisitor.RelationShip> relations;
    // Stockage des identifiants des modèles avec leur id
    Dictionary<string, int> idMappings;
    int idCnt;
    BitArray processedLinks;
    string mainSource;
    private StringBuilder buffer;
    private bool showMainSource=true;

    public DotGraphGenerator(List<DependencyGraphVisitor.RelationShip> relations)
    {
        this.relations = relations;
        // Stockage des identifiants des modèles avec leur id
        idMappings = new Dictionary<string, int>();
        idCnt = 0;
        buffer = new StringBuilder(@"digraph g {
center=true;
fontname=""verdana"";
fontsize=10;
node[fontname=""verdana""; fontsize=12];
");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public void SaveGraph(HttpContext context)
    {
        buffer.AppendLine("}");

        WINGRAPHVIZLib.DOTClass parser = new WINGRAPHVIZLib.DOTClass();
        string result = parser.ToSvg(buffer.ToString());
        context.Response.Output.Write(result);
    }

    /// <summary>
    /// Generation pour toute les relations
    /// </summary>
    public void Generate()
    {
        showMainSource = false;

        for (int ix = 0; ix < relations.Count; ix++)
        {
            DependencyGraphVisitor.RelationShip relation = relations[ix];
            GenerateRelationInGraph(relation);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="side"></param>
    public void Generate(ComponentSig mainComponentSig)
    {
        if (relations.Count == 0 || mainComponentSig.IsEmpty)
            return;

        processedLinks = new BitArray(relations.Count);
        showMainSource = true;
        TraverseRelations(mainComponentSig, GraphRelationSide.Source);
        TraverseRelations(mainComponentSig, GraphRelationSide.Target);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="startingModelId"></param>
    /// <param name="side"></param>
    private void TraverseRelations(ComponentSig startingModel, GraphRelationSide side)
    {
        for (int ix = 0; ix < relations.Count; ix++)
        {
            if (processedLinks[ix])
                continue;

            DependencyGraphVisitor.RelationShip relation = relations[ix];

            if (startingModel.Equals(relation.Source) && side == GraphRelationSide.Source)
            {
                processedLinks.Set(ix, true);
                GenerateRelationInGraph(relation);
                if( relation.Target != null )
                    TraverseRelations(new ComponentSig(relation.Target), side);
            }
            else if (relation.Target != null && startingModel.Equals(relation.Target) && side == GraphRelationSide.Target)
            {
                processedLinks.Set(ix, true);
                GenerateRelationInGraph(relation);
                TraverseRelations( new ComponentSig( relation.Source ), side);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="relation"></param>
    protected virtual void GenerateRelationInGraph(DependencyGraphVisitor.RelationShip relation)
    {
        bool sourceExists;
        string sourceId = GetId(relation.Source, null, out sourceExists);
        bool targetExists;
        string targetId = GetId(relation.Target, relation.TargetAsString, out targetExists);

        string source = null;
        string target = null;
        string edgeStyle = String.Empty;

        if (mainSource == null)
        {
            buffer.AppendLine(String.Format(@"label=""Dependencies graph for\n {0} V{1}""", relation.Source.Name, relation.Source.Version));
        }

        if( mainSource == null && showMainSource)
            source = mainSource = String.Format(@"{0}[shape=box, style=filled,fillcolor=gray, label=""{1}\nV{2}""];", sourceId, relation.Source.Name, relation.Source.Version);        
        else if (!sourceExists)
            source = String.Format(@"{0}[URL=""Details.aspx?id={3}&version={2}"", label=""{1}\nV{2}""];", sourceId, relation.Source.Name, relation.Source.Version, relation.Source.Id);

        switch (relation.Type)
        {
            case DependencyGraphVisitor.RelationShip.RelationType.Framework:
                if (!targetExists)
                    target = String.Format(@"{0}[shape=box,label="".Net\n{1}"",fontsize=12];", targetId, relation.TargetAsString);
                edgeStyle = @"[arrowhead=""none""]";
                break;
            case DependencyGraphVisitor.RelationShip.RelationType.Composants:
                if (!targetExists)
                {
                    target = String.Format(@"{0} [URL=""Details.aspx?id={2}&version={1}"", label=""{3}\nV{1}""];", targetId, relation.Target.Version, relation.Target.Id, relation.Target.Name);
                }
                if (relation.Scope == ReferenceScope.Compilation)
                    edgeStyle = @"[style=""bold""]";
                break;
            case DependencyGraphVisitor.RelationShip.RelationType.Artifacts:
                if (!targetExists)
                    target = String.Format(@"{0}[style=dotted,label=""{1}"",fontsize=12];", targetId, relation.TargetAsString);
                break;
            default:
                break;
        }

        if (source != null)
            buffer.AppendLine(source);
        if (target != null)
            buffer.AppendLine(target);
        buffer.AppendFormat("{0}->{1}{2}", sourceId, targetId, edgeStyle);
        buffer.AppendLine();
    }

    protected string GetId(CandleModel model, string txt, out bool exists)
    {
        exists = false;
        string id = txt;
        if (model != null)
            id = model.Id + model.Version.ToString();
        int cx;
        if (idMappings.TryGetValue(id, out cx))
        {
            exists = true;
            return cx.ToString();
        }
        idCnt++;
        idMappings.Add(id, idCnt);
        return idCnt.ToString();
    }
}
