using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.GetLookupSourceItems;
using System;

namespace ItemBucket.Kernel.Kernel.FieldTypes
{
    class CustomDataSource
    {
        public void Process(GetLookupSourceItemsArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            if (args.Source.StartsWith("code:"))
            {
                
                Item[] itemArray;
                string query = args.Source;

                itemArray = RunEnumeration(query, args.Item);


                if ((itemArray != null) && (itemArray.Length > 0))
                {
                    args.Result.AddRange(itemArray);
                }
                args.AbortPipeline();
            }

        }

        private Item[] RunEnumeration(string s, Item i)
        {
            s = s.Replace("code:", "");
            string[] ReflectionString = s.Split(',');
            string classname = ReflectionString[0];
            string Assemblyname = ReflectionString[1];
            var t  =  System.Type.GetType(s);
            var d = Activator.CreateInstance(t) as IDataSource;
            return d != null ? d.ListQuery(i) : new Item[] { };
        }
    }

    public class BucketListQuery : IDataSource
    {
        public Item[] ListQuery(Item itm)
        {
            return itm.Children.ToArray();
        }
    }

    public interface IDataSource
    {
        Item[] ListQuery(Item itm);
    }

}
