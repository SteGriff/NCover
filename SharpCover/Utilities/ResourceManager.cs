using System.IO;
using System.Reflection;

namespace SharpCover.Utilities
{
	public class ResourceManager
	{
		public static Stream GetResource(string name, Assembly assembly)
		{
			return assembly.GetManifestResourceStream(name);
		}

		public static Stream GetResource(string name)
		{
			return GetResource(name, typeof(ResourceManager).Assembly);
		}

		public static void WriteResourceToStream(Stream outputstream, string name, ResourceType type, Assembly assem)
		{
			Stream stream = GetResource(name, assem);
			WriteResourceToStream(stream, outputstream, type);
		}

		public static void WriteResourceToStream(Stream outputstream, string name, ResourceType type)
		{
			Stream stream = GetResource(name);
			WriteResourceToStream(stream, outputstream, type);
		}

		private static void WriteResourceToStream(Stream resourcestream, Stream outputstream, ResourceType type)
		{
			if(resourcestream == null || outputstream == null)
				return;

			switch(type)
			{
				case ResourceType.Text:
					StreamReader streamreader = new StreamReader(resourcestream);
					StreamWriter streamwriter = new StreamWriter(outputstream);
					streamwriter.Write(streamreader.ReadToEnd());
					streamwriter.Flush();
					break;
				case ResourceType.Binary:
					BinaryReader binaryreader = new BinaryReader(resourcestream);
					BinaryWriter binarywriter = new BinaryWriter(outputstream);
					binarywriter.Write(binaryreader.ReadBytes((int)binaryreader.BaseStream.Length));
					binarywriter.Flush();
					break;
			}
		}
	}
}