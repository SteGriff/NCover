using System.IO;
using System.Reflection;

namespace SharpCover.Utilities
{
    /// <summary>
    /// 
    /// </summary>
	public class ResourceManager
	{
        /// <summary>
        /// Gets the resource.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
		public static Stream GetResource(string name, Assembly assembly)
		{
			return assembly.GetManifestResourceStream(name);
		}

        /// <summary>
        /// Gets the resource.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
		public static Stream GetResource(string name)
		{
			return GetResource(name, typeof(ResourceManager).Assembly);
		}

        /// <summary>
        /// Writes the resource to stream.
        /// </summary>
        /// <param name="outputstream">The outputstream.</param>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="assem">The assem.</param>
		public static void WriteResourceToStream(Stream outputstream, string name, ResourceType type, Assembly assem)
		{
			Stream stream = GetResource(name, assem);
			WriteResourceToStream(stream, outputstream, type);
		}

        /// <summary>
        /// Writes the resource to stream.
        /// </summary>
        /// <param name="outputstream">The outputstream.</param>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
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