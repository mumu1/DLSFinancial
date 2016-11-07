using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEYON.ViewModel.App
{
    public class Column
    {
        public Column(String data, String title)
        {
            this.data = data;
            this.title = title;
        }

        /// <summary>
        /// 返回数据对应列名
        /// </summary>
        public string data { get; set; }

        /// <summary>
        /// 用于前端显示列名
        /// </summary>
        public string title { get; set; }
    }

    public class PerOrderDetailVM
    {

    }
}
