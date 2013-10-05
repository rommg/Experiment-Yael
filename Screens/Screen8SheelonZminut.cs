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
    public partial class Screen8SheelonZminut : UserControl
    {
        public event NextScreenDelegate NextScreen;
        private ArrayList oArrayListTest = new ArrayList();
        private string sXmlPath = "xml/Sheelon.xml"; //"C:\\Users\\Lior\\documents\\visual studio 2010\\Projects\\Experiment\\Experiment\\xml\\Sheelon.xml";

        private class Question
        {
            public string sText;
            public string sChosenAnswer = "";
        }

        public Screen8SheelonZminut()
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
                oArrayListTest.Clear();
            }
            catch { }
        }

        public string XMLPath
        {
            set
            {
                this.sXmlPath = value;
            }
        }

        /// <summary>
        /// call this function to load the screen and start its actions if necessary.
        /// </summary>
        public void LoadScreen()
        {
            LoadData();
            LoadTest();
        }

        public string Result
        {
            get
            {
                string sAnswers = "";
                for (int i = 0; i < oArrayListTest.Count; i++)
                {
                    Question question = (Question)oArrayListTest[i];
                    sAnswers += question.sChosenAnswer + Styling.DelimiterChar;
                }
                return sAnswers;
            }
        }

        private void LoadData()
        {
            try
            {
                buttonNext.Text = TextsGlobal.GetText("Next");
                label1.Text = Texts.GetScreenText(7, 0);

                label1.Font = Styling.GetInstructionsFont();

            }
            catch
            {
                MessageBox.Show("Problem loading screen texts.");
            }
        }

        public void LoadTest()
        {
            try
            {
                FlowLayoutPanel flowLayoutPanel1 = new FlowLayoutPanel();
                flowLayoutPanel1.AutoScroll = true;
                flowLayoutPanel1.Dock = DockStyle.Fill;
                flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
                flowLayoutPanel1.WrapContents = false;
                flowLayoutPanel1.Click += new EventHandler(flowLayoutPanel1_Click);
                flowLayoutPanel1.MouseEnter += new EventHandler(flowLayoutPanel1_MouseEnter);

                tableLayoutPanel1.Controls.Remove(label1);
                tableLayoutPanel1.Controls.Add(flowLayoutPanel1, 0, 0);


                oArrayListTest.Clear();
                // loads all test to oArrayListTest, and displays the first text and title

                System.Xml.XmlReaderSettings oSettings = new System.Xml.XmlReaderSettings();
                oSettings.IgnoreComments = true;
                oSettings.IgnoreWhitespace = true;
                System.Xml.XmlReader oXmlReader = System.Xml.XmlReader.Create(sXmlPath, oSettings);
                
                while (oXmlReader.Read()) // advance to first screen
                {
                    if (oXmlReader.NodeType == System.Xml.XmlNodeType.Element)
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
                            }
                        }
                        else if (oXmlReader.Name == "question")
                        {
                            Question oQuestion = new Question();
                            oQuestion.sText = oXmlReader.ReadString().Trim();
                            oArrayListTest.Add(oQuestion);
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

                        Label oLabelQuestion = new Label();
                        oLabelQuestion.Text = (i + 1).ToString() + ") " + nextQuestion.sText;
                        oLabelQuestion.AutoSize = true;

                        oPanel.Controls.Add(oLabelQuestion);

                        TableLayoutPanel oTable = new TableLayoutPanel();
                        oTable.AutoSize = true;
                        oTable.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
                        // row 1 - titles
                        
                        Label oLabel1 = new Label();
                        oLabel1.Text = Texts.GetScreenText(7, 1);
                        oLabel1.AutoSize = true;
                        int iMaxWidth = oLabel1.PreferredWidth;
                        Label oLabel2 = new Label();
                        oLabel2.Text = Texts.GetScreenText(7, 2);
                        oLabel2.AutoSize = true;
                        if (oLabel2.PreferredWidth > iMaxWidth)
                        {
                            iMaxWidth = oLabel2.PreferredWidth;
                        }
                        Label oLabel3 = new Label();
                        oLabel3.Text = Texts.GetScreenText(7, 3);
                        oLabel3.AutoSize = true;
                        if (oLabel3.PreferredWidth > iMaxWidth)
                        {
                            iMaxWidth = oLabel3.PreferredWidth;
                        }
                        oLabel1.Width = iMaxWidth;
                        oLabel1.AutoSize = false;
                        oLabel2.Width = iMaxWidth;
                        oLabel2.AutoSize = false;
                        oLabel3.Width = iMaxWidth;
                        oLabel3.AutoSize = false;

                        oTable.Controls.Add(oLabel1, 0, 0);
                        oTable.Controls.Add(oLabel2, 3, 0);
                        oTable.Controls.Add(oLabel3, 6, 0);

                        // insert empty labels for fixed sizing
                        for (int indexLabels = 1; indexLabels <= 5; indexLabels++)
                        {
                            if (indexLabels != 3)
                            {
                                Label oEmptyLabelForWidth1 = new Label();
                                oEmptyLabelForWidth1.Width = iMaxWidth;
                                oEmptyLabelForWidth1.AutoSize = false;
                                oEmptyLabelForWidth1.Text = "";
                                oTable.Controls.Add(oEmptyLabelForWidth1, indexLabels, 0);
                            }
                        }
                        
                        for (int i1 = 0; i1 < 7; i1++)
                        {
                            RadioButton oRB = new RadioButton();
                            oRB.Name = i.ToString() + "." + (i1 + 1).ToString();
                            oRB.Text = (i1 + 1).ToString();

                            oRB.CheckedChanged += new EventHandler(oRB_CheckedChanged);
                            oRB.AutoSize = true;
                            oTable.Controls.Add(oRB,i1, 1);
                        }
                        oPanel.Controls.Add(oTable);
                        flowLayoutPanel1.Controls.Add(oPanel);
                    }
                    catch
                    {
                        MessageBox.Show("Problem loading question " + (i + 1).ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problem loading test.");
            }
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

        private bool ValidateFields()
        {
            try
            {
                for (int i = 0; i < oArrayListTest.Count; i++)
                {
                    Question oQuestion = (Question)oArrayListTest[i];
                    if (String.IsNullOrWhiteSpace(oQuestion.sChosenAnswer))
                    {
                        MessageBox.Show(TextsGlobal.GetText("ErrorChooseAnswer") + " " + (i + 1).ToString());
                        return false;
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (ValidateFields())
            {
                NextScreen();
            }
        }

        private void flowLayoutPanel1_Click(object sender, EventArgs e)
        {
            ((Control)sender).Focus();
        }

        private void flowLayoutPanel1_MouseEnter(object sender, EventArgs e)
        {
            ((Control)sender).Focus();
        }
    }
}
