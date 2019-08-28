using System;
using System.IO.Ports;
using System.Windows.Forms;
using System.IO;
using System.Text;
using LivitationWFA.Properties;
using System.Drawing;

namespace LivitationWFA
{
    public partial class Form1 : Form
    {        
        public Form1()
        {
            InitB();
            InitializeComponent();
            comboBox2.SelectedIndex = Convert.ToInt32(Settings.Default["BaudRate"]);
            
            for (int i = 0; i < 16; i++)
                for(int j = 0; j < 16; j++)
                {
                    AntennArray[i, j] = new Int32[8];
                    for (int m = 0; m < 8; m++)
                        AntennArray[i, j][m] = 0x7fFF0000;
                }
            

            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                comboBox1.Items.Add(port);
            }

            NumPorts = ports.Length;

            int r =(int)Settings.Default["ComPortNum"];

            if (r <= NumPorts - 1)
            {
                comboBox1.SelectedIndex = Convert.ToInt32(Settings.Default["ComPortNum"]);
            }
            else
            {
                MessageBox.Show("Выберите COM порт");
            }

            timer1.Start();
        }
        public Int32[,][] AntennArray = new Int32[16, 16][];

        int AntennArrayButton_ButWidth = 28;
        int AntennArrayButton_XOffset = 536;
        int AntennArrayButton_YOffset = 20;

        private Button[,] AntennArrayButton = new Button[16, 16];
        public void InitB()
        {
            for (int i = 0; i < 16; i++)
                for (int j = 0; j < 16; j++)
                {
                    AntennArrayButton[i,j] = new Button();
                    AntennArrayButton[i, j].Location = new System.Drawing.Point(AntennArrayButton_XOffset + i* AntennArrayButton_ButWidth, AntennArrayButton_YOffset + j* AntennArrayButton_ButWidth);
                    AntennArrayButton[i, j].Size = new System.Drawing.Size(AntennArrayButton_ButWidth, AntennArrayButton_ButWidth);
                    AntennArrayButton[i, j].BackgroundImage = Resources.png;
                    AntennArrayButton[i, j].MouseClick += new MouseEventHandler(S_MouseClick);
                    AntennArrayButton[i, j].MouseMove += new MouseEventHandler(MyMouseMove);
                    //     AntennArrayButton[i, j].Enabled += new  MouseMove;
                    this.Controls.Add(AntennArrayButton[i, j]);
                }
            
        }

        private void S_MouseClick(object sender, EventArgs e)
        {
            this.Cursor = new Cursor(Cursor.Current.Handle);
            int x = (Cursor.Position.X - this.Location.X - AntennArrayButton_XOffset - 8) / AntennArrayButton_ButWidth;
            int y = (Cursor.Position.Y - this.Location.Y - AntennArrayButton_YOffset - AntennArrayButton_ButWidth - 3) / AntennArrayButton_ButWidth;
            Form2 SecondForm = new Form2(this);
            SecondForm.EmitterParamPosition_X = x;
            SecondForm.EmitterParamPosition_Y = y;
            for (int i = 0; i < 8; i++)
            {
                SecondForm.TextBoxArray[i, 0].Text = (AntennArray[x, y][i] & 0x000007FF).ToString();
                SecondForm.TextBoxArray[i, 1].Text = (AntennArray[x, y][i] >> 16 & 0x000000ff).ToString();
                SecondForm.TextBoxArray[i, 2].Text = (AntennArray[x, y][i] >> 24 & 0x000000ff).ToString();
            }
            SecondForm.ShowDialog();
         //   SecondForm.ShowEmitterParam(x, y);
        }



        int NumPorts;
        static SerialPort Serial;
        static bool Led_st = false;

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            String Name = ((string)comboBox1.SelectedItem);
            if (Serial == null)
            {
                Serial = new SerialPort(Name, Convert.ToInt32((string)comboBox2.SelectedItem), System.IO.Ports.Parity.None, 8, StopBits.One);
                if (Serial.IsOpen)
                {
                    LablePortOpen.Text = ("Порт недоступен");
                }
                else
                {
                    Serial.Open();
                    LablePortOpen.Text = ("Порт открыт");
                    int indx = comboBox1.SelectedIndex; ;
                    Settings.Default["ComPortNum"] = indx;
                    Settings.Default.Save();
                }
            }
            else
            {
                Serial.Close();
                Serial = new SerialPort(Name, Convert.ToInt32((string)comboBox2.SelectedItem), System.IO.Ports.Parity.None, 8, StopBits.One);
                if (Serial.IsOpen)
                {
                    LablePortOpen.Text = ("Порт недоступен");
                }
                else
                {
                    Serial.Open();
                    LablePortOpen.Text = ("Порт открыт");
                    int indx = comboBox1.SelectedIndex; ;
                    Settings.Default["ComPortNum"] = indx;
                    Settings.Default.Save();
                }
            }


        }

        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int indx = comboBox2.SelectedIndex; ;
            Settings.Default["BaudRate"] = indx;
            Settings.Default.Save();

            if (!(Serial == null))
            {
                Serial.BaudRate = Convert.ToInt32((string)comboBox2.SelectedItem);
            }

        }



        private void ButtonStart_Click(object sender, EventArgs e)
        {
            if (!Serial.IsOpen)
                {
                    MessageBox.Show("Выберите COM порт");
                }
            else
            {
                // Graphics g = this.CreateGraphics();
                // SolidBrush brush = new SolidBrush(Color.White);
                // g.FillRectangle(brush, new Rectangle(0, 0, 240, 500));
                //
                // Point[] pointSin = new Point[2048];
                //
                // for( int n = 0; n < pointSin.Length; n++)
                // {
                //     pointSin[n] = new Point(Convert.ToInt32(n), Convert.ToInt32(Math.Round(127 + 124*Math.Sin(2*Math.PI*n/ pointSin.Length), 0)));
                // }
                //
                // Pen gPen = new Pen(Color.Black);
                // g.DrawCurve(gPen, pointSin);
                byte[] pointSin = new byte[2048];
                for (int n = 0; n < pointSin.Length; n++)
                     {
                         pointSin[n] = Convert.ToByte(Math.Round(127 + 124*Math.Sin(2*Math.PI*n/ pointSin.Length), 0));
                     }
                    Serial.Write(BitConverter.GetBytes(2), 0, 1);
                    Serial.Write(pointSin, 0, pointSin.Length);

                for (int i = 0; i < 16; i++)
                {
                    Serial.Write(BitConverter.GetBytes(i + 3), 0, 1);
                    for (int j = 0; j < 16; j++)
                    {
                        for (int k = 0; k < 8; k++)
                        {
                            byte[] data = BitConverter.GetBytes(AntennArray[i, j][k]);
                            Serial.Write(data, 0, data.Length);
                        }
                    }

                }
                byte[] DataByte = BitConverter.GetBytes(0x01);
                Serial.Write(DataByte, 0, 1);
            }



            return;

        }


        private void ButtonStop_Click(object sender, EventArgs e)
        {
            if (!Serial.IsOpen)
            {
                MessageBox.Show("Выберите COM порт");
            }
            else
            {
                Serial.Write(BitConverter.GetBytes(0x00), 0, 1);
            }

            return;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            Led_st = !Led_st;
            if (Led_st)
            {
                pictureBox2.BackColor = System.Drawing.Color.Red;
            }
            else
            {
                pictureBox2.BackColor = System.Drawing.Color.White;
            }

            string[] ports = SerialPort.GetPortNames();
   
            if (NumPorts != ports.Length)
            {
                NumPorts = ports.Length;
                if (NumPorts < comboBox1.SelectedIndex)
                {
                    MessageBox.Show("Выберите COM порт");
                    foreach (string port in ports)
                    {
                        comboBox1.Items.Add(port);
                    }
                }
                else
                {
                    String OldPort = (string)comboBox1.SelectedItem;
                    comboBox1.Items.Clear();
                    foreach (string port in ports)
                    {
                        comboBox1.Items.Add(port);
                    }
                    if (OldPort != (string)comboBox1.SelectedItem)
                    {
                        MessageBox.Show("Выберите COM порт");
                        comboBox1.Text = null;
                    }
                }
            
            }
   
        }

        private void MyMouseMove(object sender, MouseEventArgs e)
        {
            this.Cursor = new Cursor(Cursor.Current.Handle);
            int x = (Cursor.Position.X - this.Location.X - AntennArrayButton_XOffset - 8) / AntennArrayButton_ButWidth;
            int y = (Cursor.Position.Y - this.Location.Y - AntennArrayButton_YOffset - AntennArrayButton_ButWidth - 3) / AntennArrayButton_ButWidth;
            label2.Text = "номер излучателя = " + x.ToString() + ", номер решетки = " + y.ToString();
        }
    }
}
