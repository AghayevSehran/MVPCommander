using System;

namespace MVPCommander
{
    public class MVPStubView : IMVPStubView
    {
        private readonly string mFeatureName = string.Empty;
        private readonly CLRLanguages mLanguage = CLRLanguages.CSharp;
        private string mCode = string.Empty;
        private readonly string[] mFeatureEvents = null;

        public string FeatureName => mFeatureName;

        public CLRLanguages Language => mLanguage;

        public string Code
        {
            get => mCode;
            set => mCode = value;
        }

        public string[] FeatureEvents => mFeatureEvents;

        public event EventHandler<EventArgs> Generate;

        public void Gen()
        {
            if (Generate != null)
            {
                Generate(this, EventArgs.Empty);
            }
        }

        public MVPStubView(string featureName, string[] featureEvents, CLRLanguages langugage)
        {
            mFeatureName = featureName;
            mFeatureEvents = featureEvents;
            mLanguage = langugage;
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            string featureName;
            string[] featureEvents;
            CLRLanguages langugage;
            MVPStubPresenter presenter;
            MVPStubView view;

            try
            {
                if (args.Length > 1)
                {
                    featureName = args[0];
                    featureEvents = args[1].Split(';');

                    if (args.Length > 2)
                    {
                        langugage = args[2].ToLower().Equals("c#") ? CLRLanguages.CSharp : CLRLanguages.VBNET;
                    }
                    else
                    {
                        langugage = CLRLanguages.CSharp;
                    }

                    view = new MVPStubView(featureName, featureEvents, langugage);
                    presenter = new MVPStubPresenter(view);

                    view.Gen();

                    Console.WriteLine(view.Code);
                }
            }
            catch (Exception ex)
            {
                ShowException(ex);
            }
        }

        private static void ShowException(Exception ex)
        {
            Console.WriteLine(ex.Message);

            if (ex.InnerException != null)
            {
                Console.WriteLine(ex.InnerException.Message);
            }
        }
    }
}
