using System;
using System.Drawing;
using System.Windows.Forms;

namespace SolarSystemSimulator
{
   public struct planetFeatures 
    {
        public int radius          { get; set; }
        public int weight          { get; set; }
        public int daysAroundSun   { get; set; }
        public int fromSunDistance { get; set; }
    }

    public partial class Form1 : Form
    {
        public Form1()
        {
           InitializeComponent();
            this.BackColor = Color.Black;
            // this.WindowState = FormWindowState.Maximized;
            this.Width = screenX;
            this.Height = screenY;
            this.CenterToScreen();
        }
        const int timeConst = 2112;
        // USER SCREEN SIZE
        const int screenX = 1200;
        const int screenY = 800;

        const int scale = 16000;     // MUST BE > 0, (2 = 1:2 km)
                                      //weight * 10E24 kg
                                      
           const int xSun = (int)(screenX / 2);  // Sun Parametrs 
           const int ySun = (int)(screenY / 2);
           const int rSun = (int)(695700 / scale);
           const double wSun = 1989000;
        
             planetFeatures[] planet = new planetFeatures[9];

             Point[] orbitMercury = new Point[360]; // for planet movment on orbit path
             Point[] orbitMtr = new Point[360];

        int xMtr = 710, yMtr = 210;
        int mtrPathAngle = 0, mtrSpeed, mtrR = 3;
        int startFlag = 0, a, b, focus;

        public void Form1_Load(object sender, EventArgs e)
        {
            #region PlanetsInfo
             planet[0].radius = 2440 / scale + 3;
             planet[0].fromSunDistance = 67;
             planet[0].daysAroundSun = (int)(timeConst / 88);

            #endregion

            for (int i = 0; i < 360; i++)
            {
                orbitMercury[i].X = (int)(Math.Cos(i * Math.PI / 180) * planet[0].fromSunDistance) + xSun;
                orbitMercury[i].Y = (int)(Math.Sin(i * Math.PI / 180) * planet[0].fromSunDistance) + ySun;
            }

               timer1.Interval = 120;
               timer1.Enabled = true;
        }

        int[] pathAngle = new int[9]; //full path for 100 days ( speed = 3 degree per ms [300 d/ms for 1 day]


        public void timer1_Tick(object sender, EventArgs e)
        {
           pathAngle[0] = pathAngle[0] + planet[0].daysAroundSun;
            if (pathAngle[0] >= 360) pathAngle[0] = 0;

            

            Graphics g = CreateGraphics(); //FOR draw ellipse we define top left side and bottom right one
            g.Clear(Color.Black);
              g.FillEllipse(new SolidBrush(Color.Yellow), xSun - rSun, ySun - rSun, 2 * rSun, 2 * rSun); //SUN
             
              g.DrawEllipse(new Pen(Color.DarkCyan), xSun - planet[0].fromSunDistance, ySun - planet[0].fromSunDistance, 2 * planet[0].fromSunDistance, 2 * planet[0].fromSunDistance); //Orbit for Mercury LightSkyBlue
              
            g.FillEllipse(new SolidBrush(Color.PaleGoldenrod), orbitMercury[pathAngle[0]].X - planet[0].radius , orbitMercury[pathAngle[0]].Y - planet[0].radius, 2 * planet[0].radius, 2 * planet[0].radius); //Mercury

            if (startFlag == 1) ///////// ДВИЖЕНИЕ МЕТЕОРИТА
            {
                mtrPathAngle = mtrPathAngle + mtrSpeed;
                if (mtrPathAngle >= 360)
                {
                    mtrPathAngle = 0;
                    startFlag = 0;
                }

                g.DrawEllipse(new Pen(Color.OrangeRed), xSun - (a - focus), ySun - b, 2 * a, 2 * b); // Meteor orbit

                g.FillEllipse(new SolidBrush(Color.Beige), orbitMtr[mtrPathAngle].X - mtrR, orbitMtr[mtrPathAngle].Y - mtrR, 2 * mtrR, 2 * mtrR); // Meteor way

            }

            g.Dispose();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            mtrSpeed = 6;
              a = 400;     // радиусы эллипса
              b = 250;
             focus = 310;  // смещение
             

            for (int i = 0; i < 360; i++) // задаем орбиту метеорита
            {
                orbitMtr[i].X = (int)(Math.Cos(i * Math.PI / 180) * a) + xSun + focus;
                orbitMtr[i].Y = (int)(Math.Sin(i * Math.PI / 180) * b) + ySun;
            }

            startFlag = 1;
        }
    }
}
