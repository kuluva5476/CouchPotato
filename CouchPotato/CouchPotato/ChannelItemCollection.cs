using System;
using System.Collections;
using System.Text;

namespace com.CouchPotato
{
    class ChannelItemCollection
    {
        ArrayList _ChannelItems = new ArrayList();

        public ChannelItem AddChannel(ChannelItem _Item)
        {
            _ChannelItems.Add(_Item);
            return _Item;
        }

        public ChannelItem this[int i]
        {
            get { return (ChannelItem)_ChannelItems[i]; }
        }

        public void Clear()
        {
            _ChannelItems.Clear();
        }

        public int Count
        {
            get { return _ChannelItems.Count; }
        }
    }
}
