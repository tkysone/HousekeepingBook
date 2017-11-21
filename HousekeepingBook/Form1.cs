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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        // データを追加する
        private void AddData()
        {
            ItemForm frmItem = new ItemForm(categoryDataSet1);
            DialogResult drRet = frmItem.ShowDialog();

            if (drRet == DialogResult.OK)
            {
                moneyDataSet.moneyDataTable.AddmoneyDataTableRow(
                    frmItem.monCalendar.SelectionRange.Start,
                    frmItem.cmbCategory.Text,
                    frmItem.txtItem.Text,
                    int.Parse(frmItem.mtxtMoney.Text),
                    frmItem.txtRemarks.Text);
            }
        }


        // 追加ボタン
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            AddData();
        }


        // アプリを起動したとき
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
            categoryDataSet1.CategoryDataTable.AddCategoryDataTableRow("給料", "入金");
            categoryDataSet1.CategoryDataTable.AddCategoryDataTableRow("食費", "出金");
            categoryDataSet1.CategoryDataTable.AddCategoryDataTableRow("雑費", "出金");
            categoryDataSet1.CategoryDataTable.AddCategoryDataTableRow("住居", "出金");
        }


        // メニューの追加
        private void 追加AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddData();
        }


        // 終了ボタン
        private void buttonEnd_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        // メニューの終了
        private void 終了XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // 入力データを保存する
        private void SaveData()
        {
            string path = "MoneyData.csv";
            // 1行分のデータ
            string strData = "";
            System.IO.StreamWriter sw = new System.IO.StreamWriter(
                path,
                false,
                System.Text.Encoding.Default);

            foreach (MoneyDataSet.moneyDataTableRow drMoney in moneyDataSet.moneyDataTable)
            {
                strData = drMoney.日付.ToShortDateString() + ","
                        + drMoney.分類 + ","
                        + drMoney.品名 + ","
                        + drMoney.金額.ToString() + ","
                        + drMoney.備考;
                sw.WriteLine(strData);
            }
            sw.Close();
        }

        // アプリを終了したときは保存して終了
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveData();
        }

        // メニューの保存
        private void 保存SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        // 起動したときに一番最初に読み込むデータ
        private void LoadData()
        {
            // 入力ファイル名
            string path = "MoneyData.csv";
            // 区切り文字
            string delimStr = ",";
            // 区切り文字をまとめる
            char[] delimiter = delimStr.ToCharArray();
            // 分解後の入れ物
            string[] strData;
            // 1行分のデータ
            string strLine;
            bool fileExists = System.IO.File.Exists(path);
            if (fileExists)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(
                    path,
                    System.Text.Encoding.Default);
                while (sr.Peek() >= 0)
                {
                    strLine = sr.ReadLine();
                    strData = strLine.Split(delimiter);
                    moneyDataSet.moneyDataTable.AddmoneyDataTableRow(
                        DateTime.Parse(strData[0]),
                        strData[1],
                        strData[2],
                        int.Parse(strData[3]),
                        strData[4]);
                }
                sr.Close();
            }
        }
        

        // 編集時に行う操作
        private void UpdateData()
        {
            int nowRow = dgv.CurrentRow.Index;
            DateTime oldDate = DateTime.Parse(dgv.Rows[nowRow].Cells[0].Value.ToString());
            string oldCategory = dgv.Rows[nowRow].Cells[1].Value.ToString();
            string oldItem = dgv.Rows[nowRow].Cells[2].Value.ToString();
            int oldMoney = int.Parse(dgv.Rows[nowRow].Cells[3].Value.ToString());
            string oldRemarks = dgv.Rows[nowRow].Cells[4].Value.ToString();
            ItemForm frmItem = new HousekeepingBook.ItemForm(
                categoryDataSet1,
                oldDate,
                oldCategory,
                oldItem,
                oldMoney,
                oldRemarks);
            DialogResult drRet = frmItem.ShowDialog();
            if (drRet == DialogResult.OK)
            {
                dgv.Rows[nowRow].Cells[0].Value = frmItem.monCalendar.SelectionRange.Start;
                dgv.Rows[nowRow].Cells[1].Value = frmItem.cmbCategory.Text;
                dgv.Rows[nowRow].Cells[2].Value = frmItem.txtItem.Text;
                dgv.Rows[nowRow].Cells[3].Value = int.Parse(frmItem.mtxtMoney.Text);
                dgv.Rows[nowRow].Cells[4].Value = frmItem.txtRemarks.Text;
            }
        }


        // 変更ボタン
        private void buttonChange_Click(object sender, EventArgs e)
        {
            UpdateData();
        }

        // メニューの変更ボタン
        private void 変更CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateData();
        }

        // 削除の操作
        private void DeleteData()
        {
            int nowRow = dgv.CurrentRow.Index;
            // 現在行を削除
            dgv.Rows.RemoveAt(nowRow);
        }

        // 削除ボタン
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            DeleteData();
        }

        // メニューの削除
        private void 削除DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteData();
        }


        // 日ごとの入出金を集計表示
        private void CalcSummary()
        {
            string expression;
            // 集計用データセットのテーブルを初期化
            summaryDataSet.SumDataTable.Clear();

            // 一覧表示のデータセット(MoneyDataSet)のテーブルのレコード数だけループ
            foreach (MoneyDataSet.moneyDataTableRow drMoney in moneyDataSet.moneyDataTable)
            {
                // 検索文字列作成(列名='値')
                expression = "日付='" + drMoney.日付.ToShortDateString() + "'";
                SummaryDataSet.SumDataTableRow[] curDR =
                    (SummaryDataSet.SumDataTableRow[]) // ()～でキャストして型変換している
                    summaryDataSet.SumDataTable.Select(expression); // 集計用データセットのデータテーブルから現在処理している日付のデータを検索

                // データの件数が0件ならば、集計用データセット(SummaryDataSet)のデータテーブルに該当日付のデータがまだない
                if (curDR.Length == 0)
                {
                    CategoryDataSet.CategoryDataTableRow[] selectedDataRow;
                    selectedDataRow = (CategoryDataSet.CategoryDataTableRow[])
                        categoryDataSet1.CategoryDataTable.Select("分類='" + drMoney.分類 + "'");
                    if (selectedDataRow[0].入出金分類 == "入金")
                    {
                        summaryDataSet.SumDataTable.AddSumDataTableRow(
                            drMoney.日付,
                            drMoney.金額,
                            0);
                    }
                    else if (selectedDataRow[0].入出金分類 == "出金")
                    {
                        summaryDataSet.SumDataTable.AddSumDataTableRow(
                            drMoney.日付,
                            0,
                            drMoney.金額);
                    }

                }
                // 日付がかぶっているとき、入出金を足す
                else
                {
                    CategoryDataSet.CategoryDataTableRow[] selectedDataRow;
                    selectedDataRow
                        = (CategoryDataSet.CategoryDataTableRow[])
                            categoryDataSet1.CategoryDataTable.Select(
                                "分類='" + drMoney.分類 + "'");
                    if (selectedDataRow[0].入出金分類 == "入金")
                    {
                        curDR[0].入金合計 += drMoney.金額;
                    }
                    else if (selectedDataRow[0].入出金分類 == "出金")
                    {
                        curDR[0].出金合計 += drMoney.金額;
                    }

                }
            }
        }

        // タブで集計表示を選んだ時
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalcSummary();
        }

        // メニューで一覧表示
        private void 一覧表示LToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab(tabList);
        }

        // メニューで集計表示
        private void 集計表示SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // タブの表示を切り替える
            tabControl1.SelectTab(tabSummary);
        }

        // メニューでヘルプ
        private void バージョン情報VToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HelpForm help = new HelpForm();
            DialogResult drRet = help.ShowDialog();
        }


        // 検索ボタン
        private void buttonSearch_Click(object sender, EventArgs e)
        {
            SearchForm SearchForm = new SearchForm();
            DialogResult drRet = SearchForm.ShowDialog();
        }

        private void buttonDesition_Click(object sender, EventArgs e)
        {
            //SearchForm search = new SearchForm();

            //if (search.dateTimePicker1.Value.ToShortDateString() != null)
            //{
            //    Console.WriteLine(search.dateTimePicker1.Value.ToShortDateString());
            //}
            //else
            //{
            //    MessageBox.Show(
            //        "年月日を選択してください",
            //        "エラー",
            //        MessageBoxButtons.OK,
            //        MessageBoxIcon.Error
            //        );
            //}

            
        }
    }
}