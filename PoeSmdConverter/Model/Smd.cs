using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using PoeSmdConverter.Helpers;


namespace PoeSmdConverter.Model
{
    internal class Smd
    {
        public SmdHeader Header { get; set; }
        public List<MeshDefinition> MeshDefinitions { get; set; }
        public List<string> Names { get; set; }
        public List<UInt16> IndexBuffer16 { get; set; }
        public List<UInt32> IndexBuffer32 { get; set; }
        public List<SmdVertex> VertexBuffer { get; set; }

        public Stream FileData { get; set; }

        public Smd()
        {
            Header = new SmdHeader();
            MeshDefinitions = new List<MeshDefinition>();
            Names = new List<string>();
            IndexBuffer16 = new List<UInt16>();
            IndexBuffer32 = new List<UInt32>();
            VertexBuffer = new List<SmdVertex>();
        }

        public Smd(Stream fileData)
        {
            FileData = fileData;

            Header = new SmdHeader();
            MeshDefinitions = new List<MeshDefinition>();
            Names = new List<string>();
            IndexBuffer16 = new List<UInt16>();
            IndexBuffer32 = new List<UInt32>();
            VertexBuffer = new List<SmdVertex>();
        }

        public byte[] GetBrotliChecksum()
        {
            byte[] potentialBrotliCheckbytes = new byte[MagicNumbers.BrotliCompressionIndicatorLength];

            var checkLength = FileData.Read(potentialBrotliCheckbytes, 0, MagicNumbers.BrotliCompressionIndicatorLength);

            if (checkLength != potentialBrotliCheckbytes.Length)
            {
                throw new Exception("Error reading bytes");
            }

            return potentialBrotliCheckbytes;
        }

        public void ResetFileDataPointer()
        {
            FileData.Seek(0, SeekOrigin.Begin);
        }
    }
}
