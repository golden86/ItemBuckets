using Sitecore.Data.Fields;
using Sitecore.Rules.ContentEditorWarnings;

namespace ItemBucket.Kernel.Kernel.Rules
{
    using Sitecore.Rules.Conditions;

    // TODO: Created Sitecore Item "/sitecore/system/Settings/Rules/Common/Conditions/WithinABucket" when creating WithinABucket class. Fix Text field.

    public class WithinABucket<T> : WhenCondition<T> where T : ContentEditorWarningsRuleContext
    {
        protected override bool Execute(T ruleContext)
        {
            return ((CheckboxField)ruleContext.Item.Parent.Fields["IsBucket"]).Checked;
        }
    }
}