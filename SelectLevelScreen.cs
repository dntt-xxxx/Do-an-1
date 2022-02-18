using System;
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
    public partial class SelectLevelScreen : Form
    {

       public string levelSend;
        public SelectLevelScreen()
        {
            InitializeComponent();
        }
       
        void getLevel(Panel panel)
        {
            RadioButton radioButton = null;
            foreach(RadioButton item in panel.Controls)
            {
                if (item != null)
                {
                    if (item.Checked)
                    {
                        radioButton = item;
                        break;
                    }
                }
            }
            if (radioButton != null)
            {
                levelSend= radioButton.Text;
            }
        }
       
        private void button1_Click(object sender, EventArgs e)
        {
            getLevel(panel1);
            startGameScreen startGameScreen1 = new startGameScreen(levelSend);
            startGameScreen1.Dispose();
            startGameScreen1.Close();
            this.Close();
            
        }
    }
}
