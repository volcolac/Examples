using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace main
{
    public partial class MainMenu : Form
    {
        private Point[] Points { get; set; }
        private int CountPoints { get; set; }
        private DataBase Data { get; set; }
        private FindInexpensivePath InexpensiveFinder { get; set; }
        private FindQuickPath QuickPathFinder { get; set; }
        private bool AllMap { get; set; }
        private bool Flag { get; set; }

        public MainMenu()
        {
            InitializeComponent();
            Data = new DataBase();
            Rename();
            var names = Data.Cities.Select(a => a.Name).ToArray();
            Array.Sort(names);
            comboBoxFrom.Items.AddRange(names);
            comboBoxTo.Items.AddRange(names);
            InexpensiveFinder = new FindInexpensivePath(Data);
            QuickPathFinder = new FindQuickPath(Data);
            dataGridView1.AllowUserToAddRows = false;
            Clear();
            AllMap = false;
            //HeadRow = new DataGridViewRow();
        }

        private void Rename()
        {
            radioButton1.Text = Data.Cities[0].Name;
            radioButton2.Text = Data.Cities[1].Name;
            radioButton3.Text = Data.Cities[2].Name;
            radioButton4.Text = Data.Cities[3].Name;
            radioButton5.Text = Data.Cities[4].Name;
            radioButton6.Text = Data.Cities[5].Name;
            radioButton7.Text = Data.Cities[6].Name;
            radioButton8.Text = Data.Cities[7].Name;
            radioButton9.Text = Data.Cities[8].Name;
            radioButton10.Text = Data.Cities[9].Name;
            radioButton11.Text = Data.Cities[10].Name;
            radioButton12.Text = Data.Cities[11].Name;
            radioButton13.Text = Data.Cities[12].Name;
            radioButton14.Text = Data.Cities[13].Name;
            radioButton15.Text = Data.Cities[14].Name;

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            InexpensiveFinder = new FindInexpensivePath(Data);
            Clear();
            if (comboBoxFrom.Text != comboBoxTo.Text)
            {
                var path = InexpensiveFinder.FindPath(comboBoxFrom.Text, comboBoxTo.Text);
                Points = Data.FindPoints(path.Cities);
                CountPoints = Points.Count();
                UpdateDataGrid(path);
            }
            pictureBox1.Refresh();
        }

        private void Clear()
        {
            Points = new Point[0];
            CountPoints = 0;
            pictureBox1.Refresh();
            DataTable dt = new DataTable();
            dataGridView1.DataSource = dt;
            label2.Text = "0";
            label4.Text = "0";
            label9.Text = "0";
        }

        private void UpdateDataGrid(Path path)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Время отправления");
            dt.Columns.Add("Откуда");
            dt.Columns.Add(" ");
            dt.Columns.Add("Куда");
            dt.Columns.Add("Время прибытия");
            dt.Columns.Add("Цена");
            dt.Columns.Add("Билеты в наличии/всего");
            foreach (var flight in path.Flights)
            {
                var r = dt.NewRow();
                r[0] = flight.Start;
                r[1] = Data.Cities[flight.From].Name;
                r[2] = '-';
                r[3] = Data.Cities[flight.To].Name;
                r[4] = flight.Finish;
                r[5] = flight.Cost;
                r[6] = flight.FreeSeats.ToString() + "/" + flight.Seats.ToString();
                dt.Rows.Add(r);
            }
    
            dataGridView1.DataSource = dt;
            dataGridView1.Columns[0].Width = 75;
            dataGridView1.Columns[1].Width = 100;
            dataGridView1.Columns[2].Width = 15;
            dataGridView1.Columns[3].Width = 100;
            dataGridView1.Columns[4].Width = 65;
            dataGridView1.Columns[5].Width = 60;
            label2.Text = new Time(path.WayInMinute).ToString();
            label4.Text = path.Cost.ToString();
            label9.Text = new Time(path.TimeInFly).ToString();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            // Рисуем линию
            if (AllMap)
            {
                var a = new List<int>();
                foreach (var flight in Data.Flights)
                {
                    a.Add(flight.From);
                    a.Add(flight.To);
                }
                var b = a.ToArray();
                Points = Data.FindPoints(b);
                for (var i = 0; i < Points.Count(); i += 2)
                    e.Graphics.DrawLine(Pens.Green, Points[i].X - 11, Points[i].Y - 51, Points[i + 1].X - 11, Points[i + 1].Y - 51);
            }
            else
            {
                for (var i = 0; i < CountPoints - 1; i++)
                    e.Graphics.DrawLine(Pens.Green, Points[i].X - 11, Points[i].Y - 51, Points[i + 1].X - 11, Points[i + 1].Y - 51);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            QuickPathFinder = new FindQuickPath(Data);
            Clear();
            if (comboBoxFrom.Text != comboBoxTo.Text)
            {
                var path = QuickPathFinder.FindPath(comboBoxFrom.Text, comboBoxTo.Text);
                Points = Data.FindPoints(path.Cities);
                CountPoints = Points.Count();
                UpdateDataGrid(path);
            }
            pictureBox1.Refresh();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Clear();
            AllMap = true;
            pictureBox1.Refresh();
            AllMap = false;
        }

        private void Select(string s)
        {
            if (Flag)
            {
                comboBoxTo.Text = s;
                Flag = !Flag;
            }
            else
            {
                comboBoxFrom.Text = s;
                Flag = !Flag;
            }
        }

        #region
        private void radioButton9_MouseClick(object sender, EventArgs e)
        {
            Select(radioButton9.Text);
        }

        private void radioButton2_MouseClick(object sender, EventArgs e)
        {
            Select(radioButton2.Text);
        }

        private void radioButton14_MouseClick(object sender, EventArgs e)
        {
            Select(radioButton14.Text);
        }

        private void radioButton7_MouseClick(object sender, EventArgs e)
        {
            Select(radioButton7.Text);
        }

        private void radioButton13_MouseClick(object sender, EventArgs e)
        {
            Select(radioButton13.Text);
        }

        private void radioButton5_MouseClick(object sender, EventArgs e)
        {
            Select(radioButton5.Text);
        }

        private void radioButton1_MouseClick(object sender, EventArgs e)
        {
            Select(radioButton1.Text);
        }

        private void radioButton10_MouseClick(object sender, EventArgs e)
        {
            Select(radioButton10.Text);
        }

        private void radioButton15_MouseClick(object sender, EventArgs e)
        {
            Select(radioButton15.Text);
        }

        private void radioButton6_MouseClick(object sender, EventArgs e)
        {
            Select(radioButton6.Text);
        }

        private void radioButton12_MouseClick(object sender, EventArgs e)
        {
            Select(radioButton12.Text);
        }

        private void radioButton4_MouseClick(object sender, EventArgs e)
        {
            Select(radioButton4.Text);
        }

        private void radioButton3_MouseClick(object sender, EventArgs e)
        {
            Select(radioButton3.Text);
        }

        private void radioButton8_MouseClick(object sender, EventArgs e)
        {
            Select(radioButton8.Text);
        }

        private void radioButton11_MouseClick(object sender, EventArgs e)
        {
            Select(radioButton11.Text);
        }
#endregion
    }
}
