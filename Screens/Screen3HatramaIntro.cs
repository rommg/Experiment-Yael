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
    public partial class Screen3HatramaIntro : UserControl
    {
        public event NextScreenDelegate NextScreen;
        private string _sTheText = "";
        
        public Screen3HatramaIntro()
        {
            InitializeComponent();
        }

        public string TheText
        {
            set
            {
                _sTheText = value;
            }
        }

        /// <summary>
        /// set the screen to its initial state
        /// </summary>
        public void ResetScreen()
        {
            try
            {
                
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
                label1.Text = _sTheText;
                buttonNext.Text = TextsGlobal.GetText("Next");

                label1.Font = Styling.GetInstructionsFont();
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
                NextScreen();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
