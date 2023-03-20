using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;

namespace CodingProject3
{
    public static class VendorDB
    {
        private const string Path = @"..\..\Vendors.xml";

        public static List<Vendor> GetVendors()
        {
            // create the list
            List<Vendor> vendors = new List<Vendor>();

            // create the XmlReaderSettings object
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;
            settings.IgnoreComments = true;

            // create the XmlReader object
            XmlReader xmlIn = XmlReader.Create(Path, settings);

            // read past all nodes to the first Product node
            if (xmlIn.ReadToDescendant("Vendor"))
            {
                // create one Product object for each Product node
                do
                {
                    Vendor vendor = new Vendor();
                    xmlIn.ReadStartElement("Vendor");
                    vendor.Name = xmlIn.ReadElementContentAsString();
                    vendor.Address = xmlIn.ReadElementContentAsString();
                    vendor.City = xmlIn.ReadElementContentAsString();
                    vendor.State = xmlIn.ReadElementContentAsString();
                    vendor.Zip = xmlIn.ReadElementContentAsString();
                    vendor.Phone = xmlIn.ReadElementContentAsString();
                    vendor.YTD = xmlIn.ReadElementContentAsDecimal();
                    vendor.Comment = xmlIn.ReadElementContentAsString();
                    vendor.Contact = xmlIn.ReadElementContentAsString();
                    vendor.DefaultDiscount = xmlIn.ReadElementContentAsInt();

                    vendors.Add(vendor);
                }
                while (xmlIn.ReadToNextSibling("Vendor"));
            }
            
            // close the XmlReader object
            xmlIn.Close();

            return vendors;
        }

        public static void SaveVendors(List<Vendor> vendors)
        {
            // create the XmlWriterSettings object
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = ("    ");

            // create the XmlWriter object
            XmlWriter xmlOut = XmlWriter.Create(Path, settings);

            // write the start of the document
            xmlOut.WriteStartDocument();
            xmlOut.WriteStartElement("Vendors");

            // write each product object to the xml file
            foreach (Vendor vendor in vendors)
            {
                xmlOut.WriteStartElement("Vendor");
                xmlOut.WriteElementString("Name", vendor.Name);
                xmlOut.WriteElementString("Address", vendor.Address);
                xmlOut.WriteElementString("City", vendor.City);
                xmlOut.WriteElementString("State", vendor.State);
                xmlOut.WriteElementString("ZIP", vendor.Zip);
                xmlOut.WriteElementString("Phone", vendor.Phone);
                xmlOut.WriteElementString("YTD", Convert.ToString(vendor.YTD));
                xmlOut.WriteElementString("Comment", vendor.Comment);
                xmlOut.WriteElementString("Contact", vendor.Contact);
                xmlOut.WriteElementString("DefaultDiscount", Convert.ToString(vendor.DefaultDiscount));

                xmlOut.WriteEndElement();
            }

            // write the end tag for the root element
            xmlOut.WriteEndElement();

            // close the xmlWriter object
            xmlOut.Close();
        }
    }
}
