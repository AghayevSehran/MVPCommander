using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace MVPCommander
{
    public class MVPStubVB : IMVPStub
    {

        private const string NAME = "<|NAME|>";
        private const string EVENT_WIREUPS = "<|EVENT_WIREUPS|>";
        private const string EVENT_NAME = "<|EVENT_NAME|>";
        private const string EVENT_METHODS = "<|EVENT_METHODS|>";
        private const string INTERFACE_EVENTS = "<|INTERFACE_EVENTS|>";

        #region Code
        private const string EVENT_WIREUP = @"
        AddHandler Me.mView.<|EVENT_NAME|>, AddressOf mView_<|EVENT_NAME|>
";

        private const string INTERFACE_EVENT = @"
    Event <|EVENT_NAME|> As EventHandler(Of EventArgs)
            ";

        private const string EVENT_METHOD = @"
    Private Sub mView_<|EVENT_NAME|>(sender as object, e as EventArgs)
        Try         
            
        Catch(ex as Exception)
            System.Diagnostics.Debug.WriteLine(ex.Message)
            Throw
        End Try
    End Sub
";

        private const string CODE_TEMPLATE = @"
Public Interface I<|NAME|>View
<|INTERFACE_EVENTS|>
End Interface

Public Class <|NAME|>Presenter

    Private mView as I<|NAME|>View

    Public Sub New(view as I<|NAME|>View)
        Me.mView = view
        Me.Initialize()
    End Sub

    Private Sub Initialize()
<|EVENT_WIREUPS|>
    End Sub

<|EVENT_METHODS|>
End Class
";
        #endregion

        private string[] mEvents = null;
        private string mFeatureName = string.Empty;
        private string mCode = string.Empty;

        public string[] Events
        {
            set { this.mEvents = value; }
        }

        public string FeatureName
        {
            set { this.mFeatureName = value; }
        }

        public string Code
        {
            get { return this.mCode; }
        }

        public MVPStubVB() { }

        public void Generate()
        {
            StringBuilder eventWireUps;
            StringBuilder eventMethods;
            StringBuilder interfaceEvents;
            string name;

            try
            {
                eventMethods = new StringBuilder();
                eventWireUps = new StringBuilder();
                interfaceEvents = new StringBuilder();

                this.mCode = CODE_TEMPLATE.Replace(NAME, this.mFeatureName);

                foreach (string eventName in this.mEvents)
                {
                    name = eventName.Trim();
                    eventWireUps.Append(EVENT_WIREUP.Replace(EVENT_NAME, name));
                    eventMethods.Append(EVENT_METHOD.Replace(EVENT_NAME, name));
                    eventMethods.AppendLine();
                    interfaceEvents.Append(INTERFACE_EVENT.Replace(EVENT_NAME, name));
                }

                this.mCode = this.mCode.Replace(EVENT_METHODS, eventMethods.ToString());
                this.mCode = this.mCode.Replace(EVENT_WIREUPS, eventWireUps.ToString());
                this.mCode = this.mCode.Replace(INTERFACE_EVENTS, interfaceEvents.ToString());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }
    }


}
