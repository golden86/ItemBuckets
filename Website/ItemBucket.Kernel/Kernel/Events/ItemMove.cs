using System;
using Sitecore.Data.Items;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Events;
using Sitecore.Web.UI.Sheer;
using Sitecore.Diagnostics;

namespace ItemBucket.Kernel.Kernel.Events
{
    public class ItemMove
    {
        public void Execute(object sender, EventArgs args)
        {
            var item = Sitecore.Events.Event.ExtractParameter(args, 0) as Item;
            var movedFromId = Sitecore.Events.Event.ExtractParameter(args, 1) as ID;

            Error.AssertNotNull(movedFromId, "MovedFromID");
            Error.AssertItem(item, "Item");

            var cpa = new ClientPipelineArgs();
            SitecoreEventArgs sArgs = args as SitecoreEventArgs;


            var masterdb = Factory.GetDatabase("master");
            var itemBlah = item;
            var movedFromFolderItem = masterdb.GetItem(item.Parent.ID);

            Error.AssertItem(itemBlah, "item");
            Error.AssertItem(movedFromFolderItem, "movedFromFolderItem");

            //var movedToFolderItem = BucketManager.GetDateFolderDestination(movedFromFolderItem, masterdb); //masterdb.GetItem("{1F0DF17F-F4F1-467E-9FB3-9FC9D6A46F87}");

            //if (!cpa.IsPostBack)
            //{
            //    var homeItem = masterdb.GetItem(Sitecore.Context.Site.StartPath);

            //    if (itemBlah.Axes.IsDescendantOf(homeItem))
            //    {
            //        var cr = new ClientResponse();
            //        cr.Confirm("You are moving into an Item Bucket and your item will be automatically catgorised. Continue?");

            //    }
            //}
            //else
            //{
            //    string res = cpa.Result;

            //    if (res == "yes")
            //    {
            //        ItemManager.MoveItem(item, movedToFolderItem);
            //        sArgs.Result.Cancel = true;
            //    }
            //    else
            //    {
            //        cpa.Result = string.Empty;
            //        cpa.IsPostBack = false;

            //        return;
            //    }
            //}
        }

        protected void DialogCreateRedirect(SitecoreEventArgs args)
        {

        }
    }
}