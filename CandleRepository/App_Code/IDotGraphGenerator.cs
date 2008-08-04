using System;
using DSLFactory.Candle.SystemModel;

public interface IGraphGenerator
{
    void SaveGraph(System.Web.HttpContext Context);

    void Generate();

    void Generate(ComponentSig sig);
}
