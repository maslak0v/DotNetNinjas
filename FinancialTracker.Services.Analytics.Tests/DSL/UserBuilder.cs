using FinancialTracker.Services.Analytics.Models;
using Moq;
namespace FinancialTracker.Services.Analytics.Tests.DSL;

public class UserBuilder
{
    private string _name = "Emma";
    private Guid _guid = Guid.NewGuid();
    
    public UserBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public User Please()
    {
        var user = new User
        {
            Name = _name,
            Guid = _guid
        };

        return user;
    }
}