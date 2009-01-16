using System.Windows;
using System.Windows.Forms;
using SvnTracker.Model;

namespace SvnTracker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog
                             {
                                 ShowNewFolderButton = false,
                                 Description = "Select a subversion checked out dir",
                                 SelectedPath = @"c:\checkout1\example\trunk"
                             };
            dialog.ShowDialog();
            var dirModel = new DirModel { MonitoredDir = dialog.SelectedPath };
            Window1.Instance.Add(dirModel);

            ModelFactory.Instance.Models.Add(dirModel);
            ModelFactory.Instance.Save();
        }
    }
}
