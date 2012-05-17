using System;
using MonoTouch.Foundation;

namespace Corpy {

	[Preserve(AllMembers=true)]
	public class Employee {
		public Employee ()
		{
		}
		public int Id {get;set;}
		public string Firstname {get;set;}
		public string Lastname {get;set;}
		public string Department {get;set;}
		public int Work {get;set;}
		public int Mobile {get;set;}
		public string Email {get;set;}

		public string NameFormatted {
			get {return String.Format("{0} {1}", Firstname, Lastname); }
		}
	}
}

