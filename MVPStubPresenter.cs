using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace MVPCommander
{

    public interface IMVPStubView
    {
        string FeatureName { get; }
        CLRLanguages Language { get; }
        string Code { set; }
        string[] FeatureEvents { get; }
        event EventHandler<EventArgs> Generate;
    }

    public class MVPStubPresenter
    {
        private IMVPStubView mView;

        public MVPStubPresenter(IMVPStubView view)
        {
            this.mView = view;
            this.Initialize();
        }

        private void Initialize()
        {
            this.mView.Generate += new EventHandler<EventArgs>(mView_Generate);
        }

        void mView_Generate(object sender, EventArgs e)
        {
            IMVPStub stub;

            try
            {
                if (this.mView.Language == CLRLanguages.CSharp)
                {
                    stub = new MVPStubCS();
                }
                else
                {
                    stub = new MVPStubVB();
                }

                stub.FeatureName = this.mView.FeatureName;
                stub.Events = this.mView.FeatureEvents;
                stub.Generate();
                this.mView.Code = stub.Code;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
