using System;
using System.Text;
using ItemBucket.Kernel.Kernel.Search;
using ItemBucket.Kernel.Kernel.Util;
using Sitecore;
using Sitecore.Collections;
using Sitecore.Search;
using Sitecore.Shell.Applications.ContentEditor;
using Sitecore.Data.Items;
using System.Collections;
using Sitecore.Globalization;
using Sitecore.Resources;
using System.Web.UI;
using Sitecore.Diagnostics;
using Sitecore.Text;

namespace ItemBucket.Kernel.Kernel.FieldTypes
{
    /// <summary>
    /// Defines the SiloList content field class.
    /// </summary>
    public class BucketList : MultilistEx
    {
        #region Protected Methods

        protected override void GetSelectedItems(Item[] sources, out ArrayList selected, out IDictionary unselected)
        {
            Assert.ArgumentNotNull(sources, "sources");
            var str = new ListString(Value);
            unselected = new SortedList(StringComparer.Ordinal);
            selected = new ArrayList(str.Count);
            foreach (var t in str)
            {
                selected.Add(t);
            }
            foreach (var item in sources)
            {
                //if (item != null)
                {
                    var id = item.ID.ToString();
                    var index = str.IndexOf(id);
                    if (index >= 0)
                    {
                        selected[index] = item;
                    }
                    else
                    {
                        unselected.Add(MainUtil.GetSortKey(item.Name), item);
                    }
                }
            }
        }

        //public override void HandleMessage(Sitecore.Web.UI.Sheer.Message message)
        //{
        //    message.

        //    base.HandleMessage(message);
        //}
        private static string SelectedDropTreeValue = "";

        protected override void OnLoad(EventArgs e)
        {
            //if (!Sitecore.Context.ClientPage.IsEvent)
            //{
            //    //PostBack
            //}
            //else
            //{
            //    var dropTreeForBucket = FindControl(GetID("DropTreeForBucket")) as TreeList;
            //    SelectedDropTreeValue = dropTreeForBucket.GetValue();
            //}
            //base.OnLoad(e);
        }

        /// <summary>
        /// Sends server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter"></see> object, which writes the content to be rendered on the client.
        /// </summary>
        /// <param name="output">The <see cref="T:System.Web.UI.HtmlTextWriter"></see> object that receives the server control content.</param>
        protected override void DoRender(HtmlTextWriter output)
        {
            IDictionary dictionary;
            ArrayList list;
            Item current = Sitecore.Context.ContentDatabase.GetItem(this.ItemID);
            Item[] items = this.GetItems(current);
            this.GetSelectedItems(items, out list, out dictionary);

            #region Rendering filter box

            var sb = new StringBuilder();
            foreach (DictionaryEntry entry in dictionary)
            {
                var item = entry.Value as Item;
                if (item != null)
                {
                    sb.Append(item.DisplayName + ",");
                    sb.Append(GetItemValue(item) + ",");
                }
            }

            output.Write("<input type=\"hidden\" width=\"100%\" id=\"multilistValues" + ClientID + "\" value=\"" + sb.ToString() + "\" style=\"width: 200px;margin-left:3px;\">");

            #endregion

            ServerProperties["ID"] = ID;
            var str = string.Empty;
            if (ReadOnly)
            {
                str = " disabled=\"disabled\"";
            }
            output.Write("<input id=\"" + ID + "_Value\" type=\"hidden\" value=\"" + StringUtil.EscapeQuote(Value) + "\" />");
            output.Write("<table" + this.GetControlAttributes() + ">");
            output.Write("<tr>");
            output.Write("<td class=\"scContentControlMultilistCaption\" width=\"50%\">" + Translate.Text("All") + "</td>");
            output.Write("<td width=\"20\">" + Images.GetSpacer(20, 1) + "</td>");
            output.Write("<td class=\"scContentControlMultilistCaption\" width=\"50%\">" + Translate.Text("Selected") + "</td>");
            output.Write("<td width=\"20\">" + Images.GetSpacer(20, 1) + "</td>");
            output.Write("</tr>");
            output.Write("<tr>");
            output.Write("<td valign=\"top\" height=\"100%\">");
            var textFirstName = new Sitecore.Shell.Applications.ContentEditor.Text();
            textFirstName.ID = GetID("textFirstName");
            var textLastName = new Sitecore.Shell.Applications.ContentEditor.Text();
            textLastName.ID = GetID("textLastName");
            //

            var TreeViewThing = new Sitecore.Shell.Applications.ContentEditor.TreeList();
            TreeViewThing.ID = "DropTreeForBucket";
            TreeViewThing.RenderControl(output);


            output.Write("<input type=\"text\" width=\"100%\" class=\"scIgnoreModified\" style=\"color:gray\" value=\"Type here to search\" id=\"filterBox" + ClientID + "\" style=\"width:100%\">");
            output.Write("<select id=\"" + ID + "_unselected\" class=\"scContentControlMultilistBox\" multiple=\"multiple\" size=\"10\"" + str + " >");
            foreach (DictionaryEntry entry in dictionary)
            {
                Item item = entry.Value as Item;
                if (item != null)
                {
                    output.Write("<option value=\"" + this.GetItemValue(item) + "\">" + item.DisplayName + "</option>");
                }
            }
            output.Write("</select>");
            output.Write("</td>");
            output.Write("<td valign=\"top\">");
            output.Write("<img class=\"\" height=\"16\" width=\"16\" border=\"0\" alt=\"\" style=\"margin: 2px;\" src=\"/sitecore/shell/themes/standard/Images/blank.png\"/>");
            output.Write("<br />");
            this.RenderButton(output, "Core/16x16/arrow_blue_right.png", "");
            output.Write("<br />");
            this.RenderButton(output, "Core/16x16/arrow_blue_left.png", "");
            output.Write("</td>");
            output.Write("<td valign=\"top\" height=\"100%\">");
            output.Write("<select style=\"margin-top:22px\" id=\"" + this.ID + "_selected\" class=\"scContentControlMultilistBox\" multiple=\"multiple\" size=\"10\"" + str + ">");
            for (int i = 0; i < list.Count; i++)
            {
                Item item3 = list[i] as Item;
                if (item3 != null)
                {
                    output.Write("<option value=\"" + this.GetItemValue(item3) + "\">" + item3.DisplayName + "</option>");
                }
                else
                {
                    string path = list[i] as string;
                    if (path != null)
                    {
                        string str3;
                        Item item4 = Sitecore.Context.ContentDatabase.GetItem(path);
                        if (item4 != null)
                        {
                            str3 = item4.DisplayName + ' ' + Translate.Text("[Not in the selection List]");
                        }
                        else
                        {
                            str3 = path + ' ' + Translate.Text("[Item not found]");
                        }
                        output.Write("<option value=\"" + path + "\">" + str3 + "</option>");
                    }
                }
            }
            output.Write("</select>");
            output.Write("</td>");
            output.Write("<td valign=\"top\">");
            output.Write("<img class=\"\" height=\"16\" width=\"16\" border=\"0\" alt=\"\" style=\"margin: 2px;\" src=\"/sitecore/shell/themes/standard/Images/blank.png\"/>");
            output.Write("<br />");
            RenderButton(output, "Core/16x16/arrow_blue_up.png", "javascript:scContent.multilistMoveUp('" + ID + "')");
            output.Write("<br />");
            RenderButton(output, "Core/16x16/arrow_blue_down.png", "javascript:scContent.multilistMoveDown('" + ID + "')");
            output.Write("</td>");
            output.Write("</tr>");
            output.Write("<div style=\"border:1px solid #999999;font:8pt tahoma;display:none;padding:2px;margin:4px 0px 4px 0px;height:14px\" id=\"" + ID + "_all_help\"></div>");
            output.Write("<div style=\"border:1px solid #999999;font:8pt tahoma;display:none;padding:2px;margin:4px 0px 4px 0px;height:14px\" id=\"" + ID + "_selected_help\"></div>");
            output.Write("</table>");
            RenderScript(output);
        }

        protected override Item[] GetItems(Item current)
        {
            Assert.ArgumentNotNull(current, "current");

            //Split the source from the DataSource field   
            var values = StringUtil.GetNameValues(Source, '=', '&');
            //Split the field FieldsFilter  
            var refinements = new SafeDictionary<string>();
            if (values["FieldsFilter"] != null)
            {
                var splittedFields = StringUtil.GetNameValues(values["FieldsFilter"], ':', ',');

                foreach (string key in splittedFields.Keys)
                {
                    refinements.Add(key, splittedFields[key]);
                }
            }

            var searchParam = new FieldValueSearchParam
            {
                Refinements = refinements,
                //LocationIds = values["LocationFilter"],
                LocationIds = SelectedDropTreeValue,
                TemplateIds = values["TemplateFilter"],
                FullTextQuery = values["FullTextQuery"],
                Occurance = QueryOccurance.Must,
                ShowAllVersions = false,
                Language = values["Language"]
            };

            //Execute the search   
            using (var searcher = new IndexSearcher("buckets"))
            {
                var items = searcher.GetItems(searchParam);
                return SearchHelper.GetItemListFromInformationCollection(items).ToArray();
            }
        }

        #endregion

        #region Private methods

        private void RenderButton(HtmlTextWriter output, string icon, string click)
        {
            Assert.ArgumentNotNull(output, "output");
            Assert.ArgumentNotNull(icon, "icon");
            Assert.ArgumentNotNull(click, "click");
            var builder = new ImageBuilder
                              {
                                  Src = icon,
                                  Width = 0x10,
                                  Height = 0x10,
                                  Margin = "2px",
                                  ID = icon.Contains("right") ? "btnRight" + ClientID : "btnLeft" + ClientID
                              };
            if (!ReadOnly)
            {
                builder.OnClick = click;
            }
            output.Write(builder.ToString());
        }

        /// <summary>
        /// Renders the supporting javascript
        /// </summary>
        /// <param name="output">The writer.</param>
        private void RenderScript(HtmlTextWriter output)
        {
            var script =
                @"               
                function applyMultilistFilter(id) {
                            var multilist = document.getElementById(id + '_unselected');
                            var filterBox = document.getElementById('filterBox' + id);
                            var multilistValues = document.getElementById('multilistValues' + id).value.split(',');
                            var savedStr = filterBox.value;
                            multilist.options.length = 0;
                             for (i = 0; i < multilistValues.length; i+=2) {
                                if (multilistValues[i].toLowerCase().indexOf(savedStr.toLowerCase()) != -1) {
                                    multilist.options.length++;                   
                                    multilist.options[multilist.options.length - 1] = new Option(multilistValues[i], multilistValues[i+1]);
                                }
                             }
                            }
                function onFilterFocus(filterBox) {
                    if (filterBox.value == 'Type here to search') {
                        filterBox.value = '';
                        filterBox.style.color = 'black';
                    }
                    else {
                        filterBox.select();
                    }
                }

                function onFilterBlur(filterBox) {
                    if (filterBox.value == '') {
                        filterBox.value = 'Type here to search';
                        filterBox.style.color = 'gray';
                    }

                }

                function multilistValuesMoveRight(id, allOptions) {
                              var all = scForm.browser.getControl(id + '_unselected');
                              var selected = scForm.browser.getControl(id + '_selected');
                              var multilistValues = document.getElementById('multilistValues'+id);
                              for(var n = 0; n < all.options.length; n++) {
                                var option = all.options[n];
                                if (option.selected || allOptions == true) {
                                  var opt = option.innerHTML + ',' + option.value + ',';
                                  multilistValues.value = multilistValues.value.replace(opt, '')
                                }
                              }
                            }

                function multilistValuesMoveLeft(id, allOptions) {
                              var all = scForm.browser.getControl(id + '_unselected');
                              var selected = scForm.browser.getControl(id + '_selected');
                              var multilistValues = document.getElementById('multilistValues'+id);
                              for(var n = 0; n < selected.options.length; n++) {
                                var option = selected.options[n];
                                if (option.selected || allOptions == true) {
                                  var opt = option.innerHTML + ',' + option.value + ',';
                                  multilistValues.value += opt;
                                }
                              }
                            }
                $('filterBox" +
                ClientID + @"').observe('focus', function() { onFilterFocus($('filterBox" + ClientID +
                @"')) } );
                $('filterBox" + ClientID +
                @"').observe('blur', function() { onFilterBlur($('filterBox" + ClientID +
                @"')) } );
                $('filterBox" + ClientID +
                @"').observe('keyup', function() { applyMultilistFilter('" + ClientID +
                @"') } );
            
                $('" + ID +
                "_unselected').observe('dblclick', function() { multilistValuesMoveRight('" + ID +
                @"'); javascript:scContent.multilistMoveRight('" + ID + @"') } );
                $('" + ID +
                "_selected').observe('dblclick', function() { multilistValuesMoveLeft('" + ID +
                @"'); javascript:scContent.multilistMoveLeft('" + ID + @"') } );

                $('btnRight" +
                ID + "').observe('click', function() { multilistValuesMoveRight('" + ID +
                @"'); javascript:scContent.multilistMoveRight('" + ID + @"') } );
                $('btnLeft" +
                ID + "').observe('click', function() { multilistValuesMoveLeft('" + ID +
                @"') ; javascript:scContent.multilistMoveLeft('" + ID + @"') } );";

            script = "<script type='text/javascript' language='javascript'>" + script + "</script>";
            output.Write(script);
        }
        #endregion
    }
}
