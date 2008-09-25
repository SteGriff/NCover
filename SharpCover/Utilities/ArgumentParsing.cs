using System;
using System.Collections.Specialized;

namespace SharpCover.Utilities
{
	/// <summary>
	/// Summary description for ArgumentParsing.
	/// </summary>
	public sealed class ArgumentParsing
	{
		/// <summary>
		/// Each command line arg should have the form /key:value.
		/// </summary>
		public static NameValueCollection ParseCommandLine(string[] args)
		{
			NameValueCollection parameterMap = new NameValueCollection();
			string key, value;

			if(args == null)
				return parameterMap;

			foreach (string arg in args)
			{
				ParseArgument(arg, out key, out value);
				parameterMap[key] = value;
			}

			return parameterMap;
		}

		/// <summary>
		/// Check argument is in correct format and parses it.
		/// </summary>
		public static void ParseArgument(string arg, out string key, out string value)
		{
			string [] bits = arg.Split(':');

			if (arg[0] != '/' || bits.Length > 2)
			{
				throw new ArgumentException("arguments should be in form /key:\"value\"", arg);
			}
			
			key = bits[0].Substring(1).ToLower();

			if (bits.Length == 1)
			{
				value = null;
				return;
			}

			if (bits[1][0] == '"' && bits[1][bits[1].Length - 1] == '"')
			{
				value = bits[1].Substring(1, bits[1].Length - 2).ToLower();
			}
			else
			{
				value = bits[1].ToLower();
			}
		}
	}
}