using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for thuebao
/// </summary>
public class thuebao
{
	    public string ma_tb { get; set; }
        public string ten_tb { get; set; }
        public string diachi_tb { get; set; }
        public string thuebao_id { get; set; }

        public thuebao(string _ma_tb, string _ten_tb, string _diachi_tb, string _thuebao_id)
        {
            this.ma_tb = _ma_tb;
            this.ten_tb = _ten_tb;
            this.diachi_tb = _diachi_tb;
            this.thuebao_id = _thuebao_id;
         

        }
        public thuebao()
        {

        }
}