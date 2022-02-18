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
    public partial class NamePlayerForm : Form
    {
        public string namePlayer = "Unknown";
        public bool check = true;
        public NamePlayerForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
           
        }


        
        void getName()
        {
            namePlayer = textBox1.Text;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            
            getName();
            startGameScreen startGameScreen1 = new startGameScreen(namePlayer, check);
            this.Hide();
            button1.Enabled = false;
            textBox1.Enabled = false;
            startGameScreen1.Show();
        }

       
    }
}
