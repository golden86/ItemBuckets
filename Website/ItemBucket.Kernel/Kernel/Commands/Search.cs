using System.Linq;
using ItemBucket.Kernel.Kernel.Security;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.SecurityModel;
using Sitecore.Shell.Framework.Commands;

namespace ItemBucket.Kernel.Kernel.Commands
{
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.Diagnostics;
    using Sitecore.Shell.Framework.Commands;

    // TODO: \App_Config\include\Search.config created automatically when creating Search class. In this config include file, specify command name attribute value

    public class Search : Command
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

        }
    }
}