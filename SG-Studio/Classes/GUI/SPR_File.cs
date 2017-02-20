using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Studio.Classes.GUI
{
    class SPR_File
    {
        public Dictionary<string ,SPR_Image> imgList = new Dictionary<string, SPR_Image>();
        public SPR_File(string filepath)
        {
            var lines = System.IO.File.ReadAllLines(filepath);
            int currentline = 0;   
            
            while (currentline <= lines.Count() - 1)
            {
                var currentSpr = new SPR_Image();
                currentSpr.imageName = lines[currentline];
                currentline++;
                var ImageListCount = int.Parse(lines[currentline]);
                currentline++;
                for (int i = 0; i < ImageListCount; i++)
                {
                    var sprName = lines[currentline];
                    currentline++;
                    Size imgSize = new Size(int.Parse(lines[currentline].Split(',')[0]), int.Parse(lines[currentline].Split(',')[1]));
                    currentline++;
                    int uk = int.Parse(lines[currentline]);
                    currentline++;
                    currentSpr.files.Add(new SPR_Image_Part(sprName,imgSize,uk));
                }
                imgList.Add(currentSpr.imageName, currentSpr);
            }
        }
    }
}
