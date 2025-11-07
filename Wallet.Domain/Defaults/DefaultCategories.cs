using Wallet.Domain.Entities;

namespace Wallet.Domain.Defaults;

public class DefaultCategories
{
    public static readonly List<Category> List = new()
    {
        new Category { CategoryId = 1, Name = "Еда", Icon = "utensils" },
        new Category { CategoryId = 2, Name = "Транспорт", Icon = "car-side" },
        new Category { CategoryId = 3, Name = "Жилье", Icon = "home" },
        new Category { CategoryId = 4, Name = "Здоровье", Icon = "medkit" },
        new Category { CategoryId = 5, Name = "Развлечения", Icon = "gamepad" },
        new Category { CategoryId = 6, Name = "Одежда", Icon = "tshirt" },
        new Category { CategoryId = 7, Name = "Образование", Icon = "book" },
        new Category { CategoryId = 8, Name = "Подарки", Icon = "gift" },
        new Category { CategoryId = 9, Name = "Путешествия", Icon = "plane" },
        new Category { CategoryId = 10, Name = "Подписки", Icon = "credit-card" },
        new Category { CategoryId = 11, Name = "Дети", Icon = "baby-carriage" },
        new Category { CategoryId = 12, Name = "Прочее", Icon = "ellipsis-h" }
    };
}