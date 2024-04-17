using Microsoft.Extensions.Configuration;
using PoeSmdConverter.Helpers;
using PoeSmdConverter.Model;

// Program is callable using the following args:
// --smd <path_to_smd> --obj <path_to_output_obj>

IConfiguration config = new ConfigurationBuilder()
    .AddCommandLine(args)
    .Build();

Helpers.ValidateCommandlineArgs(config, args);

var smdPath = config["smd"].Trim();
var objPath = config["obj"].Trim();

try
{

    using var sourceFile = new FileStream(smdPath, FileMode.Open, FileAccess.Read);
    using var destinationFile = new FileStream(objPath, FileMode.OpenOrCreate, FileAccess.Write);

    var smdReader = new SmdReader();

    Console.WriteLine($"Read file from \"{smdPath}\"");

    var smdFile = new Smd(sourceFile);
    var parsedSmd = new Smd();

    // Check file for Brotli Compression
    var potentialBrotliCheckbytes = smdFile.GetBrotliChecksum();

    // Check for the Brotli check bytes from the first few bytes from the source file
    if(Helpers.CheckForBrotliCompression(potentialBrotliCheckbytes))
    {
        // If the file has Brotli Compression, read the check bytes in order to properly decompress 
        var rawLengthBuffer = new byte[MagicNumbers.TotalFileLengthIndicatorLength];

        var rawLength = sourceFile.Read(rawLengthBuffer, 0, MagicNumbers.TotalFileLengthIndicatorLength);

        var readableLength = BitConverter.ToInt32(rawLengthBuffer, 0);

        Console.WriteLine("\tParsing data from Brotli compressed smd file.");

        parsedSmd = smdReader.ReadBrotliSmdFile(smdFile);
    }
    else
    {
        // If not Brotli Compressed, reset the stream to the start since
        //      checking the first few bytes causes the pointer to move
        smdFile.ResetFileDataPointer();

        Console.WriteLine("\tParsing data from smd file");

        parsedSmd = smdReader.ReadSmdFile(smdFile);
    }

    var objWriter = new ObjWriter(destinationFile);

    Console.WriteLine($"Writing processed data to \"{objPath}\"");

    objWriter.WriteObjFromSmd(parsedSmd);

}
catch(FileNotFoundException ex)
{
    Console.WriteLine($"Smd file not found at the given location: \"{smdPath}\". " + ex.ToString());
    throw;
}
catch(Exception ex)
{
    Console.WriteLine(ex.ToString());
    throw;
}
finally
{

}