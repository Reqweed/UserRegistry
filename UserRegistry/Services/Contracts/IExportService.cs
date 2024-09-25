using UserRegistry.Models;

namespace UserRegistry.Services.Contracts;

public interface IExportService
{
    MemoryStream GetCsvFile(IEnumerable<User> users);
}