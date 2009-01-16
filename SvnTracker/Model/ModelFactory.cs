using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Markup;

namespace SvnTracker.Model
{
    public class ModelFactory
    {
        private static ModelFactory factory;
        public static ModelFactory Instance { 
            get
            {
                if (factory == null)
                {
                    try
                    {
                        Loading = true;
                        factory = (ModelFactory) XamlReader.Load(new FileStream(ConfigPath, FileMode.Open));
                        Loading = false;
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                    }
                }

                if (factory == null)
                {
                    factory = new ModelFactory
                                  {
                                      SvnDir = @"c:\Program Files\CollabNet Subversion Server",
                                      PollIntervalInSeconds = 10,
                                  };
                }
                if (!factory.Models.Any())
                {
                    factory.Models.Add(new HomeModel());
                }
                return factory;
            } 
        }

        public ModelFactory()
        {
            Models = new ObservableCollection<DirModel>();
        }

        public ObservableCollection<DirModel> Models { get; set; }

        private string m_SvnDir;
        public string SvnDir
        {
            get { return m_SvnDir; }
            set { m_SvnDir = value; Save(); }
        }

        private int m_PollIntervalInSeconds;
        public int PollIntervalInSeconds
        {
            get { return m_PollIntervalInSeconds; }
            set { m_PollIntervalInSeconds = value; Save(); }
        }

        public void Save()
        {
            if (Loading)
                return;
            try
            {
                XamlWriter.Save(this, new FileStream(ConfigPath, FileMode.Create));
            } catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private static string ConfigPath
        {
            get
            {
                return typeof(ModelFactory).Assembly.Location + ".xaml";
            }
        }

        internal static bool Loading { get; set; }
    }

    public class HomeModel : DirModel
    {
        public override Visibility ClosableVisibility { get { return Visibility.Collapsed; } }

        public override string URLBase
        {
            get
            {
                return "SvnMon";
            }
        }
    }
}
