using System;
using System.Collections.Generic;
using System.Linq;
using ItemBucket.Kernel.Kernel.Util;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Sitecore;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.IO;
using Sitecore.Search;
using Sitecore.Data.Fields;
using Constants = ItemBucket.Kernel.Kernel.Util.Constants;

namespace ItemBucket.Kernel.Kernel.ItemExtensions.Axes
{

    public static class ItemExtensions
    {
        private static BucketItemAxes bucketItemAxes;

        public static BucketItemAxes GetBucketItemAxes(this Sitecore.Data.Items.Item itm)
        {
            bucketItemAxes = new BucketItemAxes(itm);
            return bucketItemAxes;
        }

        public static IEnumerable<SitecoreItem> Search(this Sitecore.Data.Items.Item itm, QueryParser queryParser)
        {
            return new List<SitecoreItem>();
        }

        public static Sitecore.Data.Items.Item AddBucketUnorganisedChild(this Sitecore.Data.Items.Item item, TemplateID templateID, string itemName)
        {
            using (var securityDisabler = new Sitecore.SecurityModel.SecurityDisabler())
            {
                Sitecore.Data.Items.Item childItem = item.Add(itemName, templateID);
                childItem.Editing.BeginEdit();
                ((CheckboxField)childItem.Fields["ShouldNotOrganiseInBucket"]).Checked = true;
                childItem.Editing.EndEdit();
                return childItem;
            }
        }

        public static Sitecore.Data.Items.Item AddBucketUnorganisedChild(this Sitecore.Data.Items.Item item, TemplateItem template, string itemName)
        {
            using (var securityDisabler = new Sitecore.SecurityModel.SecurityDisabler())
            {
                Sitecore.Data.Items.Item childItem = item.Add(itemName, template);
                childItem.Editing.BeginEdit();
                ((CheckboxField)childItem.Fields["ShouldNotOrganiseInBucket"]).Checked = true;
                childItem.Editing.EndEdit();
                return childItem;
            }
        }


    }

    public class BucketItemAxes
    {

        // Fields
        private Sitecore.Data.Items.Item _item;
        protected string IndexName
        {
            get { return "buckets"; }
        }
        // Methods
        public BucketItemAxes(Sitecore.Data.Items.Item item)
        {
            this._item = item;
        }

        //private void AddDescendants(Item item, ArrayList list)
        //{
        //    var categories = from relatedItem in GetAllRelations(item)

        //                     group relatedItem by GetDisplayNameById(relatedItem.GetItem().ID) into g

        //                     select new { CategoryName = g.Key, Items = g };

        //    list.AddRange(categories.ToList());
        //}

        //protected IEnumerable<SitecoreItem> GetAllRelations(Item itm)
        //{
        //    using (var searcher = new IndexSearcher(IndexName))
        //    {
        //        return searcher.GetRelatedItems(itm.ID.ToString());
        //    }
        //}

        protected string GetDisplayNameById(ID id)
        {
            return Sitecore.Context.ContentDatabase.GetItem(id.ToString()).DisplayName;

        }

        //public virtual List<SitecoreItem> GetRelatedItemsByMultipleFields(string query, SafeDictionary<string> refinements)
        //{
        //    var globalQuery = new BooleanQuery();

        //    AddFullTextClause(globalQuery, query);

        //    foreach (var refinement in refinements)
        //    {
        //        var fieldName = refinement.Key.ToLowerInvariant();
        //        var fieldValue = refinement.Value.ToLowerInvariant();
        //        AddFieldValueClause(globalQuery, fieldName, fieldValue, BooleanClause.Occur.MUST);
        //    }

        //    return RunQuery(globalQuery);
        //}
        protected void ApplyIdFilter(BooleanQuery query, string fieldName, string filter)
        {
            if (!String.IsNullOrEmpty(fieldName) &&
                !String.IsNullOrEmpty(filter))
            {
                var filterQuery = new BooleanQuery();

                var values = new List<string>(filter.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries));

                foreach (var value in values.Where(ID.IsID))
                {
                    AddFieldValueClause(filterQuery, fieldName, value, BooleanClause.Occur.SHOULD);
                }

                query.Add(filterQuery, BooleanClause.Occur.MUST);
            }
        }


        protected void AddFieldValueClause(BooleanQuery query, string fieldName, string fieldValue, BooleanClause.Occur occurance)
        {
            if (!String.IsNullOrEmpty(fieldName) && !String.IsNullOrEmpty(fieldValue))
            {
                // if we are searching by _id field, do not lowercase
                fieldValue = IdHelper.ProcessGuiDs(fieldValue, !fieldName.Equals(BuiltinFields.ID));
                var term = new Term(fieldName, fieldValue);

                query.Add(new TermQuery(term), occurance);
            }
        }



        protected void ApplyRelationFilter(BooleanQuery query, string ids)
        {
            ApplyIdFilter(query, BuiltinFields.Links, ids);
        }

        protected void AddFullTextClause(BooleanQuery query, string searchText)
        {
            if (!String.IsNullOrEmpty(searchText))
            {
                var analyzer = new StandardAnalyzer();
                var fullTextQuery = new QueryParser(Constants.Field.Content, analyzer).Parse(searchText);
                query.Add(fullTextQuery, BooleanClause.Occur.MUST);
            }
        }

        //public virtual List<SitecoreItem> GetRelatedItems(string ids)
        //{
        //    var globalQuery = new BooleanQuery();
        //    ApplyRelationFilter(globalQuery, ids);
        //    return RunQuery(globalQuery);
        //}

        //public virtual List<SitecoreItem> GetRelatedItemsByField(string ids, string fieldName, bool partial)
        //{
        //    var globalQuery = new BooleanQuery();

        //    if (partial)
        //    {
        //        //AddPartialFieldValueClause(globalQuery, fieldName, ids);
        //    }
        //    else
        //    {
        //        AddFieldValueClause(globalQuery, fieldName, ids, BooleanClause.Occur.SHOULD);
        //    }


        //    return RunQuery(globalQuery);
        //}

        //protected List<Item> GetDirectItemRelations(string fieldName)
        //{
        //    var directItemRelations = new List<Item>();

        //    foreach (var itemId in ItemIds.Split(new[] { "|" }, 

        //    StringSplitOptions.RemoveEmptyEntries))
        //    {
        //        var item = Sitecore.Context.Database.GetItem(itemId);

        //        if (item != null &&
        //            item.Fields[fieldName] != null &&
        //            (FieldTypeManager.GetField(item.Fields[fieldName]) is MultilistField))
        //        {
        //            MultilistField relatedItemsField = item.Fields[fieldName];
        //            directItemRelations.AddRange(relatedItemsField.GetItems());
        //        }
        //    }

        //    return directItemRelations;
        //}




        public Sitecore.Data.Items.Item GetAncestor(ID itemID, bool includeSelf)
        {
            Error.AssertItemID(itemID, "itemID", false);
            return this.GetAncestor(this._item, itemID, includeSelf);
        }

        private Sitecore.Data.Items.Item GetAncestor(Sitecore.Data.Items.Item item, ID itemID, bool includeSelf)
        {
            Error.AssertItemID(itemID, "itemID", false);
            if (includeSelf && (item.ID == itemID))
            {
                return item;
            }
            Sitecore.Data.Items.Item parent = item.Parent;
            if (parent == null)
            {
                return null;
            }
            return this.GetAncestor(parent, itemID, true);
        }

        public Sitecore.Data.Items.Item[] GetAncestors()
        {
            List<Sitecore.Data.Items.Item> list = new List<Sitecore.Data.Items.Item>();
            Sitecore.Data.Items.Item parent = this._item;
            while (parent.Parent != null)
            {
                parent = parent.Parent;
                list.Add(parent);
            }
            list.Reverse();
            return list.ToArray();
        }

        public Sitecore.Data.Items.Item GetChild(ID childID)
        {
            Sitecore.Data.Items.Item item = this._item.Database.Items[childID, this._item.Language];
            if ((item != null) && (GetParentID(item) == this._item.ID))
            {
                return item;
            }
            return null;
        }

        public Sitecore.Data.Items.Item GetChild(string itemName)
        {
            if (MainUtil.IsID(itemName))
            {
                ID id = new ID(itemName);
                Sitecore.Data.Items.Item item = this._item.Database.Items[id, this._item.Language];
                if ((item != null) && (GetParentID(item) == this._item.ID))
                {
                    return item;
                }
            }
            string str = itemName.ToLower();
            foreach (Sitecore.Data.Items.Item item2 in this._item.Children)
            {
                if (item2.Key == str)
                {
                    return item2;
                }
            }
            return null;
        }

        public Sitecore.Data.Items.Item GetDescendant(string name)
        {
            return this.GetDescendant(this._item, name);
        }

        private Sitecore.Data.Items.Item GetDescendant(Sitecore.Data.Items.Item itm, string name)
        {
            Sitecore.Data.Items.Item subItem = itm.Paths.GetSubItem(name);
            if (subItem != null)
            {
                return subItem;
            }
            foreach (Sitecore.Data.Items.Item item2 in itm.Children)
            {
                subItem = this.GetDescendant(item2, name);
                if (subItem != null)
                {
                    return subItem;
                }
            }
            return null;
        }

        //public Item[] GetDescendants()
        //{
        //    ArrayList list = new ArrayList();
        //    this.AddDescendants(this._item, list);
        //    return (list.ToArray(typeof(Item)) as Item[]);
        //}

        public Sitecore.Data.Items.Item GetItem(string path)
        {
            Error.AssertString(path, "path", true);
            path = FileUtil.MakePath(this._item.Paths.Path, path, '/');
            return this._item.Database.Items[path];
        }

        public Sitecore.Data.Items.Item GetNextSibling()
        {
            Sitecore.Data.Items.Item parent = this._item.Parent;
            if (parent != null)
            {
                ChildList children = parent.Children;
                int index = children.IndexOf(this._item);
                if ((index >= 0) && (index < (children.Count - 1)))
                {
                    return children[index + 1];
                }
            }
            return null;
        }

        private static ID GetParentID(Sitecore.Data.Items.Item item)
        {
            Sitecore.Data.Items.Item parent = item.Parent;
            if (parent != null)
            {
                return parent.ID;
            }
            return ID.Null;
        }

        public Sitecore.Data.Items.Item GetPreviousSibling()
        {
            Sitecore.Data.Items.Item parent = this._item.Parent;
            if (parent != null)
            {
                ChildList children = parent.Children;
                int index = children.IndexOf(this._item);
                if (index > 0)
                {
                    return children[index - 1];
                }
            }
            return null;
        }

        public bool IsAncestorOf(Sitecore.Data.Items.Item item)
        {
            Error.AssertObject(item, "item");
            if (item.Database.Name == this._item.Database.Name)
            {
                for (Sitecore.Data.Items.Item item2 = item; item2 != null; item2 = item2.Parent)
                {
                    if (item2.ID == this._item.ID)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        [Obsolete("Use IsDescendantOf method instead.")]
        public bool IsDecendantOf(Sitecore.Data.Items.Item item)
        {
            return this.IsDescendantOf(item);
        }

        public bool IsDescendantOf(Sitecore.Data.Items.Item item)
        {
            Assert.IsNotNull(item, "item");
            if (item.Database.Name == this._item.Database.Name)
            {
                string longID = this._item.Paths.LongID;
                string str2 = item.Paths.LongID;
                return longID.StartsWith(str2);
            }
            return false;
        }

        public Sitecore.Data.Items.Item[] SelectItems(string query)
        {
            Error.AssertString(query, "query", false);
            return Sitecore.Data.Query.Query.SelectItems(query, new Sitecore.Data.Items.Item(this._item.ID, this._item.InnerData, this._item.Database));
        }

        public Sitecore.Data.Items.Item SelectSingleItem(string query)
        {
            Error.AssertString(query, "query", false);
            return Sitecore.Data.Query.Query.SelectSingleItem(query, new Sitecore.Data.Items.Item(this._item.ID, this._item.InnerData, this._item.Database));
        }

        // Properties
        public int Level
        {
            get
            {
                if (this._item.ID == this.Root.ID)
                {
                    return 0;
                }
                return (this._item.Parent.Axes.Level + 1);
            }
        }

        public Sitecore.Data.Items.Item Root
        {
            get
            {
                return this._item.Database.GetRootItem(this._item.Language);
            }
        }

    }

}


