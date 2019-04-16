using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 八音盒配置工具
{
    class Data
    {
        //将xlsx中的数据读取到DataTable中,后续的操作，对DataTable执行
        public static DataTable WorksheetToTable(string fullFilePath, string sheet)
        {
            try
            {
                //it shouldn't always open a file , maybe  can hold the worksheets in static
                //TO-DO
       
                FileInfo exisitingFile = new FileInfo(fullFilePath);
                ExcelPackage package = new ExcelPackage(exisitingFile);
                ExcelWorksheets worksheets = package.Workbook.Worksheets;
                //ExcelWorksheet worksheet = package.Workbook.Worksheets[sheet];
                //查找是否具有该页签
                int num = worksheets.Count;
                for (int i = 1;i<= num; i++)
                {
                    if (worksheets[i].Name == sheet)
                    {
                        return WorksheetToTable(worksheets[i]);
                    }
                }
                return new DataTable();
            }
            catch(Exception)
            {
                throw;
            }
        }
        public static DataTable WorksheetToTable(ExcelWorksheet worksheet)
        {
            //获取worksheet的行数
            int rows = worksheet.Dimension.End.Row;
            //获取worksheet的列数
            int cols = worksheet.Dimension.End.Column;

            DataTable datatable = new DataTable(worksheet.Name);
            DataRow datarow = null;
            for (int i = 1; i <= rows; i++)
            {
                if (i > 1)
                    datarow = datatable.Rows.Add();

                for (int j = 1; j <= cols; j++)
                {
                    //默认将第一行设置为datatable的标题
                    if (i == 1)
                        datatable.Columns.Add(GetString(worksheet.Cells[i, j].Value));
                    //剩下的写入datatable
                    else
                        datarow[j - 1] = GetString(worksheet.Cells[i, j].Value);
                }
            }
            return datatable;
        }

        private static string GetString(object obj)
        {
            try
            {
                return obj.ToString();
            }
            catch(Exception ex)
            {
                return "";
            }
        }

        public static int GetIdCount(DataTable dt , Point idPoint)
        {
            int count = 0;
            while((dt.Rows.Count>idPoint.Y+count+1) && (dt.Rows[idPoint.Y+ count + 1][idPoint.X].ToString() != ""))
            {
                count++;
            }
            return count;
        }





        //查询“八音盒奖励配置” “珍宝八音盒奖励配置”“珍宝八音盒保底宝箱配置”“刮刮乐奖励配置”“刮刮乐保底宝箱配置”
        //查询其他每行的第一个单元格内容
        //保持X为0 ， 仅Y++
        public static Point FindTitle(DataTable dt , string str)
        {
            Point point = new Point();
            point.X = 0;

            for(int i=0;i<dt.Rows.Count;i++)
            {
                string temp = dt.Rows[i][0].ToString();
                if (temp == str)
                {
                    point.Y = i;
                    return point;
                }
            }
            return point;
        }
        //可能不存在该节点-请注意搜索输入项
        public static Point SearchStr(DataTable dt , string str)
        {
            Point point = new Point();
            for(int i=0;i<dt.Rows.Count;i++)
            {
                for(int j=0;j<dt.Columns.Count;j++)
                {
                    if(dt.Rows[i][j].ToString() == str)
                    {
                        point.X = j;
                        point.Y = i;
                        return point;
                    }
                }
            }
            return point;
        }

    }
}
