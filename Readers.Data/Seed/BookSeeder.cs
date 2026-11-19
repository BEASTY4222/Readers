    using Microsoft.EntityFrameworkCore;
    using Readers.Data;
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


                // ---------- PUBLISHERS ----------
                var tor = new Publisher { Name = "Tor Books" };
                var bantam = new Publisher { Name = "Bantam Books" };
                var seckerWarburg = new Publisher { Name = "Secker & Warburg" };
                var propylaenVerlag = new Publisher { Name = "Propyläen Verlag" };
                var theRussianMessenger = new Publisher { Name = "The Russian Messenger" };
                var otechestvennyeZapiski = new Publisher { Name = "Otechestvennye Zapiski" };
                var verlagDieSchmiede = new Publisher { Name = "Verlag Die Schmiede" };
                var kurtWolff = new Publisher { Name = "Kurt Wolff Verlag" };
                var harperVoyager = new Publisher { Name = "Harper Voyager" };
                var peition = new Publisher { Name = "Pétion" };
                var picador = new Publisher { Name = "Picador" };
                var coviciFriede = new Publisher { Name = "Covici Friede" };
                var lippincottsMonthlyMagazine = new Publisher { Name = "Lippincott's Monthly Magazine" };
                var ymcaPress = new Publisher { Name = "YMCA Press" };
                var propyläenVerlag = new Publisher { Name = "Propyläen Verlag" };




                // ---------- BOOKS ----------
                var books = new List<Book>
                {
                    // ===== George R.R. Martin — A Song of Ice and Fire =====
                    new Book { Title = "A Game of Thrones",     Author = martin,    Publisher = bantam,     Genre = "Fantasy", YearPublished = 1996, Pages = 694,  CoverImagePath = "/BookCovers/AGameOfThrones.webp" },
                    new Book { Title = "A Clash of Kings",      Author = martin,    Publisher = bantam,     Genre = "Fantasy", YearPublished = 1998, Pages = 768,  CoverImagePath = "/BookCovers/AClashOfKings.webp" },
                    new Book { Title = "A Storm of Swords",     Author = martin,    Publisher = bantam,     Genre = "Fantasy", YearPublished = 2000, Pages = 973,  CoverImagePath = "/BookCovers/AStormOfSwords.webp" },
                    new Book { Title = "A Feast for Crows",     Author = martin,    Publisher = bantam,     Genre = "Fantasy", YearPublished = 2005, Pages = 753,  CoverImagePath = "/BookCovers/AFeastForCrows.webp" },
                    new Book { Title = "A Dance with Dragons",  Author = martin,    Publisher = bantam,     Genre = "Fantasy", YearPublished = 2011, Pages = 1016, CoverImagePath = "/BookCovers/ADanceWithDragonsOneDreamsAndDust.webp" },

                    // ===== Brandon Sanderson — Mistborn + Stormlight =====
                    new Book { Title = "The Final Empire",      Author = sanderson, Publisher = tor,        Genre = "Fantasy", YearPublished = 2006, Pages = 541,  CoverImagePath = "/BookCovers/TheFinalEmpire.webp" },
                    new Book { Title = "The Well of Ascension", Author = sanderson, Publisher = tor,        Genre = "Fantasy", YearPublished = 2007, Pages = 590,  CoverImagePath = "/BookCovers/TheWellOfAscension.webp" },
                    new Book { Title = "The Hero of Ages",      Author = sanderson, Publisher = tor,        Genre = "Fantasy", YearPublished = 2008, Pages = 572,  CoverImagePath = "/BookCovers/TheHeroOfAges.webp" },
                    new Book { Title = "The Way of Kings",      Author = sanderson, Publisher = tor,        Genre = "Fantasy", YearPublished = 2010, Pages = 1007, CoverImagePath = "/BookCovers/TheWayOfKings.webp" },
                    new Book { Title = "Words of Radiance",     Author = sanderson, Publisher = tor,        Genre = "Fantasy", YearPublished = 2014, Pages = 1087, CoverImagePath = "/BookCovers/WordsOfRadiance.webp" },
                    new Book { Title = "Oathbringer",           Author = sanderson, Publisher = tor,        Genre = "Fantasy", YearPublished = 2017, Pages = 1248, CoverImagePath = "/BookCovers/Oathbringer.webp" },
                    new Book { Title = "Rhythm of War",      Author = sanderson, Publisher = tor,        Genre = "Fantasy", YearPublished = 2020, Pages = 1232, CoverImagePath = "/BookCovers/RhythmOfWar.webp" },

                    // ===== George Orwell =====
                    new Book { Title = "Animal Farm",           Author = orwell,    Publisher = seckerWarburg, Genre = "Political Satire", YearPublished = 1945, Pages = 112, CoverImagePath = "/BookCovers/AnimalFarm.webp" },

                    // ===== Fyodor Dostoevsky =====
                    new Book { Title = "Crime and Punishment",  Author = dostoevsky, Publisher = theRussianMessenger, Genre = "Psychological Fiction", YearPublished = 1866, Pages = 671, CoverImagePath = "/BookCovers/CrimeAndPunishment.webp" },
                    new Book { Title = "The Idiot",             Author = dostoevsky, Publisher = theRussianMessenger, Genre = "Philosophical Fiction", YearPublished = 1869, Pages = 656, CoverImagePath = "/BookCovers/TheIdiot.webp" },
                    new Book { Title = "The Brothers Karamazov", Author = dostoevsky, Publisher = theRussianMessenger, Genre = "Philosophical Fiction", YearPublished = 1880, Pages = 824, CoverImagePath = "/BookCovers/TheBrothersKaramazov.webp" },
                    new Book { Title = "White Nights",          Author = dostoevsky, Publisher = otechestvennyeZapiski, Genre = "Short Story", YearPublished = 1848, Pages = 96, CoverImagePath = "/BookCovers/TheWhiteNights.webp" },
                    // ===== Franz Kafka =====
                    new Book { Title = "The Trial",             Author = kafka,     Publisher = verlagDieSchmiede, Genre = "Absurdist", YearPublished = 1925, Pages = 255, CoverImagePath = "/BookCovers/TheTrial.webp" },
                    new Book { Title = "The Metamorphosis",     Author = kafka,     Publisher = kurtWolff, Genre = "Absurdist", YearPublished = 1915, Pages = 96, CoverImagePath = "/BookCovers/TheMetamorphosis.webp" },

                    // ===== R.F. Kuang — The Poppy War trilogy + Katabasis =====
                    new Book { Title = "The Poppy War",         Author = kuang,     Publisher = harperVoyager, Genre = "Fantasy", YearPublished = 2018, Pages = 544, CoverImagePath = "/BookCovers/ThePoppyWar.webp" },

                    new Book { Title = "The Dragon Republic",   Author = kuang,     Publisher = harperVoyager, Genre = "Fantasy", YearPublished = 2019, Pages = 656, CoverImagePath = "/BookCovers/TheDragonRepublic.webp" },
                    new Book { Title = "The Burning God",       Author = kuang,     Publisher = harperVoyager, Genre = "Fantasy", YearPublished = 2020, Pages = 622, CoverImagePath = "/BookCovers/TheBurningGod.webp" },
                    new Book { Title = "Katabasis",             Author = kuang,     Publisher = harperVoyager, Genre = "Fantasy", YearPublished = 2025, Pages = 400, CoverImagePath = "/BookCovers/Katabasis.webp" },

                    // ===== Others =====
                    new Book { Title = "All Quiet on the Western Front", Author = remarque, Publisher = propylaenVerlag, Genre = "War Fiction", YearPublished = 1929, Pages = 296, CoverImagePath = "/BookCovers/AllQuietOnTheWesternFront.webp" },
                    new Book { Title = "The Count of Monte Cristo",      Author = dumas,    Publisher = peition, Genre = "Adventure", YearPublished = 1844, Pages = 1276, CoverImagePath = "/BookCovers/TheCountOfMonteCristo.webp" },
                    new Book { Title = "The Book Thief",                 Author = zusak,    Publisher = picador, Genre = "Historical Fiction", YearPublished = 2005, Pages = 584, CoverImagePath = "/BookCovers/TheBookThief.webp" },
                    new Book { Title = "Of Mice and Men",                Author = steinbeck, Publisher = coviciFriede, Genre = "Classic", YearPublished = 1937, Pages = 107, CoverImagePath = "/BookCovers/OfMiceAndMen.webp" },
                    new Book { Title = "The Picture of Dorian Gray",     Author = wilde,    Publisher = lippincottsMonthlyMagazine, Genre = "Gothic", YearPublished = 1890, Pages = 254, CoverImagePath = "/BookCovers/ThePictureOfDorianGray.webp" },
                    new Book { Title = "The Master and Margarita",       Author = bulgakov, Publisher = ymcaPress, Genre = "Satire", YearPublished = 1967, Pages = 384, CoverImagePath = "/BookCovers/TheMasterAndMargarita.webp" },
                };

                context.Books.AddRange(books);

                await context.SaveChangesAsync();
            }
        }
    }