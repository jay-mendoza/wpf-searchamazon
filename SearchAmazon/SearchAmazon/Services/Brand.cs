//-----------------------------------------------------------------------
// <copyright file="Brand.cs" company="Jay Bautista Mendoza">
//     Copyright (c) Jay Bautista Mendoza. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace SearchAmazon.Services
{
    using System;
    using System.Xml.Serialization;

    /// <summary>Data model class for "Brand".</summary>
    [Serializable]
    public class Brand
    {
        /// <summary>Initializes a new instance of the <see cref="Brand" /> class.</summary>
        public Brand()
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Brand" /> class.</summary>
        /// <param name="key">Key attribute value.</param>
        /// <param name="value">Value attribute value.</param>
        /// <param name="name">Name attribute value.</param>
        public Brand(string key, string value, string name)
        {
            this.Key = key;
            this.Value = value;
            this.Name = name;
        }

        /// <summary>Gets or sets the Key attribute.</summary>
        [XmlAttribute(AttributeName = "key")]
        public string Key { get; set; }

        /// <summary>Gets or sets the Value attribute.</summary>
        [XmlAttribute(AttributeName = "value")]
        public string Value { get; set; }

        /// <summary>Gets or sets the Name attribute.</summary>
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
    }
}
