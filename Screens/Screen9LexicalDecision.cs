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
    public partial class Screen9LexicalDecision : UserControl
    {
        public event NextScreenDelegate NextScreen;
        private string[] words;
        private string[] tryoutWords;
        private string[] theOrderedWords;
        private string[] theTryoutWords;
        private Label oLabelX;
        private Label oLabelHatrama;
        private Label oLabelWaiting;
        private int[] timerTimes = { int.Parse(Preferences.GetPreference("lexicalDecisionTime1MS"))
                                       , int.Parse(Preferences.GetPreference("lexicalDecisionTime2MS"))
                                       , int.Parse(Preferences.GetPreference("lexicalDecisionTime3MS"))
                                   };
        private int iTimerIndex = 0;
        private int iTestNum = 0;
        private int iNumOfTryouts; // the same number of the tryout words
        private int iNumOfTests; // is calculated in LoadData()
        private bool bIsListening = false;
        private string sAnswers = "";
        private int iTestPhase = 0;
        private int iNumberOfShowsForWord = int.Parse(Preferences.GetPreference("lexicalDecisionNumberOfShowsPerWord"));
        private DateTime oDTStart; // to measure the time span until answering
        private TimeSpan oTryoutsTime = new TimeSpan();

        public Screen9LexicalDecision()
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
                iTestNum = 0;
                bIsListening = false;
                sAnswers = "";
                iTestPhase = 0;
            }
            catch { }
        }

        public string Result
        {
            get
            {
                return sAnswers;
            }
        }

        /// <summary>
        /// call this function to load the screen and start its actions if necessary.
        /// </summary>
        public void LoadScreen()
        {
            LoadData();
            ArrangeWordsOrder();
        }

        private void LoadData()
        {
            try
            {
                // words
                words = Texts.GetScreenText(8, 2).Split(new char[] { ',' });
                for (int i = 0; i < words.Length; i++)
                {
                    words[i] = words[i].Trim();
                }
                theOrderedWords = new string[words.Length * iNumberOfShowsForWord];

                iNumOfTests = theOrderedWords.Length;

                // tryout words
                tryoutWords = Texts.GetScreenText(8, 1).Split(new char[] { ',' });
                for (int i = 0; i < tryoutWords.Length; i++)
                {
                    tryoutWords[i] = tryoutWords[i].Trim();
                }
                theTryoutWords = new string[tryoutWords.Length];
                iNumOfTryouts = theTryoutWords.Length;

                oLabelX = new Label();
                oLabelX.Text = "X";
                oLabelX.Anchor = AnchorStyles.None;
                oLabelX.Font = Styling.GetLexicalDesicionFont();
                oLabelX.AutoSize = true;

                oLabelHatrama = new Label();
                oLabelHatrama.Anchor = AnchorStyles.None;
                oLabelHatrama.Font = Styling.GetLexicalDesicionFont();
                oLabelHatrama.AutoSize = true;

                oLabelWaiting = new Label();
                oLabelWaiting.Anchor = AnchorStyles.None;
                oLabelWaiting.Font = Styling.GetLabelsFont();
                oLabelWaiting.Text = Texts.GetScreenText(8,3);
                oLabelWaiting.AutoSize = true;

                label1.Text = Texts.GetScreenText(8, 0);
                label1.Font = Styling.GetInstructionsFont();
                buttonNext.Text = TextsGlobal.GetText("Next");

                timer1.Tick += new EventHandler(timer1_Tick);
            }
            catch
            {
                MessageBox.Show("Problem loading screen texts.");
            }
        }

        /// <summary>
        /// puts in a random manner the words in 1 array
        /// </summary>
        private void ArrangeWordsOrder()
        {
            try
            {
                Random rnd = new Random(DateTime.Now.Millisecond);
                for (int i = 0; i < iNumberOfShowsForWord; i++)
                {
                    int[] randPermutation = randomPermutation(words.Length, rnd);
                    for (int index = 0; index < randPermutation.Length; index++)
                    {
                        theOrderedWords[index + (words.Length * i)] = words[randPermutation[index]];
                    }
                }

                // the tryouts
                int[] randPermutation1 = randomPermutation(tryoutWords.Length, rnd);
                for (int index = 0; index < randPermutation1.Length; index++)
                {
                    theTryoutWords[index] = tryoutWords[randPermutation1[index]];
                }
            }
            catch (Exception ex)
            {
            }
        }

        void timer1_Tick(object sender, EventArgs e)
        {
            NextStep();
        }

        private void NextStep()
        {
            try
            {
                // set timer
                timer1.Stop();
                timer1.Interval = timerTimes[iTimerIndex];
                bIsListening = false;

                // set relevant text
                tableLayoutPanel2.Controls.Clear();

                // check if end of test, if not then start the timer.
                if (iTestPhase == 1) // tryouts
                {
                    if (iTestNum < iNumOfTryouts)
                    {
                        if (iTimerIndex == 0)
                        {
                            tableLayoutPanel2.Controls.Add(oLabelX, 0, 0);
                            iTimerIndex++;
                        }
                        else if (iTimerIndex == 1) // show word
                        {
                            iTimerIndex++;
                            oDTStart = DateTime.Now;
                            // display word
                            oLabelHatrama.Text = theTryoutWords[iTestNum];
                            sAnswers += theTryoutWords[iTestNum] + "|";
                            tableLayoutPanel2.Controls.Add(oLabelHatrama, 0, 0);
                            bIsListening = true;
                        }
                        else if (iTimerIndex == 2) // wait again for answer
                        {
                            bIsListening = true;
                            tableLayoutPanel2.Controls.Add(oLabelWaiting, 0, 0);
                        }
                        // start timer
                        timer1.Start();
                    }
                    else
                    {
                        iTestNum = 0;
                        // show text before next phase
                        Label oLabelNextPhase = new Label();
                        oLabelNextPhase.Anchor = AnchorStyles.None;
                        oLabelNextPhase.Font = Styling.GetInstructionsFont();
                        string sAvarageTryoutsTime = "2";
                        try
                        {
                            sAvarageTryoutsTime = (oTryoutsTime.TotalSeconds / iNumOfTryouts).ToString("F");
                        }
                        catch { }
                        oLabelNextPhase.Text = Texts.GetScreenText(8, 4).Replace("%1%", sAvarageTryoutsTime + " שניות");
                        oLabelNextPhase.AutoSize = true;
                        tableLayoutPanel2.Controls.Add(oLabelNextPhase, 0, 0);

                        // show next button for next test
                        buttonNext.Visible = true;
                    }
                }
                else if (iTestPhase == 2) // real test
                {
                    if (iTestNum < iNumOfTests)
                    {
                        if (iTimerIndex == 0)
                        {
                            tableLayoutPanel2.Controls.Add(oLabelX, 0, 0);
                            iTimerIndex++;
                        }
                        else if (iTimerIndex == 1) // show word
                        {
                            iTimerIndex++;
                            oDTStart = DateTime.Now;
                            // display word
                            oLabelHatrama.Text = theOrderedWords[iTestNum];
                            sAnswers += theOrderedWords[iTestNum] + "|";
                            tableLayoutPanel2.Controls.Add(oLabelHatrama, 0, 0);
                            bIsListening = true;
                        }
                        else if (iTimerIndex == 2) // wait again for answer
                        {
                            bIsListening = true;
                            tableLayoutPanel2.Controls.Add(oLabelWaiting, 0, 0);
                        }
                        // start timer
                        timer1.Start();
                    }
                    else
                    {
                        // show next button for next Screen
                        buttonNext.Visible = true;
                    }
                }
            }
            catch { }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (iTestPhase == 0)
            {
                // go to 10 try outs
                iTestPhase++;
                buttonNext.Visible = false;
                NextStep();
            }
            else if (iTestPhase == 1)
            {
                // goto real test
                iTestPhase++;
                buttonNext.Visible = false;
                NextStep();
            }
            else
            {
                NextScreen();
            }
        }

        public void OnKeyDownPressed(KeyEventArgs e)
        {
            if (bIsListening)
            {
                if (e.KeyCode == Keys.L || e.KeyCode == Keys.S)
                {
                    bIsListening = false;
                    string sKeyCode = e.KeyCode.ToString();

                    TimeSpan ts = DateTime.Now.Subtract(oDTStart);
                    // write the answer
                    sAnswers += sKeyCode + "|" + ts.Seconds.ToString() + ":" + ts.Milliseconds.ToString() + Styling.DelimiterChar;
                    if (iTestPhase == 1)
                    {
                        oTryoutsTime = oTryoutsTime.Add(ts);
                    }
                    iTestNum++;
                    iTimerIndex = 0;

                    NextStep();
                }
            }
        }

        #region HELPER FUNCTIONS
        public int[] randomPermutation(int n, Random rnd)
        {
            int[] result = new int[n];
            for (int i = 0; i < n; i++)
            {
                result[i] = i;
            }
            shuffle(result, rnd);
            return result;
        }

        public void shuffle(int[] arr, Random rnd)
        {
            int size = arr.Length;
            for (int i = size; i > 1; i--)
            {
                swap(arr, i - 1, rnd.Next(i));
            }

        }

        private void swap(int[] arr, int i, int j)
        {
            int tmp = arr[i];
            arr[i] = arr[j];
            arr[j] = tmp;
        }
        #endregion
    }
}
