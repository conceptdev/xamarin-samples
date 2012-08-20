using System;
using System.Collections.Generic;
using System.Linq;
using DrinkUp.Core.BusinessLayer;

namespace DrinkUp.Core.DataLayer {
	/// <summary>
	/// [abstracts fromt the underlying data source(s)]
	/// [if multiple data sources, can agreggate/etc without BL knowing]
	/// [superflous if only one data source]
	/// </summary>
	public static class DataManager {
		#region Events
		
		public static IEnumerable<Event> GetEvents ()
		{
			return DrinkUpDatabase.GetItems<Event> ();
		}
		
		public static Event GetSession (int id)
		{
			return DrinkUpDatabase.GetItem<Event> (id);
		}
        
		public static int SaveEvent (Event item)
		{
			return DrinkUpDatabase.SaveItem<Event> (item);
		}
		
		public static void SaveEvents (IEnumerable<Event> items)
		{
			DrinkUpDatabase.SaveItems<Event> (items);
		}
		
		#endregion

		#region Tweets
		public static void SaveTweets (IEnumerable<Tweet> items)
		{
			DrinkUpDatabase.SaveItems<Tweet> (items);
		}
        public static Tweet GetTweet(int id)
        {
            return DrinkUpDatabase.GetItem<Tweet> (id);
            //return DrinkUpDatabase.GetTweet(id);
        }
		public static IEnumerable<Tweet> GetTweets ()
		{
			return DrinkUpDatabase.GetItems<Tweet> ();
		}
		public static void DeleteTweets()
		{
			DrinkUpDatabase.ClearTable<Tweet>();
		}
		#endregion
	}
}