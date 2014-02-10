using System.Collections.Generic;

namespace ABPUtils
{
    public class PatchConfig
    {
        public PatchConfig()
        {
            RemovedItems = new List<string>();
            NewItems = new List<string>();
            ModifyItems = new List<ModifyItem>();
        }

        public List<string> RemovedItems
        {
            get;
            set;
        }

        public List<string> NewItems
        {
            get;
            set;
        }

        public List<ModifyItem> ModifyItems
        {
            get;
            set;
        }
    }

    public class ModifyItem
    {
        public string OldItem
        {
            get;
            set;
        }

        public string NewItem
        {
            get;
            set;
        }
    }
}
