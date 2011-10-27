using System.Linq;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Managers;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Security.AccessControl;
using Sitecore.SecurityModel;

namespace ItemBucket.Kernel.Kernel.Util
{
    public class ItemUtil
    {
       
        public static string GetIconPath(Sitecore.Data.Items.Item item)
        {
            Assert.ArgumentNotNull(item, "item");
            return Sitecore.Resources.Themes.MapTheme(item.Appearance.Icon); 
        }

        public static bool HasExplicitDenies(Sitecore.Data.Items.Item item)
        {
            Assert.ArgumentNotNull(item, "item");
            return item.Security.GetAccessRules().Any(rule => rule.SecurityPermission == SecurityPermission.DenyAccess);
        }

        public static bool HasLayout(Sitecore.Data.Items.Item item)
        {
            Assert.ArgumentNotNull(item, "item");

            LayoutField layoutField = item.Fields[FieldIDs.LayoutField];

            if (layoutField != null)
            {
                var isStandardValue = layoutField.InnerField.ContainsStandardValue;
                var isEmpty = !layoutField.InnerField.HasValue;

                return !isStandardValue && !isEmpty;
            }

            return false;
        }

        public static bool IsApproved(Sitecore.Data.Items.Item item)
        {
            Assert.ArgumentNotNull(item, "item");
            var stateItem = GetStateItem(item);
            return stateItem == null || stateItem[WorkflowFieldIDs.FinalState] == "1";
        }

        public static Sitecore.Data.Items.Item GetStateItem(Sitecore.Data.Items.Item item)
        {
            string stateId = GetStateId(item);
            return stateId.Length > 0 ? GetStateItem(stateId, item.Database) : null;
        }

        public static Sitecore.Data.Items.Item GetStateItem(string stateId, Database database)
        {
            var iD = MainUtil.GetID(stateId, null);
            return iD.IsNull ? null : GetItem(iD, database);
        }

  
        public static Sitecore.Data.Items.Item GetItem(ID itemId, Database database)
        {
            return ItemManager.GetItem(itemId, Language.Current, Version.Latest, database, SecurityCheck.Disable);
        }

       
        public static string GetStateId(Sitecore.Data.Items.Item item)
        {
            Assert.ArgumentNotNull(item, "item");
            var workflowInfo = item.Database.DataManager.GetWorkflowInfo(item);
            return workflowInfo != null ? workflowInfo.StateID : string.Empty;
        }

        
        public static string GetItemPath(Sitecore.Data.Items.Item item)
        {
            Assert.ArgumentNotNull(item, "item");
            return item.Paths.FullPath;
        }
    }
}
