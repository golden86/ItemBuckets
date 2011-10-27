using ItemBucket.Kernel.Kernel.Managers;
using Sitecore;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.HttpRequest;

namespace ItemBucket.Kernel.Kernel.Pipelines
{
    public class ItemSearchResolver : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            if (Context.Item != null || Context.Database == null || args.Url.ItemPath.Length == 0) return;
            Context.Item = BucketManager.GetSiloItem(args.Url.FilePath);
        }
    }
}
