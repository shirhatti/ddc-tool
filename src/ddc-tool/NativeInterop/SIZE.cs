using System;
using System.Runtime.InteropServices;

namespace Ddc.Tool.Native.Types
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct SIZE
    {
        public int cx;
        public int cy;

        public SIZE(int width, int height)
        {
            cx = width;
            cy = height;
        }

        public int Width
        {
            get { return cx; }
            set { cx = value; }
        }

        public int Height
        {
            get { return cy; }
            set { cy = value; }
        }

    }
}
