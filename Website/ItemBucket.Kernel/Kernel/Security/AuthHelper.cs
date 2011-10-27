using System;
using Sitecore.Configuration;
using Sitecore.Security.AccessControl;
using Sitecore.Security.Accounts;

namespace ItemBucket.Kernel.Kernel.Security
{
    class AuthHelper : ItemAuthorizationHelper
    {
        protected override AccessResult GetItemAccess(Sitecore.Data.Items.Item item, Account account, AccessRight accessRight, PropagationType propagationType)
        {
            // 
            //This method applies the specified AccessRight.  Since the custom AccessRight  
            //has been extended to support additional properties (max comments and  
            //time range), these properties must be considered. 
            var result = base.GetItemAccess(item, account, accessRight, propagationType);
            // 
            // 
            if (result == null || result.Permission != AccessPermission.Allow)
            {
                return result;
            }
            // 
            // 
            if (accessRight.Name != BucketRights.AddComments)
            {
                return result;
            }
            // 
            // 
            var right = accessRight as BucketAccessRight;
            if (right != null)
            {
                result = GetItemAccess(item, account, right);
            }
            return result;
        }

        protected virtual AccessResult GetItemAccess(Sitecore.Data.Items.Item item, Account account, BucketAccessRight right)
        {
            // 
            //Determine if comments should be allowed based on the max comments count. 
            //var result = HandleMaxComments(item, account, right);
            //if (result.Permission == AccessPermission.Deny)
            //{
            //    return result;
            //}
            //// 
            ////Determine if comments should be allowed based on the time range. 
            //result = HandleDaysToAllowComments(item, account, right);
            //if (result.Permission == AccessPermission.Deny)
            //{
            //    return result;
            //}
            // 
            //Allow comments. 
            var ex = new AccessExplanation("This item can be a bucket");
            return new AccessResult(AccessPermission.Allow, ex);
        }

        protected virtual AccessResult HandleMaxComments(Sitecore.Data.Items.Item item, Account account, BucketAccessRight right)
        {
            // 
            //Allow unlimited comments if no field name is specified for MaxCommentsFieldName. 
            if (string.IsNullOrEmpty(right.BucketFieldName))
            {
                var ex = new AccessExplanation("Unlimited comments are allowed.");
                return new AccessResult(AccessPermission.Allow, ex);
            } 
            // 
            //Allow unlimited comments if the specified field does not exist on the item. 
            var field = item.Fields["IsBucket"];
            if (field == null)
            {
                var ex = new AccessExplanation("The item {0} does not have a field named \"{1}\", so unlimited comments are allowed.", item.ID.ToString(), "");
                return new AccessResult(AccessPermission.Allow, ex);
            }
            // 
            //Deny commenting if the max comments value is not an integer. 
            var maxCommentCount = 0;
            if (!string.IsNullOrEmpty(field.Value) && !int.TryParse(field.Value, out maxCommentCount))
            {
                var ex = new AccessExplanation("The value specified for the field named \"{0}\" is not a valid integer: {1}", "", field.Value);
                return new AccessResult(AccessPermission.Deny, ex);
            }
            // 
            //Deny commenting if the max comments limit has already been met. 
            if (maxCommentCount > -1)
            {
                var currentCount = GetCurrentCommentCount(item);
                if (currentCount >= maxCommentCount)
                {
                    var ex = new AccessExplanation("{0} comments already exist, and the maximum number allowed is {1}.", currentCount, maxCommentCount);
                    return new AccessResult(AccessPermission.Deny, ex);
                }
            }
            // 
            //No other rules need to be implemented, so allow comments. 
            var ex1 = new AccessExplanation("Additional comments are allowed.");
            return new AccessResult(AccessPermission.Allow, ex1);
        }

        protected virtual AccessResult HandleDaysToAllowComments(Sitecore.Data.Items.Item item, Account account, BucketAccessRight right)
        {
            // 
            //Allow commenting if the value is -1 since that value means comments 
            //may be added indefinitely. 
            //if (right.DaysToAllowComments == -1) 
            //{ 
            //    var ex = new AccessExplanation("Comments can be added indefinitely."); 
            //    return new AccessResult(AccessPermission.Allow, ex); 
            //} 
            // 
            //Deny commenting if the item has not been updated within the allowed  
            //time range. 
            var d1 = item.Statistics.Updated;
            var d2 = d1.AddDays(1.0);
            if (DateTime.Compare(d1, d2) != -1)
            {
                var ex = new AccessExplanation("Comments cannot be added after {0} {1}.", d2.ToLongDateString(), d2.ToLongTimeString());
                return new AccessResult(AccessPermission.Deny, ex);
            }
            // 
            //No other rules need to be implemented, so allow comments. 
            var ex1 = new AccessExplanation("Comments can be added until {0} {1}.", d2.ToLongDateString(), d2.ToLongTimeString());
            return new AccessResult(AccessPermission.Allow, ex1);
        }

        protected virtual int GetCurrentCommentCount(Sitecore.Data.Items.Item item)
        {
            // 
            //Get the number of children that are based on the template  
            //specified in the config file. 
            var templateId = Settings.GetSetting("CommentTemplate");
            var path = string.Format("./*[@@templateid=&apos;{0}&apos;]", templateId);
            var items = item.Axes.SelectItems(path);
            if (items == null)
            {
                return 0;
            }
            return (items.Length);
        }
    }
}
