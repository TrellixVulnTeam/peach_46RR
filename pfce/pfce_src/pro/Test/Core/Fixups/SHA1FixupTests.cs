﻿using System.IO;
using NUnit.Framework;
using Peach.Core;
using Peach.Core.Analyzers;
using Peach.Core.Test;

namespace Peach.Pro.Test.Core.Fixups
{
	[TestFixture]
	[Quick]
	[Peach]
    class SHA1FixupTests : DataModelCollector
    {
        [Test]
        public void Test1()
        {
            // standard test

            string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                "<Peach>" +
                "   <DataModel name=\"TheDataModel\">" +
                "       <Blob name=\"Checksum\">" +
                "           <Fixup class=\"SHA1Fixup\">" +
                "               <Param name=\"ref\" value=\"Data\"/>" +
                "           </Fixup>" +
                "       </Blob>" +
                "       <Blob name=\"Data\" value=\"12345\"/>" +
                "   </DataModel>" +

                "   <StateModel name=\"TheState\" initialState=\"Initial\">" +
                "       <State name=\"Initial\">" +
                "           <Action type=\"output\">" +
                "               <DataModel ref=\"TheDataModel\"/>" +
                "           </Action>" +
                "       </State>" +
                "   </StateModel>" +

                "   <Test name=\"Default\">" +
                "       <StateModel ref=\"TheState\"/>" +
                "       <Publisher class=\"Null\"/>" +
                "   </Test>" +
                "</Peach>";

            PitParser parser = new PitParser();

            Peach.Core.Dom.Dom dom = parser.asParser(null, new MemoryStream(ASCIIEncoding.ASCII.GetBytes(xml)));

            RunConfiguration config = new RunConfiguration();
            config.singleIteration = true;

            Engine e = new Engine(this);
            e.startFuzzing(dom, config);

            // verify values
            // -- this is the pre-calculated checksum from Peach2.3 on the blob: { 1, 2, 3, 4, 5 }
            byte[] precalcChecksum = new byte[] { 0x8C, 0xB2, 0x23, 0x7D, 0x06, 0x79, 0xCA, 0x88, 0xDB, 0x64, 0x64, 0xEA, 0xC6, 0x0D, 0xA9, 0x63, 0x45, 0x51, 0x39, 0x64 };
            Assert.AreEqual(1, values.Count);
            Assert.AreEqual(precalcChecksum, values[0].ToArray());
        }

		[Test]
		public void TestRoundTrip()
		{
			const string xml = @"
<Peach>
	<DataModel name='DM'>
		<Blob length='20'>
			<Fixup class='Sha1'>
				<Param name='ref' value='DM' />
				<Param name='DefaultValue' value='0000000000000000000000000000000000000000' />
			</Fixup>
		</Blob>
		<Blob value='Hello' />
	</DataModel>
</Peach>
";

			VerifyRoundTrip(xml);
		}
    }
}

// end
