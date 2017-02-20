using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG_Studio.Classes.GUI
{
    public class SG_UI_Element
    {
        public enum ElementType
        {
            SimpleButton,
            SimpleCheck,
            Button,
            Check,
            Edit,
            Static,
            Window,
            none
        }

        public ElementType elementType { get; set; }
        public string Code { get; set; }
        public SG_UI_Window ChildOf { get; set; }
        public string ElementID { get; set; }
        public Rectangle Rect { get; set; }
        public string Ani { get; set; }
        public string Spr { get; set; }
        public string Caption { get; set; }
        public List<SG_UI_Element> Elements = new List<SG_UI_Element>();
        public Image GenerateElementImage()
        {
            if(ChildOf == null)
            {
                //Parent Element (Window)
                Image img = new Bitmap(Rect.Width, Rect.Height);
                Graphics drawing = Graphics.FromImage(img);
                foreach (SG_UI_Element childElement in Elements)
                {
                    drawing.DrawImage(childElement.GenerateElementImage(),0, 0);         
                }
                 return img;
            }
            else
            {
                //Child Element (Button, Check...)
                Image img = new Bitmap(ChildOf.Rect.Width, ChildOf.Rect.Height);
                if (Spr != null && Ani != null)
                {
                    if (SgStudioProject.spr.ContainsKey(Spr.ToLower()) && (SgStudioProject.spr[Spr.ToLower()].imgList.ContainsKey(Ani.ToLower())))
                    {
                        Graphics drawing = Graphics.FromImage(img);
                        drawing.DrawImage(GetImage(SgStudioProject.spr[Spr.ToLower()].imgList[Ani.ToLower()].files), Rect.X, Rect.Y);

                    }
                }
                return img;
            }
        }
        /// <summary>
        /// returns the complete image of the control
        /// Some Control Types (Buttons, Static etc.) have their background saved in multiple files
        /// </summary>
        /// <param name="imgList"></param>
        /// <returns></returns>
        private Image GetImage(List<SPR_Image_Part> imgList)
        {
            if (Rect.Width == -1)
                Rect = new Rectangle(Rect.X, Rect.Y, ChildOf.Rect.Width, Rect.Height);
            if (Rect.Height == -1)
                Rect = new Rectangle(Rect.X, Rect.Y, Rect.Width, ChildOf.Rect.Height);

            Size size = new Size((Rect.Width - Rect.X),(Rect.Height - Rect.Y));
            Image img = new Bitmap(size.Width, size.Height);
            if (imgList.Count() >= 3)
            {
                Image left = imgList[0].GetImagePart();
                Image middle = imgList[1].GetImagePart();
                Image right = imgList[2].GetImagePart();
                using (Graphics drawing = Graphics.FromImage(img))
                {
                    //Draw left
                    drawing.DrawImage(left, new Point(0, 0));
                    //Draw middle
                    drawing.DrawImage(middle, new Rectangle(left.Width, 0, (size.Width  - left.Width - right.Width), left.Height));
                    //Draw right
                    drawing.DrawImage(right, new Point(size.Width - right.Width, 0));

                    //Fill height
                    if (left.Height < size.Height && imgList.Count() >= 9)
                    {
                        Image leftmiddle = imgList[3].GetImagePart();
                        Image middlemiddle = imgList[4].GetImagePart();
                        Image rightmiddle = imgList[5].GetImagePart();
                        Image leftbottom = imgList[6].GetImagePart();
                        Image middlebottom = imgList[7].GetImagePart();
                        Image rightbottom = imgList[8].GetImagePart();

                        //Draw left
                        drawing.DrawImage(leftmiddle, new Rectangle(0,left.Height, leftmiddle.Width, size.Height - left.Height - leftbottom.Height));
                        drawing.DrawImage(leftbottom, new Point(0, size.Height - leftbottom.Height));
                        //Draw middle
                        drawing.DrawImage(middlemiddle, new Rectangle(left.Width, left.Height, size.Width - left.Width - right.Width, size.Height - left.Height - leftbottom.Height));
                        drawing.DrawImage(middlebottom, new Rectangle(left.Width,size.Height - left.Height,size.Width - left.Width -right.Width, right.Height));

                        //Draw right
                        drawing.DrawImage(rightmiddle, new Rectangle(size.Width - right.Width, left.Height, leftmiddle.Width, size.Height - left.Height - leftbottom.Height));
                        drawing.DrawImage(rightbottom, new Point(size.Width  - right.Width, size.Height - right.Height));

                    }
                    //draw caption
                    if(Caption != "")
                    {
                        StringFormat sf = new StringFormat();
                        sf.LineAlignment = StringAlignment.Center;
                        sf.Alignment = StringAlignment.Center;
                        drawing.DrawString(Caption, new Font(FontFamily.GenericSansSerif,8), new SolidBrush(Color.Black), new Rectangle(0,0,size.Width, size.Height), sf);
                    }
                }
                
                
                //img.Save(System.IO.Path.GetTempFileName() + ".png", System.Drawing.Imaging.ImageFormat.Png);
                System.Diagnostics.Debug.WriteLine("Rendering " + imgList[0].ImageFileName);
            }
            else
            {
                using (Graphics drawing = Graphics.FromImage(img))
                {
                    drawing.DrawImage(imgList[0].GetImagePart(), size.Width, size.Height);
                }
            }
            return img;
        }
    }
}
