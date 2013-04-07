﻿#region Licence
// Copyright (C) 2012 Tomasz Pluskiewicz
// http://r2rml.net/
// r2rml@r2rml.net
// 	
// ------------------------------------------------------------------------
// 	
// This file is part of r2rml4net.
// 	
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal 
// in the Software without restriction, including without limitation the rights 
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
// copies of the Software, and to permit persons to whom the Software is 
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS 
// OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE 
// OR OTHER DEALINGS IN THE SOFTWARE.
// 	
// ------------------------------------------------------------------------
// 
// r2rml4net may alternatively be used under the LGPL licence
// 
// http://www.gnu.org/licenses/lgpl.html
// 
// If these licenses are not suitable for your intended use please contact
// us at the above stated email address to discuss alternative
// terms.
#endregion
using NUnit.Framework;

namespace TCode.r2rml4net.Tests
{
    [TestFixture]
    public class MappingHelperTests
    {
        [Test]
        public void EnsuresUppercaseEscapedOctets()
        {
            // given
            const string unescaped = "some, text; with: illegal/ characters";

            // when
            string escaped = new MappingHelper(new MappingOptions()).UrlEncode(unescaped);

            // then
            Assert.AreEqual("some%2C%20text%3B%20with%3A%20illegal%2F%20characters", escaped);
        }

        [TestCase(".~_-")]
        [TestCase("abcdefghigklmnopqrstuvwxyz")]
        [TestCase("QWERTYUIOPASFFGHJKLZXCVBNM")]
        [TestCase("0123654789")]
        public void DoesntEncodeAllowedCharcters(string character)
        {
            Assert.AreEqual(character, new MappingHelper(new MappingOptions()).UrlEncode(character));
        }

        [TestCase(" ", "%20")]
        [TestCase(",", "%2C")]
        [TestCase(";", "%3B")]
        [TestCase(":", "%3A")]
        [TestCase("/", "%2F")]
        [TestCase("/(..)/", "%2F%28..%29%2F")]
        public void EncodesCharactersCaseSensitive(string character, string expectedEncoded)
        {
            Assert.AreEqual(expectedEncoded, new MappingHelper(new MappingOptions()).UrlEncode(character));
        }

        [TestCase("成")]
        [TestCase("用")]
        [TestCase("カタカ")]
        public void DoesntEscapeEasterScript(string character)
        {
            Assert.AreEqual(character, new MappingHelper(new MappingOptions()).UrlEncode(character));
            
        }
    }
}