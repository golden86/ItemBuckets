﻿using System.Collections.Generic;
using Sitecore.Search;

namespace ItemBucket.Kernel.Kernel.Search
{
   public class NumericRangeSearchParam : SearchParam
   {
      public class NumericRangeField
      {
         public NumericRangeField() { }

         public NumericRangeField(string fieldName, int start, int end)
         {
            FieldName = fieldName.ToLowerInvariant();
            Start = start;
            End = end;
         }

         #region Properties

         public string FieldName { get; set; }
         public int Start { get; set; }
         public int End { get; set; }

         #endregion Properties
      }

      public List<NumericRangeField> Ranges { get; set; }

      public QueryOccurance Occurance { get; set; }
   }
}
