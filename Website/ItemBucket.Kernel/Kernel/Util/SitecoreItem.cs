using System;
using System.Collections.Generic;
using Sitecore.Data;
using System.Collections.Specialized;
using Sitecore.Configuration;
using Sitecore.Search;

namespace ItemBucket.Kernel.Kernel.Util
{
   public class SitecoreItem
   {
      public SitecoreItem(string id, string language, string version, string databaseName)
         : this(new ItemUri(ID.Parse(id), Sitecore.Globalization.Language.Parse(language), Sitecore.Data.Version.Parse(version), databaseName))
      { }

      public SitecoreItem(string itemUri)
         : this(ItemUri.Parse(itemUri))
      { }

      public SitecoreItem(ItemUri itemUri)
      {
         Fields = new NameValueCollection();
         RenderedFields = new List<string>();
         Uri = itemUri;
         Fields.Add(BuiltinFields.Language, Uri.Language.Name);
         Fields.Add(SearchFieldIDs.Version, Uri.Version.Number.ToString());
      }

      public NameValueCollection Fields { get; set; }

      public ItemUri Uri { get; set; }

      public List<string> RenderedFields { get; set; }

      public string ItemID  { get { return Uri.ItemID.ToString(); } }

      public string Name { get { return Fields[BuiltinFields.Name]; } }

      public string Version { get { return Uri.Version.Number.ToString(); } }

      public string Language { get { return Uri.Language.Name; } }

      public string TemplateName { get { return Fields[SearchFieldIDs.TemplateName]; } }

      public string Path { get { return Fields[SearchFieldIDs.FullContentPath]; } }

      public string CreatedBy { get { return Fields[SearchFieldIDs.CreatedBy]; } }

      public string Created { get { return Fields[SearchFieldIDs.Created]; } }

      public string Updated { get { return Fields[SearchFieldIDs.Updated]; } }

      public string Editor { get { return Fields[SearchFieldIDs.Editor]; } }

      public Sitecore.Data.Items.Item GetItem()
      {
         var db = Factory.GetDatabase(Uri.DatabaseName);
         return db != null ? db.GetItem(Uri.ItemID, Uri.Language, Uri.Version) : null;
      }

      public override string ToString()
      {
         var itemInformation = String.Format("{0}, {1}, {2}", Uri.ItemID, Uri.Language, Uri.Version);

         foreach (string key in Fields.Keys)
         {
            itemInformation += ", " + Fields[key];
         }

         return itemInformation;
      }
   }
}
