using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using System.Drawing;

using DrinkUp.Core.BusinessLayer;

namespace DrinkUp.AppLayer {
	/// <summary>
	/// Originally used MonoTouch.Dialog BadgeElement but created 
	/// this custom element to fix layout issues I was having
	/// </summary>
	public class EventElement : Element {

		static NSString key = new NSString ("NewsElement");
		UIImage image;
		Event entry;
		
		public EventElement (Event showEntry, UIImage showImage) : base (showEntry.Title)
		{
			entry = showEntry;
			image = showImage;
		}


		public override UITableViewCell GetCell (UITableView tv)
		{
			var cell = tv.DequeueReusableCell (key);
			
			if (cell == null) {
				cell = new EventCell (UITableViewCellStyle.Default, key, entry.Title, image);
			} else {
				((EventCell)cell).UpdateCell (entry.Title, image);
			}
			return cell;
		}

		public override void Selected (DialogViewController dvc, UITableView tableView, MonoTouch.Foundation.NSIndexPath path)
		{
			var sds = new HomeViewController(entry);
			dvc.ActivateController (sds);
		}
	}
}