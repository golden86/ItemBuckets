using System;
using System.Linq;
using Sitecore.Data;

namespace ItemBucket.Kernel.Kernel.Util
{
   public class IdHelper
   {

       public static string[] ParseId(string value)
       {
           return value.Split(new[] { "|", " ", "," }, StringSplitOptions.RemoveEmptyEntries);
       }

       public static bool ContainsMultiGuids(string value)
       {
           string[] ids = ParseId(value).Where(ID.IsID).ToArray();
           return ids.Length > 0;
       }

       public static string ProcessGuiDs(string value, bool lowercase)
       {
           if (ID.IsID(value))
           {
               return NormalizeGuid(value, lowercase);
           }

           if (ContainsMultiGuids(value))
           {
               return string.Join(" ", ParseId(value).Select(NormalizeGuid).ToArray());
           }

           return value;
       }

       public static string ProcessGUIDs(string value)
       {
           if (String.IsNullOrEmpty(value))
               return String.Empty;

           if (ID.IsID(value))
           {
               return NormalizeGuid(value);
           }

           if (ContainsMultiGuids(value))
           {
               return string.Join(" ", ParseId(value).Select(NormalizeGuid).ToArray());
           }

           return value;
       }

       public static string NormalizeGuid(string guid, bool lowercase)
       {
           if (!String.IsNullOrEmpty(guid))
           {
               var shortId = ShortID.Encode(guid);
               return lowercase ? shortId.ToLowerInvariant() : shortId;
           }

           return guid;
       }

       public static string NormalizeGuid(string guid)
       {
           if (!String.IsNullOrEmpty(guid) && guid.StartsWith("{"))
           {
               return ShortID.Encode(guid);
           }

           return guid;
       }


       public static string NormalizeGuid(ID guid)
       {
           return NormalizeGuid(guid.ToString());
       }
   }
}
