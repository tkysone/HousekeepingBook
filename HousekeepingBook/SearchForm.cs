using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HousekeepingBook
{
    public partial class SearchForm : Form
    {
        public SearchForm()
        {
            InitializeComponent();
        }


        // 決定ボタン
        private void buttonSearch_Click(object sender, EventArgs e)
        {
            SelectDay();
        }

        // 年月日選択して反映
        private void SelectDay()
        {
            Console.WriteLine(dateTimePicker1.Value.ToShortDateString());

            Console.WriteLine("ブランチを追加してみた");

            //// 入力ファイル名
            //string path = "MoneyData.csv";
            //// 区切り文字
            //string delimStr = ",";
            //// 区切り文字をまとめる
            //char[] delimiter = delimStr.ToCharArray();
            //// 分解後の入れ物
            //string[] strData;
            //// 1行分のデータ
            //string strLine;
            //bool fileExists = System.IO.File.Exists(path);
            //if (fileExists)
            //{
            //    System.IO.StreamReader sr = new System.IO.StreamReader(
            //        path,
            //        System.Text.Encoding.Default);
            //    while (sr.Peek() >= 0)
            //    {
            //        strLine = sr.ReadLine();
            //        strData = strLine.Split(delimiter);
            //        moneyDataSet.moneyDataTable.AddmoneyDataTableRow(
            //            DateTime.Parse(strData[0]),
            //            strData[1],
            //            strData[2],
            //            int.Parse(strData[3]),
            //            strData[4]);
            //    }
            //    sr.Close();
            //}

        }
    }
}
