//-----------------------------------------------------------------------
// <copyright file="Resources.cs" company="Jay Bautista Mendoza">
//     Copyright (c) Jay Bautista Mendoza. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace SearchAmazon.Services
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    /// <summary>Serializable class for a collection of Resources object.</summary>
    [Serializable]
    [XmlRoot(ElementName = "Resources")]
    public class Resources
    {
        /// <summary>Initializes a new instance of the <see cref="Resources" /> class.</summary>
        public Resources()
        {
        }

        /// <summary>Gets or sets the list of Brand objects.</summary>
        [XmlArray("Brands")]
        [XmlArrayItem("Brand")]
        public List<Brand> Brands { get; set; }

        /// <summary>Gets or sets the Amazon URL.</summary>
        [XmlElement("AmazonUrl")]
        public string AmazonUrl { get; set; }

        /// <summary>Gets or sets the Amazon URL - parameterized.</summary>
        [XmlElement("AmazonUrlP")]
        public string AmazonUrlP { get; set; }

        /// <summary>Gets or sets the eligible filter string format.</summary>
        [XmlElement("EligibleFormat")]
        public string EligibleFormat { get; set; }

        /// <summary>Gets or sets the brand string format.</summary>
        [XmlElement("BrandFormat")]
        public string BrandFormat { get; set; }

        /// <summary>Gets or sets the keyword string format.</summary>
        [XmlElement("KeywordFormat")]
        public string KeywordFormat { get; set; }

        /// <summary>Gets or sets the price range string format.</summary>
        [XmlElement("PriceRangeFormat")]
        public string PriceRangeFormat { get; set; }

        /// <summary>Gets or sets the Prime | FREE One-Day parameter string.</summary>
        [XmlElement("PrimeFreeOneDayEligible")]
        public string PrimeFreeOneDayEligible { get; set; }

        /// <summary>Gets or sets the Prime parameter string.</summary>
        [XmlElement("PrimeEligible")]
        public string PrimeEligible { get; set; }

        /// <summary>Gets or sets the AmazonGlobal parameter string.</summary>
        [XmlElement("AmazonGlobalEligible")]
        public string AmazonGlobalEligible { get; set; }

        /// <summary>Gets or sets the AmazonGlobal with Free shipping parameter string.</summary>
        [XmlElement("AmazonGlobalEligibleFree")]
        public string AmazonGlobalEligibleFree { get; set; }

        /// <summary>Gets or sets the New condition.</summary>
        [XmlElement("NewCondition")]
        public string NewCondition { get; set; }

        /// <summary>Gets or sets the Used condition.</summary>
        [XmlElement("UsedCondition")]
        public string UsedCondition { get; set; }

        /// <summary>Gets or sets the Refurbished condition.</summary>
        [XmlElement("RefurbishedCondition")]
        public string RefurbishedCondition { get; set; }
    }
}
