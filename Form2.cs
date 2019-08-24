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

        public Int32[,][] AntennArray = new Int32[16, 16][];

        public TextBox[] TextBoxArrayPhase = new TextBox[8];
        public TextBox[] TextBoxArrayAmpl = new TextBox[8];
        public TextBox[] TextBoxArrayFreq = new TextBox[8];
        private Label[] LabelArray = new Label[8];
        int TextBox_Length = 50;
        int TextBox_Width = 28;
        int TextBoxArray_XOffset = 173;
        int TextBoxArray_YOffset = 56;

        int EmitterParamPosition_X;
        int EmitterParamPosition_Y;


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


                TextBoxArrayPhase[i] = new TextBox();
                TextBoxArrayPhase[i].Location = new System.Drawing.Point(i * (TextBox_Length + 10) + TextBoxArray_XOffset, 0 * (TextBox_Width + 5) + TextBoxArray_YOffset);
                TextBoxArrayPhase[i].Size = new System.Drawing.Size(TextBox_Length, TextBox_Width);
                TextBoxArrayPhase[i].KeyPress += new KeyEventHandler(PhaseTextChanged);
                this.Controls.Add(TextBoxArrayPhase[i]);

                TextBoxArrayAmpl[i] = new TextBox();
                TextBoxArrayAmpl[i].Location = new System.Drawing.Point(i * (TextBox_Length + 10) + TextBoxArray_XOffset, 1 * (TextBox_Width + 5) + TextBoxArray_YOffset);
                TextBoxArrayAmpl[i].Size = new System.Drawing.Size(TextBox_Length, TextBox_Width);
                TextBoxArrayAmpl[i].KeyPress += new KeyEventHandler(AmplTextChanged);
                this.Controls.Add(TextBoxArrayAmpl[i]);

                TextBoxArrayFreq[i] = new TextBox();
                TextBoxArrayFreq[i].Location = new System.Drawing.Point(i * (TextBox_Length + 10) + TextBoxArray_XOffset, 2 * (TextBox_Width + 5) + TextBoxArray_YOffset);
                TextBoxArrayFreq[i].Size = new System.Drawing.Size(TextBox_Length, TextBox_Width);
                TextBoxArrayFreq[i].KeyPress += new KeyEventHandler(FreqTextChanged);
                this.Controls.Add(TextBoxArrayFreq[i]);


            }
        }
        private void AmplTextChanged(object sender, KeyEventArgs e)
        {
            
        }
        private void FreqTextChanged(object sender, KeyEventArgs e)
        {
        }

        private void PhaseTextChanged(object sender, KeyEventArgs e)
        {

        char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8) // цифры и клавиша BackSpace
            {
                e.Handled = true;
            }

        }

        public void ShowEmitterParam(int x, int y)
        {
            EmitterParamPosition_X = x;
            EmitterParamPosition_Y = y;

            for (int i = 0; i < 8; i++)
            {
                TextBoxArrayPhase[i].Text = (AntennArray[x, y][i] & 0x0000FFFF).ToString();
                TextBoxArrayAmpl[i].Text = ((AntennArray[x, y][i] & 0x00ff0000)>>16).ToString();
                TextBoxArrayFreq[i].Text = ((AntennArray[x, y][i] & 0xff000000)>>24).ToString();
            }
            this.Show();
        }


        //    private void S_MouseClick(object sender, EventArgs e)
        //    {
        //        this.Cursor = new Cursor(Cursor.Current.Handle);
        //        int x = (Cursor.Position.X - this.Location.X - AntennArrayButton_XOffset - 8) / AntennArrayButton_ButWidth;
        //        int y = (Cursor.Position.Y - this.Location.Y - AntennArrayButton_YOffset - AntennArrayButton_ButWidth - 3) / AntennArrayButton_ButWidth;
        //        SecondForm.label1.Text = "положение x = " + x + ", y = " + y;
        //        SecondForm.ShowDialog();
        //    }

    }
}
