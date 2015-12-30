using AutoSnake.Container;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WayLib;

namespace AutoSnake
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeUI();
            InitializeComboBox();
        }
        private int howbig = 20;//格子的大小
        private int col = 50;//列数
        private int row = 30;//行数
        //格子容器
        private ContainerDictionary dict;
        //当前绘制的格子的类型
        private int type = 0;

        //界面上的起始点和终点
        private LabelContainer Start;
        private LabelContainer End;
        //不同寻找算法对应的类型
        private List<Type> Types;

        //初始化界面
        private void InitializeUI()
        {
            dict = new ContainerDictionary(col - 1, row - 1);

            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(259, 210);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "寻路";
            this.PerformLayout();

            this.Size = new Size(32 + col * howbig, 140 + row * howbig);
            int id = 0;
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    LabelContainer lab = new LabelContainer();
                    lab.CanPass = true;
                    lab.AutoSize = false;
                    lab.ID = id++;
                    lab.X = j;
                    lab.Y = i;
                    lab.Width = howbig;
                    lab.Height = howbig;
                    lab.Location = new Point(howbig * j + 10, 60 + howbig * (row - i));
                    //lab.BorderStyle = BorderStyle.FixedSingle;
                    lab.BorderStyle = BorderStyle.None;
                    lab.BackColor = Color.White;
                    lab.ContainerColor = Color.White;
                    //lab.Text = j + "" + i;
                    lab.Click += lab_Click;
                    lab.MouseMove += lab_MouseMove;
                    dict.AddOrUpdate(lab);
                    this.Controls.Add(lab);
                }
            }
        }

        private bool draw = false;//标记是否绘图
        void lab_MouseMove(object sender, MouseEventArgs e)
        {
            if (draw)
            {
                var lab = sender as LabelContainer;
                dict.SetContainerType((ContainerType)type, lab.X, lab.Y);
                if (type == 0)
                {
                    if (Start != null)
                        Start.BackColor = dict.LastStartContainer.ContainerColor;
                    Start = lab;
                }
                if (type == 1)
                {
                    if (End != null)
                        End.BackColor = dict.LastEndContainer.ContainerColor;
                    End = lab;
                }
                lab.BackColor = lab.ContainerColor;
                textBox1.Text = dict.CanPassCount().ToString();
            }
        }

        void lab_Click(object sender, EventArgs e)
        {
            draw = !draw;
        }

        private void InitializeComboBox()
        {
            Type type;
            if (Utility.TryFindType("WayLib.WayFinder", out type))
            {
                var assembly = type.Assembly;
                var types = assembly.GetTypes().ToList();
                Types = types.Where(t => t.IsSubclassOf(typeof(WayFinder))).OrderBy(t => t.Name).ToList();
                var typeName = Types.Select(t => t.Name);
                FindType.DisplayMember = "Text";
                FindType.ValueMember = "Value";
                int i = 1;
                foreach (var t in typeName)
                {
                    FindType.Items.Add(new { Text = "方法" + i++, Value = t });
                }
                FindType.SelectedIndex = 0;
            }
        }
        #region 设置不同的格子类型
        private void SetStart_Click(object sender, EventArgs e)
        {
            type = 0;
        }

        private void SetEnd_Click(object sender, EventArgs e)
        {
            type = 1;
        }

        private void SetWall_Click(object sender, EventArgs e)
        {
            type = 2;
        }

        private void SetRoad_Click(object sender, EventArgs e)
        {
            type = 3;
        }
        #endregion
        private void BeginFind_Click(object sender, EventArgs e)
        {
            var findway = ((dynamic)FindType.SelectedItem).Value;
            var type = Types.FirstOrDefault(t => t.Name == findway);
            WayFinder finder = System.Activator.CreateInstance(type) as WayFinder;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var str = finder.Execute(dict);
            sw.Stop();
            ResultText.Text = str;
            textBox1.Text = sw.Elapsed.ToString();
            if (str == "成功")
            {
                ThreadPool.QueueUserWorkItem(o =>
                    {
                        ResultText.Text = finder.WayResult.Count().ToString();
                        foreach (var i in finder.WayProcedure)
                        {
                            if (((CancellationToken)o).IsCancellationRequested)
                                break;
                            var container = dict[i.X, i.Y] as LabelContainer;
                            container.BackColor = Color.Yellow;
                            //container.Text = container.StartDistance + "-" + container.EndDistance;
                            Thread.Sleep(5);
                        }
                        foreach (var i in finder.WayResult)
                        {
                            if (((CancellationToken)o).IsCancellationRequested)
                                break;
                            var container = dict[i.X_, i.Y_] as LabelContainer;
                            container.BackColor = Color.Green;
                            Thread.Sleep(5);
                        }
                    }, source.Token);
            }

        }

        #region 重置
        private CancellationTokenSource source = new CancellationTokenSource();
        private void Reset_Click(object sender, EventArgs e)
        {
            source.Cancel();
            dict.Reset();
            this.End = null;
            this.Start = null;
            foreach (var d in dict)
            {
                var view = d as LabelContainer;
                view.BackColor = d.ContainerColor;
            }
            source = new CancellationTokenSource();
        }

        private void Refind_Click(object sender, EventArgs e)
        {
            source.Cancel();
            dict.ReStartSet();
            foreach (var d in dict)
            {
                var view = d as LabelContainer;
                view.BackColor = d.ContainerColor;
            }
            source = new CancellationTokenSource();
        }
        #endregion
    }
}
