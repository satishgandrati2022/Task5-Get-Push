
using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TableStorage.GetTasktitle;

namespace TableStorage
{
    public class TodoTableEntity : BaseTableEntity
    {
        public int sno { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string imagename { get; set; }
        public string documentname { get; set; }
        public DateTime Completiondate { get; set; }
    }
    public class BaseTableEntity : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public int sno { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string imagename { get; set; }
        public string documentname { get; set; }
        public DateTime Completiondate { get; set; }
        ETag ITableEntity.ETag { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
    public static class Mappings
    {
        public static TodoTableEntity ToTableEntity(this Todo input)
        {
            return new TodoTableEntity()
            {
                PartitionKey = input.PartitionKey,
                RowKey = input.RowKey,
                sno = input.sno,
                title = input.title,
                description = input.description,
                imagename = input.imagename,
                documentname = input.documentname,
                Completiondate = input.Completiondate

            };
        }

        public static Todo ToTodo(this TodoTableEntity input)
        {
            return new Todo()
            {
                sno = input.sno,
                title = input.title,
                description = input.description,
                imagename = input.imagename,
                documentname = input.documentname,
                Completiondate = input.Completiondate
            };
        }
    }
}
