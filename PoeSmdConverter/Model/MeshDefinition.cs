using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PoeSmdConverter.Model
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    internal struct MeshDefinition
    {
        [MarshalAs(UnmanagedType.U4, SizeConst = 1)]
        public UInt32 NameLen;
        [MarshalAs(UnmanagedType.U4, SizeConst = 1)]
        public UInt32 FaceOffset;

        public MeshDefinition()
        {

        }
    }
}
