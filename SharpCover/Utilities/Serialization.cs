using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace SharpCover.Utilities
{
	public sealed class Serialization
	{
		private Serialization()
		{
		}

		public static void ToXml(Stream stream, object obj, bool close)
		{
			XmlSerializer output = new XmlSerializer(obj.GetType());
			XmlTextWriter writer = new XmlTextWriter(stream, System.Text.Encoding.Default);
			writer.Formatting = Formatting.Indented;
			output.Serialize(writer, obj);

			if(close)
				writer.Close();
		}

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
