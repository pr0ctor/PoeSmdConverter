using Microsoft.Extensions.Configuration;
using PoeSmdConverter.Helpers;
using PoeSmdConverter.Model;

// Program is callable using the following args:
// --smd <path_to_smd> --obj <path_to_output_obj>

IConfiguration config = new ConfigurationBuilder()
    .AddCommandLine(args)
    .Build();

Helpers.ValidateCommandlineArgs(config, args);

try
{
    using var sourceFile = new FileStream(config["smd"].Trim(), FileMode.Open, FileAccess.Read);

    var smdFile = new Smd(sourceFile);

    var potentialBrotliCheckbytes = smdFile.GetBrotliChecksum();

    if(Helpers.CheckForBrotliCompression(potentialBrotliCheckbytes))
    {
        var rawLengthBuffer = new byte[MagicNumbers.TotalFileLengthIndicatorLength];

        var rawLength = sourceFile.Read(rawLengthBuffer, 0, MagicNumbers.TotalFileLengthIndicatorLength);

        var readableLength = BitConverter.ToInt32(rawLengthBuffer, 0);

        var smdReader = new SmdReader();
        var parsedSMD = smdReader.ReadBrotliSmdFile(smdFile);

        using var destinationFile = new FileStream(config["obj"].Trim(), FileMode.OpenOrCreate, FileAccess.Write);
        var objWriter = new ObjWriter(destinationFile);
        
        objWriter.WriteObjFromSmd(parsedSMD);
    }
    else
    {
        var smdReader = new SmdReader();
        smdFile.ResetFileDataPointer();
        var parsedSMD = smdReader.ReadSmdFile(smdFile);

        using var destinationFile = new FileStream(config["obj"].Trim(), FileMode.OpenOrCreate, FileAccess.Write);
        var objWriter = new ObjWriter(destinationFile);

        objWriter.WriteObjFromSmd(parsedSMD);
    }

}
catch(FileNotFoundException ex)
{
    Console.WriteLine("Smd file not found at the given location. " + ex.ToString());
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