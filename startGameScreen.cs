using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TetrisGUI
{
    public partial class startGameScreen : Form
    {
        public static string strLevel="Easy";
        public static string namePlayer;
        public startGameScreen()
        {
            InitializeComponent();
            this.Text= "Start Screen Game";
            this.StartPosition = FormStartPosition.CenterScreen;
        }
     
        public startGameScreen(string _levelReceive):this()
        {
            strLevel = _levelReceive;

        }
        public startGameScreen(string _namePlayer,bool _check):this()
        {
            namePlayer = _namePlayer;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            mainScreen mainGameScreen = new mainScreen(namePlayer,strLevel);
            mainGameScreen.Show();
        }
        //select level:
        RadioButton levelRadioButton = new RadioButton();
        SelectLevelScreen formLevel = new SelectLevelScreen();
        private void button2_Click_1(object sender, EventArgs e)
        {
           
            formLevel.StartPosition = FormStartPosition.CenterScreen;
            formLevel.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void startGameScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            
               DialogResult result = MessageBox.Show("Do you want to exit game?",
                            "Exit Screen",
                            MessageBoxButtons.OKCancel,
                            MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button2);
                if (result == DialogResult.OK)
                {
                    Application.Exit();
                }
                else if (result == DialogResult.Cancel)
                {
                    //don't exit
                    e.Cancel = true;
                }
        }
        //function return number line in score file
        //public uint numOfLineFile(string _fileName)
        //{
        //    uint numLine = 0;
        //    using (var sr = new StreamReader(_fileName))
        //    {
        //        string line;

        //        while ((line = sr.ReadLine()) != null)
        //        {
        //            numLine++;
        //        }
        //    }  
             
        //    return numLine;
        //}
        //High score
        private void button3_Click(object sender, EventArgs e)
        {
            string _fileName = "score.txt";
            HighScoreForm highScoreForm1 = new HighScoreForm();
            Button scoButton = new Button();
            highScoreForm1.StartPosition = FormStartPosition.CenterScreen;
            highScoreForm1.AutoSize = true;
            int numLine = 0;
            using (var sr = new StreamReader(_fileName))
            {
                string line;
                
                while ((line = sr.ReadLine()) != null)
                {
                    Label lb = new Label();
                    lb.AutoSize = true;
                    lb.Location = new Point(150, 50 + 30 * numLine);
                    if (numLine == 10)
                    {
                        break;
                    }
                    numLine++;
                    string temp = "";
                    for(int i = 0; i < line.Length; i++)
                    {
                        if (line[i] != '_')
                        {
                            temp += line[i];
                        }
                        else
                        {
                            temp += "  ";
                        }
                        
                    }
                    lb.Text = numLine.ToString() +":    " +"score:"+ temp;
               
                    highScoreForm1.Controls.Add(lb);
                }
            }
            highScoreForm1.ShowDialog();
        }
    }
}
