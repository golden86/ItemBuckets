using System;
using Sitecore.Data.Items;
using Sitecore.Configuration;
using Sitecore.Data.Managers;
using Sitecore.Web.UI.Sheer;
using Sitecore.Diagnostics;

namespace ItemBucket.Kernel.Kernel.Events
{
    public class ItemCopy
    {
        public void Execute(object sender, EventArgs args)
        {
            var item = Sitecore.Events.Event.ExtractParameter(args, 0) as Item;
            Item copiedFromId = Sitecore.Events.Event.ExtractParameter(args, 1) as Item;

            Error.AssertNotNull(copiedFromId, "copiedFromId");
            Error.AssertItem(item, "Item");

            var cpa = new ClientPipelineArgs();
            if (item != null) cpa.Parameters.Add("id", item.ID.ToString());
            cpa.Parameters.Add("copiedFromId", copiedFromId.ID.ToString());
            Sitecore.Context.ClientPage.Start(this, "DialogCreateRedirect", cpa);
        }

     

        protected void DialogCreateRedirect(ClientPipelineArgs args)
        {
            var masterdb = Factory.GetDatabase("master");
            var item = masterdb.GetItem(args.Parameters["id"]);
            var copiedFromFolderItem = masterdb.GetItem(args.Parameters["copiedFromId"]);

            Error.AssertItem(item, "item");
            Error.AssertItem(copiedFromFolderItem, "copiedFromFolderItem");
            var copiedToFolderItem = copiedFromFolderItem;
            //var copiedToFolderItem = Managers.BucketManager.GetDateFolderDestination(copiedFromFolderItem, masterdb); //masterdb.GetItem("{1F0DF17F-F4F1-467E-9FB3-9FC9D6A46F87}");

            if (!args.IsPostBack)
            {
                //var homeItem = masterdb.GetItem(Sitecore.Context.Site.StartPath);
                ////copiedToFolderItem = GetDateFolderDestination(copiedFromFolderItem, masterdb);
                //if (item.Axes.IsDescendantOf(homeItem))
                //{
                //    var cr = new ClientResponse();
                //    cr.Confirm("You are copying into an Item Bucket and your item will be automatically catgorised. Continue?");
                //    args.WaitForPostBack();
                //}
            }
            else
            {
                string res = args.Result;

                if (res == "yes")
                {
                    ItemManager.CopyItem(item, copiedToFolderItem, true);
                }
                else
                {
                    args.Result = string.Empty;
                    args.IsPostBack = false;
                    return;
                }
            }
        }

    }
}