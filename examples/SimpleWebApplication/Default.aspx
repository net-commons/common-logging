<%@ Page Language="C#" %>
<script language="c#" runat="server">
    private readonly Common.Logging.ILog log = Common.Logging.LogManager.GetCurrentClassLogger();

    protected override void OnInit(EventArgs ea)
    {
        base.OnInit(ea);
        log.DebugFormat("Initialized Page for Url '{0}'", this.Request.RawUrl);
    }
</script>

<h1>successfully executed page</h1>
<p>
to see the logging output, either start this web within the debugger or 
use the DebugView tool from <a href="http://technet.microsoft.com/en-us/sysinternals/bb896647.aspx" target="_blank">www.sysinternals.com</a>
</p>