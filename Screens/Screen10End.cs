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
    public partial class Screen10End : UserControl
    {
        public event NextScreenDelegate NextScreen;

        public Screen10End()
        {
            InitializeComponent();
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
                buttonNext.Text = TextsGlobal.GetText("CloseApplication");
                label1.Text = Texts.GetScreenText(10, 0);

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
            catch { }
        }
    }
}
