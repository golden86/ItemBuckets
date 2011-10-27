﻿using System.Collections.Generic;
using System;
using System.Linq;
using Lucene.Net.Documents;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Search;

namespace ItemBucket.Kernel.Kernel.Util
{
    public class SearchHelper
    {
        public static string FormatNumber(double number)
        {
            return number.ToString().PadLeft(int.MaxValue.ToString().ToCharArray().Count(), '0');
        }

        public static string FormatNumber(int number)
        {
            return FormatNumber((double)(number));
        }

        public static List<Sitecore.Data.Items.Item> GetItemListFromInformationCollection(List<SitecoreItem> SitecoreItems)
        {
            return SitecoreItems.Select(SitecoreItem => SitecoreItem.GetItem()).Where(i => i != null).ToList();
        }

        public static void GetItemsFromSearchResult(IEnumerable<SearchResult> searchResults, List<SitecoreItem> items, bool showAllVersions)
        {
            foreach (var result in searchResults)
            {
                var uriField = result.Document.GetField(BuiltinFields.Url);
                if (uriField != null && !String.IsNullOrEmpty(uriField.StringValue()))
                {
                    var itemUri = new ItemUri(uriField.StringValue());

                    var itemInfo = new SitecoreItem(itemUri);

                    foreach (Field field in result.Document.GetFields())
                    {
                        itemInfo.Fields[field.Name()] = field.StringValue();
                    }

                    items.Add(itemInfo);
                }

                if (showAllVersions)
                    GetItemsFromSearchResult(result.Subresults, items, true);
            }
        }

        public static SafeDictionary<string> CreateRefinements(string fieldName, string fieldValue)
        {
            var refinements = new SafeDictionary<string>();

            if (!String.IsNullOrEmpty(fieldValue) && !String.IsNullOrEmpty(fieldValue))
            {
                if (fieldName.Contains("|"))
                {
                    var fieldNames = fieldName.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var name in fieldNames)
                    {
                        refinements.Add(name, fieldValue);
                    }
                }
                else
                {
                    refinements.Add(fieldName, fieldValue);
                }
            }

            return refinements;
        }
    }
}
