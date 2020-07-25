#region Licence
// Copyright (C) 2012-2020 Tomasz Pluskiewicz
// http://r2rml.net/
// r2rml@t-code.pl
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

using System.Data.Common;
using System.Data.SqlClient;
using CommandLine;
using VDS.RDF;

namespace TCode.r2rml4net.CLI
{
    [Verb("direct")]
    public class DirectMappingCommand : BaseCommand
    {
        private TripleStore _output;

        [Option('o', "output")]
        public string OutFile { get; set; }

        public override void Prepare()
        {
            base.Prepare();

            this._output = new TripleStore();
        }

        public override void Run()
        {
            var rml = ProcessorExtensions.GenerateDirectMapping(this.ConnectionString, this.BaseUri);

            using (DbConnection connection = new SqlConnection(this.ConnectionString))
            {
                var processor = new W3CR2RMLProcessor(connection, this.MappingOptions);

                processor.Run(rml, this._output);
            }

            var defaultGraph = this._output[null];
            defaultGraph.SaveToFile(this.OutFile);
        }
    }
}