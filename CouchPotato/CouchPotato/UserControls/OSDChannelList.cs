using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Diagnostics;
using com.CouchPotato;

namespace com.CouchPotato.UserControls
{
    public partial class OSDChannelList : Panel, ICustomTypeDescriptor
    {
        public OSDChannelList()
        {
            InitializeComponent();
        }

        private ChannelItemCollection _Channels;
        private string _ChannelListXML = "";
        private string _ChannelThumbnailPath = "";

        private int _ChannelsOnScreen = 5;

        private int _Offset = -1;

        //private int _X = 20;
        //private int _Y = 20;

        private int _LabelWidth = 300;
        private int _LabelHeight = 32;

        // Maximum number of Labels set to 9...
        Label[] _lblChannels = new Label[9];

        //private int _SelectedIndex = -1;
        private int _HighlightedIndex = -1;

        [Description("Number of channel items to be shown"), Category("Appearance"), DefaultValue(5), Browsable(true)]
        public int ChannelsOnScreen
        {
            get { return _ChannelsOnScreen; }
            set { _ChannelsOnScreen = value; }
        }

        [Description("ChannelItem Width"), DefaultValue(300), Category("Appearance"), Browsable(true)]
        public int ItemWidth
        {
            set { _LabelWidth = value; }
        }

        [Description("ChannelItem Height"), DefaultValue(32), Category("Appearance"), Browsable(true)]
        public int ItemHeight
        {
            set { _LabelHeight = value; }
        }

        [Browsable(false)]
        public string ChannelAddress
        {
            get { return _Channels[_HighlightedIndex].SOPUrl; }
        }

        public string ChannelList
        {
            set { _ChannelListXML = value; }
        }

        public string ThumbnailPath
        {
            set { _ChannelThumbnailPath = value; }
        }

        /// <summary>
        /// Initialize OSD Channel List
        /// </summary>
        public void initChannelList()
        {
            readChannels();
            _Offset = (_ChannelsOnScreen - 1) / 2;
            _HighlightedIndex = 0;

            for (int i = 0; i < _ChannelsOnScreen; i++)
            {
                _lblChannels[i] = new Label();
                Label lbl = _lblChannels[i];

                lbl.Font = new Font(lbl.Font.FontFamily, 20);
                lbl.ForeColor = Color.FromArgb(0xffffff);
                lbl.BackColor = Color.FromArgb(0x30, 0x30, 0x30);
                lbl.AutoSize = false;
                lbl.BorderStyle = BorderStyle.FixedSingle;
                lbl.Location = new System.Drawing.Point(0, _LabelHeight * i);
                lbl.Name = "label1";
                lbl.Size = new System.Drawing.Size(_LabelWidth, _LabelHeight);
                this.Controls.Add(lbl);
            }
            // Bold & Yellow for selected channel
            _lblChannels[_Offset].Font = new Font(_lblChannels[_Offset].Font, FontStyle.Bold);
            _lblChannels[_Offset].ForeColor = Color.FromArgb(0xff, 0xff, 0xab);
            this.Height = _LabelHeight * _ChannelsOnScreen;
            this.Width = _LabelWidth;
            setOSD();
        }

        public void channelUp()
        {
            _HighlightedIndex -= 1;
            setOSD();
        }

        public void channelDown()
        {
            _HighlightedIndex += 1;
            setOSD();
        }

        /// <summary>
        /// Set OSD Channel List captions
        /// </summary>
        private void setOSD()
        {
            _HighlightedIndex += _Channels.Count;
            _HighlightedIndex %= _Channels.Count;

            for (int i = 0; i < _ChannelsOnScreen; i++)
            {
                _lblChannels[i].Text = _Channels[((_HighlightedIndex + i - _Offset + _Channels.Count) % _Channels.Count)].DisplayName;
            }
        }

        private void readChannels()
        {
            _Channels = new ChannelItemCollection();
            XmlDocument oXml = new XmlDocument();
            oXml.Load(_ChannelListXML);

            XmlNodeList oNodeList = oXml.SelectNodes("channels");
            XmlNode oRoot = oNodeList[0];

            foreach (XmlNode oNode in oRoot.ChildNodes)
            {
                string id = oNode.Attributes["id"].Value;
                string display_name = oNode.Attributes["display_name"].Value;
                string sopurl = oNode.InnerText;

                if (_ChannelThumbnailPath != "")
                {
                    string sThumbnailFile = _ChannelThumbnailPath + "\\" + id + ".jpg";
                    _Channels.AddChannel(new ChannelItem(id, sopurl, display_name, sThumbnailFile));
                }
                else
                    _Channels.AddChannel(new ChannelItem(id, sopurl, display_name));
            }
        }

        #region Hiding Unwanted Properties

        private string[] _excludeBrowsableProperties = 
        {
            "AccessibleDescription",
            "AccessibleName",
            "AccessibleRole",
            "AllowDrop",
            "Anchor",
            "AutoScroll",
            "AutoScrollOffset",
            "AutoScrollMargin",
            "AutoScrollMinSize",
            "AutoSize",
            "AutoSizeMode",
            "AutoValidate",

            "BackgroundImage",
            "BackgroundImageLayout",

            "CausesValidation",
            "ContextMenuStrip",
            "Cursor",
            "Dock",

            "GenerateMember",

            "ImeMode",
            "Locked",
            "Margin",
            "MaximumSize",
            "MinimumSize",

            "RightToLeft",
            "TabIndex",
            "TabStop"
        };

        private string[] _excludeBrowsableEvents = 
        {
            "Load"
            
            //"AutoSizeChanged",
            //"AutoValidateChanged",  
            //"BackColorChanged",
            //"BackgroundImageChanged",
            //"BindingContextChanged",
            //"Click",
            //"CausesValidationChanged",
            //"ChangeUICues",
            //"ImeModeChanged",
            //"RightToLeftChanged",
            //"Scroll",
            //"TabIndexChanged",
            //"TabStopChanged",
            //"Validated",
            //"Validating"
        };


        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return null;
            //return TypeDescriptor.GetDefaultEvent(this, true);

        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        // The following 2 methods will get the EventDescriptor objects for all events declared in
        // this user control, included those inherited from the UserControl object and its ancestors.
        // We then call the FilterEvents method to return a new EventDescriptorCollection with the 
        // required events removed.
        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            //return TypeDescriptor.GetEvents(this, true);

            EventDescriptorCollection orig = TypeDescriptor.GetEvents(this, attributes, true);
            return FilterEvents(orig);
        }

        public EventDescriptorCollection GetEvents()
        {
            EventDescriptorCollection orig = TypeDescriptor.GetEvents(this, true);
            return FilterEvents(orig);
            //return TypeDescriptor.GetEvents(this, true);
        }

        // The following 2 methods will get the PropertyDescriptor objects for all properties declared in
        // this user control, included those inherited from the UserControl object and its ancestors.
        // We then call the FilterProperties method to return a new PropertyDescriptorCollection with the 
        // required properties removed.
        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            PropertyDescriptorCollection orig = TypeDescriptor.GetProperties(this, attributes, true);
            return FilterProperties(orig);
        }

        public PropertyDescriptorCollection GetProperties()
        {
            PropertyDescriptorCollection orig = TypeDescriptor.GetProperties(this, true);
            return FilterProperties(orig);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        private PropertyDescriptorCollection FilterProperties(PropertyDescriptorCollection originalCollection)
        {
            // Create an enumerator containing only the properties that are not in the provided list of property names
            // and fill an array with those selected properties

            IEnumerable<PropertyDescriptor> selectedProperties = originalCollection.OfType<PropertyDescriptor>().Where(p => !_excludeBrowsableProperties.Contains(p.Name));
            PropertyDescriptor[] descriptors = selectedProperties.ToArray();

            // Return a PropertyDescriptorCollection containing only the filtered descriptors
            PropertyDescriptorCollection newCollection = new PropertyDescriptorCollection(descriptors);
            return newCollection;
        }

        private EventDescriptorCollection FilterEvents(EventDescriptorCollection origEvents)
        {
            // Create an enumerator containing only the events that are not in the provided list of event names
            // and fill an array with those selected events
            IEnumerable<EventDescriptor> selectedEvents = origEvents.OfType<EventDescriptor>().Where(e => _excludeBrowsableEvents.Contains(e.Name));
            //IEnumerable<EventDescriptor> selectedEvents = origEvents.OfType<EventDescriptor>().Where(e => !_excludeBrowsableEvents.Contains(e.Name));
            EventDescriptor[] descriptors = selectedEvents.ToArray();

            // Return an EventDescriptorCollection containing only the filtered descriptors
            EventDescriptorCollection newCollection = new EventDescriptorCollection(descriptors);

            return newCollection;
        }

        #endregion
    }
}
