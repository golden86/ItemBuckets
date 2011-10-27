﻿namespace ItemBucket.Kernel.Kernel.Search
{
   public class SearchParam
   {
      public string RelatedIds { get; set; }
      public string TemplateIds { get; set; }
      public string LocationIds { get; set; }
      public string FullTextQuery { get; set; }
      public bool ShowAllVersions { get; set; }
      public string Language { get; set; }
   }
}