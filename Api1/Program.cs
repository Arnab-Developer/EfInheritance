namespace EntryPoint
{
    using Application;
    using Infra;

    public class Start
    {
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var constr = builder.Configuration.GetConnectionString("Api1Db");
            builder.Services.AddSqlServer<Api1Context>(constr);
            builder.Services.AddTransient<IUseCase, UseCase>();

            var app = builder.Build();

            app.MapGet("/create", async (IUseCase useCase) => await useCase.DoWorkCreate());

            app.MapGet("/get-sound", async (IUseCase useCase) =>
            {
                var catSound = await useCase.DoWorkCat();
                var dogSound = await useCase.DoWorkDog();
                var sound = new Sound(catSound, dogSound);
                return sound;
            });

            app.Run();
        }
    }
}

namespace Application
{
    using Infra;
    using Microsoft.EntityFrameworkCore;
    using Model;

    public interface IUseCase
    {
        public Task DoWorkCreate();

        public Task<string> DoWorkCat();

        public Task<string> DoWorkDog();
    }

    public class UseCase : IUseCase
    {
        private readonly Api1Context _api1Context;

        public UseCase(Api1Context api1Context) => _api1Context = api1Context;

        async Task IUseCase.DoWorkCreate()
        {
            var cat = new Cat() { Name = "Cat2", CatData = "cat data 1" };
            var dog = new Dog() { Name = "Dog2" };

            _api1Context.Cats.Add(cat);
            _api1Context.Dogs.Add(dog);

            await _api1Context.SaveChangesAsync();
        }

        async Task<string> IUseCase.DoWorkCat()
        {
            var cats = _api1Context.Cats;
            var dogs = _api1Context.Dogs;

            foreach (var c in cats) { GetSound(c); }
            foreach (var d in dogs) { GetSound(d); }

            var cat = await _api1Context.Cats.SingleAsync(c => c.Id == 7);
            var sound = GetSound(cat);
            return sound;
        }

        async Task<string> IUseCase.DoWorkDog()
        {
            var dog = await _api1Context.Dogs.SingleAsync(c => c.Id == 8);
            var sound = GetSound(dog);
            return sound;
        }

        private static string GetSound(Animal animal) => animal.GetSound();
    }

    public record class Sound(string Cat, string Dog);
}

namespace Model
{
    public abstract class Animal
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Animal() => Name = string.Empty;

        public abstract string GetSound();
    }

    public class Cat : Animal
    {
        public string CatData { get; set; }

        public Cat() => CatData = string.Empty;

        public override string GetSound() => $"'{Id}' '{Name}' {nameof(Cat)} sound";
    }

    public class Dog : Animal
    {
        public override string GetSound() => $"'{Id}' '{Name}' {nameof(Dog)} sound";
    }
}

namespace Infra
{
    using Microsoft.EntityFrameworkCore;
    using Model;

    public class Api1Context : DbContext
    {
        public DbSet<Cat> Cats { get; set; }

        public DbSet<Dog> Dogs { get; set; }

        public Api1Context(DbContextOptions<Api1Context> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Animal>()
                .HasDiscriminator<string>("AnimalType")
                .HasValue<Cat>("Cat")
                .HasValue<Dog>("Dog");
        }
    }
}