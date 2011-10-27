using ItemBucket.Kernel.Kernel.Managers;
using Sitecore.Data.Items;
using Sitecore.Reflection;
using System;

namespace ItemBuckets.Kernel.Commands
{
    public class AddFromTemplateCommand : Sitecore.Data.Engines.DataCommands.AddFromTemplateCommand
    {
        protected override Sitecore.Data.Engines.DataCommands.AddFromTemplateCommand CreateInstance()
        {
            return new AddFromTemplateCommand();
        }

        protected override Item DoExecute()
        {
            if (BucketManager.ContainerSupported(Destination.TemplateID, Database) &&
                BucketManager.IsBucketItem(TemplateId) &&
                BucketManager.IsBucket(Destination))
            {
                var newDestination = BucketManager.GetDateFolderDestination(Destination, DateTime.Now);
                if (newDestination != null && !newDestination.Uri.Equals(Destination.Uri))
                    return Nexus.DataApi.AddFromTemplate(TemplateId, newDestination, ItemName, NewId);
            }
            return base.DoExecute();
        }
    }
}
