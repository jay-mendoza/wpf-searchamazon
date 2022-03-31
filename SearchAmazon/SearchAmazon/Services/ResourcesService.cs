//-----------------------------------------------------------------------
// <copyright file="ResourcesService.cs" company="Jay Bautista Mendoza">
//     Copyright (c) Jay Bautista Mendoza. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace SearchAmazon.Services
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Xml.Serialization;

    /// <summary>Service class for Favorites.</summary>
    public class ResourcesService
    {
        #region FIELDS │ PRIVATE │ NON-STATIC │ NON-READONLY

        /// <summary>Private instance of the xmlFile.</summary>
        private string xmlFile;

        #endregion

        #region CONSTRUCTORS │ PUBLIC │ NON-STATIC

        /// <summary>Initializes a new instance of the <see cref="ResourcesService" /> class.</summary>
        public ResourcesService()
        {
            this.xmlFile = Process.GetCurrentProcess().MainModule.FileName.Replace(".exe", ".xml");

            if (!File.Exists(this.xmlFile))
            {
                this.CreateBlankResourcesFile();
            }
        }

        #endregion

        #region PROPERTIES │ PUBLIC │ NON-STATIC
        
        /// <summary>Gets the XML file path and name based on the program's name.</summary>
        public string XmlFile
        {
            get
            {
                return this.xmlFile;
            }
        }

        #endregion

        #region METHODS │ PUBLIC │ NON-STATIC
        /// <summary>Create a blank resources file.</summary>
        public void CreateBlankResourcesFile()
        {
            Resources resources = new Resources();
            resources.AmazonUrl = "http://www.amazon.com";
            resources.AmazonUrlP = "http://www.amazon.com/s?{0}";
            resources.EligibleFormat = "rh={0}";
            ////resources.KeywordFormat = "keywords={0}";
            ////resources.BrandFormat = "field-brandtextbin={0}";
            resources.KeywordFormat = "k:{0}";
            resources.BrandFormat = "p_89:{0}";
            resources.PriceRangeFormat = "p_36:{0}-{1}"; /* in Cents. $5 = 500 */
            resources.PrimeEligible = "p_85:2470955011";
            resources.PrimeFreeOneDayEligible = "p_97:11292772011";
            resources.AmazonGlobalEligible = "p_n_shipping_option-bin:3242350011";
            resources.AmazonGlobalEligibleFree = $"{resources.AmazonGlobalEligible},p_n_is_free_international_shipping:10236242011";
            resources.NewCondition = "p_n_condition-type:6461716011";
            resources.UsedCondition = "p_n_condition-type:6461718011";
            resources.RefurbishedCondition = "p_n_condition-type:2224372011";

            resources.Brands = new List<Brand>();
            resources.Brands.Add(new Brand("amazon", "amazon", "Amazon"));
            resources.Brands.Add(new Brand("amazon-basics", "amazonbasics", "Amazon Basics"));
            resources.Brands.Add(new Brand("asus", "asus", "Asus"));

            this.SaveResources(resources);
        }

        /// <summary>Load the Resources object from the XML resources file.</summary>
        /// <returns>Resources object of the XML resources file.</returns>
        public Resources LoadResources()
        {
            using (Stream fileStream = new FileStream(this.xmlFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(Resources));
                Resources root = (Resources)deserializer.Deserialize(fileStream);
                return root;
            }
        }

        /// <summary>Save a Resources object to the XML resources file.</summary>
        /// <param name="root">Resources object for the XML resources file.</param>
        public void SaveResources(Resources root)
        {
            using (Stream fileStream = new FileStream(this.xmlFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Resources));
                serializer.Serialize(fileStream, root);
            }
        }

        /// <summary>Delete a new Brand favorite.</summary>
        /// <param name="brandKey">Key of the brand to delete.</param>
        public void DeleteBrand(string brandKey)
        {
            Resources resources = this.LoadResources();
            resources.Brands.Remove(resources.Brands.Where(b => b.Key == brandKey).FirstOrDefault());
            this.SaveResources(resources);
        }

        /// <summary>Add a new Brand favorite.</summary>
        /// <param name="brandValue">Value of the brand to add.</param>
        public void AddBrand(string brandValue)
        {
            Resources resources = this.LoadResources();

            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;

            Brand brand = new Brand(brandValue.ToLower().Replace(' ', '-'), brandValue.ToLower(), textInfo.ToTitleCase(brandValue));

            resources.Brands.Add(brand);
            resources.Brands = resources.Brands.OrderBy(b => b.Name).ToList();

            this.SaveResources(resources);
        }
        #endregion
    }
}
