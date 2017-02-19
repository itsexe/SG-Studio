using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TAlex.WPF.CommonDialogs;

namespace SG_Studio
{
    /// <summary>
    /// Interaktionslogik für WindowCreateProject.xaml
    /// </summary>
    public partial class WindowCreateProject : MetroWindow
    {
        public string ProjectPath { get; set; }
        public string ClientPath { get; set; }
        public string Projectname { get; set; }
        public WindowCreateProject()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Creates a new folder browser dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenDir(object sender, MouseButtonEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == true)
            {
                TextBox txtbx = (TextBox)sender;
                txtbx.Text = folderBrowserDialog.SelectedPath;
            }
        }
        /// <summary>
        /// Cancel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
        /// <summary>
        /// Create
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCreate_Click(object sender, RoutedEventArgs e)
        {
            ProjectPath = textBox_ProjectDir.Text;
            ClientPath = textBox_ClientDir.Text;
            Projectname = textBox_ProjectName.Text;
            DialogResult = true;
            Close();
        }
    }
}
