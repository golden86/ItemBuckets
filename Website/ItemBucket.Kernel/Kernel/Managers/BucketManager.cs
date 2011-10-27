using System;
using System.Linq;
using ItemBucket.Kernel.Kernel.Util;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.IDTables;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.SecurityModel;
using Sitecore.StringExtensions;
using Sitecore.Data.Fields;
using Constants = Sitecore.Constants;

namespace ItemBucket.Kernel.Kernel.Managers
{
    public static class BucketManager
    {
        public static Sitecore.Data.Items.Item GetParentSection(this Sitecore.Data.Items.Item item)
        {
            if (item.ParentID.Equals(ItemIDs.RootID)) return item;
            return ContainerSupported(item.Parent.TemplateID, item.Database) ? item.Parent : GetParentSection(item.Parent);
        }

        public static bool ContainerSupported(ID templateId, Database database)
        {
            var template = TemplateManager.GetTemplate(templateId, database);
            return Enumerable.Any<ID>(Config.SupportedContainerTemplates, template.DescendsFromOrEquals);
        }

        public static bool IsBucketItem(ID templateId)
        {
            //return ((database.GetItem(templateId)).Fields["Bucketable"]).Value == "1";
            return Enumerable.Contains(Config.SupportedItemTemplates, templateId);
        }

        internal static readonly string IdTableKey = "SiloResolver";

        public static bool IsBucketItem(this Sitecore.Data.Items.Item item) { return !item.Name.Equals(Constants.StandardValuesItemName) && IsBucketItem(item.TemplateID); }

        public static string ShortUrl(this Sitecore.Data.Items.Item item)
        {
            return "/{0}/{1}".FormatWith(item.GetParentSection().Name.ToLower(), item.Name.ToLowerInvariant());
        }

        public static Sitecore.Data.Items.Item GetSiloItem(string filePath)
        {
            var id = IDTable.GetID(IdTableKey, filePath);
            if (id != null && !ID.IsNullOrEmpty(id.ID))
            {
                return Context.Database.GetItem(id.ID);
            }
            return null;
        }

        public static void RegisterMapping(this Sitecore.Data.Items.Item item)
        {
            IDTable.RemoveID(IdTableKey, item.ID);
            IDTable.Add(IdTableKey, item.ShortUrl(), item.ID);
        }

        public static Sitecore.Data.Items.Item GetDateFolderDestination(Sitecore.Data.Items.Item topParent, DateTime childItemCreationDateTime)
        {
            var database = topParent.Database;
            var dateFolder = String.Format("{0}/{1}/{2}/{3}/{4}", childItemCreationDateTime.Year, childItemCreationDateTime.Month, childItemCreationDateTime.Day, childItemCreationDateTime.Hour, childItemCreationDateTime.Minute);
            var destinationFolderPath = topParent.Paths.FullPath + "/" + dateFolder;
            Sitecore.Data.Items.Item destinationFolderItem = null;
            if ((destinationFolderItem = database.GetItem(destinationFolderPath)) == null)
            {
                TemplateItem containerTemplate = database.Templates[new TemplateID(Config.ContainerTemplateId)];
                destinationFolderItem = database.CreateItemPath(destinationFolderPath, containerTemplate, containerTemplate);
            }
            return destinationFolderItem;
        }

        public static Sitecore.Data.Items.Item GetDateFolderDestination(Sitecore.Data.Items.Item topParent, Sitecore.Data.Items.Item itemToMove)
        {
            return GetDateFolderDestination(topParent, itemToMove.Statistics.Created);
        }

        public static void MakeIntoBucket(Sitecore.Data.Items.Item item)
        {
            // Call MoveItemToDateFolder on each of the children of the parent
            foreach (Sitecore.Data.Items.Item child in item.Children.ToList())
            {
                MoveItemToDateFolder(item, child);
            }
            // Mark all the first level children as hidden.
            var firstLevelChildren = item.GetChildren();
            using (new SecurityDisabler())
            {
                foreach (var firstLevelChild in firstLevelChildren.ToList())
                {
                    firstLevelChild.Editing.BeginEdit();
                    firstLevelChild.Appearance.Hidden = true;
                    firstLevelChild.Editing.EndEdit();
                }
            }
        }

        private static void MoveItemToDateFolder(Sitecore.Data.Items.Item topParent, Sitecore.Data.Items.Item toMove)
        {
            foreach (var item in toMove.Children.ToList())
            {
                // Move the child item to a date based folder under the parentItem
                // Make a recursive call this method
                MoveItemToDateFolder(topParent, item);
            }
            // Now move the item itself
            if (ShouldMoveToDateFolder(toMove))
            {
                var destinationFolder = GetDateFolderDestination(topParent, toMove);
                ItemManager.MoveItem(toMove, destinationFolder);
            }
            else if(ShouldDeleteInCreationOfBucket(toMove))
            {
                toMove.Delete();
            }
        }

        private static bool ShouldMoveToDateFolder(Sitecore.Data.Items.Item toMove)
        {
            return !ShouldDeleteInCreationOfBucket(toMove) && 
                // Check if the item should not be organised inside a bucket i.e. Move with the parent.
                    (toMove.Fields["ShouldNotOrganiseInBucket"] == null || !((Sitecore.Data.Fields.CheckboxField)toMove.Fields["ShouldNotOrganiseInBucket"]).Checked);
        }

        private static bool ShouldDeleteInCreationOfBucket(Sitecore.Data.Items.Item item)
        {
            return item.TemplateID.ToString() == "{239F9CF4-E5A0-44E0-B342-0F32CD4C6D8B}" || // folder and node
                    item.TemplateID.ToString() == "{A87A00B1-E6DB-45AB-8B54-636FEC3B5523}";
        }

        public static bool IsBucket(Sitecore.Data.Items.Item item)
        {
            return item.Fields["IsBucket"] != null && ((CheckboxField)item.Fields["IsBucket"]).Checked;
        }
    }
}
