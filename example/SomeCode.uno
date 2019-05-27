using Fuse;
using Uno;


namespace SomeCode
{
	public class ThisIsATestClass
	{

		// snippet-begin:SomeExampleUnoMethod
		public float ThisMethodCalculatesSomething(float f1, float f2)
		{
			var toIllustrateSnippetsFromUno = "Snippets are fun";
			debug_log("WoaaaH");

			// strip-begin:We do some debug logging here
			for (int i = 0; i < 10; i++)
			{
				debug_log("We dont care about this in our snippet");
			}
			// strip-end

			return f1 + f2;
		}
		// snippet-end
	}
}