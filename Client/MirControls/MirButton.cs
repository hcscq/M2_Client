using System;
using System.Drawing;
using System.Windows.Forms;
using Client.MirSounds;

namespace Client.MirControls
{
    public enum UsedSize { Index, HoverIndex, PressedIndex, Specify }
    public class MirButton : MirImageControl
    {
        #region Font Colour
        public Color FontColour
        {
            get
            {
                if (_label != null && !_label.IsDisposed)
                    return _label.ForeColour;
                return Color.Empty;
            }
            set
            {
                if (_label != null && !_label.IsDisposed)
                    _label.ForeColour = value;
            }
        }
        #endregion

        #region Hover Index
        private int _hoverIndex;
        public int HoverIndex
        {
            get { return _hoverIndex; }
            set
            {
                if (_hoverIndex == value)
                    return;
                _hoverIndex = value;
                OnHoverIndexChanged();
            }
        }
        public event EventHandler HoverIndexChanged;
        private void OnHoverIndexChanged()
        {
            if (HoverIndexChanged != null)
                HoverIndexChanged.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region Index
        public override int Index
        {
            get
            {
                if (!Enabled)
                    return base.Index;

                if (_pressedIndex >= 0 && ActiveControl == this && MouseControl == this)
                    return _pressedIndex;

                if (_hoverIndex >= 0 && MouseControl == this)
                    return _hoverIndex;

                return base.Index;
            }
            set { base.Index = value; }
        }
        #endregion

        #region Label
        private MirLabel _label;
        #endregion

        #region CenterText
        private bool _center;
        public bool CenterText
        {
            get
            {
                return _center;
            }
            set
            {
                _center = value;
                if (_center)
                {
                    _label.Size = Size;
                    _label.DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;
                }
                else
                    _label.AutoSize = true;
            }
        }
        #endregion

        #region Pressed Index
        private int _pressedIndex;
        public int PressedIndex
        {
            set
            {
                if (_pressedIndex == value)
                    return;
                _pressedIndex = value;
                OnPressedIndexChanged();
            }
            get { return _pressedIndex; }
        }

        public event EventHandler PressedIndexChanged;
        private void OnPressedIndexChanged()
        {
            if (PressedIndexChanged != null)
                PressedIndexChanged.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region Size
        public UsedSize TakeSizeMode = UsedSize.Index;
        protected override void OnSizeChanged()
        {
            base.OnSizeChanged();

            if (_label != null && !_label.IsDisposed)
                _label.Size = Size;
        }

        public override Size Size
        {
            set { base.Size = value; }
            get
            {
                switch (TakeSizeMode)
                {
                    case UsedSize.Index:
                        if (Library != null && Index >= 0)
                            return Library.GetTrueSize(Index);
                        break;
                    case UsedSize.HoverIndex:
                        if (Library != null && Index >= 0)
                            return Library.GetTrueSize(HoverIndex);
                        break;
                    case UsedSize.PressedIndex:
                        if (Library != null && Index >= 0)
                            return Library.GetTrueSize(PressedIndex);
                        break;
                    case UsedSize.Specify:
                    default:break;
                }
                return base.Size;
            }
        }
        #endregion

        #region Text
        public string Text
        {
            set
            {
                if (_label == null || _label.IsDisposed)
                    return;
                _label.Text = value;
                _label.Visible = !string.IsNullOrEmpty(value);
            }
            get { return _label.Text; }
        }
        #endregion

        public MirButton()
        {
            HoverIndex = -1;
            PressedIndex = -1;
            Sound = SoundList.ButtonB;

            _label = new MirLabel
            {
                NotControl = true,
                Parent = this,
                //Font = new Font("Constantia", 8, FontStyle.Italic),
                //OutLine = true,
                //OutLineColour = Color.FromArgb(255, 70, 50, 30),
            };
        }

        protected override void Highlight()
        {
            Redraw();
            base.Highlight();
        }
        protected override void Activate()
        {
            Redraw();
            base.Activate();
        }
        protected override void Dehighlight()
        {
            Redraw();
            base.Dehighlight();
        }
        protected override void Deactivate()
        {
            Redraw();
            base.Deactivate();
        }


        #region Disposable
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!disposing) return;

            HoverIndexChanged = null;
            _hoverIndex = 0;

            if (_label != null && !_label.IsDisposed)
                _label.Dispose();
            _label = null;

            PressedIndexChanged = null;
            _pressedIndex = 0;
        }
        #endregion
    }
}
