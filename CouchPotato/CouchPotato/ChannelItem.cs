using System;
using System.Collections;
using System.Text;
using System.Drawing;

namespace com.CouchPotato
{
    class ChannelItem
    {
        private string _SOPUrl = "";
        public string SOPUrl
        {
            get { return _SOPUrl; }
            set { _SOPUrl = value; }
        }

        private string _DisplayName = "";
        public string DisplayName
        {
            get { return _DisplayName; }
            set { _DisplayName = value; }
        }

        private Bitmap _Thumb = null;
        public Bitmap Thumbnail
        {
            get { return _Thumb; }
            set { _Thumb = value; }
        }

        private string _ChannelId = "";
        private string ChannelId
        {
            get { return _ChannelId; }
            set { _ChannelId = value; }
        }


        /// <summary>
        /// new an empty ChannelItem 
        /// </summary>
        public ChannelItem()
        {
            // don't do shit
            return;
        }

        /// <summary>
        /// new a ChannelItem with URL & Display Name
        /// </summary>
        /// <param name="_SOPUrl">SOPCast URL of the Channel</param>
        /// <param name="_DisplayName">Display Name of the Channel</param>
        public ChannelItem(string _ChannelId, string _SOPUrl, string _DisplayName)
        {
            this._SOPUrl = _SOPUrl;
            this._DisplayName = _DisplayName;
            this._ChannelId = _ChannelId;
        }

        public ChannelItem(string _ChannelId, string _SOPUrl, string _DisplayName, string _ThumbnailPath)
        {
            this._SOPUrl = _SOPUrl;
            this._DisplayName = _DisplayName;
            if (System.IO.File.Exists(_ThumbnailPath))
                this._Thumb = new Bitmap(_ThumbnailPath);
            this._ChannelId = _ChannelId;
        }
    }
}
