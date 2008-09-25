using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Specialized;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
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
		public History()
		{
		}

		public History(string filename)
		{
			this.filename = filename;
		}

		private EventCollection events		= new EventCollection();
		private EventComparer	comparer	= new EventComparer();
		private string			filename	= "";

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