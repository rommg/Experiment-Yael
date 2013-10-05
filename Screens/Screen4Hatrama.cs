using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Experiment.Screens
{
    public partial class Screen4Hatrama : UserControl
    {
        public event NextScreenDelegate NextScreen;

        private PictureBox oSnow;
        private string[] furniture;
        private string sPerson;
        private string sHatramaWord;
        private Label oLabelX;
        private Label oLabelHatrama;
        private Label oLabelPair;
        private int[] timerTimes = { int.Parse(Preferences.GetPreference("hatramaTime1MS"))
                                       , int.Parse(Preferences.GetPreference("hatramaTime2MS"))
                                       , int.Parse(Preferences.GetPreference("hatramaTime3MS")) };
        private int iTimerIndex = 0;
        private int iTestNum = 1;
        //Guy - change numOfTest according to group number
        //private int iNumOfTests = int.Parse(Preferences.GetPreference("hatramaNumberOfTests"));
        private int iNumOfTests;
        private bool bIsListening = false;
        private string sAnswers = "";
        private Timer timer1;
        private string[] randomWords; 
        private string[] HatramaWords1 = { "אהבה", "קרבה", "חיבוק", "עזרה", "אהבה", "קרבה", "חיבוק", "עזרה",
                                         "אהבה", "קרבה", "חיבוק", "עזרה","אהבה", "קרבה", "חיבוק", "עזרה",
                                         "אהבה", "קרבה", "חיבוק", "עזרה","אהבה", "קרבה", "חיבוק", "עזרה",
                                         "אהבה", "קרבה", "חיבוק", "עזרה","אהבה", "קרבה", "חיבוק", "עזרה",
                                         "אהבה", "קרבה", "חיבוק", "עזרה","אהבה", "קרבה", "חיבוק", "עזרה",
                                         "אהבה", "קרבה", "חיבוק", "עזרה","אהבה", "קרבה", "חיבוק", "עזרה",
                                         "אהבה", "קרבה", "חיבוק", "עזרה","אהבה", "קרבה", "חיבוק", "עזרה",
                                         "אהבה", "קרבה", "חיבוק", "עזרה"};
        private string[] HatramaWords2 = { "משרד", "שולחן", "סירה", "תמונה", "משרד", "שולחן", "סירה", "תמונה",
                                             "משרד", "שולחן", "סירה", "תמונה", "משרד", "שולחן", "סירה", "תמונה",
                                             "משרד", "שולחן", "סירה", "תמונה", "משרד", "שולחן", "סירה", "תמונה",
                                             "משרד", "שולחן", "סירה", "תמונה", "משרד", "שולחן", "סירה", "תמונה",
                                             "משרד", "שולחן", "סירה", "תמונה", "משרד", "שולחן", "סירה", "תמונה",
                                             "משרד", "שולחן", "סירה", "תמונה", "משרד", "שולחן", "סירה", "תמונה",
                                             "משרד", "שולחן", "סירה", "תמונה", "משרד", "שולחן", "סירה", "תמונה",
                                             "משרד", "שולחן", "סירה", "תמונה" };

        public Screen4Hatrama()
        {
            InitializeComponent();
        }

        /// <summary>
        /// set the screen to its initial state
        /// </summary>
        public void ResetScreen()
        {
            try
            {
                iTimerIndex = 0;
                iTestNum = 1;
                bIsListening = false;
                sAnswers = "";
                buttonNext.Visible = false;
            }
            catch { }
        }

        public string Person
        {
            set
            {
                sPerson = value;
            }
        }

        public string HatramaWord
        {
            set
            {
                sHatramaWord = value;
            }
        }

        //Guy: new function to set the number of tests according to group
        public int numOfTests
        {
            set
            {
                iNumOfTests = value;
            }
        }

        /// <summary>
        /// call this function to load the screen and start its actions if necessary.
        /// </summary>
        public void LoadScreen()
        {
            ResetScreen();
            LoadData();
            NextStep();
        }

        public string Result
        {
            get
            {
                return sAnswers;
            }
        }

        private void LoadData()
        {
            try
            {
                timer1 = new Timer();

                oSnow = new PictureBox();
                oSnow.Anchor = AnchorStyles.None;
                oSnow.Image = Experiment.Properties.Resources.snow;
                oSnow.Size = new System.Drawing.Size(int.Parse(Preferences.GetPreference("hatramaMisuchWidth")), int.Parse(Preferences.GetPreference("hatramaMisuchHeight")));
                oSnow.SizeMode = PictureBoxSizeMode.StretchImage;

                // furniture
                furniture = TextsGlobal.GetText("Furniture").Split(new char[]{','});
                for (int i = 0; i < furniture.Length; i++)
                {
                    furniture[i] = furniture[i].Trim();
                }

                oLabelX = new Label();
                oLabelX.Text = "X";
                oLabelX.Anchor = AnchorStyles.None;
                oLabelX.Font = Styling.GetHatramaFont();
                oLabelX.ForeColor = Color.Black;
                oLabelX.AutoSize = true;

                oLabelHatrama = new Label();
                oLabelHatrama.Anchor = AnchorStyles.None;
                oLabelHatrama.Font = Styling.GetHatramaFont();
                oLabelHatrama.ForeColor = Color.Black;
                oLabelHatrama.AutoSize = true;

                oLabelPair = new Label();
                oLabelPair.Anchor = AnchorStyles.None;
                oLabelPair.Font = Styling.GetHatramaFont();
                oLabelPair.ForeColor = Color.Black;
                oLabelPair.AutoSize = true;

                buttonNext.Text = TextsGlobal.GetText("Next");

                timer1.Tick += new EventHandler(timer1_Tick);

                //Guy - reorgenize the array in random order, according to group number
                Random rnd = new Random();
                if (String.Compare("group1array", sHatramaWord) == 0)
                    randomWords = HatramaWords1.OrderBy(x => rnd.Next()).ToArray();

                if (String.Compare("group2array", sHatramaWord) == 0)
                    randomWords = HatramaWords2.OrderBy(x => rnd.Next()).ToArray();
            }
            catch
            {
                MessageBox.Show("Problem loading screen texts.");
            }
        }

        void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                timer1.Stop();
                if (iTimerIndex == timerTimes.Length)
                {
                    iTimerIndex = 0;
                    bIsListening = true;

                    // display the pair
                    SetPairOnScreen();
                }
                else
                {
                    NextStep();
                }
            }
            catch { }
        }

        private void NextStep()
        {
            try
            {
                // set timer
                timer1.Interval = timerTimes[iTimerIndex];

                // set relevant text
                tableLayoutPanel2.Controls.Clear();
                switch (iTimerIndex)
                {
                    case 0:
                        tableLayoutPanel2.Controls.Add(oLabelX, 0, 0);
                        break;
                    case 1:
                        oLabelHatrama.Text = GetMilatHatrama();
                        tableLayoutPanel2.Controls.Add(oLabelHatrama, 0, 0);
                        break;
                    case 2:
                        tableLayoutPanel2.Controls.Add(oSnow, 0, 0);
                        break;
                }

                timer1.Start();
                iTimerIndex++;
            }
            catch { }
        }

        public void OnKeyDownPressed(KeyEventArgs e)
        {
            if (bIsListening)
            {
                if ((e.KeyCode >= Keys.D1 && e.KeyCode <= Keys.D7) || (e.KeyCode >= Keys.NumPad1 && e.KeyCode <= Keys.NumPad7))
                {
                    bIsListening = false;
                    string sKeyCode = e.KeyCode.ToString();
                    char cChar;
                    if (sKeyCode.Length == 2)
                    {
                        cChar = sKeyCode[1];
                    }
                    else
                    {
                        cChar = sKeyCode[6];
                    }

                    // write the answer
                    sAnswers += oLabelPair.Text + ": " + cChar.ToString() + Environment.NewLine;

                    // check if more tests and go
                    if (iTestNum < iNumOfTests)
                    {
                        iTestNum++;
                        NextStep();
                    }
                    else
                    {
                        tableLayoutPanel2.Controls.Clear();
                        buttonNext.Visible = true;
                    }
                }
            }
        }

        // Guy: This function now returns only sPerson, for groups 3,4
        // The function used to return a random chice between sHatramaWord and sPerson
        private string GetMilatHatrama()
        {
            //Random r = new Random(DateTime.Now.Millisecond);
            //if (r.Next(2) == 0)
            //{
            //    return sPerson;
            //}
            //else
            //{
            //    return sHatramaWord;
            //}

            if (String.Compare("group1array", sHatramaWord) == 0 || String.Compare("group2array", sHatramaWord) == 0)
                return randomWords[iTestNum - 1];

            return sPerson;
        }

        private void SetPairOnScreen()
        {
            Random r = new Random(DateTime.Now.Millisecond);
            int iIndex1, iIndex2;
            iIndex1 = r.Next(furniture.Length);
            iIndex2 = r.Next(furniture.Length);
            while(iIndex1 == iIndex2)
            {
                iIndex2 = r.Next(furniture.Length);
            }
            oLabelPair.Text = furniture[iIndex1] + " - " + furniture[iIndex2];
            tableLayoutPanel2.Controls.Clear();
            tableLayoutPanel2.Controls.Add(oLabelPair, 0, 0);
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            try
            {
                NextScreen();
            }
            catch { }
        }
    }
}
