using System;
using System.Collections.Generic;
using System.Drawing;

namespace SG_Studio.Classes.GUI
{
    public class SG_UI_Window : SG_UI_Element
    {
           
        public SG_UI_Window(string nuiPath)
        {
            Code = System.IO.File.ReadAllText(nuiPath);
            ParseElements();
        }
        public void ParseElements()
        {
            Elements.Clear();
            var elements = Code.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string e in elements)
            {
                SG_UI_Element uie = new SG_UI_Element();
                uie.ChildOf = this;
                uie.Code = e;
                foreach (string eAttribute in e.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (eAttribute == "end")
                        break;

                    //Check Window Type
                    if (eAttribute.StartsWith("begin"))
                    {
                        switch (eAttribute.Replace("begin ", ""))
                        {
                            case "genwnd":
                                uie = this;
                                elementType = ElementType.Window;
                                ChildOf = null;
                                break;
                            case "static":
                                uie.elementType = ElementType.Static;
                                break;
                            case "simplecheck":
                                uie.elementType = ElementType.SimpleCheck;
                                break;
                            case "check":
                                uie.elementType = ElementType.Check;
                                break;
                            case "simplebutton":
                                uie.elementType = ElementType.SimpleButton;
                                break;
                            case "button":
                                uie.elementType = ElementType.Button;
                                break;
                            case "edit":
                                uie.elementType = ElementType.Edit;
                                break;
                        }
                    }

                    //Check other attributes
                    var keyVal = eAttribute.Split(new string[] { " = " }, StringSplitOptions.None);

                    if (keyVal[0] == "id")
                        uie.ElementID = keyVal[1].Replace(";", "");
                    if (keyVal[0] == "spr")
                        uie.Spr = keyVal[1].Replace(";", "");
                    if (keyVal[0] == "ani")
                        uie.Ani = keyVal[1].Replace(";", "");
                    if (keyVal[0] == "rect")
                    {
                        var rect = keyVal[1].Replace(";", "").Split(',');
                        uie.Rect = new Rectangle(int.Parse(rect[0]), int.Parse(rect[1]), int.Parse(rect[2]), int.Parse(rect[3]));
                    }

                    //TODO: There are still some attributes that need to be checked here...


                    //Add Element to list
                    if (uie != this)
                        Elements.Add(uie);
                }

            }
        }
    }
}
