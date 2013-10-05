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
    public partial class Screen2KtivaAlArachim : UserControl
    {
        public event NextScreenDelegate NextScreen;

        public Screen2KtivaAlArachim()
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
                checkedListBoxArachim.Items.Clear();
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

        public string ChosenArachim
        {
            get
            {
                string sResult = "";
                foreach (string item in checkedListBoxArachim.CheckedItems)
                {
                    sResult += item + Styling.DelimiterChar.ToString();
                }
                sResult = sResult.Substring(0, sResult.Length - 1); // remove last ','
                return sResult;
            }
        }

        public string Notes
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
                label1.Text = Texts.GetScreenText(1, 0);
                label2.Text = Texts.GetScreenText(1, 1);
                buttonNext.Text = TextsGlobal.GetText("Next");
                string[] arachim = TextsGlobal.GetText("Arachim").Split(new char[] { ',' });

                foreach (string item in arachim)
                {
                    checkedListBoxArachim.Items.Add(item.Trim());
                }

                label1.Font = Styling.GetInstructionsFont();
                label2.Font = Styling.GetInstructionsFont();
            }
            catch
            {
                MessageBox.Show("Problem loading screen texts.");
            }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateFields())
                {
                    NextScreen();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private bool ValidateFields()
        {
            if (checkedListBoxArachim.CheckedItems.Count < 1 || checkedListBoxArachim.CheckedItems.Count > 2)
            {
                MessageBox.Show(TextsGlobal.GetText("ErrorArachimNumberOfSelected"));
                return false;
            }
            if (String.IsNullOrWhiteSpace(textBoxNotes.Text))
            {
                MessageBox.Show(TextsGlobal.GetText("ErrorArachimEmptyNote"));
                return false;
            }
            
            return true;
        }

        private void checkedListBoxArachim_SelectedValueChanged(object sender, EventArgs e)
        {
            if (checkedListBoxArachim.CheckedItems.Count < 1 || checkedListBoxArachim.CheckedItems.Count > 2)
            {
                MessageBox.Show(TextsGlobal.GetText("ErrorArachimNumberOfSelected"));
            }
        }
    }
}
