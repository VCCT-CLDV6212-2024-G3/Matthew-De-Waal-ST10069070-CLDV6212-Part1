using System.Collections.Specialized;

namespace ABCRetail.Models
{
    public static class Resources
    {
        public static string[] HomepageImages
        {
            get
            {
                StringCollection resources = DataStorage.MultimediaStorage.RecieveData("/homepage-images/");
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
    }
}
