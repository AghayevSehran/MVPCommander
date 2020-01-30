using System;
using System.Collections.Generic;
using System.Text;

namespace MVPCommander
{
    public interface IMVPStub
    {
        string Code { get; }
        string[] Events { set; }
        string FeatureName { set; }
        void Generate();
    }
}
