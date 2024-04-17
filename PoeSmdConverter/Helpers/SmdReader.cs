using BrotliSharpLib;
using PoeSmdConverter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;

namespace PoeSmdConverter.Helpers
{
    internal class SmdReader
    {
        public SmdReader() { }

        public Smd ReadBrotliSmdFile(Smd smd)
        {
            using var brotliStream = new BrotliSharpLib.BrotliStream(smd.FileData, CompressionMode.Decompress);
            brotliStream.Seek(0, SeekOrigin.Begin);
            brotliStream.CopyTo(smd.FileData);

            return ReadSmdFile(smd);
        }
        public Smd ReadSmdFile(Smd smd)
        {
            ReadHeader(smd);

            ReadMeshDefinitions(smd);

            ReadMeshNames(smd);

            ReadIndices(smd);

            ReadVertices(smd);

            return smd;
        }

        private void ReadHeader(Smd smd)
        {
            IntPtr headerPtr = Marshal.AllocHGlobal(Marshal.SizeOf(smd.Header));

            try
            {
                var headerBytes = Helpers.AdvanceStreamReader(smd.FileData, MagicNumbers.HeaderByteLength);

                Marshal.Copy(headerBytes, 0, headerPtr, MagicNumbers.HeaderByteLength - 1);

                smd.Header = Marshal.PtrToStructure<SmdHeader>(headerPtr);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
            finally
            {
                Marshal.FreeHGlobal(headerPtr);
            }
        }

        private void ReadMeshDefinitions(Smd smd)
        {
            IntPtr recordPtr = Marshal.AllocHGlobal(Marshal.SizeOf(new MeshDefinition()));

            try
            {
                for (int i = 0; i < smd.Header.NumMeshs; i++)
                {
                    var meshDefinitionBytes = Helpers.AdvanceStreamReader(smd.FileData, MagicNumbers.MeshDefinitionRecordLength);

                    Marshal.Copy(meshDefinitionBytes, 0, recordPtr, MagicNumbers.MeshDefinitionRecordLength - 1);

                    var meshDefinition = Marshal.PtrToStructure<MeshDefinition>(recordPtr);

                    smd.MeshDefinitions.Add(meshDefinition);
                }

            }
            finally
            {
                Marshal.FreeHGlobal(recordPtr);
            }
        }

        private void ReadMeshNames(Smd smd)
        {
            try
            {
                for (int i = 0; i < smd.Header.NumMeshs; i++)
                {
                    var meshNameBytes = Helpers.AdvanceStreamReader(smd.FileData, ((int)smd.MeshDefinitions[i].NameLen));

                    var stringData = new byte[(int)smd.MeshDefinitions[i].NameLen];

                    using var memStream = new MemoryStream(meshNameBytes);
                    using (var binaryReader = new BinaryReader(memStream))
                    {
                        binaryReader.Read(stringData, 0, stringData.Length);
                    };

                    var meshName = Helpers.DecodeUTF16(stringData);

                    smd.Names.Add(meshName);

                }

            }
            finally
            {

            }
        }

        private void ReadIndices(Smd smd)
        {
            try
            {
                for (int i = 0; i < (smd.Header.NumIdx * MagicNumbers.IndexItemMemoryOffset); i++)
                {
                    var indexBufferBytes = Helpers.AdvanceStreamReader(smd.FileData, MagicNumbers.IndexItemLength);

                    var uint16Bytes = BitConverter.ToUInt16(indexBufferBytes, 0);

                    smd.IndexBuffer16.Add(uint16Bytes);
                }
            }
            finally
            {

            }
        }

        private void ReadVertices(Smd smd)
        {
            IntPtr recordPtr = new IntPtr();
            try
            {
                recordPtr = Marshal.AllocHGlobal(Marshal.SizeOf(MagicNumbers.VetexBufferRecordLength));

                for (int i = 0; i < smd.Header.NumVert; i++)
                {

                    var vertexBytes = Helpers.AdvanceStreamReader(smd.FileData, MagicNumbers.VetexBufferRecordLength);

                    Marshal.Copy(vertexBytes, 0, recordPtr, vertexBytes.Length);

                    var smdVertex = Marshal.PtrToStructure<SmdVertex>(recordPtr);

                    smd.VertexBuffer.Add(smdVertex);

                    var UVs = smdVertex.ParseUVs();

                    if (((float)UVs.U) < 0)
                    {
                        break;
                    }
                }

            }
            finally
            {
                Marshal.FreeHGlobal(recordPtr);
            }
        }

    }
}
