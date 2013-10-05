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
    public partial class ScreenHatramaEnd : UserControl
    {
        public event NextScreenDelegate NextScreen;

        public ScreenHatramaEnd()
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
                radioButton1.Checked = false;
                radioButton2.Checked = false;
                radioButton3.Checked = false;
                radioButton4.Checked = false;
                textBox1.Text = "";
            }
            catch { }
        }

        /// <summary>
        /// call this function to load the screen and start its actions if necessary.
        /// </summary>
        public void LoadScreen()
        {
            ResetScreen();
            LoadData();
        }

        public string Answer1
        {
            get
            {
                return (radioButton1.Checked) ? radioButton1.Text : radioButton2.Text;
            }
        }
        public string Answer2
        {
            get
            {
                return (radioButton3.Checked) ? radioButton3.Text : radioButton4.Text;
            }
        }
        public string Answer3
        {
            get
            {
                return textBox1.Text;
            }
        }

        private void LoadData()
        {
            try
            {
                buttonNext.Text = TextsGlobal.GetText("Next");
                label1.Text = Texts.GetScreenText(2, 2);
                label1.Font = Styling.GetLabelsFont();

                label2.Text = Texts.GetScreenText(2, 3);
                label2.Font = Styling.GetLabelsFont();

                label3.Text = Texts.GetScreenText(2, 4);
                label3.Font = Styling.GetLabelsFont();

                radioButton1.Text = TextsGlobal.GetText("Yes");
                radioButton2.Text = TextsGlobal.GetText("No");
                radioButton3.Text = TextsGlobal.GetText("Yes");
                radioButton4.Text = TextsGlobal.GetText("No");
            }
            catch
            {
                MessageBox.Show("Problem loading screen texts.");
            }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (ValidateFields())
            {
                NextScreen();
            }
        }

        private bool ValidateFields()
        {
            try
            {
                bool isValid = true;
                if (String.IsNullOrWhiteSpace(textBox1.Text))
                {
                    isValid = false;
                }

                if(!radioButton1.Checked && !radioButton2.Checked)
                {
                    isValid = false;
                }

                if (!radioButton3.Checked && !radioButton4.Checked)
                {
                    isValid = false;
                }

                if (!isValid)
                {
                    MessageBox.Show(TextsGlobal.GetText("ErrorAnswerAllQuestions"));
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
