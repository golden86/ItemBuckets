using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using ItemBucket.Kernel.Kernel.Search;
using ItemBucket.Kernel.Kernel.Util;
using Sitecore.Web;
using Sitecore.Data;
using Sitecore.Configuration;

namespace ItemBucket.Kernel.Kernel.HttpHandlers
{
    /// <summary>
    /// Get a request from Default.aspx and return a string with results as a response.
    /// </summary>
    public class FetchItems : IHttpHandler
    {
        // Get the results from
        protected string IndexName
        {
            get { return "buckets"; }
        }

        protected string Lang
        {
            get { return WebUtil.GetQueryString("la"); }
        }

        protected string LocationFilter
        {
            get { return WebUtil.GetQueryString("id"); }
        }

        protected string Db
        {
            get { return WebUtil.GetQueryString("db"); }
        }

        protected Database ContentDatabase
        {
            get { return Factory.GetDatabase(Db) ?? Factory.GetDatabase("master"); }
        }



        protected virtual List<SitecoreItem> GetItems(string term)
        {
            var searchParam = new SearchParam
            {
                Language = Lang,
                TemplateIds = "",
                LocationIds = LocationFilter,
                FullTextQuery = term,
                ShowAllVersions = false
            };



            using (var searcher = new IndexSearcher(IndexName))
            {
                return searcher.GetItems(searchParam);
            }
            //using (var searcher = new Searcher.Searcher(IndexName))
            //{
            //    return searcher.GetItemsInRange(dateRange);
            //}



        }



        public void ProcessRequest(HttpContext context)
        {
            string strPage;
            // Check the QueryString if its null or empty
            // if its null then this is the first time the web page is loaded
            // so fetch the first page of data
            if (!String.IsNullOrEmpty(context.Request.QueryString["page"]))
                strPage = context.Request.QueryString["page"];
            else
                strPage = "1";

            string term;
            // Check the QueryString if its null or empty
            // if its null then this is the first time the web page is loaded
            // so fetch the first page of data
            if (!String.IsNullOrEmpty(context.Request.QueryString["term"]))
                term = context.Request.QueryString["term"];
            else
                term = "";


            // Create an HTTP Get request
            string responseData = String.Empty;
            StringBuilder sb = new StringBuilder();
            var items = new List<SitecoreItem>();
            items.AddRange(GetItems(term).GetRange((Int32.Parse(strPage) * 9), 9));

            foreach (var SitecoreItem in items)
            {
                // ResultLabel.Text += String.Format(@"<li><a href=""/sitecore/shell/sitecore/content/Applications/Content Editor.aspx?id={0}&la={1}&fo={0}"")>{2}</a></li>", SitecoreItem.ItemID, SitecoreItem.Language, SitecoreItem.Name);

                //ResultLabel.Text += String.Format(@"<li><a onclick=""scForm.postRequest('','','','item:load(id={0})'); return false;"" href=""#"">{1}</a></li>", SitecoreItem.ItemID, SitecoreItem.Name);




                responseData = responseData +
                               "<li class=\"BlogPostArea\"><div class=\"BlogPostViews\"><span style=\"color: #ffffff;\">" + SitecoreItem.Version + "<br />views</span><br /><br />1<br />version/s</div><h5 class=\"BlogPostHeader\"><a href=\"#\">" + SitecoreItem.Name + "</a></h5><div class=\"BlogPostContent\">" + SitecoreItem.GetItem().Fields["Text"] + "</div><div class=\"BlogPostFooter\"><div><a href=\"#\">" + SitecoreItem.GetItem().Statistics.Created.ToShortDateString() + "</a>by<a href=\"#\">" + SitecoreItem.GetItem().Statistics.CreatedBy + "</a></div><div><span id=\"ctl00_ctl00_bhcr_bcr_bcr_ctl01_ctl02_ctl08_ctl01\">Filed under: <a href=\"#\" rel=\"tag\">Fasion</a>, <a href=\"#\" rel=\"tag\">Cleo</a>, <a href=\"#\" rel=\"tag\">Food</a></span><input type=\"hidden\" name=\"ctl00$ctl00$bhcr$bcr$bcr$ctl01$ctl02$ctl08$ctl01\" id=\"ctl00_ctl00_bhcr_bcr_bcr_ctl01_ctl02_ctl08_ctl01_State\" value=\"nochange\"/></div></div></li>";

            }
            // Parse the web page and get the data we need
            // Blog list container
            String bpDelimeter = "BlogPostList";
            // Search for bpDelimeter from the start of the web page
            int intBPContainerStart = responseData.IndexOf(bpDelimeter, 0);
            int intliItemStart = 0;
            int intliItemEnd = 0;

            while (intliItemStart != -1)
            {
                // Blog post list item
                String liDelimeterStart = "BlogPostArea";
                intliItemStart = responseData.IndexOf(liDelimeterStart, intliItemEnd);
                if (intliItemStart == -1)
                {
                    // If no more Blog post list items are on the web page exist loop
                    break;
                }

                String liDelimeterEnd = "</li>";
                intliItemEnd = responseData.IndexOf(liDelimeterEnd, intliItemStart + 12);

                int intBGItemEnd = responseData.IndexOf(liDelimeterStart, intliItemStart + 12);

                string item = responseData.Substring(intliItemStart - 11, (intliItemEnd - (intliItemStart)));
                // Add the item to the string buidler
                sb.Append(item + "</li>");
            }
            string result = sb.ToString();
            // Replace the relative paths with the absolute paths
            result = result.Replace("/blogs/", "http://www.studentguru.gr/blogs/");
            result = result.Replace("/members/", "http://www.studentguru.gr/members/");
            context.Response.ContentType = "text/plain";
            context.Response.Write(result);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
