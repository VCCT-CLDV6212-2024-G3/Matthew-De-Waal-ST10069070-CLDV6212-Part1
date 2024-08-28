using System.Collections.Specialized;

namespace ABCRetail.Models
{
    public static class Resources
    {
        public static string[] HomepageImages
        {
            get
            {
                StringCollection resources = DataStorage.MultimediaStorage?.RecieveData("/homepage-images/");
                string[] result = new string[resources.Count];

                for(int i = 0; i < resources.Count; i++)
                    result[i] = DataStorage.MultimediaStorage.GetFullPath(resources[i]);

                return result;
            }
        }

        public static string[] ProductCategoryImages
        {
            get
            {
                StringCollection resources = DataStorage.ProductImageStorage.RecieveData("category-");
                string[] result = new string[resources.Count];

                for (int i = 0; i < resources.Count; i++)
                    result[i] = DataStorage.ProductImageStorage.GetFullPath(resources[i]);

                return result;
            }
        }

        public static string? AboutUsImage
        {
            get
            {
                string resource = DataStorage.MultimediaStorage?.RecieveData("/about-us-images/about-us-image.jpg")[0];
                return DataStorage.MultimediaStorage.GetFullPath(resource);
            }
        }

        public static string[] ProductCategories => new string[] { "Chairs", "Couches", "Stoves", "Tables", "Televisions", "Washing Machines" };
    }
}
