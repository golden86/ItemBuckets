using Sitecore.Data.Fields;
using Sitecore.Search.Crawlers.FieldCrawlers;

namespace ItemBucket.Kernel.Kernel.ItemExtensions.Axes
{
    class FieldCrawler  : FieldCrawlerFactory
    {
        public new static FieldCrawlerBase GetFieldCrawler(Field field)
        {
            string fieldType;

            if (((fieldType = field.Type) != null) && (fieldType == "Droplink"))
            {
                return new LookupFieldCrawler(field);
            }

            return FieldCrawlerFactory.GetFieldCrawler(field);
        }
    }
}
