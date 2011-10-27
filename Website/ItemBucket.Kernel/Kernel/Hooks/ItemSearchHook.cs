using System.Collections.Generic;
using ItemBucket.Kernel.Kernel.Managers;
using ItemBucket.Kernel.Kernel.Util;
using Sitecore.Configuration;
using Sitecore.Data.Engines.DataCommands;
using Sitecore.Data.Events;
using Sitecore.Diagnostics;
using Sitecore.Events.Hooks;
using Sitecore.StringExtensions;

namespace ItemBucket.Kernel.Kernel.Hooks
{
    public class ItemSearchHook : IHook
    {
        public ItemSearchHook(string databases)
        {
            Databases = Parser.ParseString(databases);
        }

        public IEnumerable<string> Databases { get; set; }

        public void Initialize()
        {
            foreach (var database in Databases)
            {
                MapDatabase(database);
            }
        }

        protected virtual void MapDatabase(string databaseName)
        {
            var db = Factory.GetDatabase(databaseName);

            if (db == null) return;

            db.DataManager.DataEngine.SavedItem += DataEngine_SavedItem;
            Log.Info("ContentSilo Hook initialized. Databases: ".FormatWith(Databases), this);
        }

        protected virtual void DataEngine_SavedItem(object sender, ExecutedEventArgs<SaveItemCommand> e)
        {
            var item = e.Command.Item;

            if (item.IsBucketItem())
            {
                item.RegisterMapping();
            }
        }
    }
}
