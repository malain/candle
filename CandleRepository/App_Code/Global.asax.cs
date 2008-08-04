

public class Global_asax : System.Web.HttpApplication
{
    public override void Init()
    {
        base.Init();
        this.BeginRequest += new System.EventHandler(Global_asax_BeginRequest);

        DSLFactory.Candle.SystemModel.ServiceLocator.Instance.AddService(typeof(DSLFactory.Candle.SystemModel.ILogger), new DSLFactory.Candle.Repository.Logger());
        DSLFactory.Candle.SystemModel.ServiceLocator.Instance.AddService(typeof(DSLFactory.Candle.SystemModel.IRepositorySettingsStorage), new DSLFactory.Candle.SystemModel.Config.Web.WebRepositorySettingsStorage());
    }

    void Global_asax_BeginRequest(object sender, System.EventArgs e)
    {
        DSLFactory.Candle.SystemModel.ILogger logger = DSLFactory.Candle.SystemModel.ServiceLocator.Instance.GetService<DSLFactory.Candle.SystemModel.ILogger>();
        if (logger != null && logger is DSLFactory.Candle.Repository.Logger)
        {
            ((DSLFactory.Candle.Repository.Logger)logger).SetContext(this.Context);
        }
    }
}

