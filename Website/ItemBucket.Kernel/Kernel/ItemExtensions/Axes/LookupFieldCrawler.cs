using Sitecore.Data.Fields;
using Sitecore.Search.Crawlers.FieldCrawlers;

namespace ItemBucket.Kernel.Kernel.ItemExtensions.Axes
{
    class LookupFieldCrawler : FieldCrawlerBase
    {
        public LookupFieldCrawler(Field field) : base(field){ }

        public override string GetValue()
        {
            var lookupField = new LookupField(_field);
            var targetItem = lookupField.TargetItem;
            return targetItem != null ? targetItem.Name.ToLowerInvariant() : string.Empty;
        }
    }
}
