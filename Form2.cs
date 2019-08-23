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

        public UInt32[,][] AntennArray = new UInt32[16, 16][];

        public TextBox[,] TextBoxArray = new TextBox[8, 3];
        private Label[] LabelArray = new Label[8];
        int TextBox_Length = 50;
        int TextBox_Width = 28;
        int TextBoxArray_XOffset = 180;
        int TextBoxArray_YOffset = 52;


        public void InitB()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    TextBoxArray[i, j] = new TextBox();
                    TextBoxArray[i, j].Location = new System.Drawing.Point(i * (TextBox_Length + 10) + TextBoxArray_XOffset, j * (TextBox_Width + 5) + TextBoxArray_YOffset);
                    TextBoxArray[i, j].Size = new System.Drawing.Size(TextBox_Length, TextBox_Width);
                    //  TextBoxArray[i, j].MouseClick += new MouseEventHandler(S_MouseClick);
                    this.Controls.Add(TextBoxArray[i, j]);
                }

                LabelArray[i] = new Label();
                LabelArray[i].Location = new System.Drawing.Point(i * (TextBox_Length + 10) + TextBoxArray_XOffset, TextBoxArray_YOffset - TextBox_Width);
                LabelArray[i].Size = new System.Drawing.Size(TextBox_Length, TextBox_Width);
                LabelArray[i].Text = "Sin" + i.ToString();
                LabelArray[i].Name = "Sin" + i.ToString();
                this.Controls.Add(LabelArray[i]);

            }
        }
        public void ChangeEmitterParam(int x, int y)
        {
            for (int i = 0; i < 8; i++)
            {
                byte[] T = BitConverter.GetBytes(AntennArray[x, y][i]);
                TextBoxArray[i, 0].Text = BitConverter.ToString(T,0,2);
                TextBoxArray[i, 1].Text = BitConverter.ToString(T, 2, 1);
                TextBoxArray[i, 2].Text = BitConverter.ToString(T, 3, 1);
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
