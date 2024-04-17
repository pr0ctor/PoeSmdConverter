using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PoeSmdConverter.Model
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct SmdVertex
    {
        [MarshalAs(UnmanagedType.R4, SizeConst = 1)]
        public float X;
        [MarshalAs(UnmanagedType.R4, SizeConst = 1)]
        public float Y;
        [MarshalAs(UnmanagedType.R4, SizeConst = 1)]
        public float Z;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I4, SizeConst = 4)]
        public Int16[] Unk1;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)]
        public byte[] URaw;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = 2)]
        public byte[] VRaw;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 4)]
        public byte[] BoneIndex;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 4)]
        public byte[] BoneWeight;

        public SmdVertex()
        {

        }

        // Used because the C# Marshalling doesn't have a Half Float type and this is easier to convert the extracted values
        public (Half U, Half V) ParseUVs()
        {
            return (
                U: BitConverter.ToHalf(URaw),
                V: BitConverter.ToHalf(VRaw)
            );
        }
    }
}
