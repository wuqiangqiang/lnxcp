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
using FoodSafetyMonitoring.dao;
using System.Data;
using FoodSafetyMonitoring.Common;
using Toolkit = Microsoft.Windows.Controls;
using FoodSafetyMonitoring.Manager.UserControls;
using System.Printing;


namespace FoodSafetyMonitoring.Manager
{
    /// <summary>
    /// UcCreateCertificate.xaml 的交互逻辑
    /// </summary>
    public partial class UcCreateCertificate : UserControl
    {
        //DataTable ProvinceCityTable;
        public IDBOperation dbOperation = null;
        private Dictionary<string, MyColumn> MyColumns = new Dictionary<string, MyColumn>();
        string userId = (Application.Current.Resources["User"] as UserInfo).ID;
        string loginid = (Application.Current.Resources["User"] as UserInfo).LoginName;
        string username = (Application.Current.Resources["User"] as UserInfo).ShowName;
        string deptId = (Application.Current.Resources["User"] as UserInfo).DepartmentID;
        private string cityname;
        private string shipperflag;

        public UcCreateCertificate(IDBOperation dbOperation)
        {
            InitializeComponent();
            this.dbOperation = dbOperation;

            _user_name.Text = username;
            _user_id.Text = loginid;
            _nian.Text = ConvertStr.convert_nian(DateTime.Now.Year.ToString());
            _yue.Text = ConvertStr.convert_yue(DateTime.Now.Month.ToString());
            _day.Text = ConvertStr.convert_day(DateTime.Now.Day.ToString());

            //出证人所属的市级单位,及出证人所属部门货主信息flag,赋值启运地点
            DataTable table = dbOperation.GetDbHelper().GetDataSet("select sys_city.name as city,ifnull(a.shipperflag,'') as shipperflag," +
                                    " tzcname,tzcarea,tzcaddress" +
                                    " from sys_client_sysdept a LEFT JOIN sys_city ON a.city = sys_city.id" +
                                    " where INFO_CODE = " + deptId).Tables[0];
            if(table.Rows.Count != 0)
            {
                cityname = table.Rows[0][0].ToString();
                shipperflag = table.Rows[0][1].ToString();

                string tzcarea = table.Rows[0][3].ToString();

                _city_ks.Text = tzcarea.Substring(0,2).ToString();
                _region_ks.Text = tzcarea.Substring(3, 3).ToString();
                //_town_ks.Text = table.Rows[0][4].ToString();
                _village_ks.Text = table.Rows[0][2].ToString();
            }            

            //检疫证号
            string card_id = dbOperation.GetDbHelper().GetSingle(string.Format("select f_get_cardid('{0}')", deptId)).ToString();
            _card_id.Text = card_id;
            //协检员
            ComboboxTool.InitComboboxSource(_help_user, string.Format("call p_user_helpuser({0})", userId), "lr");
            //动物种类
            ComboboxTool.InitComboboxSource(_object_id, "SELECT animalid,animalname FROM t_animal_new WHERE openflag = '1' and deptflag = '" + shipperflag + "'", "lr");
            _object_id.SelectionChanged += new SelectionChangedEventHandler(_object_id_SelectionChanged);
            _object_id.SelectedIndex = 1;
            //用途
            ComboboxTool.InitComboboxSource(_for_use, "SELECT useid,usename FROM t_for_use WHERE openflag = '1'", "lr");
            _for_use.SelectedIndex = 1;
        }

        void _object_id_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_object_id.SelectedIndex > 0)
            {
                string object_type = dbOperation.GetDbHelper().GetSingle("select unit from t_animal_new where animalid =" + (_object_id.SelectedItem as Label).Tag.ToString()).ToString();
                _object_type.Text = object_type;
            }
        }
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if(_shipper_id.Text.Trim().Length != 0)
                {
                    DataTable table = dbOperation.GetDbHelper().GetDataSet("select shippername,phone,region,town,village from t_shipper where shipperid =" + _shipper_id.Text + " and shipperflag = '" + shipperflag + "'").Tables[0];
                    if (table.Rows.Count != 0)
                    {
                        _shipper.Text = table.Rows[0][0].ToString();
                        _phone.Text = table.Rows[0][1].ToString();
                        _region_js.Text = table.Rows[0][2].ToString();
                        _town_js.Text = table.Rows[0][3].ToString();
                        _village_js.Text = table.Rows[0][4].ToString();
                        _city_js.Text = cityname;
                    }
                    else
                    {
                        Toolkit.MessageBox.Show("该货主不存在！", "系统提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        _shipper.Text = "";
                        _phone.Text = "";
                        _region_js.Text = "";
                        _town_js.Text = "";
                        _village_js.Text = "";
                        _city_js.Text = "";
                        return;
                    }
                }
                else
                {
                    _shipper.Text = "";
                    _phone.Text = "";
                    _region_js.Text = "";
                    _town_js.Text = "";
                    _village_js.Text = "";
                    _city_js.Text = "";
                }
               
            }
        }

        private void _shipper_id_LostFocus(object sender, RoutedEventArgs e)
        {
            if(_shipper_id.Text.Trim().Length != 0)
            {
                DataTable table = dbOperation.GetDbHelper().GetDataSet("select shippername,phone,region,town,village from t_shipper where shipperid =" + _shipper_id.Text + " and shipperflag = '" + shipperflag + "'").Tables[0];
                if (table.Rows.Count != 0)
                {
                    _shipper.Text = table.Rows[0][0].ToString();
                    _phone.Text = table.Rows[0][1].ToString();
                    _region_js.Text = table.Rows[0][2].ToString();
                    _town_js.Text = table.Rows[0][3].ToString();
                    _village_js.Text = table.Rows[0][4].ToString();
                    _city_js.Text = cityname;
                }
                else
                {
                    Toolkit.MessageBox.Show("该货主不存在！", "系统提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    _shipper.Text = "";
                    _phone.Text = "";
                    _region_js.Text = "";
                    _town_js.Text = "";
                    _village_js.Text = "";
                    _city_js.Text = "";
                    return;
                }
            }
            else
            {
                _shipper.Text = "";
                _phone.Text = "";
                _region_js.Text = "";
                _town_js.Text = "";
                _village_js.Text = "";
                _city_js.Text = "";
            }
            
        }

        private void _add_Click(object sender, RoutedEventArgs e)
        {
            AddShipper ship = new AddShipper(dbOperation, shipperflag);
            ship.ShowDialog();
        }  

        private void _create_Click(object sender, RoutedEventArgs e)
        {
            if (_card_id.Text.Trim().Length == 0)
            {
                Toolkit.MessageBox.Show("请输入检疫证号！", "系统提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            bool exit_flag = dbOperation.GetDbHelper().Exists(string.Format("SELECT count(cardid) from t_certificate where cardid ='{0}'", _card_id.Text));
            if (exit_flag)
            {
                Toolkit.MessageBox.Show("检疫证号已存在，请重新输入！", "系统提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (_help_user.SelectedIndex < 1)
            {
                Toolkit.MessageBox.Show("请选择协检员！", "系统提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (_shipper_id.Text.Trim().Length == 0)
            {
                Toolkit.MessageBox.Show("请输入货主代码！", "系统提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (_object_id.Text.Trim().Length == 0)
            {
                Toolkit.MessageBox.Show("请选择动物种类！", "系统提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (_object_count.Text.Trim().Length == 0)
            {
                Toolkit.MessageBox.Show("请输入数量！", "系统提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (_for_use.SelectedIndex < 1)
            {
                Toolkit.MessageBox.Show("请选择用途！", "系统提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (_city_ks.Text.Trim().Length == 0)
            {
                Toolkit.MessageBox.Show("请输入启运地点:市（州）！", "系统提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (_region_ks.Text.Trim().Length == 0)
            {
                Toolkit.MessageBox.Show("请输入启运地点:县（市、区）！", "系统提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            //if (_town_ks.Text.Trim().Length == 0)
            //{
            //    Toolkit.MessageBox.Show("请输入启运地点:乡（镇）！", "系统提示", MessageBoxButton.OK, MessageBoxImage.Information);
            //    return;
            //}
            if (_village_ks.Text.Trim().Length == 0)
            {
                Toolkit.MessageBox.Show("请输入启运地点:村（养殖场、交易市场）！", "系统提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (_city_js.Text.Trim().Length == 0)
            {
                Toolkit.MessageBox.Show("请输入到达地点:市（州）！", "系统提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            //if (_region_js.Text.Trim().Length == 0)
            //{
            //    Toolkit.MessageBox.Show("请输入到达地点:县（市、区）！", "系统提示", MessageBoxButton.OK, MessageBoxImage.Information);
            //    return;
            //}
            //if (_town_js.Text.Trim().Length == 0)
            //{
            //    Toolkit.MessageBox.Show("请输入到达地点:乡（镇）！", "系统提示", MessageBoxButton.OK, MessageBoxImage.Information);
            //    return;
            //}
            if (_village_js.Text.Trim().Length == 0)
            {
                Toolkit.MessageBox.Show("请输入到达地点:村（养殖场、交易市场）！", "系统提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            string sql = string.Format("INSERT INTO t_certificate(cardid,companyid,companyname,objectid,objectname,objectcount," +
                                        "phone,foruseid,foruse,cityks,regionks,townks,villageks,cityjs,regionjs,townjs," +
                                        "villagejs,objectlable,createdeptid,createuserid,createdate,createloginid,helpuserid)" +
                                        " values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}'," +
                                        "'{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}')"
                            , _card_id.Text, _shipper_id.Text, _shipper.Text,
                            (_object_id.SelectedItem as Label).Tag.ToString(), _object_id.Text, _object_count.Text + _object_type.Text,
                            _phone.Text, (_for_use.SelectedItem as Label).Tag.ToString(), _for_use.Text, _city_ks.Text,
                             _region_ks.Text, _town_ks.Text, _village_ks.Text, _city_js.Text, _region_js.Text,
                            _town_js.Text, _village_js.Text, _object_lable.Text, deptId, userId,
                            System.DateTime.Now, loginid,(_help_user.SelectedItem as Label).Tag.ToString());

            int i = dbOperation.GetDbHelper().ExecuteSql(sql);
            if (i >= 0)
            {
                List<string> cer_details = new List<string>() {_card_id.Text,_shipper.Text,_object_id.Text, _object_count.Text,_object_type.Text, _phone.Text,
                            _for_use.Text, _city_ks.Text, _region_ks.Text, _town_ks.Text, _village_ks.Text, _city_js.Text, _region_js.Text,
                            _town_js.Text, _village_js.Text, _object_lable.Text,username,loginid,
                            System.DateTime.Now.Year.ToString(),System.DateTime.Now.Month.ToString(),System.DateTime.Now.Day.ToString() };

                UcCertificateDetails cer = new UcCertificateDetails(cer_details);
                //grid_info.Children.Add(cer);
                PrintDialog dialog = new PrintDialog();
                //if (dialog.ShowDialog() == true)
                //{
                    dialog.PrintQueue = GetPrinter();
                    Size printSize = new Size(dialog.PrintableAreaWidth, dialog.PrintableAreaHeight);
                    cer.Measure(printSize);
                    cer.Arrange(new Rect(0, 0, dialog.PrintableAreaWidth, dialog.PrintableAreaHeight));
                    //Size printSize = new Size(793, 529);
                    //cer.Measure(printSize);
                    //cer.Arrange(new Rect(0, 0, 793, 529));

                    dialog.PrintVisual(cer, "动物检疫证");
                //}

                //Toolkit.MessageBox.Show("电子出证单生成成功！", "系统提示", MessageBoxButton.OK, MessageBoxImage.Information);
                clear();
                return;
            }
            else
            {
                Toolkit.MessageBox.Show("电子出证单生成失败！", "系统提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
        }

        public static PrintQueue GetPrinter(string printerName = null)
        {
            try
            {
                PrintQueue selectedPrinter = null;
                if (!string.IsNullOrEmpty(printerName))
                {
                    var printers = new LocalPrintServer().GetPrintQueues();
                    selectedPrinter = printers.FirstOrDefault(p => p.Name == printerName);
                }
                else
                {
                    selectedPrinter = LocalPrintServer.GetDefaultPrintQueue();
                }
                return selectedPrinter;
            }
            catch
            {
                return null;
            }
        }

        private void clear()
        {
            _card_id.Text = Convert.ToString(Convert.ToInt64(_card_id.Text) + 1);
            _shipper_id.Text = "";
            _shipper.Text = "";
            _phone.Text = "";
            _object_count.Text= "";
            _city_js.Text= "";
            _region_js.Text= "";
            _town_js.Text= "";
            _village_js.Text= "";
            _object_lable.Text = "";
        }

        private void Card_Id_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!isNumberic(text))
                { e.CancelCommand(); }
            }
            else { e.CancelCommand(); }
        }

        private void Card_Id_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        private void Card_Id_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!isNumberic(e.Text))
            {
                e.Handled = true;
            }
            else
                e.Handled = false;
        }

        private void Object_Count_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!isNumberic(text))
                { e.CancelCommand(); }
            }
            else { e.CancelCommand(); }
        }

        private void Object_Count_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        private void Object_Count_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!isNumberic(e.Text))
            {
                e.Handled = true;
            }
            else
                e.Handled = false;
        }

        //isDigit是否是数字
        public static bool isNumberic(string _string)
        {
            if (string.IsNullOrEmpty(_string))

                return false;
            foreach (char c in _string)
            {
                if (!char.IsDigit(c))
                    //if(c<'0' c="">'9')//最好的方法,在下面测试数据中再加一个0，然后这种方法效率会搞10毫秒左右
                    return false;
            }
            return true;
        }        
    }
}
