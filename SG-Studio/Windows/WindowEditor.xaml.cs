using MahApps.Metro.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SG_Studio
{
    public partial class Editor : MetroWindow
    {
        Classes.GUI.SG_UI_Window currentWindow = null;
        public Editor()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (SG_Studio.Classes.GUI.SG_UI_Element uiItem in SgStudioProject.ui)
            {
                listBox.Items.Add(uiItem.ElementID);
            }      
        }

        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            foreach (Classes.GUI.SG_UI_Element item in SgStudioProject.ui)
            {
                if (item.ElementID == (string)listBox.SelectedValue)
                {
                    currentWindow = (Classes.GUI.SG_UI_Window)item;
                }
            }
            textBox.Text = string.Join(Environment.NewLine, currentWindow.Code);
            RenderWindow();
        }
        private void RenderWindow()
        {
            if (currentWindow != null)
            {
                System.Drawing.Image img = currentWindow.GenerateElementImage();

                //Put in imagebox
                var bi = new BitmapImage();
                using (var ms = new System.IO.MemoryStream())
                {
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    ms.Position = 0;

                    bi.BeginInit();
                    bi.CacheOption = BitmapCacheOption.OnLoad;
                    bi.StreamSource = ms;
                    bi.EndInit();
                }
                image.Source = bi;
            }
        }
    }
}