using Sitecore.Security.AccessControl;

namespace ItemBucket.Kernel.Kernel.Security
{
    class BucketSecurityManager
    {

        public BucketSecurityManager(Sitecore.Data.Items.Item item)
        {
            this.ContextItem = item;
           
        }

        public Sitecore.Data.Items.Item ContextItem { get; private set; }
 
        public virtual bool IsAddCommentAllowed
        {
            get
            {
                var right = AccessRight.FromName(BucketRights.AddComments);
                if (right == null)
                {
                    return false;
                }
                var allowed = AuthorizationManager.IsAllowed(this.ContextItem, right, Sitecore.Context.User);
                return allowed;
            }
        }

        public virtual Sitecore.Data.Items.Item AddComment()
        {
            if (!this.IsAddCommentAllowed)
            {
                return null;
            }
            // 
            //TODO: Add logic to actually add a comment. 
            return null;
        }
        public virtual Sitecore.Data.Items.Item UnMakeBucket()
        {
            if (!this.IsUnMakeBucketAllowed)
            {
                return null;
            }
            // 
            //TODO: Add logic to actually add a comment. 
            return null;
        }

        public virtual bool IsUnMakeBucketAllowed
        {
            get
            {
                var right = AccessRight.FromName(BucketRights.UnmakeBucket);
                if (right == null)
                {
                    return false;
                }
                var allowed = AuthorizationManager.IsAllowed(this.ContextItem, right, Sitecore.Context.User);
                return allowed;
            }
        }

       




    }
}




