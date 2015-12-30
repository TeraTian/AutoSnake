using ReallyAutoSnake.Container;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WayLib;

namespace ReallyAutoSnake
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeUI();
        }

        private int howbig = 15;//格子的大小
        private int padding = 5;
        private int col = 10;//列数
        private int row = 10;//行数

        private Dictionary<string, Label> fillDict = new Dictionary<string, Label>();

        public MapManager map;

        private void InitializeUI()
        {
            this.BackColor = Color.White;
            var dict = new ContainerDict(col - 1, row - 1);
            this.panel1.Width = col * (howbig+padding) + (col - 1) * padding+18;
            this.panel1.Height = row * (howbig+padding) + (row - 1) * padding+4;

            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(259, 210);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "自动贪吃蛇";
            this.PerformLayout();

            this.Size = new Size(32 + col * (howbig + padding), 140 + row * (howbig + padding));
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    LabelContainer lab = new LabelContainer();
                    lab.AutoSize = false;
                    lab.X = j;
                    lab.Y = i;
                    lab.Width = howbig;
                    lab.Height = howbig;
                    //lab.Location = new Point((howbig + padding) * j + 10, 60 + (howbig + padding) * (row - i));
                    lab.Location = new Point((howbig + padding) * j,(howbig + padding) * (row - i-1));

                    //lab.Text = j + " " + i;
                    //lab.BorderStyle = BorderStyle.None;
                    lab.Click += lab_Click;
                    //lab.MouseMove += lab_MouseMove;
                    //设置格子属性
                    lab.SnakeType = SnakeTypeEnum.None;
                    dict.AddOrUpdate(lab);

                    this.panel1.Controls.Add(lab);
                }
            }
            foreach (var l in dict)
            {
                //if (ii++ > 1)
                //{
                //    break;
                //}
                foreach (var dir in Direction.DirectionArray)
                {
                    var curX = l.X + dir.X_;
                    var curY = l.Y + dir.Y_;
                    if (!(curX < 0 || curX > col - 1 || curY < 0 || curY > row - 1))//超出范围
                    {
                        var key = string.Format("{0}{1}{2}{3}", l.X, l.Y, curX, curY);
                        if (!fillDict.ContainsKey(key))
                        {
                            var lab = new Label();
                            lab.AutoSize = false;
                            if (l.X == curX)
                            {
                                lab.Width = howbig;
                                lab.Height = padding;
                            }
                            if (l.Y == curY)
                            {
                                lab.Width = padding;
                                lab.Height = howbig;
                            }
                            lab.Location = new Point(l.Location.X + dir.X_ * howbig, l.Location.Y - dir.Y_ * padding);
                            //lab.Text = key;
                            lab.BackColor = Color.White;
                            //lab.BorderStyle = BorderStyle.FixedSingle;
                            fillDict.Add(key, lab);
                            this.panel1.Controls.Add(lab);
                        }
                    }
                }
            }
            map = new MapManager(dict, fillDict,this.StartTextBox);
        }

        void lab_MouseMove(object sender, MouseEventArgs e)
        {
            var obj = sender as LabelContainer;
            StartTextBox.Text = obj.SnakeType.ToString();
        }

        void lab_Click(object sender, EventArgs e)
        {
            var obj = sender as LabelContainer;
            obj.SnakeType = SnakeTypeEnum.Apple;
            obj.BackColor = Color.Red;
            map.Apple = obj;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            map.InitializeSnake();
            ThreadPool.QueueUserWorkItem(async o =>
            {
                while (true)
                {
                    map.CreateApple();
                    if (map.Complete)
                        break;
                    await map.Run();
                }
            });
        }

        private void NextStepButton_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(async o =>
                {
                    while (true)
                    {
                        map.CreateApple();
                        if (map.Complete)
                            break;
                        await map.Run();
                    }
                });
        }

        private void LastStepButton_Click(object sender, EventArgs e)
        {
            map.OneStepBack();
        }

        private void DetailButton_Click(object sender, EventArgs e)
        {
            map.CreateApple();
            map.RunToApple();
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            map.ClearDetail();
        }
    }

    public class MyTextBox : TextBox
    {
        public int a { get; set; }
    }
}
