using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;


namespace SG_Studio
{
    static class SgStudioProject
    {
        private static BackgroundWorker _bgUnpack = new BackgroundWorker();
        private static WindowLoad  _formLoad = new WindowLoad();
        private static Classes.SGStudioProjectFile projectFile;
        internal static List<Classes.GUI.SG_UI_Element> ui = new List<Classes.GUI.SG_UI_Element>();
        internal static Dictionary<string, Classes.GUI.SPR_File> spr = new Dictionary<string, Classes.GUI.SPR_File>();
        internal static string rootDir;
        internal static string locale = "cp1141";
        internal static void LoadFromFile(string path)
        {
            rootDir = System.IO.Path.GetDirectoryName(path);
            projectFile = new Classes.SGStudioProjectFile(path);
            OpenEditor();
        }
        /// <summary>
        /// Creates a new project
        /// </summary>
        /// <param name="clientPath">Path to the folder containg the res files</param>
        /// <param name="projectRootDir">Path where to save the files</param>
        /// <param name="projectName">Project name</param>
        internal static void CreateProjectFromClient(string clientPath, string projectRootDir, string projectName)
        {

            if (_bgUnpack.IsBusy == false)
            {
                //Show Load Window
                _formLoad = new WindowLoad();
                _formLoad.Show();
                _formLoad.CancelEvent += CancelLoad;
                _bgUnpack.WorkerSupportsCancellation = true;
                _bgUnpack.WorkerReportsProgress = true;
                _bgUnpack.ProgressChanged += new ProgressChangedEventHandler(LoadDataProgress);
                _bgUnpack.DoWork += new DoWorkEventHandler(LoadData);
                _bgUnpack.RunWorkerCompleted += LoadDataFinished;
                _bgUnpack.RunWorkerAsync(new Tuple<string, string, string>(clientPath,projectRootDir,projectName));
            }
        }
        /// <summary>
        /// Extracts the client and creates the project file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void LoadData(object sender, DoWorkEventArgs e)
        {
     Tuple<string, string, string> t = (Tuple<string, string, string>)e.Argument;

                //Remove Illegal Characters from Project Name
                string regexSearch = new string(System.IO.Path.GetInvalidFileNameChars()) + new string(System.IO.Path.GetInvalidPathChars());
                Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
                string savePath = t.Item2 + @"\" + r.Replace(t.Item3, "");

            rootDir = savePath;
                //Create Directories
                if (System.IO.Directory.Exists(t.Item2))
                    System.IO.Directory.CreateDirectory(t.Item2);
                if (System.IO.Directory.Exists(savePath))
                    System.IO.Directory.CreateDirectory(savePath);

                //Extract Client Data
                SG_ResManagerLib.Program fileManager = new SG_ResManagerLib.Program();
                var contents = fileManager.ReadContents(t.Item1 + @"\res.000");
                foreach (SG_ResManagerLib.SgFile file in contents)
                {
                    if (_bgUnpack.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                    
                        

                    if (!System.IO.Directory.Exists(savePath + @"\" + System.IO.Path.GetExtension(file.FileName).Replace(".", "")))
                        System.IO.Directory.CreateDirectory(savePath + @"\" + System.IO.Path.GetExtension(file.FileName).Replace(".", ""));

                    System.IO.File.WriteAllBytes(savePath + @"\" + System.IO.Path.GetExtension(file.FileName).Replace(".", "") + @"\" + file.FileName, fileManager.GetFile(t.Item1, file));
                    _bgUnpack.ReportProgress((int)((float)contents.IndexOf(file) / (float)(contents.Count() - 1) * 100), file.FileName);
                }

            //Create sspf 
            projectFile = new Classes.SGStudioProjectFile(savePath + @"\" + t.Item3 + ".sspf", t.Item3);
        }
        /// <summary>
        /// Progress of LoadData
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void LoadDataProgress(object sender, ProgressChangedEventArgs e)
        {
           _formLoad.UpdateProgress(e.ProgressPercentage, (string)e.UserState);
        }
        /// <summary>
        /// Cancel Loading
        /// </summary>
        private static void CancelLoad()
        {
            if (_bgUnpack.IsBusy)
                _bgUnpack.CancelAsync();
        }
        /// <summary>
        /// Gets called when _bgLoadData is finished
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void LoadDataFinished(object sender, RunWorkerCompletedEventArgs e)
        {
            _formLoad.Close();
            if (e.Cancelled == false)
            {
                OpenEditor();
            }else
            {
                StartupWindow sw = new StartupWindow();
                sw.Show();
            }
        }

        /// <summary>
        /// Opens the editor form
        /// </summary>
        /// <param name="sspf">Street Gears Project File</param>
        private static void OpenEditor()
        {
            LoadUI();
            Editor formEditor = new Editor();
            formEditor.Show();
            
        }

        private static void LoadUI()
        {
            ui.Clear();

            //nui
            foreach (string nuiPath in System.IO.Directory.GetFiles(rootDir + "\\nui"))
            {
                ui.Add(new SG_Studio.Classes.GUI.SG_UI_Window(nuiPath));
            }

            //spr
            foreach (string sprPath in System.IO.Directory.GetFiles(rootDir + "\\spr"))
            {
                if (spr.ContainsKey(System.IO.Path.GetFileName(sprPath)))
                    continue;
                spr.Add(System.IO.Path.GetFileName(sprPath), new Classes.GUI.SPR_File(sprPath));
            }
        }
    }
}
