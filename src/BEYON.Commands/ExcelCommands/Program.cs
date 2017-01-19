using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelCommands
{
    class Program
    {
        
        static void Main(string[] args)
        {
            if(args == null || args.Length < 1)
            {
                Console.WriteLine("请输入-p 文件路径，-f 文件名和-s 流水账号");
                return;
            }

            string filePath = null;
            String fileName = null;
            //String userID = null;
            String serialNumber = null;
            for (var i = 0; i < args.Length; i++ )
            {
                switch (args[i].ToLower())
                {
                    case "-p":
                        filePath = args[i + 1];
                        i++;
                        break;
                    case "-f":
                        fileName = args[i + 1];
                        i++;
                        break;
                    //case "-u":
                    //    userID = args[i + 1];
                    //    i++;
                    //    break;
                    case "-s":
                        serialNumber = args[i + 1];
                        i++;
                        break;
                }
            }

            if (String.IsNullOrEmpty(filePath) || String.IsNullOrEmpty(fileName) ||
                String.IsNullOrEmpty(serialNumber))
                return;

            try
            {
                ApplyPrintService service = new ApplyPrintService();
                service.ApplyExcel(filePath, fileName, serialNumber);
            }
            catch(Exception ex)
            {
                
            }
            
        }
    }
}
