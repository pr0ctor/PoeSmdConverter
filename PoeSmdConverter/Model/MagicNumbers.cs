using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoeSmdConverter.Model
{
    internal static class MagicNumbers
    {
        public static readonly int HeaderByteLength = 40;
        public static readonly int BrotliCompressionIndicatorLength = 3;
        public static readonly int TotalFileLengthIndicatorLength = 4;
        public static readonly int MeshDefinitionRecordLength = 8;
        public static readonly int IndexItemLength = 2;
        public static readonly int IndexItemMemoryOffset = 3;
        public static readonly int VetexBufferRecordLength = 32;

        public static readonly byte[] BrotliCompressionIndicators = new byte[3] { 0x43, 0x4D, 0x50 };

    }
}
