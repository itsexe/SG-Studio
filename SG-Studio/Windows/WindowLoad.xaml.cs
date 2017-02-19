using MahApps.Metro.Controls;
using System;
using System.Windows;


namespace SG_Studio
{
    /// <summary>
    /// Interaktionslogik für WindowLoad.xaml
    /// </summary>
    public partial class WindowLoad : MetroWindow
    {
        public delegate void EventDelegate();
        public event EventDelegate CancelEvent;
        public WindowLoad()
        {
            InitializeComponent();
        }
        public void UpdateProgress(int percentage, string filename)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                CurrentFile.Content = filename;
                pb.Value = percentage;
            }));
        }

        /// <summary>
        /// Cancel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            CancelEvent();
        }
    }
}
