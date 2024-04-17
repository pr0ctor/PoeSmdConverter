using PoeSmdConverter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoeSmdConverter.Helpers
{
    internal class ObjWriter
    {
        private FileStream DestinationFile { get; set; }

        public ObjWriter() { }

        public ObjWriter(FileStream destinationFile)
        {
            DestinationFile = destinationFile;
        }

        public void WriteObjFromSmd(Smd smd)
        {
            using StreamWriter writer = new StreamWriter(DestinationFile);

            WriteVertices(writer, smd);

            WriteUVs(writer, smd);

            WriteFaces(writer, smd);
        }

        private void WriteVertices(StreamWriter writer, Smd smd)
        {
            foreach(var vertex in smd.VertexBuffer)
            {
                writer.WriteLine($"v {vertex.X} {vertex.Z * -1} {vertex.Y}");
            }
        }

        private void WriteUVs(StreamWriter writer, Smd smd)
        {
            foreach (var vertex in smd.VertexBuffer)
            {
                var UVs = vertex.ParseUVs();
                writer.WriteLine($"vt { UVs.U } { UVs.V }");
            }
        }

        private void WriteFaces(StreamWriter writer, Smd smd)
        {
            var currentGroup = 0;
            for(int i = 0; i < smd.Header.NumIdx; i++)
            {
                if(smd.MeshDefinitions.Count() > currentGroup && i == smd.MeshDefinitions[currentGroup].FaceOffset)
                {
                    writer.WriteLine($"g {smd.Names[currentGroup]}");
                    currentGroup++;
                }

                var baseOffset = i * 3;
                writer.WriteLine($"f { smd.IndexBuffer16[baseOffset] + 1 }/{smd.IndexBuffer16[baseOffset] + 1} {smd.IndexBuffer16[baseOffset + 1] + 1}/{smd.IndexBuffer16[baseOffset + 1] + 1} {smd.IndexBuffer16[baseOffset + 2] + 1}/{smd.IndexBuffer16[baseOffset + 2] + 1}");
            }
        }
    }
}
