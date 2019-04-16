using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

public struct EightToneBoxItem
{
    public int display;    //行号，对应xml中的行标识
    public string type;     //标识    珍品2 ， 精品1 ， 空白0
    public string male_type;    //男ID
    public string female_type;  //女ID
    public string count;    //数量单位  永久0，NN
    public string probability; //出现概率 ， 出现概率*100000， 最终和100，否则弹窗提示
    public string free_lottery_probability;     //出现概率（仅刮刮了使用）
    public string broadcast;    //是否计入公告
    public string board;    //是否广播
    public string daily_max;    //奖励期望  ， 仅 八音盒奖励配置xml生成    空白，则不需要输入        
};

public struct BaseBoxItem
{
    public string id;
    public string display;  //宝箱名称
    public string para1;    //男ID
    public string para2;    //女ID
    public string para3;    //时效或数量
};
public struct BaseBox
{
    public string ID;
    public string display;  //宝箱名称
    public BaseBoxItem[] basebox_items; //宝箱物品
};


public struct EightToneBox
{
    public EightToneBoxItem[] normal_items; //八音盒奖励配置
    public string normal_start_time;  //八音盒奖励配置开始时间
    public string normal_end_time;  //八音盒奖励配置结束时间

    public EightToneBoxItem[] advance_items;   //珍宝八音盒奖励配置
    public string advance_start_time;  //珍宝八音盒奖励配置开始时间
    public string advance_end_time;  //珍宝八音盒奖励配置结束时间

    public BaseBox[] baseboxs;    //珍宝八音盒保底宝箱配置
    public string basebox_start_time;  //珍宝八音盒保底宝箱配置开始时间
    public string basebox_end_time;  //珍宝八音盒保底宝箱配置结束时间
};

public struct ScrapingData
{
    public EightToneBoxItem[] scraping_items;   //刮刮乐奖励配置
    public string scraping_start_time;  //刮刮乐奖励配置开始时间
    public string scraping_end_time;  //刮刮乐奖励配置结束时间

    public BaseBox[] baseboxs;    //刮刮乐保底宝箱配置
    public string basebox_start_time;  //刮刮乐保底宝箱配置开始时间
    public string basebox_end_time;  //刮刮乐保底宝箱配置结束时间
}

namespace 八音盒配置工具
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //导出八音盒配置文件
        private void btnGenerateXmlOne_Click(object sender ,EventArgs e)
        {
            if ("" == this.XlsxFileText.Text)
            {
                this.LogBox.Text = "请选择导入的xls文件！";
                return;
            }
            if ("" == this.XmlOneText.Text)
            {
                this.LogBox.Text = "请选择导出的xml文件！";
                return;
            }

            //去读取文件
            this.LogBox.Text = "正在读取文件...";
            EightToneBox eightToneBox = new EightToneBox();
            eightToneBox = GetEightToneBoxData();

            //开始准备写入
            this.LogBox.Text = "读取完成，正在写入文件...";
            XmlDocument _doc = new XmlDocument();
            _doc.Load(XmlOneText.Text);
            XmlNode root = _doc.SelectSingleNode("SlotMachine");

            //八音盒奖励配置中有数据
            if (eightToneBox.advance_start_time != "")
            {
                XmlNode normalSlotMachineNode = root.FirstChild;
                XmlElement raffleTable_normal = _doc.CreateElement("RaffleTable");
                raffleTable_normal.SetAttribute("begin_time", eightToneBox.normal_start_time);
                raffleTable_normal.SetAttribute("end_time", eightToneBox.normal_end_time);
                raffleTable_normal.SetAttribute("enableMultiProb", "1");
                raffleTable_normal.SetAttribute("show_multiplier", "1");
                normalSlotMachineNode.AppendChild(raffleTable_normal);

                for (int i = 0; i < eightToneBox.normal_items.Length; i++)
                {
                    XmlElement item = _doc.CreateElement("item");

                    item.SetAttribute("display", "{str_slot_machine_display_" + eightToneBox.normal_items[i].display + "}");
                    item.SetAttribute("type", eightToneBox.normal_items[i].type);
                    item.SetAttribute("male_type", eightToneBox.normal_items[i].male_type);
                    item.SetAttribute("female_type", eightToneBox.normal_items[i].female_type);
                    item.SetAttribute("count", eightToneBox.normal_items[i].count);
                    item.SetAttribute("probability", eightToneBox.normal_items[i].probability);
                    item.SetAttribute("broadcast", eightToneBox.normal_items[i].broadcast);
                    item.SetAttribute("board", eightToneBox.normal_items[i].board);
                    if (eightToneBox.normal_items[i].daily_max != "")
                    {
                        item.SetAttribute("daily_max", eightToneBox.normal_items[i].daily_max);
                    }
                    raffleTable_normal.AppendChild(item);
                }
            }
            //珍宝八音盒奖励配置
            if (eightToneBox.advance_start_time != "")
            {
                XmlNode advancedSlotMachineNode = root.LastChild;
                XmlNodeList advanceNodeList = _doc.SelectNodes("/SlotMachine/AdvancedSlotMachine/RaffleTable");

                //找到最后一个
                XmlElement lastRaffleTable = null;
                foreach (XmlElement ele in advanceNodeList)
                {
                    lastRaffleTable = ele;
                }

                XmlElement raffleTable_advance = _doc.CreateElement("RaffleTable");
                raffleTable_advance.SetAttribute("begin_time", eightToneBox.advance_start_time);
                raffleTable_advance.SetAttribute("end_time", eightToneBox.advance_end_time);
                raffleTable_advance.SetAttribute("enableMultiProb", "1");
                raffleTable_advance.SetAttribute("show_multiplier", "1");
                advancedSlotMachineNode.InsertAfter(raffleTable_advance, lastRaffleTable);

                for (int i = 0; i < eightToneBox.advance_items.Length; i++)
                {
                    XmlElement item = _doc.CreateElement("item");

                    item.SetAttribute("display", "{str_slot_machine_display_" + eightToneBox.advance_items[i].display + "}");
                    item.SetAttribute("type", eightToneBox.advance_items[i].type);
                    item.SetAttribute("male_type", eightToneBox.advance_items[i].male_type);
                    item.SetAttribute("female_type", eightToneBox.advance_items[i].female_type);
                    item.SetAttribute("count", eightToneBox.advance_items[i].count);
                    item.SetAttribute("probability", eightToneBox.advance_items[i].probability);
                    item.SetAttribute("broadcast", eightToneBox.advance_items[i].broadcast);
                    item.SetAttribute("board", eightToneBox.advance_items[i].board);

                    raffleTable_advance.AppendChild(item);
                }
            }


            //珍宝八音盒保底宝箱配置
            if (eightToneBox.basebox_start_time != "")
            {
                XmlNode advancedSlotMachineNode = root.LastChild;
                XmlNode treasureBoxConfigNode = advancedSlotMachineNode.LastChild;
                XmlElement lastBox = (XmlElement)treasureBoxConfigNode.LastChild;

                string id = lastBox.GetAttribute("id");

                XmlElement advanceTreasureBox = _doc.CreateElement("TreasureBox");
                advanceTreasureBox.SetAttribute("id", ((int.Parse(id)) + 1).ToString());
                advanceTreasureBox.SetAttribute("begin_time", eightToneBox.basebox_start_time);
                advanceTreasureBox.SetAttribute("end_time", eightToneBox.basebox_end_time);
                advanceTreasureBox.SetAttribute("rule_des", "{str_music_dream_box_activity_rule_0}");
                treasureBoxConfigNode.AppendChild(advanceTreasureBox);

                //宝箱个数固定为四个
                for(int i=0;i<4;i++)
                {
                    XmlElement box = _doc.CreateElement("box");
                    box.SetAttribute("index", (i+1).ToString());
                    box.SetAttribute("display", eightToneBox.baseboxs[i].display);

                    //设置dreamvalue
                    if(i==0)
                    {
                        box.SetAttribute("dream_value", "1");
                    }
                    else if(i==1)
                    {
                        box.SetAttribute("dream_value", "20");
                    }
                    else if(i==3)
                    {
                        box.SetAttribute("dream_value", "100");
                    }
                    else
                    {
                        box.SetAttribute("dream_value", "200");
                    }

                    advanceTreasureBox.AppendChild(box);
                    //每个宝箱中奖励个数不定
                    for(int j= 0; j<eightToneBox.baseboxs[i].basebox_items.Length;j++)
                    {
                        XmlElement item = _doc.CreateElement("item");
                        item.SetAttribute("type", "3");
                        item.SetAttribute("para1", eightToneBox.baseboxs[i].basebox_items[j].para1);
                        item.SetAttribute("para2", eightToneBox.baseboxs[i].basebox_items[j].para2);
                        item.SetAttribute("para3", eightToneBox.baseboxs[i].basebox_items[j].para3);

                        box.AppendChild(item);
                    }
                }
            }

            //保存和另存为

            _doc.Save("temp_eightTone.xml");
            StreamReader streamReader = new StreamReader("temp_eightTone.xml", Encoding.Default);
            string fileString = streamReader.ReadToEnd();

            this.LogBox.Text = "执行完成，请选择输出目标...";
            string fileName = "slot_machine";
            SaveToOther(fileName, fileString);
            //完成
            this.LogBox.Text = "";
        }

        //导出刮刮乐配置文件
        private void btnGenerateXmlTwo_Click(object sender , EventArgs e)
        {
            if ("" == this.XlsxFileText.Text)
            {
                this.LogBox.Text = "请选择导入的xls文件！";
                return;
            }
            if ("" == this.XmlTwoText.Text)
            {
                this.LogBox.Text = "请选择导出的xml文件！";
                return;
            }


            //读取文件
            this.LogBox.Text = "正在读取文件...";
            ScrapingData scrapingData = new ScrapingData();
            scrapingData = GetScrapingData();

            //开始准备写入
            this.LogBox.Text = "读取完成，正在写入文件...";
            XmlDocument _doc = new XmlDocument();
            _doc.Load(XmlTwoText.Text);
            XmlNode root = _doc.SelectSingleNode("SlotMachine");

            //刮刮乐奖励配置有数据
            if(scrapingData.scraping_start_time!= "")
            {
                XmlNodeList scrapingNodeList = _doc.SelectNodes("/SlotMachine/RaffleTable");

                //找到最后一个
                XmlElement lastRaffleTable = null;
                foreach (XmlElement ele in scrapingNodeList)
                {
                    lastRaffleTable = ele;
                }

                XmlElement raffleTable_scraping = _doc.CreateElement("RaffleTable");
                raffleTable_scraping.SetAttribute("begin_time", scrapingData.scraping_start_time);
                raffleTable_scraping.SetAttribute("end_time", scrapingData.scraping_end_time);
                raffleTable_scraping.SetAttribute("enableMultiProb", "1");

                root.InsertAfter(raffleTable_scraping, lastRaffleTable);

                for (int i = 0; i < scrapingData.scraping_items.Length; i++)
                {
                    XmlElement item = _doc.CreateElement("item");

                    item.SetAttribute("display", "{str_slot_machine_display_" + scrapingData.scraping_items[i].display + "}");
                    item.SetAttribute("type", scrapingData.scraping_items[i].type);
                    item.SetAttribute("male_type", scrapingData.scraping_items[i].male_type);
                    item.SetAttribute("female_type", scrapingData.scraping_items[i].female_type);
                    item.SetAttribute("count", scrapingData.scraping_items[i].count);
                    item.SetAttribute("probability", scrapingData.scraping_items[i].probability);
                    item.SetAttribute("free_lottery_probability", scrapingData.scraping_items[i].free_lottery_probability);
                    item.SetAttribute("broadcast", scrapingData.scraping_items[i].broadcast);
                    item.SetAttribute("board", scrapingData.scraping_items[i].board);

                    raffleTable_scraping.AppendChild(item);
                }
            }

            //刮刮乐保底宝箱配置
            if (scrapingData.basebox_start_time != "")
            {
                XmlNode treasureBoxConfigNode = root.LastChild;
                XmlElement lastBox = (XmlElement)treasureBoxConfigNode.LastChild;

                string id = lastBox.GetAttribute("id");

                XmlElement advanceTreasureBox = _doc.CreateElement("TreasureBox");
                advanceTreasureBox.SetAttribute("id", ((int.Parse(id)) + 1).ToString());
                advanceTreasureBox.SetAttribute("begin_time", scrapingData.basebox_start_time);
                advanceTreasureBox.SetAttribute("end_time", scrapingData.basebox_end_time);
                advanceTreasureBox.SetAttribute("rule_des", "{str_mobile_music_dream_box_activity_rule_0}");
                treasureBoxConfigNode.AppendChild(advanceTreasureBox);

                //宝箱个数固定为四个
                for (int i = 0; i < 4; i++)
                {
                    XmlElement box = _doc.CreateElement("box");
                    box.SetAttribute("index", (i+1).ToString());
                    //设置dreamvalue
                    if (i == 0)
                    {
                        box.SetAttribute("display", "一级幻音");
                        box.SetAttribute("dream_value", "1");
                    }
                    else if (i == 1)
                    {
                        box.SetAttribute("display", "二级幻音");
                        box.SetAttribute("dream_value", "20");
                    }
                    else if (i == 3)
                    {
                        box.SetAttribute("display", "三级幻音");
                        box.SetAttribute("dream_value", "100");
                    }
                    else
                    {
                        box.SetAttribute("display", "四级幻音");
                        box.SetAttribute("dream_value", "200");
                    }

                    advanceTreasureBox.AppendChild(box);
                    //每个宝箱中奖励个数不定
                    for (int j = 0; j < scrapingData.baseboxs[i].basebox_items.Length; j++)
                    {
                        XmlElement item = _doc.CreateElement("item");
                        item.SetAttribute("type", "3");
                        item.SetAttribute("para1", scrapingData.baseboxs[i].basebox_items[j].para1);
                        item.SetAttribute("para2", scrapingData.baseboxs[i].basebox_items[j].para2);
                        item.SetAttribute("para3", scrapingData.baseboxs[i].basebox_items[j].para3);

                        box.AppendChild(item);
                    }
                }
            }

            //保存和另存为

            _doc.Save("temp_scraping.xml");
            StreamReader streamReader = new StreamReader("temp_scraping.xml", Encoding.Default);
            string fileString = streamReader.ReadToEnd();

            this.LogBox.Text = "执行完成，请选择输出目标...";
            string fileName = "slot_machine_mobile";
            SaveToOther(fileName, fileString);
            this.LogBox.Text = "";
            //完成
        }

        //另存为功能
        private void SaveToOther(string fileName, string fileString)
        {
            SaveFileDialog SaveFile = new SaveFileDialog();
            SaveFile.Filter = ".xml文件(*.xml)|*.xml";
            SaveFile.Title = "另存为";
            SaveFile.RestoreDirectory = true;


            SaveFile.FileName = fileName;
            if (SaveFile.ShowDialog() == DialogResult.OK)
            {
                //保存文件
                File.WriteAllText(SaveFile.FileName, fileString, Encoding.Default);
            }
        }
        
        //得到八音盒配置数据
        public EightToneBox GetEightToneBoxData()
        {
            EightToneBox eightToneBox = new EightToneBox();

            DataTable dataTable_Normal;
            dataTable_Normal = Data.WorksheetToTable(XlsxFileText.Text, "八音盒奖励配置");
            //这个dataTable存在
            if (!(dataTable_Normal.Rows.Count == 0 && dataTable_Normal.Columns.Count == 0))
            {
                //获取数据
                //开始时间，结束时间
                //count
                //男ID；
                //Items数量
                eightToneBox.normal_start_time = GetStartAndEndTime(dataTable_Normal)[0];
                eightToneBox.normal_end_time = GetStartAndEndTime(dataTable_Normal)[1];
                //eightToneBox.normal_start_time = dataTable_Normal.Rows[dataTable_Normal_StartTime_Point.Y][dataTable_Normal_StartTime_Point.X+1].ToString() + " " + dataTable_Normal.Rows[dataTable_Normal_StartTime_Point.Y][dataTable_Normal_StartTime_Point.X+3].ToString();
                //eightToneBox.normal_end_time = dataTable_Normal.Rows[dataTable_Normal_StartTime_Point.Y+1][dataTable_Normal_StartTime_Point.X+1].ToString() + " " + dataTable_Normal.Rows[dataTable_Normal_StartTime_Point.Y+1][dataTable_Normal_StartTime_Point.X+3].ToString();
                Point dataTable_Normal_ManID_Point = Data.SearchStr(dataTable_Normal, "男ID");
                int idCount = Data.GetIdCount(dataTable_Normal, dataTable_Normal_ManID_Point);
                eightToneBox.normal_items = GetNormalFromData(dataTable_Normal, dataTable_Normal_ManID_Point, idCount, 1);
            }
            else
            {
                eightToneBox.normal_start_time = "";
            }

            DataTable dataTable_Advance;
            dataTable_Advance = Data.WorksheetToTable(XlsxFileText.Text, "珍宝八音盒奖励配置");
            //这个dataTable存在
            if (!(dataTable_Advance.Rows.Count == 0 && dataTable_Advance.Columns.Count == 0))
            {
                //获取数据
                eightToneBox.advance_start_time = GetStartAndEndTime(dataTable_Advance)[0];
                eightToneBox.advance_end_time = GetStartAndEndTime(dataTable_Advance)[1];
                Point dataTable_Advance_ManID_Point = Data.SearchStr(dataTable_Advance, "男ID");
                int idCount = Data.GetIdCount(dataTable_Advance, dataTable_Advance_ManID_Point);
                eightToneBox.advance_items = GetNormalFromData(dataTable_Advance, dataTable_Advance_ManID_Point, idCount, 2);

            }
            else
            {
                eightToneBox.advance_start_time = "";
            }

            DataTable dataTable_Box;
            dataTable_Box = Data.WorksheetToTable(XlsxFileText.Text, "珍宝八音盒保底宝箱配置");
            //这个dataTable存在
            if (!(dataTable_Box.Rows.Count == 0 && dataTable_Box.Columns.Count == 0))
            {
                //获取数据
                eightToneBox.basebox_start_time = GetStartAndEndTime(dataTable_Box)[0];
                eightToneBox.basebox_end_time = GetStartAndEndTime(dataTable_Box)[1];

                Point ManID_Point = Data.SearchStr(dataTable_Box, "男ID");
                int idCount = Data.GetIdCount(dataTable_Box, ManID_Point);

                eightToneBox.baseboxs = GetBoxFromData(dataTable_Box, ManID_Point, idCount);
            }
            else
            {
                eightToneBox.basebox_start_time = "";
            }
            return eightToneBox;
        }
        
        //得到刮刮乐配置数据
        public ScrapingData GetScrapingData()
        {
            ScrapingData scrapingData = new ScrapingData();
            DataTable dataTable_scraping;
            dataTable_scraping = Data.WorksheetToTable(XlsxFileText.Text, "刮刮乐奖励配置");

            //这个dataTable存在
            if (!(dataTable_scraping.Rows.Count == 0 && dataTable_scraping.Columns.Count == 0))
            {
                //获取数据
                scrapingData.scraping_start_time = GetStartAndEndTime(dataTable_scraping)[0];
                scrapingData.scraping_end_time = GetStartAndEndTime(dataTable_scraping)[1];

                Point ManID_Point = Data.SearchStr(dataTable_scraping, "男ID");
                int idCount = Data.GetIdCount(dataTable_scraping, ManID_Point);

                scrapingData.scraping_items = GetNormalFromData(dataTable_scraping, ManID_Point, idCount, 3);
            }
            else
            {
                scrapingData.scraping_start_time = "";
            }

            DataTable dataTable_Box;
            dataTable_Box = Data.WorksheetToTable(XlsxFileText.Text, "刮刮乐保底宝箱配置");
            //这个dataTable存在
            if (!(dataTable_Box.Rows.Count == 0 && dataTable_Box.Columns.Count == 0))
            {
                //获取数据
                scrapingData.basebox_start_time = GetStartAndEndTime(dataTable_Box)[0];
                scrapingData.basebox_end_time = GetStartAndEndTime(dataTable_Box)[1];

                Point ManID_Point = Data.SearchStr(dataTable_Box, "男ID");
                int idCount = Data.GetIdCount(dataTable_Box, ManID_Point);

                scrapingData.baseboxs = GetBoxFromData(dataTable_Box, ManID_Point, idCount);
            }
            else
            {
                scrapingData.basebox_start_time = "";
            }
            return scrapingData;
        }

        //找到开始结束时间 - 按title找
        public string[] GetStartAndEndTime(DataTable dt)
        {
            string[] times = new string[2];

            Point StartTime_Point = Data.FindTitle(dt, "开始时间");
            string temp_date = dt.Rows[StartTime_Point.Y][StartTime_Point.X + 1].ToString();
            string temp_time = dt.Rows[StartTime_Point.Y][StartTime_Point.X + 3].ToString();
            //string test = DateTime.FromOADate(Convert.ToInt32(temp_time)).ToString("d");
            //if(dataTable_Normal.Rows[dataTable_Normal_StartTime_Point.Y][dataTable_Normal_StartTime_Point.X + 1] is DateTime)
            //{
            //}
            temp_time = DateTime.FromOADate(Convert.ToDouble(temp_time)).ToString();
            string[] data = temp_date.Split(' ');
            string[] time = temp_time.Split(' ');
            times[0] = data[0] + " " + time[1];

            temp_date = dt.Rows[StartTime_Point.Y + 1][StartTime_Point.X + 1].ToString();
            temp_time = dt.Rows[StartTime_Point.Y + 1][StartTime_Point.X + 3].ToString();
            temp_time = DateTime.FromOADate(Convert.ToDouble(temp_time)).ToString();
            data = temp_date.Split(' ');
            time = temp_time.Split(' ');
            times[1] = data[0] + " " + time[1];
            return times;
        }

        //type表示要读取的DataTable类型，1 ， 八音盒奖励配置， 2珍宝八音盒 ， 3刮刮乐
        public EightToneBoxItem[] GetNormalFromData(DataTable dt , Point manPoint, int count, int type)
        {
            EightToneBoxItem[] eightToneBoxItems = new EightToneBoxItem[count];
            double probabilitySum = 0;
            double free_probabilitySum = 0;
            //读取datatable_normal中的数据
            for (int i=0; i<count;i++)
            {
                eightToneBoxItems[i].display = i;
                eightToneBoxItems[i].male_type = dt.Rows[manPoint.Y+i+1][manPoint.X].ToString();
                eightToneBoxItems[i].female_type = dt.Rows[manPoint.Y + i + 1][manPoint.X+2].ToString();
                //count 需要处理
                eightToneBoxItems[i].count = dt.Rows[manPoint.Y + i + 1][manPoint.X + 3].ToString();
                if(eightToneBoxItems[i].count =="永久")
                {
                    eightToneBoxItems[i].count = "0";
                }
                else
                {
                    eightToneBoxItems[i].count = Regex.Replace(eightToneBoxItems[i].count, @"[^0-9]+", "");
                }
                

                if(type == 1)
                {
                    //probability需要处理
                    eightToneBoxItems[i].probability = dt.Rows[manPoint.Y + i + 1][manPoint.X + 4].ToString();
                    //现在就是小数
                    double result = double.Parse(eightToneBoxItems[i].probability);
                    probabilitySum += result;
                    eightToneBoxItems[i].probability = ((int)(result * 100000)).ToString();

                    //daily_max需要处理
                    eightToneBoxItems[i].daily_max = dt.Rows[manPoint.Y + i + 1][manPoint.X + 5].ToString();
                    if (eightToneBoxItems[i].daily_max.ToString() != "")
                    {
                        //提取其中的数字进行替换
                        eightToneBoxItems[i].daily_max = Regex.Replace(eightToneBoxItems[i].daily_max, @"[^0-9]+", "");
                    }
                    
                }
                else if(type == 2)
                {
                    //probability需要处理
                    eightToneBoxItems[i].probability = dt.Rows[manPoint.Y + i + 1][manPoint.X + 5].ToString();
                    //现在就是小数
                    double result = double.Parse(eightToneBoxItems[i].probability);
                    probabilitySum += result;
                    eightToneBoxItems[i].probability = ((int)(result * 100000)).ToString();
                }
                else
                {
                    //probability需要处理
                    eightToneBoxItems[i].free_lottery_probability = dt.Rows[manPoint.Y + i + 1][manPoint.X + 4].ToString();
                    //现在就是小数
                    double result1 = double.Parse(eightToneBoxItems[i].free_lottery_probability);
                    free_probabilitySum += result1;
                    eightToneBoxItems[i].free_lottery_probability = ((int)(result1 * 100000)).ToString();


                    //probability需要处理
                    eightToneBoxItems[i].probability = dt.Rows[manPoint.Y + i + 1][manPoint.X + 5].ToString();
                    //现在就是小数
                    double result2 = double.Parse(eightToneBoxItems[i].probability);
                    probabilitySum += result2;
                    eightToneBoxItems[i].probability = ((int)(result2 * 100000)).ToString();
                }
            
                eightToneBoxItems[i].broadcast = dt.Rows[manPoint.Y + i + 1][manPoint.X + 6].ToString();
                eightToneBoxItems[i].board = dt.Rows[manPoint.Y + i + 1][manPoint.X + 7].ToString();      
                eightToneBoxItems[i].type = dt.Rows[manPoint.Y + i + 1][manPoint.X + 8].ToString(); 
                if(eightToneBoxItems[i].type == "珍品")
                {
                    eightToneBoxItems[i].type = "2";
                }
                else if(eightToneBoxItems[i].type == "精品")
                {
                    eightToneBoxItems[i].type = "1";
                }
                else
                {
                    eightToneBoxItems[i].type = "0";
                }
            }
            if (!((int)(probabilitySum*100) == 100))
            {
                MessageBox.Show("八音盒奖励配置:出现概率（免费奖池概率）总和 不等于100", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            if(type ==3 && !((int)(free_probabilitySum*100) == 100))
            {
                MessageBox.Show("刮刮乐奖励配置:出现概率总和 不等于100", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            return eightToneBoxItems;
        }

        //每个宝箱内奖励数目不定，，，这里采用最低效的方式进行读取和处理
        public BaseBox[] GetBoxFromData(DataTable dt , Point manIdPoint , int count)
        {
            //BaseBox[] baseBoxes = new BaseBox[count];

            BaseBoxItem[] baseBoxItems = new BaseBoxItem[count];

            for(int i=0; i<count;i++)
            {
                baseBoxItems[i].display = dt.Rows[manIdPoint.Y + i + 1][manIdPoint.X - 2].ToString();
                if (baseBoxItems[i].display == "一级音梦宝箱"|| baseBoxItems[i].display == "一级幻音")
                {
                    baseBoxItems[i].id = "1";
                }
                else if(baseBoxItems[i].display == "二级音梦宝箱" || baseBoxItems[i].display == "二级幻音")
                {
                    baseBoxItems[i].id = "2";
                }
                else if(baseBoxItems[i].display == "三级音梦宝箱" || baseBoxItems[i].display == "三级幻音")
                {
                    baseBoxItems[i].id = "3";
                }
                else
                {
                    baseBoxItems[i].id = "4";
                }

                baseBoxItems[i].para1 = dt.Rows[manIdPoint.Y + i + 1][manIdPoint.X].ToString();
                baseBoxItems[i].para2 = dt.Rows[manIdPoint.Y + i + 1][manIdPoint.X+2].ToString();
                //para3 - count需要处理
                baseBoxItems[i].para3 = dt.Rows[manIdPoint.Y + i + 1][manIdPoint.X + 3].ToString();
                if(baseBoxItems[i].para3 == "永久")
                {
                    baseBoxItems[i].para3 = "0";
                }
                else
                {
                    baseBoxItems[i].para3 = Regex.Replace(baseBoxItems[i].para3, @"[^0-9]+", "");
                }
            }

            //再次处理
            int count1=0, count2=0, count3=0, count4=0;
            for(int i = 0; i < count; i++)
            {
                if(baseBoxItems[i].id == "1")
                {
                    count1++;
                }
                if (baseBoxItems[i].id == "2")
                {
                    count2++;
                }
                if (baseBoxItems[i].id == "3")
                {
                    count3++;
                }
                if (baseBoxItems[i].id == "4")
                {
                    count4++;
                }
            }

            //再次处理，生成易写入的格式

            BaseBox[] baseBoxes = new BaseBox[4];

            BaseBoxItem[] baseBoxItems_Id1 = new BaseBoxItem[count1];
            BaseBoxItem[] baseBoxItems_Id2 = new BaseBoxItem[count2];
            BaseBoxItem[] baseBoxItems_Id3 = new BaseBoxItem[count3];
            BaseBoxItem[] baseBoxItems_Id4 = new BaseBoxItem[count4];

            int flag1 = 0, flag2 = 0, flag3 = 0, flag4 = 0;
            for (int i = 0;i<count; i++)
            {
                if (baseBoxItems[i].id == "1")
                {
                    baseBoxItems_Id1[flag1] = baseBoxItems[i];
                    flag1++;
                }

                if (baseBoxItems[i].id == "2")
                {
                    baseBoxItems_Id2[flag2] = baseBoxItems[i];
                    flag2++;
                }
                if (baseBoxItems[i].id == "1")
                {
                    baseBoxItems_Id3[flag3] = baseBoxItems[i];
                    flag3++;
                }

                if (baseBoxItems[i].id == "1")
                {
                    baseBoxItems_Id4[flag4] = baseBoxItems[i];
                    flag4++;
                }
            }

            baseBoxes[0].ID = "1";
            baseBoxes[0].display = "一级音梦宝箱";
            baseBoxes[0].basebox_items = baseBoxItems_Id1;

            baseBoxes[1].ID = "2";
            baseBoxes[1].display = "二级音梦宝箱";
            baseBoxes[1].basebox_items = baseBoxItems_Id2;
            baseBoxes[2].ID = "3";
            baseBoxes[2].display = "三级音梦宝箱";
            baseBoxes[2].basebox_items = baseBoxItems_Id3;
            baseBoxes[3].ID = "4";
            baseBoxes[3].display = "四级音梦宝箱";
            baseBoxes[3].basebox_items = baseBoxItems_Id4;

            return baseBoxes;
        }


        private void XlsxFileTextDragDrop(object sender , DragEventArgs e)
        {
            string path = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            if(File.Exists(path)&&Utility.IsXlsFile(path))
            {
                this.XlsxFileText.Text = path;
                this.LogBox.Text = "";
            }
            else
            {
                this.LogBox.Text = "拖入的输入excel文件应为xls或xlsx格式，请重新拖入！";
            }
        }
        private void XlsxFileTextDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }
        private void btnGetXlsxFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.RestoreDirectory = true;
            ofd.Filter = "xls files (*.xls)|*.xls|xlsx files (*.xlsx)|*.xlsx";
            ofd.Title = "请选择导入excel文件";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.XlsxFileText.Text = ofd.FileName;
                this.LogBox.Text = "";
            }
        }

        private void XmlOneTextDragDrop(object sender, DragEventArgs e)
        {
            string path = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            if (File.Exists(path) && Utility.IsXmlFile(path))
            {
                this.XmlOneText.Text = path;
                this.LogBox.Text = "";
            }
            else
            {
                this.LogBox.Text = "拖入的输出文件应为xml格式，请重新拖入！";
            }
        }
        private void XmlOneTextDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }
        private void btnGetXmlOne_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.RestoreDirectory = true;
            ofd.Filter = "*.xml(xml文件)|*.xml";
            ofd.Title = "请选择导出xml文件";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.XmlOneText.Text = ofd.FileName;
                this.LogBox.Text = "";
            }
        }

        private void XmlTwoTextDragDrop(object sender, DragEventArgs e)
        {
            string path = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            if (File.Exists(path) && Utility.IsXmlFile(path))
            {
                this.XmlTwoText.Text = path;
                this.LogBox.Text = "";
            }
            else
            {
                this.LogBox.Text = "拖入的输出文件应为xml格式，请重新拖入！";
            }
        }
        private void XmlTwoTextDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }
        private void btnGetXmlTwo_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.RestoreDirectory = true;
            ofd.Filter = "*.xml(xml文件)|*.xml";
            ofd.Title = "请选择导出xml文件";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                this.XmlTwoText.Text = ofd.FileName;
                this.LogBox.Text = "";
            }
        }
    } 
}
