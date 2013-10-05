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
    public partial class Screen5MathIntro : UserControl
    {
        public event NextScreenDelegate NextScreen;
        private bool bIsHard = true;
        private string sSelectedGender = "";

        public Screen5MathIntro()
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
                bIsHard = true;
                sSelectedGender = "";
                radioButtonFemale.Checked = false;
                radioButtonMale.Checked = false;
            }
            catch { }
        }

        public bool IsHard
        {
            set
            {
                bIsHard = value;
            }
        }
        /// <summary>
        /// call this function to load the screen and start its actions if necessary.
        /// </summary>
        public void LoadScreen()
        {
            LoadData();
        }

        public string Gender
        {
            get
            {
                return sSelectedGender;
            }
        }

        private void LoadData()
        {
            try
            {
                groupBoxMigdar.Text = Texts.GetScreenText(4, 0);
                labelText.Text = Texts.GetScreenText(4, 1); // instructions math
                radioButtonMale.Text = TextsGlobal.GetText("Male");
                radioButtonFemale.Text = TextsGlobal.GetText("Female");
                // choose between 2 versions of text - (4,2)/(4,3)
                if (bIsHard)
                {
                    groupBoxMigdar.Visible = true;
                }
                else
                {
                    groupBoxMigdar.Visible = false;
                }
                
                buttonNext.Text = TextsGlobal.GetText("Next");

                labelText.Font = Styling.GetInstructionsFont();
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
            if (bIsHard && String.IsNullOrWhiteSpace(sSelectedGender))
            {
                MessageBox.Show(TextsGlobal.GetText("ErrorMigdar"));
                return false;
            }
            return true;
        }

        private void radioButtonMale_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonMale.Checked)
            {
                sSelectedGender = radioButtonMale.Text;
            }
        }

        private void radioButtonFemale_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonFemale.Checked)
            {
                sSelectedGender = radioButtonFemale.Text;
            }
        }
    }
}
