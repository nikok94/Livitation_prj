using System;
using System.IO.Ports;
using System.Windows.Forms;
using System.IO;
using System.Text;
using LivitationWFA.Properties;

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
                    SecondForm.AntennArray[i, j] = new UInt32[8];
                    for (int m = 0; m < 8; m++)
                        SecondForm.AntennArray[i, j][m] = 0x7fFF0000;
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

        Form2 SecondForm = new Form2();

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
                    AntennArrayButton[i, j].MouseClick += new MouseEventHandler(S_MouseClick);
                    this.Controls.Add(AntennArrayButton[i, j]);
                }
            
        }
        private void S_MouseClick(object sender, EventArgs e)
        {
            this.Cursor = new Cursor(Cursor.Current.Handle);
            int x = (Cursor.Position.X - this.Location.X - AntennArrayButton_XOffset - 8) / AntennArrayButton_ButWidth;
            int y = (Cursor.Position.Y - this.Location.Y - AntennArrayButton_YOffset - AntennArrayButton_ButWidth - 3) / AntennArrayButton_ButWidth;
            SecondForm.ChangeEmitterParam(x, y);
  //          SecondForm.label1.Text = "положение x = " + x + ", y = " + y;
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
                byte[] DataByte = BitConverter.GetBytes(0x00);
                Serial.Write(DataByte, 0, 1);

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
    }
}
