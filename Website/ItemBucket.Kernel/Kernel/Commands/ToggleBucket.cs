using System;
using Sitecore;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.HtmlControls;

namespace ItemBucket.Kernel.Kernel.Commands
{


    [Serializable]
    class ToggleBucket : Command
    {
        // Methods
        public override void Execute(CommandContext context)
        {
        }


        public override CommandState QueryState(CommandContext context)
        {



            //AccessRuleCollection rules = context.Items[0].Security.GetAccessRules();
            //Sitecore.Security.Accounts.Account account = Sitecore.Security.Accounts.Account.FromName(Sitecore.Context.User.Name, Sitecore.Security.Accounts.AccountType.User);
            //rules.Helper.RemoveExactMatches(account);
            //AccessRight accessRight = new AccessRight("item:read");
            //AccessRule rule = AccessRule.Create(account, accessRight, PropagationType.Entity, SecurityPermission.DenyAccess);
            //rules.Add(rule);
            //context.Items[0].Security.SetAccessRules(rules);



            if (!ShowHiddenItems)
            {
                return CommandState.Enabled;

            }
            return CommandState.Down;


        }

        public static bool ShowHiddenItems
        {
            get
            {
                return (Registry.GetBool("/Current_User/UserOptions.View.ShowBucketItems", Context.IsAdministrator));
            }
            set
            {
                Registry.SetBool("/Current_User/UserOptions.View.ShowBucketItems", value);
            }
        }

    }


}




