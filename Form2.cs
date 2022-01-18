using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ВольтМетр
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            
            List<double> X = new List<double>();
            chart1.Series[0].Points.Clear();
            for (int i = 0; i < Data.Xi.Count; i++)
            {
                Data.Xi[i] = Math.Round(Data.Xi[i], 0);
            }
            for (int i = 0; i < Data.Xi.Count; i++)
            {
                X.Add(Data.Xi[i]);
            }
            X.Distinct();
            int cnt = 0;
            for (int i = 0; i < X.Count - 1; i++)
            {
                cnt = 0;
                for (int j = 0; j < Data.Xi.Count; j++)
                {
                    if (X[i] == Data.Xi[j])
                    {
                        cnt++;
                    }           
                }
                chart1.Series[0].Points.AddXY(X[i], cnt);
            }
            
            X.Clear();
        }
    }
}
