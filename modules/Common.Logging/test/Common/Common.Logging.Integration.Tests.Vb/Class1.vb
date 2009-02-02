Imports Common.Logging
Imports NUnit.Framework

<TestFixture()> _
Public Class Class1

    <Test()> _
    Public Sub CanCompile()
        Dim log As Common.Logging.ILog = LogManager.GetCurrentClassLogger()
        log.Trace(Function(m) m("test {0}", "test"))
    End Sub
End Class
