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

using System;
using System.Net;
using Anotar.NLog;
using CommandLine;
using VDS.RDF;
using VDS.RDF.Parsing.Handlers;
using VDS.RDF.Query;
using VDS.RDF.Storage;
using VDS.RDF.Update;

namespace TCode.r2rml4net.CLI
{
    public abstract class MappingCommand : BaseCommand
    {
        protected IRdfHandler Output { get; private set; }

        protected ITripleStore Store { get; } = new TripleStore();

        [Option('o', "output")]
        public string OutFile { get; set; }

        [Option('e', "endpoint")]
        public string SparqlEndpoint { get; set; }

        [Option('u', "update-endpoint")]
        public string SparqlUpdateEndpoint { get; set; }

        [Option( "user")]
        public string SparqlEndpointUser { get; set; }

        [Option( "pass")]
        public string SparqlEndpointPassword { get; set; }

        [Option("batch-size", Default = 1000)]
        public int BatchSize { get; set; }

        public override void Prepare()
        {
            if (this.OutFile != null)
            {
                LogTo.Info("Saving to local file");
                base.Prepare();
                this.Output = new StoreHandler(this.Store);
            }
            else if (this.SparqlEndpoint != null)
            {
                base.Prepare();

                var endpoint = CreateEndpoint(uri => new SparqlRemoteEndpoint(uri));
                var upEndpoint = CreateEndpoint(uri => new SparqlRemoteUpdateEndpoint(uri), true);

                LogTo.Info("Saving to SPARQL Endpoint with batch size {0}", this.BatchSize);

                this.Output = new EfficientBatchSparqlInsertHandler(new ReadWriteSparqlConnector(endpoint, upEndpoint), this.BatchSize);
                this.Output.StartRdf();
            }
        }

        public override void SaveOutput()
        {
            this.Output.EndRdf(true);
        }

        private TEndpoint CreateEndpoint<TEndpoint>(Func<Uri, TEndpoint> create, bool update = false) where TEndpoint : BaseEndpoint
        {
            var endpointUri = update ? this.SparqlUpdateEndpoint ?? this.SparqlEndpoint : this.SparqlEndpoint;

            var endpoint = create(new Uri(endpointUri));
            if (this.SparqlEndpointUser != null)
            {
                if (this.SparqlEndpointPassword == null)
                {
                    LogTo.Warn("Endpoint user provided but no password");
                }
                else
                {
                    endpoint.Credentials = new NetworkCredential(
                        this.SparqlEndpointUser,
                        this.SparqlEndpointPassword);
                }
            }

            return endpoint;
        }
    }
}