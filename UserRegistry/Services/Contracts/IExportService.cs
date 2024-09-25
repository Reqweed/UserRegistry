using UserRegistry.Models;

namespace UserRegistry.Services.Contracts;

public interface IExportService
{
    MemoryStream GetFile(IEnumerable<User> users);
}