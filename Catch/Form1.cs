using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Catch
{
    public partial class Form1 : Form
    {
        //Hero variables
        Rectangle hero = new Rectangle(280, 540, 40, 10);
        int heroSpeed = 10;

        //Ball variables
        int ballSize = 10;
        //List of balls
        new List <Rectangle> ballList = new List<Rectangle>();
        new List <int> ballSpeeds = new List<int>();
        new List<string> ballColour = new List<string>();

        int score = 0;
        int time = 500;

        bool leftPressed = false;
        bool rightPressed = false;

        SolidBrush greenBrush = new SolidBrush(Color.Green);
        SolidBrush whiteBrush = new SolidBrush(Color.White);
        SolidBrush redBrush = new SolidBrush(Color.Red);
        SolidBrush goldBrush = new SolidBrush(Color.Gold);

        Random randGen = new Random();
        int randValue = 0;

        int groundHeight = 50;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftPressed = true;
                    break;
                case Keys.Right:
                    rightPressed = true;
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftPressed = false;
                    break;
                case Keys.Right:
                    rightPressed = false;
                    break;
            }
        }


        private void gameTime_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < ballList.Count; i++)
            {
                int y = ballList[i].Y + ballSpeeds[i];

                ballList[i] = new Rectangle(ballList[i].X, y, ballSize, ballSize);
            }

            //Move Player
            //Set boundaries
            if (leftPressed == true && hero.Y > 0)
            {
                hero.X -= heroSpeed;
            }
            if (rightPressed && hero.Y < this.Width - hero.Width)
            {
                hero.X += heroSpeed;
            }

            //Randomize ball spawn.
            randValue = randGen.Next(0, 100);
            if (randValue < 20)
            {
                if (randValue < 10)
                {
                    randValue = randGen.Next(10, this.Width - ballSize);
                    Rectangle ball = new Rectangle(randValue, 0, ballSize, ballSize);
                    ballList.Add(ball);
                    ballColour.Add("green");
                    ballSpeeds.Add(randGen.Next(5, 15));
                }
                else if (randValue < 14)
                {
                    randValue = randGen.Next(10, this.Width - ballSize);
                    Rectangle ball = new Rectangle(randValue, 0, ballSize, ballSize);
                    ballList.Add(ball);
                    ballColour.Add("red");
                    ballSpeeds.Add(randGen.Next(5, 15));
                }
                else
                {
                    randValue = randGen.Next(10, this.Width - ballSize);
                    Rectangle ball = new Rectangle(randValue, 0, ballSize, ballSize);
                    ballList.Add(ball);
                    ballColour.Add("gold");
                    ballSpeeds.Add(randGen.Next(5, 15));
                }
            }
            //Remove ball once it hits ground.
            for (int i = 0; i < ballList.Count; i++)
            {
                if (ballList[i].Y > this.Height - groundHeight)
                {
                    ballList.RemoveAt(i);
                    ballColour.RemoveAt(i);
                    ballSpeeds.RemoveAt(i);
                }
            }


            // Check for collision

            for (int i = 0; i < ballList.Count; i++)
            {
                if(ballList[i].IntersectsWith(hero))
                {
                    if (ballColour[i] == "green")
                    {
                        score++;
                        ballList.RemoveAt(i);
                        ballColour.RemoveAt(i);
                        ballSpeeds.RemoveAt(i);
                    }
                    else if (ballColour[i] == "red")
                    {
                        score--;
                        ballList.RemoveAt(i);
                        ballColour.RemoveAt(i);
                        ballSpeeds.RemoveAt(i);     
                    }
                    else
                    {
                        time = time + 20;
                        ballList.RemoveAt(i);
                        ballColour.RemoveAt(i);
                        ballSpeeds.RemoveAt(i);
                    }
                }
            }

            //Decrease time and check if it has run out.

            time--;
            if (time == 0)
            {
                gameTimer.Stop();
            }



            //redraw the screen
            Refresh();

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //update labels
            timeLabel.Text = $"Time Left: {time}";
            scoreLabel.Text = $"Score: {score}";

            //draw ground
            e.Graphics.FillRectangle(greenBrush, 0, this.Height - groundHeight, this.Width, groundHeight);

            //draw hero
            e.Graphics.FillRectangle(whiteBrush, hero);

            //draw balls
            for (int i = 0; i < ballList.Count; i++)
            {
                if (ballColour[i] == "green")
                {
                    e.Graphics.FillEllipse(greenBrush, ballList[i]);
                }
                else if (ballColour[i] == "red")
                {
                    e.Graphics.FillEllipse(redBrush, ballList[i]);
                }
                else
                {
                    e.Graphics.FillEllipse(greenBrush, ballList[i]);
                }

            }
        }
    }
}
