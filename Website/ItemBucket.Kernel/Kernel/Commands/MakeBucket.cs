using System.Linq;
using ItemBucket.Kernel.Kernel.Managers;
using ItemBucket.Kernel.Kernel.Security;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.SecurityModel;
using Sitecore.Shell.Framework.Commands;

namespace ItemBucket.Kernel.Kernel.Commands
{
    class MakeBucket : Command
    {
        public override void Execute(CommandContext context)
        {
            Assert.ArgumentNotNull(context, "context");
            Assert.IsNotNull(context.Items, "Context items list is null");
            Item ContextItem = context.Items[0];
            Sitecore.Data.Fields.MultilistField Editors = ((Sitecore.Data.Fields.MultilistField)ContextItem.Fields["__Editors"]);
            using (new SecurityDisabler())
            {
                ContextItem.Editing.BeginEdit();
                if (!Editors.Items.Contains("{59F53BBB-D1F5-4E38-8EBA-0D73109BB59B}"))
                {
                    Editors.Add("{59F53BBB-D1F5-4E38-8EBA-0D73109BB59B}");
                }
                ContextItem.Editing.EndEdit();
            }
            Sitecore.Shell.Applications.Dialogs.ProgressBoxes.ProgressBox.Execute("Turning into a Bucket", "Categorising Items", new Sitecore.Shell.Applications.Dialogs.ProgressBoxes.ProgressBoxMethod(StartProcess), new object[] { ContextItem });

            Sitecore.Context.ClientPage.SendMessage(this, "item:load(id=" + ContextItem.ID.ToString() + ")");
            Sitecore.Context.ClientPage.SendMessage(this, "item:refreshchildren(id=" + ContextItem.ID.ToString() + ")");
        }

        public string Add(string value, bool includeBlank, string list)
        {
            Assert.ArgumentNotNull(value, "value");
            if (includeBlank || (value.Length > 0))
            {
                if (list.Length > 0)
                {
                    list = list + "|";
                }
                list = list + value;
            }
            return list;
        }

        public void StartProcess(params object[] parameters)
        {
            Item ContextItem = (Item)parameters[0];
            BucketManager.MakeIntoBucket(ContextItem);
            ContextItem.Editing.BeginEdit();
            if(ContextItem.Fields["IsBucket"]!= null)
            {
                ((CheckboxField) ContextItem.Fields["IsBucket"]).Checked = true;
            }
            ContextItem.Editing.EndEdit();
        }

        public override CommandState QueryState(CommandContext context)
        {
            Error.AssertObject(context, "context");
            BucketSecurityManager bManager = new BucketSecurityManager(context.Items[0]);
            if (!bManager.IsAddCommentAllowed)
            {
                return CommandState.Disabled;
            }
            return context.Items[0].Fields["IsBucket"] != null
                       ? (((CheckboxField)context.Items[0].Fields["IsBucket"]).Checked
                              ? CommandState.Disabled
                              : CommandState.Enabled)
                       : CommandState.Enabled;
        }
    }
}
