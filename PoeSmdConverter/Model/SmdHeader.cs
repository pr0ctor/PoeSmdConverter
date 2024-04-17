using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PoeSmdConverter.Model
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct SmdHeader
    {
        [MarshalAs(UnmanagedType.U1, SizeConst=1)]
        public byte Version;

        [MarshalAs(UnmanagedType.U4, SizeConst = 1)]
        public UInt32 NumIdx;

        [MarshalAs(UnmanagedType.U4, SizeConst = 1)]
        public UInt32 NumVert;

        [MarshalAs(UnmanagedType.U1, SizeConst = 1)]
        public byte Unk1;

        [MarshalAs(UnmanagedType.U1, SizeConst = 1)]
        public byte NumMeshs;

        [MarshalAs(UnmanagedType.U1, SizeConst = 1)]
        public byte Unk2;

        [MarshalAs(UnmanagedType.U4, SizeConst = 1)]
        public UInt32 TotalStringLen;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 6)]
        public float[] BoundingBox; // { get; set; } = new float[6];
       
        public SmdHeader()
        {

        }

    }
}
