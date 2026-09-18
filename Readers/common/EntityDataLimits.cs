using System;
using System.Security.Cryptography.X509Certificates;

namespace Common
{
    // This class contains the limits for the data of the entities in the application.
    // It is used to validate the data before it is saved to the database.
    public class EntityDataLimits
    {
        /*Book Begin*/
        public const int TitleMinLenght = 1;
        public const int TitleMaxLength = 100;

        public const int GanreMinLenght = 1;
        public const int GanreMaxLength = 100;

        public const int AuthorMinLenght = 1;
        public const int AuthorMaxLength = 100;

        public const int YearPublishedMinLenght = 1600;
        public const int YearPublishedMaxLength = 2026;

        public const int PublishingHouseMinLenght = 1;
        public const int PublishingHouseMaxLength = 100;

        public const int PagesMinLenght = 50;
        public const int PagesMaxLength = 4215;
        /*Book End*/
    }
}
