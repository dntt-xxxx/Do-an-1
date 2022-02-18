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
    public partial class mainScreen : Form
    {
        Shape currentShape;
        Shape nextShape;
        Timer timer = new Timer();
        //khai báo delegate truyền level từ formlevel sang form này.
       

        public mainScreen()
        {
            //InitializeComponent();

            //loadCanvas();
            //currentShape = getRandomShapeWithCenterAligned();
            //nextShape = getNextShape();


            //timer.Tick += Timer_Tick;
            //if (strLevel == "Easy")
            //{
            //    timer.Interval = 700;
            //}
            //else if (strLevel == "Medium")
            //{
            //    timer.Interval = 500;
            //}
            //else if (strLevel == "Hard")
            //{
            //    timer.Interval = 350;
            //}
            //timer.Interval = setSpeedLevel(strLevel);
            //timer.Start();


            //this.KeyDown += Form1_KeyDown;
        }
        //khởi tạo để truyền level từ form level
        public static string strLevel = "Easy";
        public static string playerName = "Unknown";
        public mainScreen(string NamePlayer, string levelrecive) : this()
        {
            playerName = NamePlayer;
            strLevel = levelrecive;
            InitializeComponent();
            
            loadCanvas();
            currentShape = getRandomShapeWithCenterAligned();
            nextShape = getNextShape();


            timer.Tick += Timer_Tick;
            if (strLevel == "Easy")
            {
                timer.Interval = 700;
            }
            else if (strLevel == "Medium")
            {
                timer.Interval = 500;
            }
            else if (strLevel == "Hard")
            {
                timer.Interval = 350;
            }
            timer.Interval = setSpeedLevel(strLevel);
            timer.Start();


            this.KeyDown += Form1_KeyDown;

        }
       
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            var verticalMove = 0;
            var horizontalMove = 0;

            switch (e.KeyCode)
            {
                case Keys.Left:
                    verticalMove--;
                    break;
                case Keys.Right:
                    verticalMove++;
                    break;
                case Keys.Down:
                    horizontalMove++;
                    break;
                case Keys.Up:
                    currentShape.turn();
                    break;
                default:
                    return;
            }
            var isMoveSuccess = moveShapeIfPossible(horizontalMove, verticalMove);
            if (!isMoveSuccess && e.KeyCode == Keys.Up)
            {
                currentShape.rollback();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var isMoveSuccess = moveShapeIfPossible(moveDown: 1);

            // if shape reached bottom or touched any other shapes
            if (!isMoveSuccess)
            {
                // copy working image into canvas image
                canvasBitmap = new Bitmap(workingBitmap);

                updateCanvasDotArrayWithCurrentShape();

                // get next shape
                currentShape = nextShape;
                nextShape = getNextShape();
                clearFilledRowAndUpdateScore();

            }
        }

        Bitmap canvasBitmap;
        Graphics canvasGraphics;
        int canvasWidth = 15;
        int canvasHeight = 25;
        int[,] canvasDotArray;
        int dotSize = 20;
        static DateTime timeStartPlay;
        private void loadCanvas()
        {
            Size size = new Size(600, 600);
            this.Size = size;

            // Resize the picture box based on the dotsize and canvas size
            pictureBox1.Width = canvasWidth * dotSize;
            pictureBox1.Height = canvasHeight * dotSize;
            //Create Bitmap with picture box's size
            canvasBitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            canvasGraphics = Graphics.FromImage(canvasBitmap);

            canvasGraphics.FillRectangle(Brushes.LightGray, 0, 0, canvasBitmap.Width, canvasBitmap.Height);

            //Load bitmap into picture box
            pictureBox1.Image = canvasBitmap;


            label1.Location = new Point(pictureBox1.Location.X + pictureBox1.Width + 50, pictureBox1.Location.Y + 240);

            //pictureBox2
            pictureBox2.Location = new Point(pictureBox1.Location.X + pictureBox1.Width + 50, pictureBox1.Location.Y + 30);
            //lable2-picturebox2
            label2.Location = new Point(pictureBox1.Location.X + pictureBox1.Width + 50, pictureBox1.Location.Y);

            //level: 
            label3.Location = new Point(pictureBox1.Location.X + pictureBox1.Width + 50, pictureBox1.Location.Y + 160);
            label3.Text = "Level: " + strLevel;

            //namePlayer:
            label5.Location = new Point(pictureBox1.Location.X + pictureBox1.Width + 50, pictureBox1.Location.Y + 200);
            label5.Text = "Player: " + playerName;

            label6.Location= new Point(pictureBox1.Location.X - 30 + pictureBox1.Width + 50, pictureBox1.Location.Y + 360);
            

            label4.Location = new Point(pictureBox1.Location.X - 30 + pictureBox1.Width + 50, pictureBox1.Location.Y + 400);
            label4.Text = "Hướng Dẫn:" + "\n" + "Mũi tên lên: xoay" + "\n" + "Mũi tên phải: di chuyển hình sang phải" +
                          "\n" + "Mũi tên trái: di chuyển hình sang trái" + "\n" + "Mũi tên xuống: tăng tốc độ rơi của hình";
            // Initialize canvas dot array. by default all elements are zero
            canvasDotArray = new int[canvasWidth, canvasHeight];
        }
        int speedDown;
        int setSpeedLevel(string Level)
        {
            if (Level == "Easy")
            {
                speedDown = 700;
            }
            else if (Level == "Medium")
            {
                speedDown = 500;
            }
            else if (Level == "Hard")
            {
                speedDown = 350;
            }

            return speedDown;
        }
        int CurrentX;
        int CurrentY;
        private Shape getRandomShapeWithCenterAligned()
        {
            var shape = ShapeHandle.GetRandomShape();
            CurrentX = 7;
            CurrentY = -shape.Height;

            return shape;
        }

        private bool moveShapeIfPossible(int moveDown = 0, int moveSide = 0)
        {
            var newX = CurrentX + moveSide;
            var newY = CurrentY + moveDown;

            // check if it reaches the bottom or side bar
            if (newX < 0 || newX + currentShape.Weight > canvasWidth
                || newY + currentShape.Height > canvasHeight)
                return false;

            // check if it touches any other blocks 
            for (int i = 0; i < currentShape.Weight; i++)
            {
                for (int j = 0; j < currentShape.Height; j++)
                {
                    if (newY + j > 0 && canvasDotArray[newX + i, newY + j] == 1 && currentShape.Dots[j, i] == 1)
                    {
                        return false;
                    }

                }
            }

            CurrentX = newX;
            CurrentY = newY;

            drawShape();

            return true;
        }

        Bitmap workingBitmap;
        Graphics workingGraphics;
        private void drawShape()
        {
            workingBitmap = new Bitmap(canvasBitmap);
            workingGraphics = Graphics.FromImage(workingBitmap);
            for (int i = 0; i < currentShape.Weight; i++)
            {
                for (int j = 0; j < currentShape.Height; j++)
                {
                    if (currentShape.Dots[j, i] == 1)
                        workingGraphics.FillRectangle(Brushes.Black,
                                                    (CurrentX + i) * dotSize,
                                                    (CurrentY + j) * dotSize,
                                                    dotSize,
                                                    dotSize);

                }
            }
            pictureBox1.Image = workingBitmap;
        }
        private void updateCanvasDotArrayWithCurrentShape()
        {
            
            for (int i = 0; i < currentShape.Weight; i++)
            {
                for (int j = 0; j < currentShape.Height; j++)
                {
                    if (currentShape.Dots[j, i] == 1)
                    {
                        if (!checkIfGameOver(ref checkEndGame))
                        {
                            canvasDotArray[CurrentX + i, CurrentY + j] = 1;
                        }
                        else
                        {
                            break;
                        }
                        //break;
                    }
                }
                if (checkEndGame)
                {
                    break;
                }
            }
        }
        uint score;

        public void clearFilledRowAndUpdateScore()
        {
            for (int i = 0; i < canvasHeight; i++)
            {
                int j;
                for (j = canvasWidth - 1; j >= 0; j--)
                {
                    if (canvasDotArray[j, i] == 0)
                    {
                        break;
                    }
                }
                if (j == -1)
                {
                    score++;
                    label1.Text = "Score: " + score.ToString();
                    //

                    //if (score >= 0 && score < 2)
                    //{
                    //    timer.Interval = 400;
                    //}
                    //else if (score >= 2 && score < 4)
                    //{
                    //    timer.Interval = 300;
                    //}
                    //else
                    //{
                    //    timer.Interval = 200;
                    //}

                    //increase speed:
                    // timer.Interval -= 20;

                    //update canvasDotArray
                    for (j = 0; j < canvasWidth; j++)
                    {
                        for (int k = i; k > 0; k--)
                        {
                            canvasDotArray[j, k] = canvasDotArray[j, k - 1];
                        }
                        canvasDotArray[j, 0] = 0;
                    }
                }
            }
            //Draw update canvasDotArray
            for (int i = 0; i < canvasWidth; i++)
            {
                for (int j = 0; j < canvasHeight; j++)
                {
                    canvasGraphics = Graphics.FromImage(canvasBitmap);
                    canvasGraphics.FillRectangle(canvasDotArray[i, j] == 1 ? Brushes.Black : Brushes.LightGray,
                                                  i * dotSize, j * dotSize, dotSize, dotSize);
                }
            }
            pictureBox1.Image = canvasBitmap;
        }

        Bitmap nextShapeBitmap;
        Graphics nextShapeGraphics;

        private Shape getNextShape()
        {
            var shape = getRandomShapeWithCenterAligned();
            nextShapeBitmap = new Bitmap(6 * dotSize, 6 * dotSize);
            nextShapeGraphics = Graphics.FromImage(nextShapeBitmap);

            nextShapeGraphics.FillRectangle(Brushes.LightGray, 0, 0, nextShapeBitmap.Width, nextShapeBitmap.Height);
            //possition shape in pictureBox2:
            var nextShapeX = (6 - shape.Weight) / 2;
            var nextShapeY = (6 - shape.Height) / 2;
            for (int i = 0; i < shape.Height; i++)
            {
                for (int j = 0; j < shape.Weight; j++)
                {
                    nextShapeGraphics.FillRectangle(shape.Dots[i, j] == 1 ? Brushes.Black : Brushes.LightGray,
                                                    (nextShapeX + j) * dotSize, (nextShapeY + i) * dotSize, dotSize, dotSize);

                }
            }
            pictureBox2.Size = nextShapeBitmap.Size;
            pictureBox2.Image = nextShapeBitmap;
            return shape;

        }
        public void HighScore()
        {
            bool doneWrite = false;
            string tempFile = Path.GetTempFileName();

            using (var sr = new StreamReader("score.txt"))
            using (var sw = new StreamWriter(tempFile))
            {
                string line;
                string temp = null;

                while ((line = sr.ReadLine()) != null)
                {
                    uint scoreR = 0;
                    for (int i = 0; i < line.Length; i++)
                    {
                        if (line[i] != '_')
                        {

                            scoreR = scoreR * 10 + line[i] - '0';
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (score == scoreR)
                    {
                        doneWrite = true;
                    }
                    if (score > scoreR && doneWrite == false)
                    {
                        doneWrite = true;
                        temp = score.ToString() + "_" + playerName + "_" + DateTime.Now.ToLongDateString();
                        sw.WriteLine(temp);
                    }
                    sw.WriteLine(line);
                }
            }

            File.Delete("score.txt");
            File.Move(tempFile, "score.txt");
        }
       public bool checkEndGame=false;
        private bool checkIfGameOver(ref bool _checkEndGame)
        {
            _checkEndGame = false;

            if (CurrentY < 0)
            {
                timer.Stop();
                HighScore();
                DialogResult result = MessageBox.Show("Game Over, Do you want to play again?",
                    "GameOver Screen",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1);

                if (result == DialogResult.OK)
                {
                    _checkEndGame = true;
                    this.Close();
                    mainScreen mainScreen1 = new mainScreen(playerName,strLevel);
                    mainScreen1.Show();
                }
                else if (result == DialogResult.Cancel)
                {
                    _checkEndGame = true;
                    this.Close();
                }
               

            }
            return _checkEndGame;
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //pause: 
        
        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Stop();

             resumForm resumForm1 = new resumForm();
             Button resumeButton = new Button();
            resumForm1.Text = "";
            
            resumForm1.StartPosition = FormStartPosition.CenterScreen;
            resumForm1.Size = new Size(this.Size.Width, this.Size.Height);
            resumeButton.Size = new Size(80, 80);
            resumeButton.Location = new Point(resumForm1.Location.X + resumForm1.Width / 2-40, resumForm1.Location.Y + resumForm1.Height / 2-55);
            resumeButton.BackgroundImage = global::TetrisGUI.Properties.Resources.pause;
            resumeButton.BackgroundImageLayout = ImageLayout.Zoom;
            resumeButton.FlatStyle = FlatStyle.Flat;
            resumeButton.FlatAppearance.BorderSize = 0;
            resumeButton.FlatAppearance.MouseOverBackColor = Color.LightGreen;
            resumeButton.FlatAppearance.MouseDownBackColor = Color.DarkGreen;

            resumForm1.Opacity = 0.8;
            resumeButton.Text = "";


            resumForm1.Controls.Add(resumeButton);
            resumeButton.Click += ResumeButton_Click;
            resumForm1.ShowDialog();
        }

        private void ResumeButton_Click(object sender, EventArgs e)
        {

            timer.Start();
            if (Application.OpenForms.OfType<resumForm>().Any())
            {
                Application.OpenForms.OfType<resumForm>().First().Close();
            }
        }
        private void mainScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (checkEndGame == false)//nếu là game over thì ko thông báo thoát này.
            {
                timer.Stop();
                DialogResult result = MessageBox.Show("Are you sure you want to quit game? Your data will not save!!",
                         "Quit Game Screen",
                         MessageBoxButtons.OKCancel,
                         MessageBoxIcon.Warning,
                         MessageBoxDefaultButton.Button1);
                if (result == DialogResult.OK)
                {

                }
                else if (result == DialogResult.Cancel)
                {
                    //continue playing game.
                    e.Cancel = true;
                    timer.Start();
                }

            }
            
        }

        //show time
        private void timer1_Tick(object sender, EventArgs e)
        {
            timeStartPlay = DateTime.Now;
            timer1.Start();
            label6.Text = "Time: " + timeStartPlay.ToString();
            
        }
    }
}
