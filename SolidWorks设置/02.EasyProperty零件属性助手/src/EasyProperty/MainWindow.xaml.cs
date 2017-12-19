using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using Environment = System.Environment;

namespace EasyProperty
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    struct Info
        {
        public string DeaNo ;
        public string Name ;
        public string Designer ;
        public string DesignDate;
        public string DwgNo ;
        public string DwgName ;
        public string Note ;
        }
    public partial class MainWindow : Window
    {
        #region Class Variables
        public SldWorks SwApp = null;
        public ModelDoc2 SwModel = null;
        public ModelDocExtension SwModelExt = null;
        public CustomPropertyManager SwCustPropMgr = null;
        public ConfigurationManager SwConfigMgr = null;
        public Configuration SwConfig = null;
        public SelectionMgr SwSelMgr = null;
        Info _swInfo;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            this.Reset();
            CkbSameInfo.IsChecked = true;
        }

        private void Reset()
        {
            SwApp = new SldWorks();
            if (SwApp == null)
            {
                MessageBox.Show("未找到已启动的Solidworks主程序！");
                Environment.Exit(0);
            }
            SwApp.Visible = true;

            SwModel = (ModelDoc2)SwApp.ActiveDoc;
            if (SwModel != null)
            {
                if (((int)SwModel.GetType() != (int)swDocumentTypes_e.swDocPART) && ((int)SwModel.GetType() != (int)swDocumentTypes_e.swDocASSEMBLY))
                {
                    MessageBox.Show("当前文件不是零件或装配体文件！");
                    Environment.Exit(0);
                }
            }
            else
            {
                MessageBox.Show("请打开零件或装配体文件！");
                Environment.Exit(0);
            }

            SwConfigMgr =  SwModel.ConfigurationManager;
            SwModelExt = SwModel.Extension;
            SwCustPropMgr = SwModelExt.get_CustomPropertyManager("");
            
            string[] configNames = null;
            configNames = (string[])SwModel.GetConfigurationNames();
            CboxConfigName.Items.Clear();
            for (int i = 0; i < configNames.Length; i++)
            {
                CboxConfigName.Items.Add(configNames[i]);
                if (configNames[i].Contains(SwConfigMgr.ActiveConfiguration.Name))
                {
                    //cboxConfigName.SelectedIndex = i;
                    CboxConfigName.Text = SwConfigMgr.ActiveConfiguration.Name;
                }
            }

            DatePickerDesignDate.SelectedDate = DateTime.Today;

            TextColorReset();

            TextBlock.Text = "已成功连接到SolidWorks文件!   按F5重新载入!";
   
        }

        private void ReadConfigInfo()
        {
            
            string configName = CboxConfigName.Text;
            object Params = null;
            object value = null;
            bool retVal;
        
            retVal = SwConfigMgr.GetConfigurationParams(configName, out Params, out value);
            string[] configParams =(string[])Params;
            string[] configParamsValue=(string[])value;

            for (int i = 0; i < (configParams.Length); i++)
            {
                if (configParams[i].Contains("东方自控编码"))
                {
                    TextBoxDeaNo.Text = configParamsValue[i].Trim();
                    _swInfo.DeaNo = configParamsValue[i];
                }
                else if (configParams[i].Contains("名称"))
                {
                    TextBoxName.Text = configParamsValue[i].Trim();
                    _swInfo.Name = configParamsValue[i].Trim();
                }
                else if (configParams[i].Contains("设计者"))
                {
                    TextBoxDesigner.Text = configParamsValue[i].Trim();
                    _swInfo.Designer = configParamsValue[i].Trim();
                }
                else if (configParams[i].Contains("日期"))
                {
                    DatePickerDesignDate.Text = configParamsValue[i].Trim();
                    _swInfo.DesignDate = configParamsValue[i].Trim();
                }
            }
        }

        private void ReadCustInfo()
        {
            string val="";
            string valOut="";
            bool retVal;
            
            retVal=SwCustPropMgr.Get4("图样代号",false,out val, out valOut);
            TextBoxDwgNo.Text = valOut.Trim();
            _swInfo.DwgNo = valOut.Trim();

            retVal = SwCustPropMgr.Get4("图样名称", false, out val, out valOut);
            TextBoxDwgName.Text = valOut.Trim();
            _swInfo.DwgName = valOut.Trim();

            retVal = SwCustPropMgr.Get4("备注", false, out val, out valOut);
            TextBoxNote.Text = valOut.Trim();
            _swInfo.Note = valOut.Trim();
        }

        private void cboxConfigName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TextColorReset();
            ReadConfigInfo();
            ReadCustInfo();
        }

        #region
        private void textBoxDEANo_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!TextBoxDeaNo.Text.Equals(_swInfo.DeaNo))
            { TextBoxDeaNo.Foreground = Brushes.Red; }
            else
            { TextBoxDeaNo.Foreground = Brushes.Black; }

            if (CkbSameInfo.IsChecked == true)
            {
                TextBoxDwgNo.Text = TextBoxDeaNo.Text;
            }
        }

        private void textBoxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!TextBoxName.Text.Equals(_swInfo.Name))
            { TextBoxName.Foreground = Brushes.Red; }
            else
            { TextBoxName.Foreground = Brushes.Black; }

            if (CkbSameInfo.IsChecked == true)
            {
                TextBoxDwgName.Text = TextBoxName.Text;
            }
        }

        private void textBoxDesigner_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!TextBoxDesigner.Text.Equals(_swInfo.Designer))
            { TextBoxDesigner.Foreground = Brushes.Red; }
            else
            { TextBoxDesigner.Foreground = Brushes.Black; }
        }

        private void datePickerDesignDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!DatePickerDesignDate.Text.Equals(_swInfo.DesignDate))
            { DatePickerDesignDate.Foreground = Brushes.Red; }
            else
            { DatePickerDesignDate.Foreground = Brushes.Black; }
        }

        private void textBoxDwgNo_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!TextBoxDwgNo.Text.Equals(_swInfo.DwgNo))
            { TextBoxDwgNo.Foreground = Brushes.Red; }
            else
            { TextBoxDwgNo.Foreground = Brushes.Black; }

            if (CkbSameInfo.IsChecked == true)
            {
                TextBoxDeaNo.Text = TextBoxDwgNo.Text;
            }
        }

        private void textBoxDwgName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!TextBoxDwgName.Text.Equals(_swInfo.DwgName))
            { TextBoxDwgName.Foreground = Brushes.Red; }
            else
            { TextBoxDwgName.Foreground = Brushes.Black; }

            if (CkbSameInfo.IsChecked == true)
            {
                TextBoxName.Text = TextBoxDwgName.Text;
            }
        }

        private void textBoxNote_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!TextBoxNote.Text.Equals(_swInfo.Note))
            { TextBoxNote.Foreground = Brushes.Red; }
            else
            { TextBoxNote.Foreground = Brushes.Black; }
        }
        #endregion

        private void TextColorReset()
        {
            TextBoxDeaNo.Foreground = Brushes.Black;
            TextBoxName.Foreground = Brushes.Black;
            TextBoxDesigner.Foreground = Brushes.Black;
            DatePickerDesignDate.Foreground = Brushes.Black;
            TextBoxDwgNo.Foreground = Brushes.Black;
            TextBoxDwgName.Foreground = Brushes.Black;
            TextBoxNote.Foreground = Brushes.Black;
        }

        private void buttonSaveInfo_Click(object sender, RoutedEventArgs e)
        {
            SaveAll();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if ( e.Key == Key.Escape)
            {
                this.Close();
            }
            else if (e.Key == Key.S && e.KeyboardDevice.IsKeyDown(Key.LeftCtrl))
            {
                SaveAll();
            }
            else if (e.Key == Key.F5)
            {
                SwModel = (ModelDoc2)SwApp.ActiveDoc;
                if (SwModel != null)
                {
                    if (((int)SwModel.GetType() != (int)swDocumentTypes_e.swDocPART) && ((int)SwModel.GetType() != (int)swDocumentTypes_e.swDocASSEMBLY))
                    {
                        MessageBox.Show("当前文件不是零件或装配体文件！");
                        Environment.Exit(0);
                    }
                }
                else
                {
                    MessageBox.Show("请打开零件或装配体文件！");
                    Environment.Exit(0);
                }

                SwConfigMgr = SwModel.ConfigurationManager;
                SwModelExt = SwModel.Extension;
                SwCustPropMgr = SwModelExt.get_CustomPropertyManager("");

                string[] configNames = null;
                configNames = (string[])SwModel.GetConfigurationNames();
                CboxConfigName.Items.Clear();
                for (int i = 0; i < configNames.Length; i++)
                {
                    CboxConfigName.Items.Add(configNames[i]);
                    if (configNames[i].Contains(SwConfigMgr.ActiveConfiguration.Name))
                    {
                        CboxConfigName.Text = SwConfigMgr.ActiveConfiguration.Name;
                    }
                }

                DatePickerDesignDate.SelectedDate = DateTime.Today;

                TextColorReset();

                TextBlock.Text = "已成功连接到SolidWorks文件!   按F5重新载入!";
            }

        }

        private void SaveAll()
        {
            try
            {
                int intVal;
                SwConfig = (Configuration)SwModel.GetConfigurationByName((string)CboxConfigName.SelectedValue);
                CustomPropertyManager swConfigCustMgr = SwConfig.CustomPropertyManager;

                intVal = swConfigCustMgr.Add2(TextBox1.Text, (int)swCustomInfoType_e.swCustomInfoText, TextBoxDeaNo.Text);
                if (intVal == -1)
                {
                    intVal = swConfigCustMgr.Set(TextBox1.Text, TextBoxDeaNo.Text.Trim());
                }

                intVal = swConfigCustMgr.Add2(TextBox2.Text, (int)swCustomInfoType_e.swCustomInfoText, TextBoxName.Text);
                if (intVal == -1)
                {
                    intVal = swConfigCustMgr.Set(TextBox2.Text, TextBoxName.Text.Trim());
                }

                intVal = swConfigCustMgr.Add2(TextBox3.Text, (int)swCustomInfoType_e.swCustomInfoText, TextBoxDesigner.Text);
                if (intVal == -1)
                {
                    intVal = swConfigCustMgr.Set(TextBox3.Text, TextBoxDesigner.Text.Trim());
                }

                intVal = swConfigCustMgr.Add2(TextBox4.Text, (int)swCustomInfoType_e.swCustomInfoDate, DatePickerDesignDate.SelectedDate.ToString());
                if (intVal == -1)
                {
                    intVal = swConfigCustMgr.Set(TextBox4.Text, DatePickerDesignDate.SelectedDate.ToString().Trim());
                }



                intVal = SwCustPropMgr.Add2(TextBox5.Text, (int)swCustomInfoType_e.swCustomInfoText, TextBoxDwgNo.Text);
                if (intVal == 0)
                {
                    intVal = SwCustPropMgr.Set(TextBox5.Text, TextBoxDwgNo.Text.Trim());
                }

                intVal = SwCustPropMgr.Add2(TextBox6.Text, (int)swCustomInfoType_e.swCustomInfoText, TextBoxDwgName.Text);
                if (intVal == 0)
                {
                    intVal = SwCustPropMgr.Set(TextBox6.Text, TextBoxDwgName.Text.Trim());
                }

                intVal = SwCustPropMgr.Add2(TextBox7.Text, (int)swCustomInfoType_e.swCustomInfoText, TextBoxNote.Text);
                if (intVal == 0)
                {
                    intVal = SwCustPropMgr.Set(TextBox7.Text, TextBoxNote.Text.Trim());
                }

                //intVal = swCustPropMgr.Set(textBox5.Text, textBoxDwgNo.Text);
                //intVal = swCustPropMgr.Set(textBox6.Text, textBoxDwgName.Text);
                //intVal = swCustPropMgr.Set(textBox7.Text, textBoxNote.Text);

                SwModel.ForceRebuild3(true);

                TextColorReset();

                TextBlock.Text = "写入完成。";
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            

            //this.Hide();
            //System.Threading.Thread th = new System.Threading.Thread(new System.Threading.ThreadStart(msg));
            //th.Start();
            //System.Threading.Thread.Sleep(1000);
            //System.Environment.Exit(0);
            this.Close();
        }
        void Msg()
        {
            MessageBox.Show("写入完成。", "EasyProperty");
            Thread.Sleep(1000);
        }
    }
}
