using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TableStorage
{
    internal class ClsTasks
    {
        public string title { get; set; }
        public string description{ get; set; }
        public string imagename{ get; set; }
        public string documentname { get; set; }     

        public DateTime Completiondate { get; set; }
        
    }

   
    public class Root
    {
        public List<Value> value { get; set; }
    }



    public class Value
    {
        //public string PartitionKey { get; set; }
        //public string RowKey { get; set; }
        //public DateTime Timestamp { get; set; }
        public int sno { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string imagename { get; set; }
        public string documentname { get; set; }
        public DateTime Completiondate { get; set; }
    }
}
