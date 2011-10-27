using ItemBucket.Kernel.Kernel.Managers;
using Sitecore.Diagnostics;
using Sitecore.Links;

namespace ItemBucket.Kernel.Kernel.LinkProvider
{
    public class ItemSearchLinkProvider : Sitecore.Links.LinkProvider
    {
        public override string GetItemUrl(Sitecore.Data.Items.Item item, UrlOptions options)
        {
            Assert.ArgumentNotNull(item, "item");
            Assert.ArgumentNotNull(options, "options");
            return item.IsBucketItem() ? item.ShortUrl() : base.GetItemUrl(item, options);
        }
    }
}
