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
    public partial class Screen7Arachim2 : UserControl
    {
        public event NextScreenDelegate NextScreen;

        private ResultsLogger oLogger;
        public ResultsLogger Logger
        {
            set
            {
                oLogger = value;
            }
        }

        public Screen7Arachim2()
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
                oLogger = null;
                textBoxNotes.Text = "";
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

        private void LoadData()
        {
            try
            {
                buttonNext.Text = TextsGlobal.GetText("Next");
                label1.Text = Texts.GetScreenText(6, 0) + " " + oLogger.screen2_ChosenArachim + Environment.NewLine + Texts.GetScreenText(6, 1);

                label1.Font = Styling.GetInstructionsFont();
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
                    MessageBox.Show(TextsGlobal.GetText("ErrorArachimEmptyNote"));
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
