using System.Collections.Specialized;
using Sitecore.Security.AccessControl;

namespace ItemBucket.Kernel.Kernel.Security
{
    class BucketAccessRight : AccessRight
    {
        public BucketAccessRight(string name)
            : base(name)
        {
        }

        public string BucketFieldName { get; private set; }


        public override void Initialize(NameValueCollection config)
        {
            base.Initialize(config);
            this.BucketFieldName = config["BucketFieldName"];
        }
    }
}
