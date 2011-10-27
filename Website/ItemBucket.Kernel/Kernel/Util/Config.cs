using System.Collections.Generic;
using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data;

namespace ItemBucket.Kernel.Kernel.Util
{
    public class Config
    {
        public static IEnumerable<ID> SupportedItemTemplates
        {
            get
            {
                return Parser.ParseIds(Settings.GetSetting("ItemSearch.SupportedItemTemplates"));
            }
        }

        public static IEnumerable<ID> SupportedContainerTemplates
        {
            get
            {
                return Parser.ParseIds(Settings.GetSetting("ItemSearch.SupportedContainerTemplates"));
            }
        }

        public static int LevelCount
        {
            get
            {
                return Settings.GetIntSetting("ItemSearch.LevelDepth", 3);
            }
        }

        public static ID ContainerTemplateId
        {
            get
            {
                return ID.Parse(Settings.GetSetting("ItemSearch.ContainerTemplateId", "{ADB6CA4F-03EF-4F47-B9AC-9CE2BA53FF97}"));
            }
        }

        public static ID BucketTriggerCount
        {
            get
            {
                return ID.Parse(Settings.GetSetting("BucketTriggerCount", TemplateIDs.Folder.ToString()));
            }
        }
    }
}
