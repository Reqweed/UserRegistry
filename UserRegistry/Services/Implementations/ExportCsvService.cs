using System.Globalization;
using CsvHelper;
using UserRegistry.Models;
using UserRegistry.Services.Contracts;

namespace UserRegistry.Services.Implementations;

public class ExportCsvService : IExportService
{
    public MemoryStream GetFile(IEnumerable<User> users)
    {
        var memoryStream = new MemoryStream();
        var streamWriter = new StreamWriter(memoryStream);
        var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

        csvWriter.WriteRecords(users);
        streamWriter.Flush();
        memoryStream.Position = 0;

        return memoryStream;
    }
}