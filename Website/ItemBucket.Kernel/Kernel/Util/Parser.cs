using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore;
using Sitecore.Data;

namespace ItemBucket.Kernel.Kernel.Util
{
    public static class Parser
    {
        public static IEnumerable<ID> ParseIds(string rawIdList)
        {
            var ids = MainUtil.RemoveEmptyStrings(rawIdList.ToLower().Split(new[] { '|', ' ', ',' }, StringSplitOptions.RemoveEmptyEntries));
            return from id in ids where ID.IsID(id) select ID.Parse(id);
        }

        public static IEnumerable<string> ParseString(string rawIdList)
        {
            return MainUtil.RemoveEmptyStrings(rawIdList.ToLower().Split(new[] { '|', ' ', ',' }, StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
