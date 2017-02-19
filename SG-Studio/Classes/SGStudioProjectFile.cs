using System.Xml;

namespace SG_Studio.Classes
{
    class SGStudioProjectFile
    {
        public string FilePath { get; set; }
        public string ProjectName { get; set; }

        public SGStudioProjectFile(string filePath)
        {
            FilePath = filePath;
            LoadConfig();
        }
        public SGStudioProjectFile(string filePath, string projectName)
        {
            FilePath = filePath;
            ProjectName = projectName;
            SaveConfig();
        }
        private void SaveConfig()
        {
            using(XmlWriter writer = XmlWriter.Create(FilePath))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Project");
                writer.WriteElementString("Name", ProjectName);
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }
        private void LoadConfig()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(System.IO.File.ReadAllText(FilePath));
            ProjectName = doc.SelectSingleNode("//Project/Name").Value;
        }
    }
}
