using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using SvnTracker.Model;
using SvnTracker.View;

namespace SvnTracker
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1
    {
//        public static DirModel dirModel;
        public static Window1 Instance;

        public Window1()
        {
            Instance = this;
            InitializeComponent();

            foreach(var model in ModelFactory.Instance.Models)
            {
                Add(model);
            }

            notifyIcon.Icon = Properties.Resources.outstanding0;
            notifyIcon.Visibility = Visibility.Hidden;            
            notifyIcon.Visibility = Visibility.Visible;
        }

        public void Add(DirModel dirModel2)
        {
            dirModel2.CheckinOccured += model_Changed;
            dirModel2.ConflictDetected += model_ConflictDetected;
        }

        void model_ConflictDetected(SvnPath filename, string file, WatcherChangeTypes changeType)
        {
            notifyIcon.ShowBalloonTip(10 * 1000, "Conflict warning!", file + " was " + filename.Action + " by " + filename.LogEntry.User + " for:\r\n"  + filename.LogEntry.Message, BalloonTipIcon.Warning);
        }

        void model_Changed(IList<IChange> changes)
        {
            notifyIcon.Icon = GetIcon(changes.Count);
            
            int countOfDistinctFilesTouched = changes.SelectMany(change => change.Files)
                .Select(path => path.Path).Distinct().Count();

            if (!changes.Any())
            {
                return;
            }

            IChange latestChange = changes[changes.Count - 1];
            string title = latestChange.User + "'s checked in" + sizeOf(latestChange.Files.Count) + ".";
            string text = latestChange.Message +
                          "\r\nYou have " + SayPlural(changes.Count, " pending change")
                          + ", touching " + SayPlural(countOfDistinctFilesTouched, " file") + ".";
            notifyIcon.ShowBalloonTip(10*1000, title, text, BalloonTipIcon.Info);
        }

        private static Icon GetIcon(int count)
        {
            switch (count)
            {
                case 0: return Properties.Resources.outstanding0;
                case 1: return Properties.Resources.outstanding1;
                case 2: return Properties.Resources.outstanding2;
                case 3: return Properties.Resources.outstanding3;
                case 4: return Properties.Resources.outstanding4;
                default: return Properties.Resources.outstanding5;
            }            
        }

        private static string sizeOf(int count)
        {
            if (count > 50) return ". Monster.";
            if (count > 20) return " a whopper.";
            if (count < 5) return " a swift one.";
            return "";
        }

        public string SayPlural(int count, string text)
        {
            return count + text + (count > 1 ? "s" : "");
        }

        private void OnOpenClick(object sender, RoutedEventArgs e)
        {
            Show();
        }

        private void OnExitClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnNotifyIconDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                Show();
            }
        }

        private void CloseTab(object sender, RoutedEventArgs e)
        {
            var model = GetModel(sender);
            model.CheckinOccured -= model_Changed;
            model.ConflictDetected -= model_ConflictDetected;
            model.Dispose();
            ModelFactory.Instance.Models.Remove(model);
            ModelFactory.Instance.Save();
        }

        public DirModel GetModel(object sender)
        {
            return (DirModel) ((FrameworkElement) sender).Tag;
        }
    }
}
