﻿using System.IO;
using NUnit.Framework;
using Peach.Core;
using Peach.Core.Analyzers;
using Peach.Core.Test;

namespace Peach.Pro.Test.Core.Transformers.Compress
{
	[TestFixture]
	[Quick]
	[Peach]
    class Bz2DecompressTests : DataModelCollector
    {
        [Test]
        public void Test1()
        {
            // standard test

            string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                "<Peach>" +
                "   <DataModel name=\"TheDataModel\">" +
                "       <Block name=\"TheBlock\">" +
                "           <Transformer class=\"Bz2Decompress\"/>" +
				"           <Blob name=\"Data\" valueType=\"hex\" value=\"0x42, 0x5A, 0x68, 0x39, 0x31, 0x41, 0x59, 0x26, 0x53, 0x59, 0x64, " +
				"0x8C, 0xBB, 0x73, 0x00, 0x00, 0x00, 0x01, 0x00, 0x38, 0x00, 0x20, 0x00,"+
				"0x30, 0xcc, 0x0c, 0xc2, 0x30, 0xbb, 0x92, 0x29,"+
				"0xc2, 0x84, 0x83, 0x24, 0x65, 0xdb, 0x98\"/>" +
                "       </Block>" +
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
            // -- this is the pre-calculated result from Peach2.3 on the blob: ""
            byte[] precalcResult = new byte[] { (byte)'a', (byte)'b', (byte)'c' };
            Assert.AreEqual(1, values.Count);
            Assert.AreEqual(precalcResult, values[0].ToArray());
        }
    }
}

// end
