using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SharpCover.Utilities
{
    /// <summary>
    /// 
    /// </summary>
	public sealed class Serialization
	{
		private Serialization()
		{
		}

        /// <summary>
        /// Toes the XML.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="obj">The obj.</param>
        /// <param name="close">if set to <c>true</c> [close].</param>
		public static void ToXml(Stream stream, object obj, bool close)
		{
			XmlSerializer output = new XmlSerializer(obj.GetType());
			XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8);
			writer.Formatting = Formatting.Indented;
			output.Serialize(writer, obj);

			if(close)
				writer.Close();
		}

        /// <summary>
        /// Froms the XML.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
		public static object FromXml(Stream stream, Type type)
		{
			XmlSerializer input = new XmlSerializer(type);
			XmlTextReader reader = new XmlTextReader(stream);
			object retval = input.Deserialize(reader);
			reader.Close();
		
			return retval;
		}
	}
}
