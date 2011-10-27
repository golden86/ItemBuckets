//using System;
//using Ninemsn.CMSPilot.Buckets.Managers;
//using Sitecore.Configuration;
//using Sitecore.Data.Items;
//using Sitecore.Data.Managers;
//using Sitecore.Diagnostics;
//using Sitecore.Events;
//using Sitecore.Web.UI.Sheer;

//namespace Ninemsn.CMSPilot.Buckets.Events
//{
//    class AutoBucket
//    {
//        public void Execute(object sender, EventArgs args)
//        {
//            var item = Sitecore.Events.Event.ExtractParameter(args, 0) as Item;
//            Error.AssertItem(item, "Item");

//            if (item.Parent.Children.Count >= BucketTriggerCount)
//            {
//                var cpa = new ClientPipelineArgs();
//                SitecoreEventArgs sArgs = args as SitecoreEventArgs;

//                if (!cpa.IsPostBack)
//                {
//                    var cr = new ClientResponse();
//                    cr.Confirm("You have reached the recommended sibling item count and it is recommended to turn the parent item into a bucket. Continue?");

//                }
//                else
//                {
//                    string res = cpa.Result;

//                    if (res == "yes")
//                    {

//                        foreach (Item itm in item.Parent.Children)
//                        {
//                            ItemManager.MoveItem(itm,
//                                                 BucketManager.GetDateFolderDestination(itm,
//                                                                                        Sitecore.Context.ContentDatabase));
//                        }
//                        sArgs.Result.Cancel = true;
//                    }
//                    else
//                    {
//                        cpa.Result = string.Empty;
//                        cpa.IsPostBack = false;
//                        return;
//                    }
//                }
//            }
//        }

//        public static int BucketTriggerCount
//        {
//            get
//            {
//                return Settings.GetIntSetting("BucketTriggerCount", 60);
//            }
//        }
//    }
//}
