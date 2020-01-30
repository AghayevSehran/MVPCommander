using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace MVPCommander
{
    public class MVPStubCS : IMVPStub
    {

        private const string NAME = "<|NAME|>";
        private const string EVENT_WIREUPS = "<|EVENT_WIREUPS|>";
        private const string EVENT_NAME = "<|EVENT_NAME|>";
        private const string EVENT_METHODS = "<|EVENT_METHODS|>";
        private const string INTERFACE_EVENTS = "<|INTERFACE_EVENTS|>";

        #region Code
        private const string EVENT_WIREUP = @"
        this.mView.<|EVENT_NAME|> += new EventHandler<EventArgs>(mView_<|EVENT_NAME|>);
";
        private const string EVENT_METHOD = @"
    private void mView_<|EVENT_NAME|>(object sender, EventArgs e)
    {
        try 
        {	        
            
	    }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
            throw;
        }
    }
";

        private const string INTERFACE_EVENT = @"
    event EventHandler<EventArgs> <|EVENT_NAME|>;
            ";

        private const string CODE_TEMPLATE = @"
public interface I<|NAME|>View
{
<|INTERFACE_EVENTS|>
}

public class <|NAME|>Presenter
{
    private I<|NAME|>View mView;

    public <|NAME|>Presenter(I<|NAME|>View view)
    {
        this.mView = view;
        this.Initialize();
    }

    private void Initialize()
    {
<|EVENT_WIREUPS|>
    }

<|EVENT_METHODS|>
}
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

        public MVPStubCS() { }

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
