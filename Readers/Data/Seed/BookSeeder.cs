using Microsoft.EntityFrameworkCore;
using Readers.Data.DataModels;

namespace Readers.Data.Seed
{
    public static class BookSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (await context.Books.AnyAsync())
                return; // already seeded

            // ---------- AUTHORS ----------
            var martin = new Author { Name = "George R.R. Martin", Country = "United States" };
            var sanderson = new Author { Name = "Brandon Sanderson", Country = "United States" };
            var orwell = new Author { Name = "George Orwell", Country = "United Kingdom" };
            var dostoevsky = new Author { Name = "Fyodor Dostoevsky", Country = "Russia" };
            var kafka = new Author { Name = "Franz Kafka", Country = "Austria-Hungary" };
            var kuang = new Author { Name = "R.F. Kuang", Country = "United States" };
            var remarque = new Author { Name = "Erich Maria Remarque", Country = "Germany" };
            var dumas = new Author { Name = "Alexandre Dumas", Country = "France" };
            var zusak = new Author { Name = "Markus Zusak", Country = "Australia" };
            var steinbeck = new Author { Name = "John Steinbeck", Country = "United States" };
            var wilde = new Author { Name = "Oscar Wilde", Country = "Ireland" };
            var bulgakov = new Author { Name = "Mikhail Bulgakov", Country = "Russia" };

            // ---------- BOOKS ----------
            var books = new List<Book>
            {
                // ===== George R.R. Martin — A Song of Ice and Fire =====
                new Book { Title = "A Game of Thrones",     Author = martin,    PublishingHouse = "Bantam",     Ganre = "Fantasy", YearPublished = 1996, Pages = 694,  CoverImagePath = "/BookCovers/AGameOfThrones.webp" },
                new Book { Title = "A Clash of Kings",      Author = martin,    PublishingHouse = "Bantam",     Ganre = "Fantasy", YearPublished = 1998, Pages = 768,  CoverImagePath = "/BookCovers/AClashOfKings.webp" },
                new Book { Title = "A Storm of Swords",     Author = martin,    PublishingHouse = "Bantam",     Ganre = "Fantasy", YearPublished = 2000, Pages = 973,  CoverImagePath = "/BookCovers/AStormOfSwords.webp" },
                new Book { Title = "A Feast for Crows",     Author = martin,    PublishingHouse = "Bantam",     Ganre = "Fantasy", YearPublished = 2005, Pages = 753,  CoverImagePath = "/BookCovers/AFeastForCrows.webp" },
                new Book { Title = "A Dance with Dragons",  Author = martin,    PublishingHouse = "Bantam",     Ganre = "Fantasy", YearPublished = 2011, Pages = 1016, CoverImagePath = "/BookCovers/ADanceWithDragonsOneDreamsAndDust.webp" },

                // ===== Brandon Sanderson — Mistborn + Stormlight =====
                new Book { Title = "The Final Empire",      Author = sanderson, PublishingHouse = "Tor",        Ganre = "Fantasy", YearPublished = 2006, Pages = 541,  CoverImagePath = "/BookCovers/TheFinalEmpire.webp" },
                new Book { Title = "The Well of Ascension", Author = sanderson, PublishingHouse = "Tor",        Ganre = "Fantasy", YearPublished = 2007, Pages = 590,  CoverImagePath = "/BookCovers/TheWellOfAscension.webp" },
                new Book { Title = "The Hero of Ages",      Author = sanderson, PublishingHouse = "Tor",        Ganre = "Fantasy", YearPublished = 2008, Pages = 572,  CoverImagePath = "/BookCovers/TheHeroOfAges.webp" },
                new Book { Title = "The Way of Kings",      Author = sanderson, PublishingHouse = "Tor",        Ganre = "Fantasy", YearPublished = 2010, Pages = 1007, CoverImagePath = "/BookCovers/TheWayOfKings.webp" },
                new Book { Title = "Words of Radiance",     Author = sanderson, PublishingHouse = "Tor",        Ganre = "Fantasy", YearPublished = 2014, Pages = 1087, CoverImagePath = "/BookCovers/WordsOfRadiance.webp" },
                new Book { Title = "Oathbringer",           Author = sanderson, PublishingHouse = "Tor",        Ganre = "Fantasy", YearPublished = 2017, Pages = 1248, CoverImagePath = "/BookCovers/Oathbringer.webp" },
                new Book { Title = "Rhythm of War",         Author = sanderson, PublishingHouse = "Tor",        Ganre = "Fantasy", YearPublished = 2020, Pages = 1232, CoverImagePath = "/BookCovers/TheRythmOfWar.webp" },
                new Book { Title = "Wind and Truth",        Author = sanderson, PublishingHouse = "Tor",        Ganre = "Fantasy", YearPublished = 2024, Pages = 1344, CoverImagePath = "/BookCovers/WindAndTruth.webp" },

                // ===== George Orwell =====
                new Book { Title = "Animal Farm",           Author = orwell,    PublishingHouse = "Secker & Warburg", Ganre = "Political Satire", YearPublished = 1945, Pages = 112, CoverImagePath = "/BookCovers/AnimalFarm.webp" },

                // ===== Fyodor Dostoevsky =====
                new Book { Title = "Crime and Punishment",  Author = dostoevsky, PublishingHouse = "The Russian Messenger", Ganre = "Psychological Fiction", YearPublished = 1866, Pages = 671, CoverImagePath = "/BookCovers/CrimeAndPunishment.webp" },
                new Book { Title = "The Idiot",             Author = dostoevsky, PublishingHouse = "The Russian Messenger", Ganre = "Philosophical Fiction", YearPublished = 1869, Pages = 656, CoverImagePath = "/BookCovers/TheIdiot.webp" },
                new Book { Title = "The Brothers Karamazov", Author = dostoevsky, PublishingHouse = "The Russian Messenger", Ganre = "Philosophical Fiction", YearPublished = 1880, Pages = 824, CoverImagePath = "/BookCovers/TheBrothersKaramazov.webp" },
                new Book { Title = "White Nights",          Author = dostoevsky, PublishingHouse = "Otechestvennye zapiski", Ganre = "Short Story", YearPublished = 1848, Pages = 96, CoverImagePath = "/BookCovers/TheWhiteNights.webp" },

                // ===== Franz Kafka =====
                new Book { Title = "The Trial",             Author = kafka,     PublishingHouse = "Verlag Die Schmiede", Ganre = "Absurdist", YearPublished = 1925, Pages = 255, CoverImagePath = "/BookCovers/TheTrial.webp" },
                new Book { Title = "The Metamorphosis",     Author = kafka,     PublishingHouse = "Kurt Wolff", Ganre = "Absurdist", YearPublished = 1915, Pages = 96, CoverImagePath = "/BookCovers/TheMetamorphosis.webp" },

                // ===== R.F. Kuang — The Poppy War trilogy + Katabasis =====
                new Book { Title = "The Poppy War",         Author = kuang,     PublishingHouse = "Harper Voyager", Ganre = "Fantasy", YearPublished = 2018, Pages = 527, CoverImagePath = "/BookCovers/ThePoppyWar.webp" },
                new Book { Title = "The Dragon Republic",   Author = kuang,     PublishingHouse = "Harper Voyager", Ganre = "Fantasy", YearPublished = 2019, Pages = 656, CoverImagePath = "/BookCovers/TheDragonRepublic.webp" },
                new Book { Title = "The Burning God",       Author = kuang,     PublishingHouse = "Harper Voyager", Ganre = "Fantasy", YearPublished = 2020, Pages = 622, CoverImagePath = "/BookCovers/TheBurningGod.webp" },
                new Book { Title = "Katabasis",             Author = kuang,     PublishingHouse = "Harper Voyager", Ganre = "Fantasy", YearPublished = 2025, Pages = 400, CoverImagePath = "/BookCovers/Katabasis.webp" },

                // ===== Others =====
                new Book { Title = "All Quiet on the Western Front", Author = remarque, PublishingHouse = "Propyläen Verlag", Ganre = "War Fiction", YearPublished = 1929, Pages = 296, CoverImagePath = "/BookCovers/AllQuietOnTheWesternFront.webp" },
                new Book { Title = "The Count of Monte Cristo",      Author = dumas,    PublishingHouse = "Pétion", Ganre = "Adventure", YearPublished = 1844, Pages = 1276, CoverImagePath = "/BookCovers/TheCountOfMonteCristo.webp" },
                new Book { Title = "The Book Thief",                 Author = zusak,    PublishingHouse = "Picador", Ganre = "Historical Fiction", YearPublished = 2005, Pages = 584, CoverImagePath = "/BookCovers/TheBookThief.webp" },
                new Book { Title = "Of Mice and Men",                Author = steinbeck, PublishingHouse = "Covici Friede", Ganre = "Classic", YearPublished = 1937, Pages = 107, CoverImagePath = "/BookCovers/OfMiceAndMen.webp" },
                new Book { Title = "The Picture of Dorian Gray",     Author = wilde,    PublishingHouse = "Lippincott's Monthly Magazine", Ganre = "Gothic", YearPublished = 1890, Pages = 254, CoverImagePath = "/BookCovers/ThePictureOfDorianGray.webp" },
                new Book { Title = "The Master and Margarita",       Author = bulgakov, PublishingHouse = "YMCA Press", Ganre = "Satire", YearPublished = 1967, Pages = 384, CoverImagePath = "/BookCovers/TheMasterAndMargarita.webp" },
            };

            context.Books.AddRange(books);
            await context.SaveChangesAsync();
        }
    }
}