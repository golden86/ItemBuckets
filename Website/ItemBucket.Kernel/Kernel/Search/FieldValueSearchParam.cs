using Sitecore.Collections;
using Sitecore.Search;

namespace ItemBucket.Kernel.Kernel.Search
{
   public class FieldValueSearchParam : SearchParam
   {
      public FieldValueSearchParam()
      {
         Refinements = new SafeDictionary<string>();
      }

      public QueryOccurance Occurance { get; set; }

      public SafeDictionary<string> Refinements { get; set; }
   }
}
