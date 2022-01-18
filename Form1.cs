using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ВольтМетр
{
    public partial class Form1 : Form
    {
        double voltage;
        double random_error;
        double system_error;
        double min_rand;
        double max_rand;
        int rowsCount = 0;
        int ROWS = 0;
        int n = 0;
        double average;
        double sum;
        double Vi;
        double Vi_square;
        double Vi_square_9;
        double kvadrotkl;
        string result;
        string plusminus = "+-";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Программа разработана студентами ХНУРЭ, факультета КИУ, кафедры БИТ, группы КБИКС-19-2: Зубрич А. В., Гаража Р. Ю., Боровской А. Е., Винник И. Р., Рутковский М. В., Рибаков А. А.", "Информация");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8 || e.KeyChar == 44)
                e.Handled = false;
            else e.Handled = true;
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8 || e.KeyChar == 44)
                e.Handled = false;
            else e.Handled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
            {
                MessageBox.Show("Введите значение.");
                textBox1.Focus();
            }
            else if (textBox3.Text.Length == 0)
            {
                MessageBox.Show("Введите значение.");
                textBox3.Focus();
            }
            else if (listBox2.SelectedItem != "-" && listBox2.SelectedItem != "+")
            {
                MessageBox.Show("Выберите значение.");
                listBox2.SetSelected(0, true);
            }
            else if (listBox1.SelectedItem != "0,05" && listBox1.SelectedItem != "0,1" && listBox1.SelectedItem != "0,2" && listBox1.SelectedItem != "0,5" && listBox1.SelectedItem != "1,0" && listBox1.SelectedItem != "1,5" && listBox1.SelectedItem != "2,5" && listBox1.SelectedItem != "4,0")
            {
                MessageBox.Show("Выберите значение.");
                listBox1.SetSelected(0, true);
            }
            else
            {
                voltage = Convert.ToDouble(textBox1.Text);                 
                var random = new Random();
                max_rand = (Convert.ToDouble(textBox3.Text) / 100) * voltage;
                min_rand = (-1) * max_rand;
                random_error = min_rand + random.NextDouble() * (2 * max_rand);
                if (listBox2.SelectedItem == "-")
                {
                    system_error = (-1) * Convert.ToDouble(listBox1.SelectedItem);
                }
                else
                {
                    system_error = Convert.ToDouble(listBox1.SelectedItem);
                }
                voltage = Math.Round(voltage + system_error + random_error, 3);
                if (voltage < 0) voltage = 0;
                textBox2.Text = voltage.ToString();

                dataGridView1.RowCount = ++ROWS;
                dataGridView1.Rows[rowsCount].Cells[0].Value = ROWS;
                dataGridView1.Rows[rowsCount].Cells[1].Value = voltage;
                Data.Xi.Add(voltage);
                rowsCount++;
            }
            if (ROWS > 1)
            {
                button4.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            voltage = 0;
            system_error= 0;
            random_error = 0;
            min_rand = 0;
            max_rand = 0;
            average = 0;
            sum = 0;
            n = 0;
            Vi = 0;
            Vi_square = 0;
            Vi_square_9 = 0;
            kvadrotkl = 0;
            ROWS = 0;
            rowsCount = 0;
            Data.Xi.Clear();
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            button1.Enabled = true;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            button4.Enabled = false;
            button1.Enabled = false;
            dataGridView2.RowCount = 1;
            Data.Xi.Sort();
            for (int i = 0; i < ROWS; ++i)
            {
                dataGridView1.Rows[i].Cells[2].Value = Data.Xi[i];
            }
            average = Data.Xi.Sum() / ROWS;
            for (int i = 0; i < ROWS; ++i)
            {
                kvadrotkl += Math.Pow(Data.Xi[i] - average, 2);
            }
            dataGridView1.Rows[0].Cells[5].Value = Math.Round(Math.Sqrt(kvadrotkl / (Data.Xi.Count - 1)), 3);
            for (int i = 0; i < Data.Xi.Count; ++i)
            {
                if (Data.Xi[i] < (average - (2 * Math.Sqrt(kvadrotkl / (Data.Xi.Count - 1)))))
                {
                    Data.Xi.RemoveAt(i);
                    dataGridView1.Rows[i].Cells[3].Value = "промах";
                    dataGridView1.Rows[i].Cells[4].Value = "промах";
                    dataGridView2.Rows[0].Cells[7].Value = "промах";
                    n++;
                }
                else
                {
                    dataGridView2.Rows[0].Cells[7].Value = "норма";
                }

                if (Data.Xi[i] > (average + (2 * Math.Sqrt(kvadrotkl / (Data.Xi.Count - 1)))))
                {
                    Data.Xi.RemoveAt(i);
                    dataGridView1.Rows[i].Cells[3].Value = "промах";
                    dataGridView1.Rows[i].Cells[4].Value = "промах";
                    dataGridView2.Rows[0].Cells[8].Value = "промах";
                }
                else
                {
                    dataGridView2.Rows[0].Cells[8].Value = "норма";
                }
            }
            if (n == 1)
            {
                dataGridView2.Rows[0].Cells[7].Value = "промах";
            }
            ROWS = Data.Xi.Count;

            kvadrotkl = 0;
            average = Data.Xi.Sum() / ROWS;
            dataGridView2.Rows[0].Cells[0].Value = Math.Round(average, 3);

            sum = Data.Xi.Sum();
            dataGridView2.Rows[0].Cells[1].Value = sum;
            for (int i = 0; i < ROWS; ++i)
            {
                Vi = Data.Xi[i] - average;
                dataGridView1.Rows[i + n].Cells[3].Value = Math.Round(Vi, 3);
                Vi_square = Vi * Vi;
                dataGridView1.Rows[i + n].Cells[4].Value = Math.Round(Vi_square, 3);
            }
            for (int i = 0; i < ROWS; ++i)
            {
                kvadrotkl += Math.Pow(Data.Xi[i] - average, 2);
            }
            dataGridView2.Rows[0].Cells[2].Value = Math.Round(kvadrotkl, 3);
            dataGridView2.Rows[0].Cells[3].Value = Math.Round(kvadrotkl / (ROWS - 1), 3);
            dataGridView2.Rows[0].Cells[4].Value = Math.Round(Math.Sqrt(kvadrotkl / (ROWS - 1)), 3);
            dataGridView2.Rows[0].Cells[5].Value = Math.Round(average - (3 * Math.Sqrt(kvadrotkl / (ROWS - 1))), 3);
            dataGridView2.Rows[0].Cells[6].Value = Math.Round(average + (3 * Math.Sqrt(kvadrotkl / (ROWS - 1))), 3);

            dataGridView2.Rows[0].Cells[9].Value = Math.Round(Math.Sqrt(kvadrotkl / (ROWS - 1)) / Math.Sqrt(ROWS - 1), 3);
            dataGridView2.Rows[0].Cells[10].Value = Math.Round((100 * (Math.Sqrt(kvadrotkl / (ROWS - 1)) / Math.Sqrt(ROWS - 1))) / average, 3);

            if (ROWS == 2)
            {
                dataGridView2.Rows[0].Cells[11].Value = 12.71;
                dataGridView2.Rows[0].Cells[12].Value = Math.Round(12.71 * Math.Sqrt(kvadrotkl / (ROWS - 1)), 3);
            }
            else if (ROWS == 3)
            {
                dataGridView2.Rows[0].Cells[11].Value = 4.3;
                dataGridView2.Rows[0].Cells[12].Value = Math.Round(4.3 * Math.Sqrt(kvadrotkl / (ROWS - 1)), 3);
            }
            else if (ROWS == 4)
            {
                dataGridView2.Rows[0].Cells[11].Value = 3.18;
                dataGridView2.Rows[0].Cells[12].Value = Math.Round(3.18 * Math.Sqrt(kvadrotkl / (ROWS - 1)), 3);
            }
            else if (ROWS == 5)
            {
                dataGridView2.Rows[0].Cells[11].Value = 2.78;
                dataGridView2.Rows[0].Cells[12].Value = Math.Round(2.78 * Math.Sqrt(kvadrotkl / (ROWS - 1)), 3);
            }
            else if (ROWS == 6)
            {
                dataGridView2.Rows[0].Cells[11].Value = 2.57;
                dataGridView2.Rows[0].Cells[12].Value = Math.Round(2.57 * Math.Sqrt(kvadrotkl / (ROWS - 1)), 3);
            }
            else if (ROWS == 7)
            {
                dataGridView2.Rows[0].Cells[11].Value = 2.45;
                dataGridView2.Rows[0].Cells[12].Value = Math.Round(2.45 * Math.Sqrt(kvadrotkl / (ROWS - 1)), 3);
            }
            else if (ROWS == 8)
            {
                dataGridView2.Rows[0].Cells[11].Value = 2.36;
                dataGridView2.Rows[0].Cells[12].Value = Math.Round(2.36 * Math.Sqrt(kvadrotkl / (ROWS - 1)), 3);
            }
            else if (ROWS == 9)
            {
                dataGridView2.Rows[0].Cells[11].Value = 2.31;
                dataGridView2.Rows[0].Cells[12].Value = Math.Round(2.31 * Math.Sqrt(kvadrotkl / (ROWS - 1)), 3);
            }
            else if (ROWS == 10)
            {
                dataGridView2.Rows[0].Cells[11].Value = 2.26;
                dataGridView2.Rows[0].Cells[12].Value = Math.Round(2.26 * Math.Sqrt(kvadrotkl / (ROWS - 1)), 3);
            }
            else if (ROWS == 11)
            {
                dataGridView2.Rows[0].Cells[11].Value = 2.23;
                dataGridView2.Rows[0].Cells[12].Value = Math.Round(2.23 * Math.Sqrt(kvadrotkl / (ROWS - 1)), 3);
            }
            else if (ROWS == 12)
            {
                dataGridView2.Rows[0].Cells[11].Value = 2.2;
                dataGridView2.Rows[0].Cells[12].Value = Math.Round(2.2 * Math.Sqrt(kvadrotkl / (ROWS - 1)), 3);
            }
            else if (ROWS == 13)
            {
                dataGridView2.Rows[0].Cells[11].Value = 2.18;
                dataGridView2.Rows[0].Cells[12].Value = Math.Round(2.18 * Math.Sqrt(kvadrotkl / (ROWS - 1)), 3);
            }
            else if (ROWS == 14)
            {
                dataGridView2.Rows[0].Cells[11].Value = 2.16;
                dataGridView2.Rows[0].Cells[12].Value = Math.Round(2.16 * Math.Sqrt(kvadrotkl / (ROWS - 1)), 3);
            }
            else if (ROWS == 15)
            {
                dataGridView2.Rows[0].Cells[11].Value = 2.15;
                dataGridView2.Rows[0].Cells[12].Value = Math.Round(2.15 * Math.Sqrt(kvadrotkl / (ROWS - 1)), 3);
            }
            else if (ROWS == 16)
            {
                dataGridView2.Rows[0].Cells[11].Value = 2.13;
                dataGridView2.Rows[0].Cells[12].Value = Math.Round(2.13 * Math.Sqrt(kvadrotkl / (ROWS - 1)), 3);
            }
            else if (ROWS == 17)
            {
                dataGridView2.Rows[0].Cells[11].Value = 2.12;
                dataGridView2.Rows[0].Cells[12].Value = Math.Round(2.12 * Math.Sqrt(kvadrotkl / (ROWS - 1)), 3);
            }
            else if (ROWS == 18)
            {
                dataGridView2.Rows[0].Cells[11].Value = 2.11;
                dataGridView2.Rows[0].Cells[12].Value = Math.Round(2.11 * Math.Sqrt(kvadrotkl / (ROWS - 1)), 3);
            }
            else if (ROWS == 19)
            {
                dataGridView2.Rows[0].Cells[11].Value = 2.1;
                dataGridView2.Rows[0].Cells[12].Value = Math.Round(2.1 * Math.Sqrt(kvadrotkl / (ROWS - 1)), 3);
            }
            else if (ROWS == 20)
            {
                dataGridView2.Rows[0].Cells[11].Value = 2.09;
                dataGridView2.Rows[0].Cells[12].Value = Math.Round(2.09 * Math.Sqrt(kvadrotkl / (ROWS - 1)), 3);
            }
            else if (ROWS == 21)
            {
                dataGridView2.Rows[0].Cells[11].Value = 2.09;
                dataGridView2.Rows[0].Cells[12].Value = Math.Round(2.09 * Math.Sqrt(kvadrotkl / (ROWS - 1)), 3);
            }
            else if (ROWS == 22)
            {
                dataGridView2.Rows[0].Cells[11].Value = 2.08;
                dataGridView2.Rows[0].Cells[12].Value = Math.Round(2.08 * Math.Sqrt(kvadrotkl / (ROWS - 1)), 3);
            }
            else if (ROWS == 23)
            {
                dataGridView2.Rows[0].Cells[11].Value = 2.07;
                dataGridView2.Rows[0].Cells[12].Value = Math.Round(2.07 * Math.Sqrt(kvadrotkl / (ROWS - 1)), 3);
            }
            else if (ROWS == 24)
            {
                dataGridView2.Rows[0].Cells[11].Value = 2.07;
                dataGridView2.Rows[0].Cells[12].Value = Math.Round(2.07 * Math.Sqrt(kvadrotkl / (ROWS - 1)), 3);
            }
            else if (ROWS == 25)
            {
                dataGridView2.Rows[0].Cells[11].Value = 2.06;
                dataGridView2.Rows[0].Cells[12].Value = Math.Round(2.06 * Math.Sqrt(kvadrotkl / (ROWS - 1)), 3);
            }
            else if (ROWS == 26)
            {
                dataGridView2.Rows[0].Cells[11].Value = 2.06;
                dataGridView2.Rows[0].Cells[12].Value = Math.Round(2.06 * Math.Sqrt(kvadrotkl / (ROWS - 1)), 3);
            }
            else if (ROWS == 27)
            {
                dataGridView2.Rows[0].Cells[11].Value = 2.06;
                dataGridView2.Rows[0].Cells[12].Value = Math.Round(2.06 * Math.Sqrt(kvadrotkl / (ROWS - 1)), 3);
            }
            else if (ROWS == 28)
            {
                dataGridView2.Rows[0].Cells[11].Value = 2.05;
                dataGridView2.Rows[0].Cells[12].Value = Math.Round(2.05 * Math.Sqrt(kvadrotkl / (ROWS - 1)), 3);
            }
            else if (ROWS == 29)
            {
                dataGridView2.Rows[0].Cells[11].Value = 2.05;
                dataGridView2.Rows[0].Cells[12].Value = Math.Round(2.05 * Math.Sqrt(kvadrotkl / (ROWS - 1)), 3);
            }
            else if (ROWS == 30)
            {
                dataGridView2.Rows[0].Cells[11].Value = 2.05;
                dataGridView2.Rows[0].Cells[12].Value = Math.Round(2.05 * Math.Sqrt(kvadrotkl / (ROWS - 1)), 3);
            }
            else if (ROWS == 31)
            {
                dataGridView2.Rows[0].Cells[11].Value = 2.04;
                dataGridView2.Rows[0].Cells[12].Value = Math.Round(2.04 * Math.Sqrt(kvadrotkl / (ROWS - 1)), 3);
            }
            else if (ROWS > 31 && ROWS <= 41)
            {
                dataGridView2.Rows[0].Cells[11].Value = 2.02;
                dataGridView2.Rows[0].Cells[12].Value = Math.Round(2.02 * Math.Sqrt(kvadrotkl / (ROWS - 1)), 3);
            }
            else if (ROWS > 41 && ROWS <= 61)
            {
                dataGridView2.Rows[0].Cells[11].Value = 2.0;
                dataGridView2.Rows[0].Cells[12].Value = Math.Round(2.0 * Math.Sqrt(kvadrotkl / (ROWS - 1)), 3);
            }
            else if (ROWS > 61 && ROWS <= 121)
            {
                dataGridView2.Rows[0].Cells[11].Value = 1.98;
                dataGridView2.Rows[0].Cells[12].Value = Math.Round(1.98 * Math.Sqrt(kvadrotkl / (ROWS - 1)), 3);
            }
            else if (ROWS > 121)
            {
                dataGridView2.Rows[0].Cells[11].Value = 1.96;
                dataGridView2.Rows[0].Cells[12].Value = Math.Round(1.96 * Math.Sqrt(kvadrotkl / (ROWS - 1)), 3);
            }

            result = Math.Round(average, 3).ToString() + plusminus + dataGridView2.Rows[0].Cells[12].Value.ToString();
            dataGridView2.Rows[0].Cells[13].Value = result;

            Form2 f = new Form2();
            f.Show();
        }

    }
}