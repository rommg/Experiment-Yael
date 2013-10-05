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
    public partial class ScreenLastQuestion : UserControl
    {
        public event NextScreenDelegate NextScreen;

        public ScreenLastQuestion()
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
                textBoxNotes.Text = "";
                radioButtonYN1No.Checked = false;
                radioButtonYN2No.Checked = false;
                radioButtonYN3No.Checked = false;
                radioButtonYN1Yes.Checked = false;
                radioButtonYN2Yes.Checked = false;
                radioButtonYN3Yes.Checked = false;
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
                return textBoxNotes.Text;
            }
        }
        public string YesNo1Result
        {
            get
            {
                if (radioButtonYN1No.Checked)
                {
                    return radioButtonYN1No.Text;
                }
                return radioButtonYN1Yes.Text;
            }
        }
        public string YesNo2Result
        {
            get
            {
                if (radioButtonYN2No.Checked)
                {
                    return radioButtonYN2No.Text;
                }
                return radioButtonYN2Yes.Text;
            }
        }
        public string YesNo3Result
        {
            get
            {
                if (radioButtonYN3No.Checked)
                {
                    return radioButtonYN3No.Text;
                }
                return radioButtonYN3Yes.Text;
            }
        }

        private void LoadData()
        {
            try
            {
                buttonNext.Text = TextsGlobal.GetText("Next");
                label1.Text = Texts.GetScreenText(9, 0);
                label1.Font = Styling.GetInstructionsFont();

                labelLanguageQ1.Text = Texts.GetScreenText(9, 1);
                labelLanguageQ1.Font = Styling.GetLabelsFont();

                labelYN1.Text = Texts.GetScreenText(9, 2);
                labelYN1.Font = Styling.GetLabelsFont();

                labelYN2.Text = Texts.GetScreenText(9, 3);
                labelYN2.Font = Styling.GetLabelsFont();

                labelYN3.Text = Texts.GetScreenText(9, 4);
                labelYN3.Font = Styling.GetLabelsFont();

                radioButtonYN1No.Text = Experiment.TextsGlobal.GetText("No");
                radioButtonYN1No.Font = Styling.GetLabelsFont();
                radioButtonYN2No.Text = Experiment.TextsGlobal.GetText("No");
                radioButtonYN2No.Font = Styling.GetLabelsFont();
                radioButtonYN3No.Text = Experiment.TextsGlobal.GetText("No");
                radioButtonYN3No.Font = Styling.GetLabelsFont();
                radioButtonYN1Yes.Text = Experiment.TextsGlobal.GetText("Yes");
                radioButtonYN1Yes.Font = Styling.GetLabelsFont();
                radioButtonYN2Yes.Text = Experiment.TextsGlobal.GetText("Yes");
                radioButtonYN2Yes.Font = Styling.GetLabelsFont();
                radioButtonYN3Yes.Text = Experiment.TextsGlobal.GetText("Yes");
                radioButtonYN3Yes.Font = Styling.GetLabelsFont();
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
                if (String.IsNullOrWhiteSpace(textBoxNotes.Text))
                {
                    MessageBox.Show(TextsGlobal.GetText("ErrorWhatIsYourLanguageEmpty"));
                    return false;
                }
                else if (!radioButtonYN1No.Checked && !radioButtonYN1Yes.Checked)
                {
                    MessageBox.Show(TextsGlobal.GetText("ErrorYesNoQuestionNotAnswered"));
                    return false;
                }
                else if (!radioButtonYN2No.Checked && !radioButtonYN2Yes.Checked)
                {
                    MessageBox.Show(TextsGlobal.GetText("ErrorYesNoQuestionNotAnswered"));
                    return false;
                }
                else if (!radioButtonYN3No.Checked && !radioButtonYN3Yes.Checked)
                {
                    MessageBox.Show(TextsGlobal.GetText("ErrorYesNoQuestionNotAnswered"));
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
