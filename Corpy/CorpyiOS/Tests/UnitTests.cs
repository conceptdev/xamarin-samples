using System;
using NUnit.Framework;
using Corpy;

// Added as an example - doesn't really test much at this stage

namespace Tests {
	[TestFixture]
	public class UnitTests {
		[Test]
		public void NameCorrect ()
		{
			var e = new Employee ();
			e.Firstname = "Mono";
			e.Lastname = "Monkey";
				
			Assert.True (e.NameFormatted == "Mono Monkey");
		}

		[Test]
		public void NameNotInCorrect ()
		{
			var e = new Employee ();
			e.Firstname = "John";
			e.Lastname = "Doe";
				
			Assert.True (e.NameFormatted == "John Doe");
		}
		
		[Test]
		public void Pass ()
		{
			Assert.True (true);
		}

		[Test]
		public void Fail ()
		{
			Assert.False (true); // will fail :-)
		}

		[Test]
		[Ignore ("another time")]
		public void Ignore ()
		{
			Assert.True (false);
		}
	}
}
