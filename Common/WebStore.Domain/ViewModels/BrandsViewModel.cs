namespace WebStore.Domain.ViewModels
{
    public record BrandsViewModel
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public int ProductCount { get; init; }
    }
}
