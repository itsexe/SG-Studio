using MahApps.Metro.Controls;
using System;
using System.Windows;
using Microsoft.Win32;

namespace SG_Studio
{
    public partial class StartupWindow : MetroWindow
    {
        public StartupWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Opens a new project
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenProject(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "SG Studio Project File (*.sspf)|*.sspf";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                SgStudioProject.LoadFromFile(openFileDialog.FileName);
                Close();
            }
        }
        /// <summary>
        /// Creates a new Project
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewProject(object sender, RoutedEventArgs e)
        {
            SG_Studio.WindowCreateProject formCreateProject = new SG_Studio.WindowCreateProject();
            if (formCreateProject.ShowDialog() == true)
            {
                Hide();
                SgStudioProject.CreateProjectFromClient(formCreateProject.ClientPath, formCreateProject.ProjectPath, formCreateProject.Projectname);
                Close();
            }

            
        }

    }
}