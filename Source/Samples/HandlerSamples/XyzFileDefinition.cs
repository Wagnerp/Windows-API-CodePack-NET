﻿using System.IO;
using System.Xml.Linq;

namespace HandlerSamples
{
    public class XyzFileDefinition
    {
        public XyzFileDefinition(Stream stream)
        {
            XDocument document = XDocument.Load(stream);

            Properties = new XyzFileProperties(document.Root?.Element("XyzFileProperties"));
            EncodedImage = document.Root?.Element("EncodedImage")?.Value;
            Content = document.Root?.Element("Content")?.Value;
        }

        public XyzFileProperties Properties { get; private set; }
        public string EncodedImage { get; private set; }
        public string Content { get; private set; }
    }

    public class XyzFileProperties
    {
        public XyzFileProperties(XElement properties)
        {
            Author = properties.Element("Author")?.Value;
            Name = properties.Element("Name")?.Value;
            var value = properties.Element("Rating")?.Value;
            if (value != null)
            {
                Rating = int.Parse(value);
                Region = properties.Element("Region")?.Value;
            }
        }

        public string Name { get; private set; }
        public string Author { get; private set; }

        public int Rating { get; private set; }
        public string Region { get; private set; }
    }
}
