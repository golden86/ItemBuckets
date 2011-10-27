using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Jobs;
using Sitecore.Publishing;
using Sitecore.Publishing.Pipelines.Publish;
using Sitecore.Publishing.Pipelines.PublishItem;

namespace ItemBucket.Kernel.Kernel.Publishing
{
    public class ProcessQueue : PublishProcessor
    {
        // Methods
        private PublishItemContext CreateItemContext(PublishingCandidate entry, PublishContext context)
        {
            Assert.ArgumentNotNull(entry, "entry");
            PublishItemContext context2 = PublishManager.CreatePublishItemContext(entry.ItemId, entry.PublishOptions);
            context2.Job = context.Job;
            context2.User = context.User;
            context2.PublishContext = context;
            return context2;
        }

        public override void Process(PublishContext context)
        {
            Stopwatch watch = new Stopwatch();
            Log.Info("publish process started", this);
            watch.Start();
            Assert.ArgumentNotNull(context, "context");

            //Parallel.ForEach(context.Queue, entries => ProcessEntries(entries, context));
            foreach (IEnumerable<PublishingCandidate> enumerable in context.Queue)
            {
                ProcessEntries(enumerable, context, 0);
            }
            UpdateJobStatus(context);
            watch.Stop();
            Log.Info("publish process completed elapsed: " + watch.Elapsed, this);
        }

        protected virtual void ProcessEntries(IEnumerable<PublishingCandidate> entries, PublishContext context, int depth)
        {
            //foreach (PublishingCandidate candidate in entries)
            //{
            //    ProcessCandidate(candidate, context);
            //}
            int level = depth - 5;
            if (level < 1) level = 1;

            Parallel.ForEach(entries, new ParallelOptions { MaxDegreeOfParallelism = level }, candidate => ProcessCandidate(candidate, context, depth));
             
        }

        private void ProcessCandidate(PublishingCandidate candidate, PublishContext context, int depth)
        {
            PublishItemResult result = PublishItemPipeline.Run(CreateItemContext(candidate, context));
            if (!SkipReferrers(result, context))
            {
                ProcessEntries(result.ReferredItems, context, depth + 1);
            }
            if (!SkipChildren(result, candidate, context))
            {
                ProcessEntries(candidate.ChildEntries, context, depth + 1);
            }
        }

        private bool SkipChildren(PublishItemResult result, PublishingCandidate entry, PublishContext context)
        {
            if (result.ChildAction == PublishChildAction.Skip)
            {
                return true;
            }
            if (result.ChildAction != PublishChildAction.Allow)
            {
                return false;
            }
            if ((entry.PublishOptions.Mode != PublishMode.SingleItem) && (result.Operation == PublishOperation.Created))
            {
                return false;
            }
            return !entry.PublishOptions.Deep;
        }

        protected virtual bool SkipReferrers(PublishItemResult result, PublishContext context)
        {
            return (result.ReferredItems.Count == 0);
        }

        private void UpdateJobStatus(PublishContext context)
        {
            Job job = context.Job;
            if (job != null)
            {
                job.Status.Messages.Add(string.Format("{0}{1}", Translate.Text("Items created: "), context.Statistics.Created));
                job.Status.Messages.Add(string.Format("{0}{1}", Translate.Text("Items deleted: "), context.Statistics.Deleted));
                job.Status.Messages.Add(string.Format("{0}{1}", Translate.Text("Items updated: "), context.Statistics.Updated));
                job.Status.Messages.Add(string.Format("{0}{1}", Translate.Text("Items skipped: "), context.Statistics.Skipped));
            }
        }
    }


}
