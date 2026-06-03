using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DemoExamTemplate;

// Шаблон для ДЭ 09.02.07-2-2026.
// Предметная область: магазин обуви.
// Файл можно использовать как основу для WinForms, WPF, ASP.NET MVC или консольной проверки логики.

public enum UserRole
{
    Guest,
    Client,
    Manager,
    Admin
}

public sealed class User
{
    public int Id { get; set; }
    public string Login { get; set; } = "";
    public string Password { get; set; } = "";
    public string FullName { get; set; } = "";
    public UserRole Role { get; set; }
}

public sealed class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Category { get; set; } = "";
    public string Description { get; set; } = "";
    public string Manufacturer { get; set; } = "";
    public string Supplier { get; set; } = "";
    public decimal Price { get; set; }
    public string Unit { get; set; } = "пара";
    public int StockQuantity { get; set; }
    public int DiscountPercent { get; set; }
    public string ImagePath { get; set; } = "picture.png";

    public decimal FinalPrice => Math.Round(Price * (100 - DiscountPercent) / 100m, 2);
    public bool HasDiscount => DiscountPercent > 0;
    public bool HasBigDiscount => DiscountPercent > 15;
    public bool IsOutOfStock => StockQuantity == 0;
}

public sealed class Order
{
    public int Id { get; set; }
    public string Article { get; set; } = "";
    public string Status { get; set; } = "";
    public string PickupAddress { get; set; } = "";
    public DateTime OrderDate { get; set; } = DateTime.Today;
    public DateTime? DeliveryDate { get; set; }
    public int ProductId { get; set; }
}

public sealed class ProductListRequest
{
    public string SearchText { get; set; } = "";
    public string SupplierFilter { get; set; } = "Все поставщики";
    public ProductSortMode SortMode { get; set; } = ProductSortMode.Default;
}

public enum ProductSortMode
{
    Default,
    StockAscending,
    StockDescending
}

public sealed class ProductRowViewModel
{
    public required Product Product { get; init; }
    public required string BackgroundColor { get; init; }
    public required string PriceText { get; init; }
    public required string FinalPriceText { get; init; }
    public bool ShowOldPrice { get; init; }
}

public sealed class ExamRepository
{
    public const string AllSuppliers = "Все поставщики";
    public const string PlaceholderImage = "picture.png";

    private readonly List<User> _users =
    [
        new User { Id = 1, Login = "client", Password = "123", FullName = "Иванов Иван Иванович", Role = UserRole.Client },
        new User { Id = 2, Login = "manager", Password = "123", FullName = "Петрова Мария Сергеевна", Role = UserRole.Manager },
        new User { Id = 3, Login = "admin", Password = "123", FullName = "Сидоров Алексей Павлович", Role = UserRole.Admin }
    ];

    private readonly List<Product> _products =
    [
        new Product
        {
            Id = 1,
            Name = "Кроссовки Active Run",
            Category = "Кроссовки",
            Description = "Легкая спортивная модель",
            Manufacturer = "StepLine",
            Supplier = "ООО СпортПоставка",
            Price = 4990.00m,
            Unit = "пара",
            StockQuantity = 17,
            DiscountPercent = 10,
            ImagePath = PlaceholderImage
        },
        new Product
        {
            Id = 2,
            Name = "Ботинки Urban Warm",
            Category = "Ботинки",
            Description = "Утепленные ботинки для города",
            Manufacturer = "NordWay",
            Supplier = "ИП Волков",
            Price = 7490.00m,
            Unit = "пара",
            StockQuantity = 4,
            DiscountPercent = 20,
            ImagePath = PlaceholderImage
        },
        new Product
        {
            Id = 3,
            Name = "Туфли Classic Office",
            Category = "Туфли",
            Description = "Классическая офисная обувь",
            Manufacturer = "Elegance",
            Supplier = "ООО Комфорт",
            Price = 6390.00m,
            Unit = "пара",
            StockQuantity = 0,
            DiscountPercent = 0,
            ImagePath = PlaceholderImage
        }
    ];

    private readonly List<Order> _orders =
    [
        new Order
        {
            Id = 1,
            Article = "ORD-1001",
            Status = "Новый",
            PickupAddress = "ул. Центральная, 10",
            OrderDate = DateTime.Today,
            DeliveryDate = DateTime.Today.AddDays(3),
            ProductId = 1
        }
    ];

    public User Guest => new()
    {
        Id = 0,
        Login = "guest",
        FullName = "Гость",
        Role = UserRole.Guest
    };

    public User GetCurrentUserOrGuest(string login, string password)
    {
        return Login(login, password) ?? Guest;
    }

    public User? Login(string login, string password)
    {
        return _users.FirstOrDefault(user =>
            user.Login.Equals(login, StringComparison.OrdinalIgnoreCase)
            && user.Password == password);
    }

    public List<ProductRowViewModel> GetProductRows(User currentUser, ProductListRequest request)
    {
        return GetProducts(currentUser, request)
            .Select(ToProductRow)
            .ToList();
    }

    public List<Product> GetProducts(User currentUser, ProductListRequest request)
    {
        IEnumerable<Product> query = _products;

        if (CanSearchFilterAndSort(currentUser))
        {
            if (!string.IsNullOrWhiteSpace(request.SearchText))
            {
                query = query.Where(product =>
                    Contains(product.Name, request.SearchText)
                    || Contains(product.Category, request.SearchText)
                    || Contains(product.Description, request.SearchText)
                    || Contains(product.Manufacturer, request.SearchText)
                    || Contains(product.Supplier, request.SearchText)
                    || Contains(product.Unit, request.SearchText));
            }

            if (!string.IsNullOrWhiteSpace(request.SupplierFilter)
                && request.SupplierFilter != AllSuppliers)
            {
                query = query.Where(product => product.Supplier == request.SupplierFilter);
            }

            query = request.SortMode switch
            {
                ProductSortMode.StockAscending => query.OrderBy(product => product.StockQuantity),
                ProductSortMode.StockDescending => query.OrderByDescending(product => product.StockQuantity),
                _ => query.OrderBy(product => product.Id)
            };
        }

        return query.ToList();
    }

    public Product? GetProductById(int id)
    {
        return _products.FirstOrDefault(product => product.Id == id);
    }

    public List<string> GetCategories()
    {
        return _products
            .Select(product => product.Category)
            .Distinct()
            .OrderBy(value => value)
            .ToList();
    }

    public List<string> GetManufacturers()
    {
        return _products
            .Select(product => product.Manufacturer)
            .Distinct()
            .OrderBy(value => value)
            .ToList();
    }

    public List<string> GetSuppliers()
    {
        return _products
            .Select(product => product.Supplier)
            .Distinct()
            .OrderBy(value => value)
            .Prepend(AllSuppliers)
            .ToList();
    }

    public void AddProduct(User currentUser, Product product)
    {
        CheckAdmin(currentUser);
        ValidateProduct(product);

        product.Id = _products.Count == 0 ? 1 : _products.Max(item => item.Id) + 1;
        product.ImagePath = NormalizeImagePath(product.ImagePath);
        _products.Add(product);
    }

    public void UpdateProduct(User currentUser, Product updatedProduct)
    {
        CheckAdmin(currentUser);
        ValidateProduct(updatedProduct);

        Product product = GetProductById(updatedProduct.Id)
            ?? throw new InvalidOperationException("Товар не найден.");

        product.Name = updatedProduct.Name;
        product.Category = updatedProduct.Category;
        product.Description = updatedProduct.Description;
        product.Manufacturer = updatedProduct.Manufacturer;
        product.Supplier = updatedProduct.Supplier;
        product.Price = updatedProduct.Price;
        product.Unit = updatedProduct.Unit;
        product.StockQuantity = updatedProduct.StockQuantity;
        product.DiscountPercent = updatedProduct.DiscountPercent;
        product.ImagePath = NormalizeImagePath(updatedProduct.ImagePath);
    }

    public void DeleteProduct(User currentUser, int productId)
    {
        CheckAdmin(currentUser);

        if (_orders.Any(order => order.ProductId == productId))
        {
            throw new InvalidOperationException("Нельзя удалить товар, который присутствует в заказе.");
        }

        Product product = GetProductById(productId)
            ?? throw new InvalidOperationException("Товар не найден.");

        _products.Remove(product);
    }

    public List<Order> GetOrders(User currentUser)
    {
        CheckManagerOrAdmin(currentUser);
        return _orders.OrderByDescending(order => order.OrderDate).ToList();
    }

    public Order? GetOrderById(int id)
    {
        return _orders.FirstOrDefault(order => order.Id == id);
    }

    public List<string> GetOrderStatuses()
    {
        return ["Новый", "В обработке", "Готов к выдаче", "Выдан", "Отменен"];
    }

    public void AddOrder(User currentUser, Order order)
    {
        CheckAdmin(currentUser);
        ValidateOrder(order);

        order.Id = _orders.Count == 0 ? 1 : _orders.Max(item => item.Id) + 1;
        _orders.Add(order);
    }

    public void UpdateOrder(User currentUser, Order updatedOrder)
    {
        CheckAdmin(currentUser);
        ValidateOrder(updatedOrder);

        Order order = GetOrderById(updatedOrder.Id)
            ?? throw new InvalidOperationException("Заказ не найден.");

        order.Article = updatedOrder.Article;
        order.Status = updatedOrder.Status;
        order.PickupAddress = updatedOrder.PickupAddress;
        order.OrderDate = updatedOrder.OrderDate;
        order.DeliveryDate = updatedOrder.DeliveryDate;
        order.ProductId = updatedOrder.ProductId;
    }

    public void DeleteOrder(User currentUser, int orderId)
    {
        CheckAdmin(currentUser);

        Order order = GetOrderById(orderId)
            ?? throw new InvalidOperationException("Заказ не найден.");

        _orders.Remove(order);
    }

    public static bool CanSearchFilterAndSort(User currentUser)
    {
        return currentUser.Role is UserRole.Manager or UserRole.Admin;
    }

    public static bool CanEditData(User currentUser)
    {
        return currentUser.Role == UserRole.Admin;
    }

    public static ProductRowViewModel ToProductRow(Product product)
    {
        string backgroundColor = product.IsOutOfStock
            ? "LightBlue"
            : product.HasBigDiscount
                ? "#2E8B57"
                : "Transparent";

        return new ProductRowViewModel
        {
            Product = product,
            BackgroundColor = backgroundColor,
            PriceText = $"{product.Price:0.00} руб.",
            FinalPriceText = $"{product.FinalPrice:0.00} руб.",
            ShowOldPrice = product.HasDiscount
        };
    }

    private static bool Contains(string source, string searchText)
    {
        return source.Contains(searchText, StringComparison.OrdinalIgnoreCase);
    }

    private static string NormalizeImagePath(string? imagePath)
    {
        return string.IsNullOrWhiteSpace(imagePath) ? PlaceholderImage : imagePath;
    }

    private static void CheckManagerOrAdmin(User currentUser)
    {
        if (currentUser.Role is not (UserRole.Manager or UserRole.Admin))
        {
            throw new UnauthorizedAccessException("Доступ разрешен только менеджеру и администратору.");
        }
    }

    private static void CheckAdmin(User currentUser)
    {
        if (!CanEditData(currentUser))
        {
            throw new UnauthorizedAccessException("Операция доступна только администратору.");
        }
    }

    private static void ValidateProduct(Product product)
    {
        if (string.IsNullOrWhiteSpace(product.Name))
            throw new ArgumentException("Введите наименование товара.");

        if (string.IsNullOrWhiteSpace(product.Category))
            throw new ArgumentException("Выберите категорию товара.");

        if (string.IsNullOrWhiteSpace(product.Manufacturer))
            throw new ArgumentException("Выберите производителя.");

        if (string.IsNullOrWhiteSpace(product.Supplier))
            throw new ArgumentException("Введите поставщика.");

        if (product.Price < 0)
            throw new ArgumentException("Стоимость товара не может быть отрицательной.");

        if (product.StockQuantity < 0)
            throw new ArgumentException("Количество товара не может быть отрицательным.");

        if (product.DiscountPercent is < 0 or > 100)
            throw new ArgumentException("Скидка должна быть от 0 до 100.");
    }

    private void ValidateOrder(Order order)
    {
        if (string.IsNullOrWhiteSpace(order.Article))
            throw new ArgumentException("Введите артикул заказа.");

        if (string.IsNullOrWhiteSpace(order.Status))
            throw new ArgumentException("Выберите статус заказа.");

        if (string.IsNullOrWhiteSpace(order.PickupAddress))
            throw new ArgumentException("Введите адрес пункта выдачи.");

        if (_products.All(product => product.Id != order.ProductId))
            throw new ArgumentException("Выбранный товар не найден.");
    }
}

public static class ImageStorage
{
    // Для WinForms/WPF здесь можно добавить уменьшение изображения до 300x200.
    // Для ASP.NET MVC аналогичная логика обычно живет в сервисе загрузки файлов.
    public static string SaveProductImage(string sourceFilePath, string productImagesFolder)
    {
        if (!File.Exists(sourceFilePath))
            throw new FileNotFoundException("Файл изображения не найден.", sourceFilePath);

        Directory.CreateDirectory(productImagesFolder);

        string extension = Path.GetExtension(sourceFilePath);
        string fileName = $"{Guid.NewGuid():N}{extension}";
        string destinationPath = Path.Combine(productImagesFolder, fileName);

        File.Copy(sourceFilePath, destinationPath, overwrite: false);
        return destinationPath;
    }

    public static void DeleteOldProductImage(string imagePath)
    {
        if (string.IsNullOrWhiteSpace(imagePath)
            || imagePath == ExamRepository.PlaceholderImage
            || !File.Exists(imagePath))
        {
            return;
        }

        File.Delete(imagePath);
    }
}

public static class Program
{
    public static void Main()
    {
        var repository = new ExamRepository();

        User currentUser = repository.GetCurrentUserOrGuest(login: "admin", password: "123");
        Console.WriteLine($"Пользователь: {currentUser.FullName}, роль: {currentUser.Role}");

        var request = new ProductListRequest
        {
            SearchText = "",
            SupplierFilter = ExamRepository.AllSuppliers,
            SortMode = ProductSortMode.StockDescending
        };

        foreach (ProductRowViewModel row in repository.GetProductRows(currentUser, request))
        {
            Product product = row.Product;
            Console.WriteLine(
                $"{product.Name} | остаток: {product.StockQuantity} | цена: {row.FinalPriceText} | фон: {row.BackgroundColor}");
        }

        Console.WriteLine();
        Console.WriteLine("Поля формы товара: фото, название, категория, описание, производитель, поставщик, цена, единица, остаток, скидка.");
        Console.WriteLine("Поля формы заказа: артикул, статус, адрес пункта выдачи, дата заказа, дата выдачи.");
    }
}
