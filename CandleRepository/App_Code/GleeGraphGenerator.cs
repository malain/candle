//using System;
//using System.Data;
//using System.Configuration;
//using System.Web;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Web.UI.HtmlControls;
//using System.Collections.Generic;
//using DSLFactory.Candle.SystemModel.Dependencies;
//using Microsoft.Glee.Drawing;
//using Microsoft.Glee.GraphViewerGdi;
//using System.Drawing;

///// <summary>
///// Summary description for GleeGraphGenerator
///// </summary>
//public class GleeGraphGenerator : IGraphGenerator
//{
//    private List<DependencyGraphVisitor.RelationShip> relations;

//    public GleeGraphGenerator(List<DependencyGraphVisitor.RelationShip> relations)
//    {
//        this.relations = relations;
//    }

//    #region IGraphGenerator Members

//    public void Create(HttpContext context, DotGraphGenerator.RelationSide side)
//    {
//        Graph g = new Graph("Dependencies");

//        foreach (DependencyGraphVisitor.RelationShip relation in relations)
//        {
//            string nodeId = relation.Source.Id.ToString() + relation.Source.Version.ToString();
//            Node source = g.AddNode(nodeId);
//            source.Attr.Label = relation.Source.Name + "V" + relation.Source.Version.ToString();

//            Microsoft.Glee.Drawing.Style style = Microsoft.Glee.Drawing.Style.Solid;
//            Node target=null;
//            switch (relation.Type)
//            {
//                case DependencyGraphVisitor.RelationShip.RelationType.Framework:
//                     nodeId = "FWK" + relation.TargetAsString;
//                    target = g.AddNode(nodeId);
//                        target.Attr.Label = "Framework V" + relation.TargetAsString;
//                        target.Attr.Shape = Shape.Box;
//                        style = Microsoft.Glee.Drawing.Style.Dashed; 
//                    break;

//                case DependencyGraphVisitor.RelationShip.RelationType.Composants:
//                     nodeId = relation.Target.Id.ToString() + relation.Target.Version.ToString();
//                    target = g.AddNode(nodeId);
//                    target.Attr.Label = relation.Target.Name + "V" + relation.Target.Version.ToString();
//                    break;

//                case DependencyGraphVisitor.RelationShip.RelationType.Artifacts:
//                     nodeId = relation.TargetAsString;
//                    target = g.AddNode(nodeId);
//                    target.Attr.Label = nodeId;
//                    break;
//            }

//            Edge edge = g.AddEdge(source.Id, target.Id);
//            edge.Attr.AddStyle(style);
//        }

//        GraphRenderer render = new GraphRenderer(g);
//        System.Drawing.Image image = new Bitmap(700,600);
//        render.Render(image);
//        context.Response.ContentType = "image/png";
//        image.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Png);
//    }

//    #endregion
//}
