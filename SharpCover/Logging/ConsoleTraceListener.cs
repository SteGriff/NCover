using System;
using System.Diagnostics;

namespace SharpCover.Logging
{
    /// <summary>
    /// 
    /// </summary>
	public class ConsoleTraceListener : TraceListener
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleTraceListener"/> class.
        /// </summary>
		public ConsoleTraceListener() : base("Console")
		{
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleTraceListener"/> class.
        /// </summary>
        /// <param name="name">The name of the <see cref="T:System.Diagnostics.TraceListener"/>.</param>
		public ConsoleTraceListener(string name) : base(name)
		{
		}

        /// <summary>
        /// Writes the specified STR.
        /// </summary>
        /// <param name="str">The STR.</param>
		public override void Write(string str)
		{
			Console.Out.Write(str);
		}

        /// <summary>
        /// Writes the specified obj.
        /// </summary>
        /// <param name="obj">The obj.</param>
		public override void Write(object obj)
		{
			Write(obj.ToString());
		}

        /// <summary>
        /// Writes the specified STR.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="category">The category.</param>
		public override void Write(string str, string category)
		{
			Console.Out.Write(category + ":\t" + str);
		}

        /// <summary>
        /// Writes the specified obj.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="category">The category.</param>
		public override void Write(object obj, string category)
		{
			Write(obj.ToString(), category);
		}

        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="str">The STR.</param>
		public override void WriteLine(string str)
		{
			Console.Out.WriteLine(str);
		}

        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="obj">The obj.</param>
		public override void WriteLine(object obj)
		{
			WriteLine(obj.ToString());
		}

        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="category">The category.</param>
		public override void WriteLine(string str, string category)
		{
			Console.Out.WriteLine(category + ":\t" + str);
		}

        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="category">The category.</param>
		public override void WriteLine(object obj, string category)
		{
			WriteLine(obj.ToString(), category);
		}

        /// <summary>
        /// When overridden in a derived class, flushes the output buffer.
        /// </summary>
		public override void Flush()
		{
			Console.Out.Flush();
		}

        /// <summary>
        /// Fails the specified error.
        /// </summary>
        /// <param name="error">The error.</param>
		public override void Fail(string error)
		{
			WriteLine("An error has occurred:\t" + error);
		}

        /// <summary>
        /// Fails the specified error.
        /// </summary>
        /// <param name="error">The error.</param>
        /// <param name="description">The description.</param>
		public override void Fail(string error, string description)
		{
			Fail(error);
			WriteLine(description);
		}
	}
}