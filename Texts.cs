using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Experiment
{
    
    public static class Styling
    {
        public static System.Drawing.Font GetInstructionsFont()
        {
            System.Drawing.Font oFont = new System.Drawing.Font("Arial", int.Parse(Preferences.GetPreference("fontSizeInstructions")));
            return oFont;
        }

        public static System.Drawing.Font GetLabelsFont()
        {
            System.Drawing.Font oFont = new System.Drawing.Font("Arial", int.Parse(Preferences.GetPreference("fontSizeLabels")));
            return oFont;
        }

        public static System.Drawing.Font GetHatramaFont()
        {
            System.Drawing.Font oFont = new System.Drawing.Font("Arial", int.Parse(Preferences.GetPreference("fontSizeHatrama")));
            return oFont;
        }

        public static System.Drawing.Font GetLexicalDesicionFont()
        {
            System.Drawing.Font oFont = new System.Drawing.Font("Arial", int.Parse(Preferences.GetPreference("fontSizeLexicalDesicion")));
            return oFont;
        }

        public static char DelimiterChar = '¬';
    }

    /// <summary>
    /// must give path to xml first
    /// </summary>
    public static class Texts
    {
        private static ArrayList oArrayList = new ArrayList();
        private static bool bAlreadyReadXML = false;
        private static string sPathToXml = "";

        private static void ReadTheXml()
        {
            System.Xml.XmlReaderSettings oSettings = new System.Xml.XmlReaderSettings();
            oSettings.IgnoreComments = true;
            oSettings.IgnoreWhitespace = true;
            System.Xml.XmlReader oXmlReader = System.Xml.XmlReader.Create(sPathToXml, oSettings);
            ArrayList oArrayListTexts = new ArrayList();
            while (oXmlReader.Read()) // advance to first screen
            {
                if (oXmlReader.NodeType == System.Xml.XmlNodeType.EndElement)
                {
                    if (oXmlReader.Name == "screen")
                    {
                        oArrayList.Add(oArrayListTexts);  //Guy: this array holds the texts for each screen
                        oArrayListTexts = new ArrayList();
                    }
                }
                else
                {
                    if (oXmlReader.Name == "text")
                    {
                        string[] sLines = oXmlReader.ReadString().Split(new string[] {"\n","\r\n"}, StringSplitOptions.RemoveEmptyEntries);
                        string sText = "";
                        foreach (string line in sLines)
                        {
                            sText += line.Trim() + Environment.NewLine;
                        }
                        oArrayListTexts.Add(sText.Trim());
                    }
                }
            }
        }

        public static string PathToXml
        {
            set
            {
                sPathToXml = value;
            }
        }

        public static string GetScreenText(int iScreenIndex, int iTextIndex)
        {
            if (!bAlreadyReadXML)
            {
                ReadTheXml();
                bAlreadyReadXML = true;
            }

            return ((ArrayList)oArrayList[iScreenIndex])[iTextIndex].ToString(); //Guy:  specific text is return according to asking screen
        }
    }

    public static class TextsGlobal
    {
        private static Hashtable oHashtable = new Hashtable();
        private static bool bAlreadyReadXML = false;
        private static string sPathToXml = "";

        private static void ReadTheXml()
        {
            System.Xml.XmlReaderSettings oSettings = new System.Xml.XmlReaderSettings();
            oSettings.IgnoreComments = true;
            oSettings.IgnoreWhitespace = true;
            System.Xml.XmlReader oXmlReader = System.Xml.XmlReader.Create(sPathToXml, oSettings);
            while (oXmlReader.Read())
            {
                if (oXmlReader.Name == "text" && oXmlReader.NodeType == System.Xml.XmlNodeType.Element)
                {
                    string sName = oXmlReader.GetAttribute("name");
                    string[] sLines = oXmlReader.ReadString().Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    string sText = "";
                    foreach (string line in sLines)
                    {
                        sText += line.Trim() + Environment.NewLine;
                    }
                    oHashtable[sName] = sText.Trim();
                }
            }
        }

        public static string PathToXml
        {
            set
            {
                sPathToXml = value;
            }
        }

        public static string GetText(string name)
        {
            if (!bAlreadyReadXML)
            {
                ReadTheXml();
                bAlreadyReadXML = true;
            }

            return oHashtable[name].ToString(); ;
        }
    }

    public static class Preferences
    {
        private static Hashtable oHashtable = new Hashtable();
        private static bool bAlreadyReadXML = false;
        private static string sPathToXml = "";

        private static void ReadTheXml()
        {
            System.Xml.XmlReaderSettings oSettings = new System.Xml.XmlReaderSettings();
            oSettings.IgnoreComments = true;
            oSettings.IgnoreWhitespace = true;
            System.Xml.XmlReader oXmlReader = System.Xml.XmlReader.Create(sPathToXml, oSettings);
            while (oXmlReader.Read())
            {
                if (oXmlReader.Name == "preference" && oXmlReader.NodeType == System.Xml.XmlNodeType.Element)
                {
                    string sName = oXmlReader.GetAttribute("name");
                    string sText = oXmlReader.ReadString();
                    oHashtable[sName] = sText.Trim();
                }
            }
        }

        public static string PathToXml
        {
            set
            {
                sPathToXml = value;
            }
        }

        public static string GetPreference(string name)
        {
            if (!bAlreadyReadXML)
            {
                ReadTheXml();
                bAlreadyReadXML = true;
            }

            return oHashtable[name].ToString(); ;
        }
    }

    public class Tester
    {
        public string Id = "";
        public int Group = 0;
        public string Gender = "";
        public string familiar_name = "";
        public string unfamiliar_name = "";
    }

    public static class Testers
    {
        private static Hashtable oHashtable = new Hashtable();
        private static bool bAlreadyReadXML = false;
        private static string sPathToXml = "";

        private static void ReadTheXml()
        {
            System.Xml.XmlReaderSettings oSettings = new System.Xml.XmlReaderSettings();
            oSettings.IgnoreComments = true;
            oSettings.IgnoreWhitespace = true;
            System.Xml.XmlReader oXmlReader = System.Xml.XmlReader.Create(sPathToXml, oSettings);
            Tester oTester = new Tester();
            while (oXmlReader.Read())
            {
                if (oXmlReader.NodeType == System.Xml.XmlNodeType.EndElement)
                {
                    if (oXmlReader.Name == "test")
                    {
                        oHashtable.Add(oTester.Id, oTester);
                        oTester = new Tester();
                    }
                }
                else
                {
                    if (oXmlReader.Name == "id")
                    {
                        oTester.Id = oXmlReader.ReadString().Trim();
                    }
                    else if (oXmlReader.Name == "group")
                    {
                        oTester.Group = int.Parse(oXmlReader.ReadString().Trim());
                    }
                    else if (oXmlReader.Name == "gender")
                    {
                        oTester.Gender = oXmlReader.ReadString().Trim();
                    }
                    else if (oXmlReader.Name == "familiar_name")
                    {
                        oTester.familiar_name = oXmlReader.ReadString().Trim();
                    }
                    else if (oXmlReader.Name == "unfamiliar_name")
                    {
                        oTester.unfamiliar_name = oXmlReader.ReadString().Trim();
                    }
                }
            }
        }

        public static string PathToXml
        {
            set
            {
                sPathToXml = value;
            }
        }

        public static Tester GetTester(string Id)
        {
            if (!bAlreadyReadXML)
            {
                ReadTheXml();
                bAlreadyReadXML = true;
            }

            return (Tester)oHashtable[Id];
        }
    }
}
