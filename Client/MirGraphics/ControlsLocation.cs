using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.MirControls;
using System.Drawing;
//using System.IO;

namespace Client.MirGraphics
{
    public struct LocationNode
    {
        public int Index;
        public Point Loaction;
        public List<LocationNode> children;
    }
    public class ControlsLocation
    {
        public static byte MaxDepth=255;
        public static byte CurDepth = 0;
        public static LocationNode GetLocationList(MirControl rootControl)
        {
            LocationNode locationNode = new LocationNode();
            if (rootControl == null || rootControl.IsDisposed) return locationNode;
            locationNode.Loaction = rootControl.Location;
            locationNode.Index = rootControl.DevIndex;
            if (rootControl.Controls.Count > 0)
            {
                locationNode.children = new List<LocationNode>();
                foreach (MirControl item in rootControl.Controls)
                {
                    locationNode.children.Add(GetLocationList(item));
                }
            }
            else
            {
                locationNode.children = null;
            }

            return locationNode;
        }
        public static bool SetLocation(MirControl rootControl,LocationNode locationNode)
        {

            return true;
        }
        public static bool SaveLocationStream(LocationNode locationNode, string path = "")
        {
            
            return true;
        }
        public static bool SaveLocationXML(LocationNode locationNode,string  path="")
        {
            return true;
        }
    }
}
