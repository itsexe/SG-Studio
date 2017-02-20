using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Studio.Classes.GUI
{
    class SPR_Image
    {
        public string imageName { get; set; }
        public List<SPR_Image_Part> files = new List<SPR_Image_Part>();
    }
    class SPR_Image_Part
    {
        public string ImageFileName { get; set;}
        public Size size { get; set; }
        public int uk { get; set; }
        public SPR_Image_Part(string filename, Size size, int uk)
        {
            ImageFileName = filename;
            this.size = size;
            this.uk = uk;
        }
        public Image GetImagePart()
        {
            if (ImageFileName.EndsWith("dds"))
            {
                if (System.IO.File.Exists(SgStudioProject.rootDir + "\\dds\\" + ImageFileName))
                {
                    return new KUtility.DDSImage(System.IO.File.ReadAllBytes(SgStudioProject.rootDir + "\\dds\\" + ImageFileName)).images[0];
                }
            }
            else if (ImageFileName.EndsWith("tga"))
            {
                if (System.IO.File.Exists(string.Format("{0}\\tga\\{1}({2}).tga", SgStudioProject.rootDir, ImageFileName.Replace(".tga", ""), SgStudioProject.locale)))
                {
                    return Paloma.TargaImage.LoadTargaImage(SgStudioProject.rootDir + "\\tga\\" + ImageFileName.Replace(".tga", "") + "(cp1141)" + ".tga");
                }
                else if (System.IO.File.Exists(SgStudioProject.rootDir + "\\tga\\" + ImageFileName))
                {
                    return Paloma.TargaImage.LoadTargaImage(SgStudioProject.rootDir + "\\tga\\" + ImageFileName);
                }
            }
            System.Diagnostics.Debug.WriteLine("File not found " + ImageFileName);
            return new Bitmap(1, 1);
        }
    }
}

