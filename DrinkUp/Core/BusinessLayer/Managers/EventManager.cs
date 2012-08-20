using System;
using System.Collections.Generic;
using System.Linq;

namespace DrinkUp.Core.BusinessLayer
{
	public static class EventManager
	{
		static EventManager ()
		{
		}

		public static Event GetCurrentEvent() {
			var events = DataLayer.DataRepository.GetEvents().ToList();
			return events[0];
		}

		public static List<Event> GetAll() {
			return DataLayer.DataRepository.GetEvents().ToList ();
		}

		public static void SaveRegistration (int eventID) {
			DataLayer.DataRepository.SaveRegistration (eventID);
		}
	}
}

