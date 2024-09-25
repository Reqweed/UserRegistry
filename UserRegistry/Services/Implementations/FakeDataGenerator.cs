using Bogus;
using Microsoft.Extensions.Options;
using UserRegistry.Models;
using UserRegistry.Services.Contracts;
using UserRegistry.Settings;
using UserRegistry.ViewModels;

namespace UserRegistry.Services.Implementations;

public class FakeDataGenerator(IOptions<CountryConfiguration> option) : IDataGenerator
{
    private readonly CountryConfiguration _countryConfig = option.Value;

    public List<User> GenerateUsers(GeneratorSettings settings)
    {
        var faker = CreateFaker(settings);
        var fakeUsers = new List<User>();
        
        for (var i = 0; i < settings.Count; i++)
        {
            var user = GenerateUser(faker, settings, i);
            fakeUsers.Add(user);
        }

        for (var i = 0; i < fakeUsers.Count; i++)
        {
            fakeUsers[i] = ApplyErrors(faker, settings, fakeUsers[i]);
        }
        
        return fakeUsers;
    }

    private Faker CreateFaker(GeneratorSettings settings)
    {
        return new Faker(_countryConfig.Countries[settings.Region].Region)
        {
            Random = new Randomizer(settings.Seed + settings.Page)
        };
    }

    private User GenerateUser(Faker faker, GeneratorSettings settings, int index)
    {
        var user = new User
        {
            Number = (settings.Page - 1) * settings.Count + index + 1,
            Identifier = faker.Random.Guid().ToString(),
            FullName = GenerateFullName(faker),
            Address = GetAddress(faker),
            PhoneNumber = GetPhone(faker, settings.Region)
        };

        return user;
    }

    private string GenerateFullName(Faker faker) =>
        $"{faker.Name.FirstName()} {faker.Name.LastName()}";

    private string GetAddress(Faker faker)
    {
        return $"{faker.Address.City()}, {faker.Address.StreetName()}, " +
               $"{faker.Address.BuildingNumber()}, {faker.Address.SecondaryAddress()}";
    }
    
    private string GetPhone(Faker faker, string region) =>
        faker.Phone.PhoneNumber(_countryConfig.Countries[region].PhoneFormats);
    
    private User ApplyErrors(Faker faker, GeneratorSettings settings, User user)
    {
        var errorCountInt = (int)settings.ErrorCount;

        for (var i = 0; i < errorCountInt; i++)
        {
            user = AddErrors(faker, user, settings.Region);
        }

        if (faker.Random.Double() < settings.ErrorCount - errorCountInt)
        {
            user = AddErrors(faker, user, settings.Region);
        }

        return user;
    }
    
    private User AddErrors(Faker faker, User user, string region)
    {
        var errorType = faker.Random.Int(0, 2);
        
        switch (errorType)
        {
            case 0:
                user.FullName = ApplyRandomError(faker, user.FullName,  region);
                break;
            case 1:
                user.Address = ApplyRandomError(faker, user.Address,  region);
                break;
            case 2:
                user.PhoneNumber = ApplyRandomError(faker, user.PhoneNumber, region);
                break;
        }
        
        return user;
    }

    private string ApplyRandomError(Faker faker, string data, string region)
    {
        var errorType = faker.Random.Int(0, 2);
        var position = faker.Random.Int(0, data.Length - 1);

        return errorType switch
        {
            0 when data.Length > 5 => data.Remove(position, 1), 
            1 => InsertRandomChar(faker, data, position, region),
            2 when position < data.Length - 1 => SwapCharacters(data, position),
            _ => data
        };
    }

    private string InsertRandomChar(Faker faker, string data, int position, string region)
    {
        var alphabet = _countryConfig.Countries[region].Alphabet;
        var randomChar = alphabet[faker.Random.Int(0, alphabet.Length - 1)];
        return data.Insert(position, randomChar.ToString());
    }

    private string SwapCharacters(string data, int position)
    {
        var charArray = data.ToCharArray();
        (charArray[position], charArray[position + 1]) = (charArray[position + 1], charArray[position]);
        return new string(charArray);
    }
}