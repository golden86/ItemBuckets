using System.Linq;
using ItemBucket.Kernel.Kernel.Security;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.SecurityModel;
using Sitecore.Shell.Framework.Commands;

namespace ItemBucket.Kernel.Kernel.Commands
{
    class UnMakeBucket : Command
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
                if (Editors.Items.Contains("{59F53BBB-D1F5-4E38-8EBA-0D73109BB59B}"))
                {
                    Editors.Remove("{59F53BBB-D1F5-4E38-8EBA-0D73109BB59B}");
                    ContextItem.Editing.EndEdit();
                }
            }

            //ContextItem.Template.BaseTemplates.ToList().Remove(new TemplateItem(ContextItem)); 

            Sitecore.Shell.Applications.Dialogs.ProgressBoxes.ProgressBox.Execute("Turning into a Bucket", "Categorising Items", new Sitecore.Shell.Applications.Dialogs.ProgressBoxes.ProgressBoxMethod(StartProcess), new object[] { ContextItem });
            Sitecore.Context.ClientPage.SendMessage(this, "item:load(id=" + ContextItem.ID.ToString() + ")");
            Sitecore.Context.ClientPage.SendMessage(this, "item:refreshchildren(id=" + ContextItem.ID.ToString() + ")");

        }

        public void StartProcess(params object[] parameters)
        {
            Item ContextItem = (Item)parameters[0];

            ShowAllSubFolders(ContextItem);
            if (ContextItem.Fields["IsBucket"] != null)
            {
                using (new SecurityDisabler())
                {
                    ContextItem.Editing.BeginEdit();

                    ((CheckboxField)ContextItem.Fields["IsBucket"]).Checked = false;
                    ContextItem.Editing.EndEdit();
                }
            }
        }

        public void ShowAllSubFolders(Item ContextItem)
        {
            foreach (Item itm in ContextItem.Axes.GetDescendants())
            {
                if (ContextItem.Fields["__Hidden"] != null)
                {
                    using (new SecurityDisabler())
                    {
                        itm.Editing.BeginEdit();
                        ((CheckboxField)itm.Fields["IsBucket"]).Checked = false;
                        ((CheckboxField)itm.Fields["__Hidden"]).Checked = false;
                        itm.Editing.EndEdit();
                    }
                }
            }

        }

        public override CommandState QueryState(CommandContext context)
        {
            Error.AssertObject(context, "context");

            BucketSecurityManager bManager = new BucketSecurityManager(context.Items[0]);

            if (!bManager.IsUnMakeBucketAllowed)
            {
                return CommandState.Disabled;
            }

            
            
            return context.Items[0].Fields["IsBucket"] != null
                       ? (((CheckboxField)context.Items[0].Fields["IsBucket"]).Checked
                              ? CommandState.Enabled
                              : CommandState.Disabled)
                       : CommandState.Disabled;
        }


    }
}