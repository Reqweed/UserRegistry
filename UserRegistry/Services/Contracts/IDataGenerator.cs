using UserRegistry.Models;
using UserRegistry.ViewModels;

namespace UserRegistry.Services.Contracts;

public interface IDataGenerator
{
    List<User> GenerateUsers(GeneratorSettings settings);
}