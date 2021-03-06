using System.Xml.Serialization;
using SharpCover.Collections;

namespace SharpCover.Reporting
{
	/// <summary>
	/// Summary description for History.
	/// 
	/// Compare with previous.
	/// if no history, highlight [new]. otherwise ^ \/ =
	/// </summary>
	[XmlRoot()]
	public class History
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="History"/> class.
        /// </summary>
		public History()
		{
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="History"/> class.
        /// </summary>
        /// <param name="filename">The filename.</param>
		public History(string filename)
		{
			this.filename = filename;
		}

		private EventCollection events		= new EventCollection();
		private EventComparer	comparer	= new EventComparer();
		private string			filename	= "";

        /// <summary>
        /// Gets or sets the filename.
        /// </summary>
        /// <value>The filename.</value>
		[XmlIgnore()]
		public string Filename
		{
			get{return this.filename;}
			set{this.filename = value;}
		}

		/// <summary>
		/// Returns the events sorted in reverse historical order. I.e. Events[0] will be newest.
		/// </summary>
		public EventCollection Events
		{
			get 
			{
				events.Sort(this.comparer);
				return this.events;
			}
			set{this.events = value;}
		}
		
		/// <summary>
		/// Returns the most recent event added to history.
		/// </summary>
		public Event GetLastEvent()
		{
			if (events == null || events.Count == 0)
				return null;

			this.Events.Sort(this.comparer);

			Event mostRecent = (Event) events[0];
			foreach (Event anEvent in events)
			{
				//Lets hope they've opperator overloaded dates...
				if (anEvent.EventDate > mostRecent.EventDate)
				{
					mostRecent = anEvent;
				}
			}
			return mostRecent;
		}
	}
}