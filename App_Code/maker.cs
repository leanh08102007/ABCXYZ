using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace websevice_lamvtneww_v2

{
    public class maker
    {
        public string id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public string type { get; set; }


        public maker(string _id, string _name, string _address, string _lat,string _lng, string _type)
        {
            this.id = _id;
            this.name = _name;
            this.address = _address;
            this.lat = _lat;
            this.lng = _lng;
            this.type = _type;
        }
        public maker()
        {

        }
      
    }



}