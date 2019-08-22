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
            InitializeComponent();
            textBox1.Text = Settings.Default["FilePath"].ToString();
            textBox2.Text = Settings.Default["Set1Path"].ToString();
            comboBox2.SelectedIndex = Convert.ToInt32(Settings.Default["BaudRate"]); 

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
           // if (!String.Equals(LastName, Name))
           // {
           //     Serial.Close();
           //     Serial = new SerialPort(Name, Convert.ToInt32((string)comboBox2.SelectedItem), System.IO.Ports.Parity.None, 8, StopBits.One);
           //     if (Serial.IsOpen)
           //     {
           //         LablePortOpen.Text = ("Порт недоступен");
           //     }
           //     else
           //     {
           //         Serial.Open();
           //         LablePortOpen.Text = ("Порт открыт");
           //     }
           //
           // }
           
        
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

                if (Serial.BytesToRead > 0)
                {
                    byte ReadData = Convert.ToByte(Serial.ReadByte());
                    Serial.DiscardInBuffer();
                    if (ReadData == 1)
                    {
                        pictureBox1.BackColor = System.Drawing.Color.Lime;
                    }
                    else
                    {
                        pictureBox1.BackColor = System.Drawing.Color.White;
                    }
                }

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
                if (Serial.BytesToRead > 0)
                {
                    byte ReadData = Convert.ToByte(Serial.ReadByte());
                    Serial.DiscardInBuffer();
                    if (ReadData == 0)
                    {
                        pictureBox1.BackColor = System.Drawing.Color.White;
                    }
                    else
                    {
                        pictureBox1.BackColor = System.Drawing.Color.White;
                    }
                }

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


        private void ButtonLoad_Click(object sender, EventArgs e)
        {
            Settings.Default["FilePath"] = textBox1.Text;
            Settings.Default.Save();
            byte[] sinusform = new byte[2048];

            if (System.IO.File.Exists(textBox1.Text))
            {
                if (!Serial.IsOpen)
                {
                    MessageBox.Show("Выберите COM порт");
                }
                else
                {
                    using (StreamReader sr = new StreamReader(textBox1.Text, System.Text.Encoding.Default))
                    {
                        string line;
                        int i = 0;
                        while ((line = sr.ReadLine()) != null)
                        {
                            sinusform[i] = Convert.ToByte(line);
                            i = i + 1;
                        }
                    }
                    byte[] DataByte = BitConverter.GetBytes(0x02);
                    Serial.Write(DataByte, 0, 1);
                    Serial.Write(sinusform, 0, sinusform.Length);
                }

            }
            else
            {
                MessageBox.Show("Укажите расположение файла");
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Settings.Default["Set1Path"] = textBox2.Text;
            Settings.Default.Save();
            byte[] param1 = new byte[512];

            if (System.IO.File.Exists(textBox2.Text))
            {
                if (!Serial.IsOpen)
                {
                    MessageBox.Show("Выберите COM порт");
                }
                else
                {
                    using (StreamReader sr = new StreamReader(textBox2.Text, System.Text.Encoding.Default))
                    {
                        string line;
                        int i = 0;
                        while ((line = sr.ReadLine()) != null)
                        {
                            param1[i] = Convert.ToByte(line);
                            i = i + 1;
                        }
                    }
                    byte[] DataByte = BitConverter.GetBytes(0x03);
                    Serial.Write(DataByte, 0, 1);
                    Serial.Write(param1, 0, param1.Length);
                }
            }
            else
            {
                MessageBox.Show("Укажите расположение файла");
            }
        }
    }
}
