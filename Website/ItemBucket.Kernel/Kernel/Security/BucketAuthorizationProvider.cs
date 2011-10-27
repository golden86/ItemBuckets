using Sitecore.Security.AccessControl;
using Sitecore.Security.Accounts;

namespace ItemBucket.Kernel.Kernel.Security
{
    class BucketAuthorizationProvider : SqlServerAuthorizationProvider 
    {
        public BucketAuthorizationProvider() 
        { 
            _itemHelper = new AuthHelper(); 
        } 
  
        private ItemAuthorizationHelper _itemHelper; 
        protected override ItemAuthorizationHelper ItemHelper 
        { 
            get { return _itemHelper; } 
            set { _itemHelper = value; } 
        } 
          
        protected override void AddAccessResultToCache(ISecurable entity, Account account, AccessRight accessRight, AccessResult accessResult, PropagationType propagationType) 
        { 
            // 
            //Do not cache the access result because the result depends  
            //on the value that is currently set on the item. 
            if (accessRight.Name == BucketRights.AddComments) 
            { 
                return; 
            } 
            base.AddAccessResultToCache(entity, account, accessRight, accessResult, propagationType); 
        } 
    } 
}
