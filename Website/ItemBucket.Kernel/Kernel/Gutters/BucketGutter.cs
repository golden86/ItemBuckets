using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Shell.Applications.ContentEditor.Gutters;

namespace ItemBucket.Kernel.Kernel.Gutters
{
    class BucketGutter : GutterRenderer
    {
        // Methods
        protected override GutterIconDescriptor GetIconDescriptor(Item item)
        {
            Assert.ArgumentNotNull(item, "item");

            if (((CheckboxField)item.Fields["IsBucket"]) == null)
            {
                return null;
            }

            if (!((CheckboxField)item.Fields["IsBucket"]).Checked)
            {
                return null;
            }
            GutterIconDescriptor descriptor = new GutterIconDescriptor();
            descriptor.Icon = "business/32x32/chest_add.png";
            descriptor.Tooltip = "This item is a bucket and all items below this are hidden";
            return descriptor;
        }

    }
}
