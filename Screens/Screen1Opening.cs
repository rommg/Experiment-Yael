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
    public partial class Screen1Opening : UserControl
    {
        public event NextScreenDelegate NextScreen;

        public Screen1Opening()
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
                textBoxIdNumber.Text = "";
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

        private void LoadData()
        {
            try
            {
                label1.Text = TextsGlobal.GetText("introText");
                label2.Text = TextsGlobal.GetText("enterIdNum");
                buttonNext.Text = TextsGlobal.GetText("Next");

                label1.Font = Styling.GetInstructionsFont();
                label2.Font = Styling.GetLabelsFont();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problem loading screen texts.");
            }
        }

        public string IdNumber
        {
            get
            {
                return textBoxIdNumber.Text;
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
            if (String.IsNullOrWhiteSpace(textBoxIdNumber.Text))
            {
                MessageBox.Show(TextsGlobal.GetText("ErrorIdEmpty"));
                return false;
            }
            if (textBoxIdNumber.Text.Length != 4)
            {
                MessageBox.Show(TextsGlobal.GetText("ErrorIdLength"));
                return false;
            }
            return true;
        }

        private void textBoxIdNumber_KeyDown(object sender, KeyEventArgs e)
        {
            Keys[] allowed = { Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9, Keys.NumPad0, Keys.NumPad1, Keys.NumPad2, Keys.NumPad3, Keys.NumPad4, Keys.NumPad5, Keys.NumPad6, Keys.NumPad7, Keys.NumPad8, Keys.NumPad9, Keys.Back, Keys.Delete };

            if (!allowed.Contains(e.KeyCode))
            {
                e.SuppressKeyPress = true;
            }
        }
    }
}
