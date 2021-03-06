﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using FoodSafetyMonitoring.dao;
using FoodSafetyMonitoring.Manager.UserControls;

namespace FoodSafetyMonitoring.Manager
{
    /// <summary>
    /// UcTaskReportCountry.xaml 的交互逻辑
    /// </summary>
    public partial class UcSamplingReportDept : UserControl
    {
        private Dictionary<string, MyColumn> MyColumns = new Dictionary<string, MyColumn>();
        private IDBOperation dbOperation;
        private List<SamplingInfo> list = new List<SamplingInfo>();
        private DataTable currenttable;
        private string user_flag_tier;
        public string Sj { get; set; }
        public string DeptId { get; set; }
        public string ItemId { get; set; }

        public UcSamplingReportDept(IDBOperation dbOperation, string sj, string deptId, string itemId)
        {
            InitializeComponent();

            this.dbOperation = dbOperation;
            this.Sj = sj;
            this.DeptId = deptId;
            this.ItemId = itemId;
            user_flag_tier = (Application.Current.Resources["User"] as UserInfo).FlagTier;

            getdata();

            _tableview.DetailsRowEnvent += new UcTableOperableView_NoPages.DetailsRowEventHandler(_tableview_DetailsRowEnvent);
        }

        private void getdata()
        {
            DataTable table = dbOperation.GetDbHelper().GetDataSet(string.Format("call p_sampling_report_country('{0}','{1}','{2}')",
                                Sj, DeptId, ItemId)).Tables[0];
            currenttable = table;
            list.Clear();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                SamplingInfo info = new SamplingInfo();
                //info.DeptId = table.Rows[i][0].ToString();
                info.DeptName = table.Rows[i][1].ToString();
                //info.ItemId = table.Rows[i][2].ToString();
                info.ItemName = table.Rows[i][3].ToString();
                info.SumActual = table.Rows[i][4].ToString();
                info.SamplingrateActual = table.Rows[i][5].ToString();
                info.SamplingratePercent = table.Rows[i][6].ToString();
                info.SumSamplingrateActual = table.Rows[i][7].ToString();
                info.SumPercent = table.Rows[i][8].ToString();
                list.Add(info);
            }

            //得到行和列标题 及数量            
            string[] DeptNames = list.Select(t => t.DeptName).Distinct().ToArray();
            string[] ItemNames = list.Select(t => t.ItemName).Distinct().ToArray();

            //创建DataTable
            DataTable tabledisplay = new DataTable();

            //表中第一行第一列交叉处一般显示为第1列标题
            tabledisplay.Columns.Add(new DataColumn("序号"));
            MyColumns.Add("序号", new MyColumn("序号", "序号") { BShow = true, Width = 5 });
            tabledisplay.Columns.Add(new DataColumn("检测单位"));
            MyColumns.Add("检测单位", new MyColumn("检测单位", "检测单位") { BShow = true, Width = 16 });


            tabledisplay.Columns.Add(new DataColumn("批次总头数"));
            MyColumns.Add("批次总头数", new MyColumn("批次总头数", "批次总头数") { BShow = true, Width = 8 });

            //表中后面每列的标题其实是列分组的关键字
            for (int i = 0; i < ItemNames.Length; i++)
            {
                DataColumn column = new DataColumn(ItemNames[i]);
                tabledisplay.Columns.Add(column);
                MyColumns.Add(ItemNames[i].ToString().ToLower(), new MyColumn(ItemNames[i].ToString().ToLower(), ItemNames[i].ToString() + "检测量") { BShow = true, Width = 10 });
                tabledisplay.Columns.Add(new DataColumn("抽检率" + i));
                MyColumns.Add("抽检率" + i, new MyColumn("抽检率" + i, "抽检率") { BShow = true, Width = 10 });
            }

            //当选择了检测项目作为查询条件时，不显示任务完成总量和任务总完成率
            bool flag;
            if (ItemId == "")
            {
                flag = true;
            }
            else
            {
                flag = false;
            }

            //表格后面为合计列
            tabledisplay.Columns.Add(new DataColumn("总抽检数"));
            MyColumns.Add("总抽检数", new MyColumn("总抽检数", "总抽检数") { BShow = flag, Width = 10 });
            tabledisplay.Columns.Add(new DataColumn("综合平均抽检率"));
            MyColumns.Add("综合平均抽检率", new MyColumn("综合平均抽检率", "综合平均抽检率") { BShow = flag, Width = 10 });

            //为表中各行生成数据
            for (int i = 0; i < DeptNames.Length; i++)
            {
                var row = tabledisplay.NewRow();
                //每行第0列为行分组关键字
                row[0] = i + 1;
                row[1] = DeptNames[i];
                string count = list.Where(t => t.DeptName == DeptNames[i]).Select(t => t.SumActual).FirstOrDefault();

                if (count == null || count == "")
                {
                    count = '0'.ToString();
                }
                row[2] = count;

                //每行的其余列为行列交叉对应的汇总数据
                for (int j = 0; j < ItemNames.Length; j++)
                {
                    string num = list.Where(t => t.DeptName == DeptNames[i] && t.ItemName == ItemNames[j]).Select(t => t.SamplingrateActual).FirstOrDefault();

                    if (num == null || num == "")
                    {
                        num = '0'.ToString();
                    }
                    row[ItemNames[j]] = num;

                    string percent = list.Where(t => t.DeptName == DeptNames[i] && t.ItemName == ItemNames[j]).Select(t => t.SamplingratePercent).FirstOrDefault();

                    if (percent == null || percent == "")
                    {
                        percent = '0'.ToString();
                    }
                    else
                    {
                        percent = percent + "%";
                    }
                    row[4 + 2 * j] = percent;
                }
                row[ItemNames.Length * 2 + 3] = list.Where(t => t.DeptName == DeptNames[i]).Select(t => t.SumSamplingrateActual).FirstOrDefault();

                string sumpercent = list.Where(t => t.DeptName == DeptNames[i]).Select(t => t.SumPercent).FirstOrDefault();

                if (sumpercent == null || sumpercent == "")
                {
                    sumpercent = '0'.ToString();
                }
                else
                {
                    sumpercent = sumpercent + "%";
                }
                row[ItemNames.Length * 2 + 4] = sumpercent;


                tabledisplay.Rows.Add(row);
            }
            _tableview.MyColumns = MyColumns;
            _tableview.BShowDetails = true;
            _tableview.Table = tabledisplay;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        void _tableview_DetailsRowEnvent(string id)
        {
            string dept_id;

            DataRow[] rows = currenttable.Select("PART_NAME = '" + id + "'");
            dept_id = rows[0]["PART_ID"].ToString();

            UcSamplingReportDetails daydetails = new UcSamplingReportDetails(dbOperation, Sj, dept_id, ItemId);
            daydetails.SetValue(Grid.RowProperty, 0);
            daydetails.SetValue(Grid.RowSpanProperty, 2);

            grid_info.Children.Add(daydetails);
           
        }

        public class SamplingInfo
        {
            //public string DeptId { get; set; }

            public string DeptName { get; set; }

            //public string ItemId { get; set; }

            public string ItemName { get; set; }

            public string SumActual { get; set; }

            public string SamplingrateActual { get; set; }

            public string SamplingratePercent { get; set; }

            public string SumSamplingrateActual { get; set; }

            public string SumPercent { get; set; }
        }
    }
}
