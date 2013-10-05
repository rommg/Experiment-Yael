using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Experiment.Screens;

namespace Experiment
{
    public delegate void NextScreenDelegate();
    public delegate void KeyDownPressed(KeyEventArgs e);

    public partial class Form1 : Form
    {
        private UserControl[] Screens;
        private int[] currentUserScreensIndexes;
        //private int[] group1ScreenOrder = { 1, 2, 5, 6, 7, 6, 8, 9, 10, 11 }; // Guy: removed group 1 screen order
        private int[] group2ScreenOrder = { 1, 3, 4, 12, 5, 6, 13, 3, 4, 12, 6, 8, 9, 10, 11 }; // 12 = HatramaEnd  Guy: added screen 13 - Thunot
        // private int[] group2ScreenOrder = { 1, 3, 4, 12, 13 , 8, 9, 10, 11 }; // Guy: for version without math part
        private KeyDownPressed currentKeyDownReceiver;
        private ResultsLogger oResultsLogger;
        private int iScreenIndex;
        private Tester oCurrentTester;
        private int iCurrentMathTestNum;
        private int iCurrentHatrama;
        private string sMath1XmlPath = "xml/Math1.xml"; //"C:\\Users\\Lior\\documents\\visual studio 2010\\Projects\\Experiment\\Experiment\\xml\\Math1.xml";
        private string sMath2XmlPath = "xml/Math2.xml"; //"C:\\Users\\Lior\\documents\\visual studio 2010\\Projects\\Experiment\\Experiment\\xml\\Math2.xml";
        private int[] mathTestMinutes = { 20, 15 }; // is set again from preferences in loaddata()
        private bool bWroteFile = false;
        private int iHatramaNumber = 1;

        public Form1()
        {
            InitializeComponent();
            LoadData();
            Properties.Settings.Default.Folder_saveResults = "results/";
        }

        private void LoadData()
        {
            try
            {
                TextsGlobal.PathToXml = "xml/TextsGlobal.xml"; //"C:\\Users\\Lior\\documents\\visual studio 2010\\Projects\\Experiment\\Experiment\\xml\\TextsGlobal.xml";
                Testers.PathToXml = "xml/testers.xml"; //"C:\\Users\\Lior\\documents\\visual studio 2010\\Projects\\Experiment\\Experiment\\xml\\testers.xml";
                Preferences.PathToXml = "xml/Preferences.xml";

                mathTestMinutes[0] = int.Parse(Preferences.GetPreference("math1TimeMinuts"));
                mathTestMinutes[1] = int.Parse(Preferences.GetPreference("math2TimeMinuts"));

                oResultsLogger = new ResultsLogger();
                iScreenIndex = 0;
                iCurrentMathTestNum = 1;
                iCurrentHatrama = 1;

                // initialize screens
                Screen1Opening Screen1 = new Screen1Opening();
                Screen2KtivaAlArachim Screen2 = new Screen2KtivaAlArachim();
                Screen3HatramaIntro Screen3 = new Screen3HatramaIntro();
                Screen4Hatrama Screen4 = new Screen4Hatrama();
                Screen5MathIntro Screen5 = new Screen5MathIntro();
                Screen6Math Screen6 = new Screen6Math();
                Screen7Arachim2 Screen7 = new Screen7Arachim2();
                Screen8SheelonZminut Screen8 = new Screen8SheelonZminut();
                Screen9LexicalDecision Screen9 = new Screen9LexicalDecision();
                ScreenLastQuestion ScreenLastQuestion = new Experiment.Screens.ScreenLastQuestion();
                Screen10End Screen10 = new Screen10End();
                Screen11Thunot Screen11 = new Screen11Thunot();
                ScreenHatramaEnd Screen12 = new ScreenHatramaEnd();

                Screen1.Dock = DockStyle.Fill;
                Screen2.Dock = DockStyle.Fill;
                Screen3.Dock = DockStyle.Fill;
                Screen4.Dock = DockStyle.Fill;
                Screen5.Dock = DockStyle.Fill;
                Screen6.Dock = DockStyle.Fill;
                Screen7.Dock = DockStyle.Fill;
                Screen8.Dock = DockStyle.Fill;
                Screen9.Dock = DockStyle.Fill;
                ScreenLastQuestion.Dock = DockStyle.Fill;
                Screen10.Dock = DockStyle.Fill;
                Screen11.Dock = DockStyle.Fill;
                Screen12.Dock = DockStyle.Fill;

                Screen1.NextScreen += new NextScreenDelegate(GotoNextScreen);
                Screen2.NextScreen += new NextScreenDelegate(GotoNextScreen);
                Screen3.NextScreen += new NextScreenDelegate(GotoNextScreen);
                Screen4.NextScreen += new NextScreenDelegate(GotoNextScreen);
                Screen5.NextScreen += new NextScreenDelegate(GotoNextScreen);
                Screen6.NextScreen += new NextScreenDelegate(GotoNextScreen);
                Screen7.NextScreen += new NextScreenDelegate(GotoNextScreen);
                Screen8.NextScreen += new NextScreenDelegate(GotoNextScreen);
                Screen9.NextScreen += new NextScreenDelegate(GotoNextScreen);
                ScreenLastQuestion.NextScreen += new NextScreenDelegate(GotoNextScreen);
                Screen10.NextScreen += new NextScreenDelegate(GotoNextScreen);
                Screen11.NextScreen += new NextScreenDelegate(GotoNextScreen);
                Screen12.NextScreen += new NextScreenDelegate(GotoNextScreen);

                Screens = new UserControl[] { Screen1, Screen2, Screen3, Screen4, Screen5, Screen6, Screen7, Screen8, Screen9, ScreenLastQuestion, Screen10, Screen12, Screen11 };

                // insert all to main form
                foreach (UserControl uc in Screens)
                {
                    this.Controls.Add(uc);
                }

                // show first screen
                LoadScreen(1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problem loading form:" + Environment.NewLine + ex.Message);
            }
        }

        private void ResetApplication() // not in use
        {
            try
            {
                oResultsLogger = new ResultsLogger();
                iScreenIndex = 0;
                iCurrentMathTestNum = 1;
                iCurrentHatrama = 1;

                // reset all screens
                ((Screen1Opening)Screens[0]).ResetScreen();
                ((Screen2KtivaAlArachim)Screens[1]).ResetScreen();
                ((Screen3HatramaIntro)Screens[2]).ResetScreen();
                ((Screen4Hatrama)Screens[3]).ResetScreen();
                ((Screen5MathIntro)Screens[4]).ResetScreen();
                ((Screen6Math)Screens[5]).ResetScreen();
                ((Screen7Arachim2)Screens[6]).ResetScreen();
                ((Screen8SheelonZminut)Screens[7]).ResetScreen();
                ((Screen9LexicalDecision)Screens[8]).ResetScreen();
                ((Screen11Thunot)Screens[10]).ResetScreen();

                // show first screen
                LoadScreen(1);
            }
            catch { }
        }

        private void ShowScreen(int iScreenNum)
        {
            // hide all screens
            foreach (UserControl oUC in Screens)
            {
                oUC.Visible = false;
            }

            // show the Screen
            Screens[iScreenNum - 1].Visible = true;
        }

        private void LoadScreen(int iScreenNum)
        {
            // set keydown to no action
            currentKeyDownReceiver = new KeyDownPressed(OnKeyDownPressedNoAction);
            Cursor.Current = Cursors.WaitCursor;
            switch (iScreenNum)
            {
                case 1:
                    ((Screen1Opening)Screens[0]).LoadScreen();
                    break;
                case 2:
                    ((Screen2KtivaAlArachim)Screens[1]).LoadScreen();
                    break;
                case 3:
                    Screen3HatramaIntro oScreen3HatramaIntro = (Screen3HatramaIntro)Screens[2];
                    if (iHatramaNumber == 1)
                    {
                        oScreen3HatramaIntro.TheText = Texts.GetScreenText(2, 0);
                    }
                    else
                    {
                        oScreen3HatramaIntro.TheText = Texts.GetScreenText(2, 1);
                    }
                    oScreen3HatramaIntro.LoadScreen();
                    break;

                case 4:
                    // Guy: general: the veriable HatramaWord is used as a sign for Screen4Hatrama, whether to use the Person veriable or an array of words/
                    ((Screen4Hatrama)Screens[3]).Person = oCurrentTester.familiar_name;
                    ((Screen4Hatrama)Screens[3]).HatramaWord = "noWord";

                    // Guy: group 4 gets unfamiliar_name for hatrama
                    if (oCurrentTester.Group == 4)
                    {
                        ((Screen4Hatrama)Screens[3]).Person = oCurrentTester.unfamiliar_name;
                    }

                    //Guy: set the number of tests according to group
                    if (oCurrentTester.Group == 1)
                    {
                        // ((Screen4Hatrama)Screens[3]).numOfTests = 60;
                        ((Screen4Hatrama)Screens[3]).numOfTests = int.Parse(Preferences.GetPreference("hatramaNumberOfTestsGroup1and2"));
                        ((Screen4Hatrama)Screens[3]).HatramaWord = "group1array"; // array of words for group1
                    }
                    else if (oCurrentTester.Group == 2)
                    {
                        // ((Screen4Hatrama)Screens[3]).numOfTests = 60;
                        ((Screen4Hatrama)Screens[3]).numOfTests = int.Parse(Preferences.GetPreference("hatramaNumberOfTestsGroup1and2"));
                        ((Screen4Hatrama)Screens[3]).HatramaWord = "group2array"; // array of words for group2
                    }
                    else if (oCurrentTester.Group == 3 || oCurrentTester.Group == 4)// groups 3 & 4 only 20 hatrama steps
                    {
                        ((Screen4Hatrama)Screens[3]).numOfTests = int.Parse(Preferences.GetPreference("hatramaNumberOfTestsGroup3and4"));
                        //((Screen4Hatrama)Screens[3]).numOfTests = 20;
                    }

                    //else if (oCurrentTester.Group == 3) // gets only unfamiliar_name, the hatrama will choose between Person and HatramaWord but it will always be unfamiliar_name
                    //{
                    //    ((Screen4Hatrama)Screens[3]).Person = oCurrentTester.unfamiliar_name;
                    //    ((Screen4Hatrama)Screens[3]).HatramaWord = oCurrentTester.unfamiliar_name;
                    //}
                    //else if (oCurrentTester.Group == 4) // gets only familiar_name, the hatrama will choose between Person and HatramaWord but it will always be familiar_name
                    //{
                    //    ((Screen4Hatrama)Screens[3]).Person = oCurrentTester.familiar_name;
                    //    ((Screen4Hatrama)Screens[3]).HatramaWord = oCurrentTester.familiar_name;
                    //}
                    //else
                    //{
                    //    ((Screen4Hatrama)Screens[3]).Person = oCurrentTester.familiar_name;
                    //    ((Screen4Hatrama)Screens[3]).HatramaWord = TextsGlobal.GetText("HatramaSafeBase");
                    //}

                    currentKeyDownReceiver = new KeyDownPressed(((Screen4Hatrama)Screens[3]).OnKeyDownPressed);
                    ((Screen4Hatrama)Screens[3]).LoadScreen();
                    break;
                case 5:
                    ((Screen5MathIntro)Screens[4]).IsHard = false;
                    if (oCurrentTester.Group == 1 || oCurrentTester.Group == 5)
                    {
                        ((Screen5MathIntro)Screens[4]).IsHard = true;
                    }
                    ((Screen5MathIntro)Screens[4]).LoadScreen();
                    break;
                case 6:
                    ((Screen6Math)Screens[5]).LoadScreen();
                    if (iCurrentMathTestNum == 1)
                    {
                        ((Screen6Math)Screens[5]).LoadTest(sMath1XmlPath, mathTestMinutes[0]);
                    }
                    else if (iCurrentMathTestNum == 2)
                    {
                        ((Screen6Math)Screens[5]).LoadTest(sMath2XmlPath, mathTestMinutes[1]);
                    }
                    break;
                case 7:
                    ((Screen7Arachim2)Screens[6]).Logger = oResultsLogger;
                    ((Screen7Arachim2)Screens[6]).LoadScreen();
                    break;
                case 8:
                    ((Screen8SheelonZminut)Screens[7]).LoadScreen();
                    break;
                case 9:
                    currentKeyDownReceiver = new KeyDownPressed(((Screen9LexicalDecision)Screens[8]).OnKeyDownPressed);
                    ((Screen9LexicalDecision)Screens[8]).LoadScreen();
                    break;
                case 10:
                    ((ScreenLastQuestion)Screens[9]).LoadScreen();
                    break;
                case 11:
                    ((Screen10End)Screens[10]).LoadScreen();
                    break;
                case 12:
                    ((ScreenHatramaEnd)Screens[11]).LoadScreen();
                    break;
                case 13:
                    ((Screen11Thunot)Screens[12]).LoadScreen();
                    break;
            }
            ShowScreen(iScreenNum);
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// this function is being called when we are out of a screen, and want to go to the next one
        /// </summary>
        private void GotoNextScreen()
        {
            if (iScreenIndex == 0)
            {
                // get id, search for it.
                try
                {
                    oCurrentTester = Testers.GetTester(((Screen1Opening)Screens[0]).IdNumber);
                    if (oCurrentTester == null)
                    {
                        MessageBox.Show(TextsGlobal.GetText("ErrorIdNotExist"));
                    }
                    else
                    {
                        /*if (oCurrentTester.Group == 1)
                        {
                            currentUserScreensIndexes = group1ScreenOrder;
                        }
                        else
                        {
                            currentUserScreensIndexes = group2ScreenOrder;
                        }
                         */
                        // Guy: removed group 1 screen order

                        currentUserScreensIndexes = group2ScreenOrder;

                        if (oCurrentTester.Gender == "female")
                        {
                            Texts.PathToXml = "xml/Texts_female.xml";
                            ((Screen8SheelonZminut)Screens[7]).XMLPath = "xml/Sheelon_female.xml";
                            ((Screen11Thunot)Screens[12]).XMLPath = "xml/Thunot_female.xml"; //Guy: set Thunot xml name
                        }
                        else
                        {
                            Texts.PathToXml = "xml/Texts_male.xml";
                            ((Screen8SheelonZminut)Screens[7]).XMLPath = "xml/Sheelon_male.xml";
                            ((Screen11Thunot)Screens[12]).XMLPath = "xml/Thunot_male.xml"; //Guy: set Thunot xml name
                        }

                        // add data to logger
                        oResultsLogger.familiar_name = oCurrentTester.familiar_name;
                        oResultsLogger.Gender = oCurrentTester.Gender;
                        oResultsLogger.Group = oCurrentTester.Group;
                        oResultsLogger.Id = oCurrentTester.Id;
                        oResultsLogger.unfamiliar_name = oCurrentTester.unfamiliar_name;
                        oResultsLogger.oTesterDatetime = DateTime.Now;

                        // goto next screen
                        iScreenIndex++;
                        LoadScreen(currentUserScreensIndexes[iScreenIndex]);
                    }
                }
                catch (Exception ex)
                {
                }
            }
            else if (currentUserScreensIndexes[iScreenIndex] == 2)
            {
                oResultsLogger.screen2_ChosenArachim = ((Screen2KtivaAlArachim)Screens[1]).ChosenArachim;
                oResultsLogger.screen2_notes = ((Screen2KtivaAlArachim)Screens[1]).Notes;

                // goto next screen
                iScreenIndex++;
                LoadScreen(currentUserScreensIndexes[iScreenIndex]);
            }
            else if (currentUserScreensIndexes[iScreenIndex] == 3)
            {
                iHatramaNumber++;
                // goto next screen
                iScreenIndex++;
                LoadScreen(currentUserScreensIndexes[iScreenIndex]);
            }
            else if (currentUserScreensIndexes[iScreenIndex] == 4)
            {
                if (iCurrentHatrama == 1)
                {
                    oResultsLogger.screen4Hatrama_result = ((Screen4Hatrama)Screens[3]).Result;
                }
                else if (iCurrentHatrama == 2)
                {
                    oResultsLogger.screen4Hatrama_again_result = ((Screen4Hatrama)Screens[3]).Result;
                }

                // goto next screen
                iScreenIndex++;
                iCurrentHatrama++;
                LoadScreen(currentUserScreensIndexes[iScreenIndex]);
            }
            else if (currentUserScreensIndexes[iScreenIndex] == 12) // Hatrama end
            {
                if (iCurrentHatrama == 2) // after 1st hatrama
                {
                    oResultsLogger.screenHatramaEnd_1_Answer1 = ((ScreenHatramaEnd)Screens[11]).Answer1;
                    oResultsLogger.screenHatramaEnd_1_Answer2 = ((ScreenHatramaEnd)Screens[11]).Answer2;
                    oResultsLogger.screenHatramaEnd_1_Answer3 = ((ScreenHatramaEnd)Screens[11]).Answer3;
                }
                if (iCurrentHatrama == 3) // after 2nd hatrama
                {
                    oResultsLogger.screenHatramaEnd_2_Answer1 = ((ScreenHatramaEnd)Screens[11]).Answer1;
                    oResultsLogger.screenHatramaEnd_2_Answer2 = ((ScreenHatramaEnd)Screens[11]).Answer2;
                    oResultsLogger.screenHatramaEnd_2_Answer3 = ((ScreenHatramaEnd)Screens[11]).Answer3;
                }
                // goto next screen
                iScreenIndex++;
                LoadScreen(currentUserScreensIndexes[iScreenIndex]);
            }
            else if (currentUserScreensIndexes[iScreenIndex] == 5)
            {
                oResultsLogger.screen5MathIntro_Gender = ((Screen5MathIntro)Screens[4]).Gender;
                // goto next screen
                iScreenIndex++;
                LoadScreen(currentUserScreensIndexes[iScreenIndex]);
            }
            else if (currentUserScreensIndexes[iScreenIndex] == 6)
            {
                if (iCurrentMathTestNum == 1)
                {
                    oResultsLogger.screen6Math_1_result = ((Screen6Math)Screens[5]).Result;
                }
                else if (iCurrentMathTestNum == 2)
                {
                    oResultsLogger.screen6Math_2_result = ((Screen6Math)Screens[5]).Result;
                }

                // goto next screen
                iScreenIndex++;
                iCurrentMathTestNum++;
                LoadScreen(currentUserScreensIndexes[iScreenIndex]);
            }
            else if (currentUserScreensIndexes[iScreenIndex] == 7)
            {
                oResultsLogger.screen2_notes_again = ((Screen7Arachim2)Screens[6]).Result;
                // goto next screen
                iScreenIndex++;
                LoadScreen(currentUserScreensIndexes[iScreenIndex]);
            }
            else if (currentUserScreensIndexes[iScreenIndex] == 8)
            {
                oResultsLogger.screen8Sheelon_result = ((Screen8SheelonZminut)Screens[7]).Result;
                // goto next screen
                iScreenIndex++;
                LoadScreen(currentUserScreensIndexes[iScreenIndex]);
            }
            else if (currentUserScreensIndexes[iScreenIndex] == 9)
            {
                oResultsLogger.screen9Lexical_result = ((Screen9LexicalDecision)Screens[8]).Result;
                // goto next screen
                iScreenIndex++;
                LoadScreen(currentUserScreensIndexes[iScreenIndex]);
            }
            else if (currentUserScreensIndexes[iScreenIndex] == 10) // last question
            {
                oResultsLogger.screenLastQuestionYourLanguage = ((ScreenLastQuestion)Screens[9]).Result;
                oResultsLogger.screenLastQuestionYesNo1 = ((ScreenLastQuestion)Screens[9]).YesNo1Result;
                oResultsLogger.screenLastQuestionYesNo2 = ((ScreenLastQuestion)Screens[9]).YesNo2Result;
                oResultsLogger.screenLastQuestionYesNo3 = ((ScreenLastQuestion)Screens[9]).YesNo3Result;
                // goto next screen
                iScreenIndex++;
                LoadScreen(currentUserScreensIndexes[iScreenIndex]);
            }
            else if (currentUserScreensIndexes[iScreenIndex] == 11)
            {
                try
                {
                    oResultsLogger.WriteFile();
                    bWroteFile = true;
                }
                catch { }
                this.Close();
            }
            //Guy: new screen number 13
            else if (currentUserScreensIndexes[iScreenIndex] == 13)
            {
                
                oResultsLogger.screen13Sheelon_result = ((Screen11Thunot)Screens[12]).Result;
                // goto next screen
                iScreenIndex++;
                LoadScreen(currentUserScreensIndexes[iScreenIndex]);
            }

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                currentKeyDownReceiver(e);
            }
            catch { }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (!bWroteFile)
                {
                    oResultsLogger.WriteFile();
                }
            }
            catch { }
        }

        public void OnKeyDownPressedNoAction(KeyEventArgs e)
        {

        }
    }
}
