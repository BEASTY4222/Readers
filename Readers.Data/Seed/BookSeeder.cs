    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Readers.Data;
    using Readers.Data.DataModels;

    namespace Readers.Data.Seed
    {
        public static class BookSeeder
        {
            private const string AdminEmail = "admin@readers.local";
            private const string AdminPassword = "ReadersAdmin2026!";
            private const string AdminRole = "Admin";

            public static async Task SeedAsync(
                ApplicationDbContext context,
                UserManager<ApplicationUser> userManager,
                RoleManager<IdentityRole> roleManager)
            {
                if (!await roleManager.RoleExistsAsync(AdminRole))
                {
                    var roleResult = await roleManager.CreateAsync(new IdentityRole(AdminRole));
                    if (!roleResult.Succeeded)
                        throw new InvalidOperationException($"Could not create Admin role: {string.Join(", ", roleResult.Errors.Select(error => error.Description))}");
                }

                var admin = await userManager.FindByEmailAsync(AdminEmail);
                if (admin == null)
                {
                    admin = new ApplicationUser
                    {
                        UserName = AdminEmail,
                        Email = AdminEmail,
                        EmailConfirmed = true,
                        UserTypedName = "admin"
                    };

                    var userResult = await userManager.CreateAsync(admin, AdminPassword);
                    if (!userResult.Succeeded)
                        throw new InvalidOperationException($"Could not create admin user: {string.Join(", ", userResult.Errors.Select(error => error.Description))}");
                }

                if (!await userManager.IsInRoleAsync(admin, AdminRole))
                {
                    var roleResult = await userManager.AddToRoleAsync(admin, AdminRole);
                    if (!roleResult.Succeeded)
                        throw new InvalidOperationException($"Could not assign Admin role: {string.Join(", ", roleResult.Errors.Select(error => error.Description))}");
                }

                if (await context.Books.AnyAsync())
                    return;

                // ---------- AUTHORS ----------
                var martin = new Author { Name = "George R.R. Martin", Country = "United States" ,ImageUrl = "https://pub-ca6d55b9471c427cadc570b86113a1a6.r2.dev/Authors/GeorgeRRMartin.webp" };
                var sanderson = new Author { Name = "Brandon Sanderson", Country = "United States" ,ImageUrl = "https://pub-ca6d55b9471c427cadc570b86113a1a6.r2.dev/Authors/BrandonSanderson.webp" };
                var orwell = new Author { Name = "George Orwell", Country = "United Kingdom" ,ImageUrl = "https://pub-ca6d55b9471c427cadc570b86113a1a6.r2.dev/Authors/GeorgeOrwell.webp" };
                var dostoevsky = new Author { Name = "Fyodor Dostoevsky", Country = "Russia" ,ImageUrl = "https://pub-ca6d55b9471c427cadc570b86113a1a6.r2.dev/Authors/FyodorDostoevsky.webp" };
                var kafka = new Author { Name = "Franz Kafka", Country = "Austria-Hungary" ,ImageUrl = "https://pub-ca6d55b9471c427cadc570b86113a1a6.r2.dev/Authors/FranzKafka.webp" };
                var kuang = new Author { Name = "R.F. Kuang", Country = "United States" ,ImageUrl = "https://pub-ca6d55b9471c427cadc570b86113a1a6.r2.dev/Authors/RFKuang.webp" };
                var remarque = new Author { Name = "Erich Maria Remarque", Country = "Germany" ,ImageUrl = "https://pub-ca6d55b9471c427cadc570b86113a1a6.r2.dev/Authors/ErichMariaRemarque.webp" };
                var dumas = new Author { Name = "Alexandre Dumas", Country = "France" ,ImageUrl = "https://pub-ca6d55b9471c427cadc570b86113a1a6.r2.dev/Authors/AlexandreDumas.webp" };
                var zusak = new Author { Name = "Markus Zusak", Country = "Australia" ,ImageUrl = "https://pub-ca6d55b9471c427cadc570b86113a1a6.r2.dev/Authors/MarkusZusak.webp" };
                var steinbeck = new Author { Name = "John Steinbeck", Country = "United States" ,ImageUrl = "https://pub-ca6d55b9471c427cadc570b86113a1a6.r2.dev/Authors/JohnSteinbeck.webp" };
                var wilde = new Author { Name = "Oscar Wilde", Country = "Ireland" ,ImageUrl = "https://pub-ca6d55b9471c427cadc570b86113a1a6.r2.dev/Authors/OscarWilde.webp" };
                var bulgakov = new Author { Name = "Mikhail Bulgakov", Country = "Russia" ,ImageUrl = "https://pub-ca6d55b9471c427cadc570b86113a1a6.r2.dev/Authors/MikhailBulgakov.webp" };


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
                    new Book { Title = "A Game of Thrones",     Author = martin,    Publisher = bantam,     Genre = "Fantasy", YearPublished = 1996, Pages = 694,  CoverImagePath = "https://pub-ca6d55b9471c427cadc570b86113a1a6.r2.dev/Books/AGameOfThrones.webp" },
                    new Book { Title = "A Clash of Kings",      Author = martin,    Publisher = bantam,     Genre = "Fantasy", YearPublished = 1998, Pages = 768,  CoverImagePath = "https://pub-ca6d55b9471c427cadc570b86113a1a6.r2.dev/Books/AClashOfKings.webp" },
                    new Book { Title = "A Storm of Swords",     Author = martin,    Publisher = bantam,     Genre = "Fantasy", YearPublished = 2000, Pages = 973,  CoverImagePath = "https://pub-ca6d55b9471c427cadc570b86113a1a6.r2.dev/Books/AStormOfSwords.webp" },
                    new Book { Title = "A Feast for Crows",     Author = martin,    Publisher = bantam,     Genre = "Fantasy", YearPublished = 2005, Pages = 753,  CoverImagePath = "https://pub-ca6d55b9471c427cadc570b86113a1a6.r2.dev/Books/AFeastForCrows.webp" },
                    new Book { Title = "A Dance with Dragons",  Author = martin,    Publisher = bantam,     Genre = "Fantasy", YearPublished = 2011, Pages = 1016, CoverImagePath = "https://pub-ca6d55b9471c427cadc570b86113a1a6.r2.dev/Books/ADanceWithDragonsOneDreamsAndDust.webp" },

                    // ===== Brandon Sanderson — Mistborn + Stormlight =====
                    new Book { Title = "The Final Empire",      Author = sanderson, Publisher = tor,        Genre = "Fantasy", YearPublished = 2006, Pages = 541,  CoverImagePath = "https://pub-ca6d55b9471c427cadc570b86113a1a6.r2.dev/Books/TheFinalEmpire.webp" },
                    new Book { Title = "The Well of Ascension", Author = sanderson, Publisher = tor,        Genre = "Fantasy", YearPublished = 2007, Pages = 590,  CoverImagePath = "https://pub-ca6d55b9471c427cadc570b86113a1a6.r2.dev/Books/TheWellOfAscension.webp" },
                    new Book { Title = "The Hero of Ages",      Author = sanderson, Publisher = tor,        Genre = "Fantasy", YearPublished = 2008, Pages = 572,  CoverImagePath = "https://pub-ca6d55b9471c427cadc570b86113a1a6.r2.dev/Books/TheHeroOfAges.webp" },
                    new Book { Title = "The Way of Kings",      Author = sanderson, Publisher = tor,        Genre = "Fantasy", YearPublished = 2010, Pages = 1007, CoverImagePath = "https://pub-ca6d55b9471c427cadc570b86113a1a6.r2.dev/Books/TheWayOfKings.webp" },
                    new Book { Title = "Words of Radiance",     Author = sanderson, Publisher = tor,        Genre = "Fantasy", YearPublished = 2014, Pages = 1087, CoverImagePath = "https://pub-ca6d55b9471c427cadc570b86113a1a6.r2.dev/Books/WordsOfRadiance.webp" },
                    new Book { Title = "Oathbringer",           Author = sanderson, Publisher = tor,        Genre = "Fantasy", YearPublished = 2017, Pages = 1248, CoverImagePath = "https://pub-ca6d55b9471c427cadc570b86113a1a6.r2.dev/Books/Oathbringer.webp" },
                    new Book { Title = "Rhythm of War",      Author = sanderson, Publisher = tor,        Genre = "Fantasy", YearPublished = 2020, Pages = 1232, CoverImagePath = "https://pub-ca6d55b9471c427cadc570b86113a1a6.r2.dev/Books/RhythmOfWar.webp" },
                    new Book { Title = "Wind and Truth",          Author = sanderson, Publisher = tor,        Genre = "Fantasy", YearPublished = 2020, Pages = 1232, CoverImagePath = "https://pub-ca6d55b9471c427cadc570b86113a1a6.r2.dev/Books/WindAndTruth.webp" },
                   
                    // ===== George Orwell =====
                    new Book { Title = "Animal Farm",           Author = orwell,    Publisher = seckerWarburg, Genre = "Political Satire", YearPublished = 1945, Pages = 112, CoverImagePath = "https://pub-ca6d55b9471c427cadc570b86113a1a6.r2.dev/Books/AnimalFarm.webp" },

                    // ===== Fyodor Dostoevsky =====
                    new Book { Title = "Crime and Punishment",  Author = dostoevsky, Publisher = theRussianMessenger, Genre = "Psychological Fiction", YearPublished = 1866, Pages = 671, CoverImagePath = "https://pub-ca6d55b9471c427cadc570b86113a1a6.r2.dev/Books/CrimeAndPunishment.webp" },
                    new Book { Title = "The Idiot",             Author = dostoevsky, Publisher = theRussianMessenger, Genre = "Philosophical Fiction", YearPublished = 1869, Pages = 656, CoverImagePath = "https://pub-ca6d55b9471c427cadc570b86113a1a6.r2.dev/Books/TheIdiot.webp" },
                    new Book { Title = "The Brothers Karamazov", Author = dostoevsky, Publisher = theRussianMessenger, Genre = "Philosophical Fiction", YearPublished = 1880, Pages = 824, CoverImagePath = "https://pub-ca6d55b9471c427cadc570b86113a1a6.r2.dev/Books/TheBrothersKaramazov.webp" },
                    new Book { Title = "White Nights",          Author = dostoevsky, Publisher = otechestvennyeZapiski, Genre = "Short Story", YearPublished = 1848, Pages = 96, CoverImagePath = "https://pub-ca6d55b9471c427cadc570b86113a1a6.r2.dev/Books/TheWhiteNights.webp" },
                    // ===== Franz Kafka =====
                    new Book { Title = "The Trial",             Author = kafka,     Publisher = verlagDieSchmiede, Genre = "Absurdist", YearPublished = 1925, Pages = 255, CoverImagePath = "https://pub-ca6d55b9471c427cadc570b86113a1a6.r2.dev/Books/TheTrial.webp" },
                    new Book { Title = "The Metamorphosis",     Author = kafka,     Publisher = kurtWolff, Genre = "Absurdist", YearPublished = 1915, Pages = 96, CoverImagePath = "https://pub-ca6d55b9471c427cadc570b86113a1a6.r2.dev/Books/TheMetamorphosis.webp" },

                    // ===== R.F. Kuang — The Poppy War trilogy + Katabasis =====
                    new Book { Title = "The Poppy War",         Author = kuang,     Publisher = harperVoyager, Genre = "Fantasy", YearPublished = 2018, Pages = 544, CoverImagePath = "https://pub-ca6d55b9471c427cadc570b86113a1a6.r2.dev/Books/ThePoppyWar.webp" },

                    new Book { Title = "The Dragon Republic",   Author = kuang,     Publisher = harperVoyager, Genre = "Fantasy", YearPublished = 2019, Pages = 656, CoverImagePath = "https://pub-ca6d55b9471c427cadc570b86113a1a6.r2.dev/Books/TheDragonRepublic.webp" },
                    new Book { Title = "The Burning God",       Author = kuang,     Publisher = harperVoyager, Genre = "Fantasy", YearPublished = 2020, Pages = 622, CoverImagePath = "https://pub-ca6d55b9471c427cadc570b86113a1a6.r2.dev/Books/TheBurningGod.webp" },
                    new Book { Title = "Katabasis",             Author = kuang,     Publisher = harperVoyager, Genre = "Fantasy", YearPublished = 2025, Pages = 400, CoverImagePath = "https://pub-ca6d55b9471c427cadc570b86113a1a6.r2.dev/Books/Katabasis.webp" },

                    // ===== Others =====
                    new Book { Title = "All Quiet on the Western Front", Author = remarque, Publisher = propylaenVerlag, Genre = "War Fiction", YearPublished = 1929, Pages = 296, CoverImagePath = "https://pub-ca6d55b9471c427cadc570b86113a1a6.r2.dev/Books/AllQuietOnTheWesternFront.webp" },
                    new Book { Title = "The Count of Monte Cristo",      Author = dumas,    Publisher = peition, Genre = "Adventure", YearPublished = 1844, Pages = 1276, CoverImagePath = "https://pub-ca6d55b9471c427cadc570b86113a1a6.r2.dev/Books/TheCountOfMonteCristo.webp" },
                    new Book { Title = "The Book Thief",                 Author = zusak,    Publisher = picador, Genre = "Historical Fiction", YearPublished = 2005, Pages = 584, CoverImagePath = "https://pub-ca6d55b9471c427cadc570b86113a1a6.r2.dev/Books/TheBookThief.webp" },
                    new Book { Title = "Of Mice and Men",                Author = steinbeck, Publisher = coviciFriede, Genre = "Classic", YearPublished = 1937, Pages = 107, CoverImagePath = "https://pub-ca6d55b9471c427cadc570b86113a1a6.r2.dev/Books/OfMiceAndMen.webp" },
                    new Book { Title = "The Picture of Dorian Gray",     Author = wilde,    Publisher = lippincottsMonthlyMagazine, Genre = "Gothic", YearPublished = 1890, Pages = 254, CoverImagePath = "https://pub-ca6d55b9471c427cadc570b86113a1a6.r2.dev/Books/ThePictureOfDorianGray.webp" },
                    new Book { Title = "The Master and Margarita",       Author = bulgakov, Publisher = ymcaPress, Genre = "Satire", YearPublished = 1967, Pages = 384, CoverImagePath = "https://pub-ca6d55b9471c427cadc570b86113a1a6.r2.dev/Books/TheMasterAndMargarita.webp" },
                };

                context.Books.AddRange(books);

                await context.SaveChangesAsync();
            }
        }
    }