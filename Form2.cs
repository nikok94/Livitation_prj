using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LivitationWFA
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            InitB();
        }

        public Form2(Form1 FirstForm)
        {
            InitializeComponent();
            InitB();
            nex = FirstForm;

        }

        Form1 nex; 
        public TextBox[,] TextBoxArray = new TextBox[8, 3];
        private Label[] LabelArray = new Label[8];
        int TextBox_Length = 50;
        int TextBox_Width = 28;
        int TextBoxArray_XOffset = 173;
        int TextBoxArray_YOffset = 56;

        public int EmitterParamPosition_X;
        public int EmitterParamPosition_Y;


        public void InitB()
        {
            for (int i = 0; i < 8; i++)
            {
                LabelArray[i] = new Label();
                LabelArray[i].Location = new System.Drawing.Point(i * (TextBox_Length + 10) + TextBoxArray_XOffset + 5 , TextBoxArray_YOffset - TextBox_Width);
                LabelArray[i].Size = new System.Drawing.Size(TextBox_Length, TextBox_Width);
                LabelArray[i].Text = "Sin" + i.ToString();
                LabelArray[i].Name = "Sin" + i.ToString();
                this.Controls.Add(LabelArray[i]);

                for (int j = 0; j < 3; j++)
                {
                    TextBoxArray[i, j] = new TextBox();
                    TextBoxArray[i, j].Location = new System.Drawing.Point(i * (TextBox_Length + 10) + TextBoxArray_XOffset, j * (TextBox_Width + 5) + TextBoxArray_YOffset);
                    TextBoxArray[i, j].Size = new System.Drawing.Size(TextBox_Length, TextBox_Width);
                    TextBoxArray[i, j].KeyPress += new KeyPressEventHandler(PhaseTextChanged);
                    this.Controls.Add(TextBoxArray[i, j]);
                }

            }
        }

        private void PhaseTextChanged(object sender, KeyPressEventArgs e)
        {
        char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8) // цифры и клавиша BackSpace
            {
                e.Handled = true;
            }

            for (int i = 0; i < 8; i++)
            {
                if (!(TextBoxArray[i, 0].Text == ""))
                    TextBoxArray[i, 0].Text = (Convert.ToInt32(TextBoxArray[i, 0].Text) & 0x000007FF).ToString();
                if (!(TextBoxArray[i, 1].Text == ""))
                    TextBoxArray[i, 1].Text = (Convert.ToInt32(TextBoxArray[i, 1].Text) & 0x000000FF).ToString();
                if (!(TextBoxArray[i, 2].Text == ""))
                    TextBoxArray[i, 2].Text = (Convert.ToInt32(TextBoxArray[i, 2].Text) & 0x000000FF).ToString();
            }

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 8; i++)
                nex.AntennArray[EmitterParamPosition_X, EmitterParamPosition_Y][i] = Convert.ToInt32(TextBoxArray[i, 0].Text) | Convert.ToInt32(TextBoxArray[i, 1].Text) << 16 | Convert.ToInt32(TextBoxArray[i, 2].Text) << 24;
            this.Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 16; i++)
                for (int j = 0; j < 16; j++)
                {
                    for (int k = 0; k < 8; k++)
                    {
                        nex.AntennArray[i, j][k] = Convert.ToInt32(TextBoxArray[k, 0].Text) | Convert.ToInt32(TextBoxArray[k, 1].Text) << 16 | Convert.ToInt32(TextBoxArray[k, 2].Text) << 24;
                    }
                }
            this.Close();
        }
    }
}
