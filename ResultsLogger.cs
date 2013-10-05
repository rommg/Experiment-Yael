using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CarlosAg.ExcelXmlWriter;

namespace Experiment
{
    public class ResultsLogger
    {
        public string Id = "";
        public int Group = 0;
        public string Gender = "";
        public string familiar_name = "";
        public string unfamiliar_name = "";
        public string screen2_ChosenArachim = "";
        public string screen2_notes = "";
        public string screen2_notes_again = "";
        public string screen4Hatrama_result = "";
        public string screen4Hatrama_again_result = "";
        public string screen5MathIntro_Gender = "";
        public string screen6Math_1_result = "";
        public string screen6Math_2_result = "";
        public string screen8Sheelon_result = "";
        public string screen9Lexical_result = "";
        public string screen13Sheelon_result = "";
        public DateTime oTesterDatetime = DateTime.MinValue;
        public string screenLastQuestionYourLanguage = "";
        public string screenLastQuestionYesNo1 = "";
        public string screenLastQuestionYesNo2 = "";
        public string screenLastQuestionYesNo3 = "";
        public string screenHatramaEnd_1_Answer1 = "";
        public string screenHatramaEnd_1_Answer2 = "";
        public string screenHatramaEnd_1_Answer3 = "";
        public string screenHatramaEnd_2_Answer1 = "";
        public string screenHatramaEnd_2_Answer2 = "";
        public string screenHatramaEnd_2_Answer3 = "";

        public bool WriteFile()
        {
            try
            {
                
                // for math
                string[] arrChosenArachim = screen2_ChosenArachim.Split(new char[] { Styling.DelimiterChar });
                string[] arrMath1 = screen6Math_1_result.Split(new char[] { Styling.DelimiterChar });
                string[] arrMath2 = screen6Math_2_result.Split(new char[] { Styling.DelimiterChar });
                string[] arrZminut = screen8Sheelon_result.Split(new char[] { Styling.DelimiterChar });
                string[] arrThunot = screen13Sheelon_result.Split(new char[] { Styling.DelimiterChar });
                string[] arrLexical = screen9Lexical_result.Split(new char[] { Styling.DelimiterChar });

                Workbook book = new Workbook();
                Worksheet sheet = book.Worksheets.Add("Results");
                WorksheetRow row = sheet.Table.Rows.Add();
                

                // ----------------------------------------------------------add titles:
                row.Cells.Add("DATE");
                row.Cells.Add("TIME");
                row.Cells.Add("מספר מזהה");
                row.Cells.Add("קבוצה");
                row.Cells.Add("מין");
                row.Cells.Add("שם מוכר");
                row.Cells.Add("שם לא מוכר");
                row.Cells.Add("ערך 1");
                row.Cells.Add("ערך 2");
                row.Cells.Add("הסבר 1");
                row.Cells.Add("הסבר 2");
                row.Cells.Add("הטרמה 1");
                row.Cells.Add("הטרמה 2");
                row.Cells.Add("יוצא דופן במטלה1?");
                row.Cells.Add("הבזקים מטלה1?");
                row.Cells.Add("אותיות או ספרות1?");
                row.Cells.Add("יוצא דופן במטלה2?");
                row.Cells.Add("הבזקים מטלה2?");
                row.Cells.Add("אותיות או ספרות2?");
                row.Cells.Add("מין בגירסה המאיימת");
                
                // Guy: loop for Thunot
                for (int i = 0; i < arrThunot.Length - 1; i++)
                {
                    row.Cells.Add("שאלון תכונות " + (i + 1).ToString());
                }

                // loop for math 
                for (int i = 0; i < arrMath1.Length-1; i++)
                {
                    row.Cells.Add("מתמטיקה1." + (i+1).ToString());
                    row.Cells.Add("מתמטיקה1." + (i + 1).ToString() + "_ניחוש");
                }
                // loop for math 2
                for (int i = 0; i < arrMath2.Length-1; i++)
                {
                    row.Cells.Add("מתמטיקה2." + (i + 1).ToString());
                    row.Cells.Add("מתמטיקה2." + (i + 1).ToString() + "_ניחוש");
                }

                // loop for zminut
                for (int i = 0; i < arrZminut.Length-1; i++)
                {
                    row.Cells.Add("שאלון זמינות " + (i + 1).ToString());
                }
                
                // loop for lexical
                for (int i = 0; i < arrLexical.Length-1; i++)
                {
                    row.Cells.Add("החלטה לקסיקלית_מילה" + (i + 1).ToString());
                    row.Cells.Add("החלטה לקסיקלית_ז.תגובה" + (i + 1).ToString());
                    row.Cells.Add("החלטה לקסיקלית_מקש" + (i + 1).ToString());
                }

                row.Cells.Add("מסך אחרון - שפה");
                row.Cells.Add("מסך אחרון -" + Texts.GetScreenText(9, 1));
                row.Cells.Add("מסך אחרון -" + Texts.GetScreenText(9, 2));
                row.Cells.Add("מסך אחרון -" + Texts.GetScreenText(9, 3));

                // ----------------------------------------------------------------- add content
                row = sheet.Table.Rows.Add();
                row.Cells.Add(oTesterDatetime.ToString("dd/MM/yyyy"));
                row.Cells.Add(oTesterDatetime.ToString("HH:mm"));
                row.Cells.Add(Id);
                row.Cells.Add(Group.ToString());
                row.Cells.Add(Gender);
                row.Cells.Add(familiar_name);
                row.Cells.Add(unfamiliar_name);

                
                // ערך 1
                try
                {
                    row.Cells.Add(arrChosenArachim[0]);
                }
                catch
                {
                    row.Cells.Add("");
                }
                // ערך 2
                try
                {
                    row.Cells.Add(arrChosenArachim[1]);
                }
                catch
                {
                    row.Cells.Add("");
                }
                
                row.Cells.Add(screen2_notes);
                row.Cells.Add(screen2_notes_again);

                row.Cells.Add(screen4Hatrama_result);
                row.Cells.Add(screen4Hatrama_again_result);

                row.Cells.Add(screenHatramaEnd_1_Answer1);
                row.Cells.Add(screenHatramaEnd_1_Answer2);
                row.Cells.Add(screenHatramaEnd_1_Answer3);
                row.Cells.Add(screenHatramaEnd_2_Answer1);
                row.Cells.Add(screenHatramaEnd_2_Answer2);
                row.Cells.Add(screenHatramaEnd_2_Answer3);

                row.Cells.Add(screen5MathIntro_Gender);

                // loop for Thunot
                for (int i = 0; i < arrThunot.Length - 1; i++)
                {
                    row.Cells.Add(arrThunot[i]);
                }

                // loop math
                for (int i = 0; i < arrMath1.Length-1; i++)
                {
                    string[] arrAnswer = arrMath1[i].Split(new char[] { '.' });
                    row.Cells.Add(arrAnswer[0]);
                    row.Cells.Add(arrAnswer[1]);
                }
                // loop for math 2
                for (int i = 0; i < arrMath2.Length-1; i++)
                {
                    string[] arrAnswer = arrMath2[i].Split(new char[] { '.' });
                    row.Cells.Add(arrAnswer[0]);
                    row.Cells.Add(arrAnswer[1]);
                }

                // loop for zminut
                for (int i = 0; i < arrZminut.Length-1; i++)
                {
                    row.Cells.Add(arrZminut[i]);
                }

                // loop for lexical
                for (int i = 0; i < arrLexical.Length-1; i++)
                {
                    string[] arrAnswer = arrLexical[i].Split(new char[] { '|' });
                    row.Cells.Add(arrAnswer[0]);
                    row.Cells.Add(arrAnswer[2]);
                    row.Cells.Add(arrAnswer[1]);
                }

                row.Cells.Add(screenLastQuestionYourLanguage);
                row.Cells.Add(screenLastQuestionYesNo1);
                row.Cells.Add(screenLastQuestionYesNo2);
                row.Cells.Add(screenLastQuestionYesNo3);

                book.Save(Properties.Settings.Default.Folder_saveResults + Id + ".xls");
               
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
