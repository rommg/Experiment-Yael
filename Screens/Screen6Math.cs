using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace Experiment.Screens
{
    public partial class Screen6Math : UserControl
    {
        public event NextScreenDelegate NextScreen;
        private ArrayList oArrayListTest = new ArrayList();
        private string sPathToImages = "images/" ; //"C:\\Users\\Lior\\documents\\visual studio 2010\\Projects\\Experiment\\Experiment\\images\\";

        private class Question
        {
            public string sText;
            public string sBeforeImageText;
            public string sImage;
            public ArrayList oAnswers; // array of Answers (class)
            public string sChosenAnswer = "";
            public bool bGuess = false;

            public Question()
            {
                oAnswers = new ArrayList();
            }
        }

        private class Answer
        {
            public string sAnswer = "";
            public bool bIsImage = false;
        }

        public Screen6Math()
        {
            InitializeComponent();
            timer1.Tick += new EventHandler(timer1_Tick);
        }

        /// <summary>
        /// set the screen to its initial state
        /// </summary>
        public void ResetScreen()
        {
            try
            {
                oArrayListTest.Clear();   
            }
            catch { }
        }

        /// <summary>
        /// call this function to load the screen and start its actions if necessary.
        /// </summary>
        public void LoadScreen()
        {
            LoadData();
        }

        public string Result
        {
            get
            {
                string sAnswers = "";
                for (int i = 0; i < oArrayListTest.Count; i++)
                {
                    Question question = (Question)oArrayListTest[i];
                    sAnswers += question.sChosenAnswer + "." + question.bGuess.ToString() + Styling.DelimiterChar;
                }
                return sAnswers;
            }
        }

        private void LoadData()
        {
            try
            {
                buttonNext.Text = TextsGlobal.GetText("Next");
            }
            catch
            {
                MessageBox.Show("Problem loading screen texts.");
            }
        }

        public void LoadTest(string sXmlPath, int minutes)
        {
            try
            {
                flowLayoutPanel1.Controls.Clear();
                oArrayListTest.Clear();
                // loads all test to oArrayListTest, and displays the first text and title
                System.Xml.XmlReaderSettings oSettings = new System.Xml.XmlReaderSettings();
                oSettings.IgnoreComments = true;
                oSettings.IgnoreWhitespace = true;
                System.Xml.XmlReader oXmlReader = System.Xml.XmlReader.Create(sXmlPath, oSettings);
                Question oQuestion = new Question();
                while (oXmlReader.Read()) // advance to first screen
                {
                    if (oXmlReader.NodeType == System.Xml.XmlNodeType.EndElement)
                    {
                        if (oXmlReader.Name == "question")
                        {
                            oArrayListTest.Add(oQuestion);
                            oQuestion = new Question();
                        }
                    }
                    else
                    {
                        if (oXmlReader.Name == "maintext")
                        {
                            // add a label and display the text
                            string[] sLines = oXmlReader.ReadString().Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                            string sText = "";
                            foreach (string line in sLines)
                            {
                                sText += line.Trim() + Environment.NewLine;
                            }
                            if (!String.IsNullOrWhiteSpace(sText))
                            {
                                Label oLabel = new Label();
                                oLabel.Text = sText.Trim();
                                oLabel.AutoSize = true;
                                oLabel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 50);
                                oLabel.Font = Styling.GetInstructionsFont();
                                flowLayoutPanel1.Controls.Add(oLabel);

                                labelEndTime.Text = Environment.NewLine + TextsGlobal.GetText("endTime") + " " + DateTime.Now.AddMinutes(minutes).ToShortTimeString();
                                labelEndTime.AutoSize = true;
                                labelEndTime.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
                                labelEndTime.Font = Styling.GetInstructionsFont();
                            }
                        }
                        else if (oXmlReader.Name == "text")
                        {
                            string[] sLines = oXmlReader.ReadString().Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                            string sText = "";
                            foreach (string line in sLines)
                            {
                                sText += line.Trim() + Environment.NewLine;
                            }
                            oQuestion.sText = sText.Trim();
                        }
                        else if (oXmlReader.Name == "beforeImageText")
                        {
                            string[] sLines = oXmlReader.ReadString().Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                            string sText = "";
                            foreach (string line in sLines)
                            {
                                sText += line.Trim() + Environment.NewLine;
                            }
                            oQuestion.sBeforeImageText = sText.Trim();
                        }
                        else if (oXmlReader.Name == "image")
                        {
                            oQuestion.sImage = oXmlReader.ReadString().Trim();
                        }
                        else if (oXmlReader.Name == "answer")
                        {
                            Answer oAnswer = new Answer();
                            try
                            {
                                oAnswer.bIsImage = bool.Parse(oXmlReader.GetAttribute("isImage"));
                            }
                            catch { }

                            oAnswer.sAnswer = oXmlReader.ReadString().Trim();
                            
                            oQuestion.oAnswers.Add(oAnswer);
                        }
                    }
                }

                // load all questions, each in its own flow layout
                for (int i = 0; i < oArrayListTest.Count; i++)
                {
                    try
                    {
                        FlowLayoutPanel oPanel = new FlowLayoutPanel();
                        oPanel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 50);
                        oPanel.FlowDirection = FlowDirection.TopDown;
                        oPanel.AutoSize = true;
                        Question nextQuestion = (Question)oArrayListTest[i];

                        if (!String.IsNullOrWhiteSpace(nextQuestion.sBeforeImageText))
                        {
                            Label oLabelBeforeImageText = new Label();
                            oLabelBeforeImageText.Text = nextQuestion.sBeforeImageText;
                            oLabelBeforeImageText.AutoSize = true;
                            oLabelBeforeImageText.Font = Styling.GetLabelsFont();

                            oPanel.Controls.Add(oLabelBeforeImageText);
                        }

                        if (!String.IsNullOrWhiteSpace(nextQuestion.sImage))
                        {
                            PictureBox oPicture = new PictureBox();
                            oPicture.ImageLocation = sPathToImages + nextQuestion.sImage;
                            oPicture.SizeMode = PictureBoxSizeMode.AutoSize;
                            oPanel.Controls.Add(oPicture);
                        }

                        Label oLabelQuestion = new Label();
                        oLabelQuestion.Text = (i + 1).ToString() + ") " + nextQuestion.sText;
                        oLabelQuestion.AutoSize = true;
                        oLabelQuestion.Font = Styling.GetLabelsFont();

                        oPanel.Controls.Add(oLabelQuestion);

                        for (int i1 = 0; i1 < nextQuestion.oAnswers.Count; i1++)
                        {
                            RadioButton oRB = new RadioButton();
                            oRB.Name = i.ToString() + "." + (i1 + 1).ToString();
                            oRB.Font = Styling.GetLabelsFont();
                            Answer oAnswer = (Answer)nextQuestion.oAnswers[i1];
                            if (oAnswer.bIsImage)
                            {
                                oRB.Image = new Bitmap(sPathToImages + oAnswer.sAnswer);
                            }
                            else
                            {
                                oRB.Text = ((Answer)nextQuestion.oAnswers[i1]).sAnswer;
                            }
                            oRB.CheckedChanged += new EventHandler(oRB_CheckedChanged);
                            oRB.AutoSize = true;
                            oPanel.Controls.Add(oRB);
                        }

                        CheckBox oCheckBoxGuess = new CheckBox();
                        oCheckBoxGuess.Name = "checkboxGuess." + i.ToString();
                        oCheckBoxGuess.Text = TextsGlobal.GetText("Guess");
                        oCheckBoxGuess.AutoSize = true;
                        oCheckBoxGuess.CheckedChanged += new EventHandler(oCheckBoxGuess_CheckedChanged);
                        oPanel.Controls.Add(oCheckBoxGuess);

                        flowLayoutPanel1.Controls.Add(oPanel);
                    }
                    catch
                    {
                        MessageBox.Show("Problem loading question " + (i+1).ToString());
                    }
                }

                // start timer
                timer1.Interval = minutes * 60000;
                timer1.Start();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Problem loading test.");
            }
        }

        

        void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                timer1.Stop();
                // display message and move to next screen (without validation)
                MessageBox.Show(TextsGlobal.GetText("TimeIsUp"));
                NextScreen();
            }
            catch { }
        }

        void oRB_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                RadioButton rb = (RadioButton)sender;
                string[] QA = rb.Name.Split(new char[] { '.' });
                int iQuestionIndex = int.Parse(QA[0]);
                if (rb.Checked)
                {
                    ((Question)oArrayListTest[iQuestionIndex]).sChosenAnswer = QA[1];
                }
            }
            catch { }
        }

        void oCheckBoxGuess_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox cb = (CheckBox)sender;
                string[] QA = cb.Name.Split(new char[] { '.' });
                int iQuestionIndex = int.Parse(QA[1]);
                ((Question)oArrayListTest[iQuestionIndex]).bGuess =cb.Checked ;
            }
            catch { }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (ValidateFields())
            {
                timer1.Stop();
                NextScreen();
            }
        }

        private bool ValidateFields()
        {
            try
            {
                for (int i = 0; i < oArrayListTest.Count; i++)
                {
                    Question oQuestion = (Question)oArrayListTest[i];
                    if (String.IsNullOrWhiteSpace(oQuestion.sChosenAnswer))
                    {
                        DialogResult oResult = MessageBox.Show(TextsGlobal.GetText("ErrorNotAllAnswersChosen"), "", MessageBoxButtons.YesNo);
                        if (oResult == DialogResult.Yes)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            catch {
                return false;
            }
        }

        private void flowLayoutPanel1_MouseEnter(object sender, EventArgs e)
        {
            flowLayoutPanel1.Focus();
        }

        private void flowLayoutPanel1_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Focus();
        }
    }
}
