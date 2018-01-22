﻿#region Licence
// Copyright (C) 2012-2018 Tomasz Pluskiewicz
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
using System.IO;
using TCode.r2rml4net.Mapping;
using TCode.r2rml4net.RDB;
using VDS.RDF;

namespace TCode.r2rml4net.Log
{
    /// <summary>
    /// Simple implementation of the <see cref="LogFacadeBase"/>, which writes messages to a <see cref="TextWriter"/>
    /// </summary>
    public class TextWriterLog : LogFacadeBase
    {
        private readonly TextWriter _writer;

        /// <summary>
        /// Initializes an instance <see cref="TextWriterLog"/>, which will write to the given <paramref name="writer"/>
        /// </summary>
        public TextWriterLog(TextWriter writer)
        {
            _writer = writer;
        }

        /// <summary>
        /// Logs an error of not found column in the SQL reuslts
        /// </summary>
        public override void LogColumnNotFound(ITermMap termMap, string columnName)
        {
            _writer.WriteLine("Column {0} not found", columnName);
        }

        /// <summary>
        /// Logs an RDF term generated
        /// </summary>
        public override void LogTermGenerated(INode node)
        {
            _writer.WriteLine("Generated term {0}", node);
        }

        /// <summary>
        /// Logs a null RDF term generated
        /// </summary>
        public override void LogNullTermGenerated(ITermMap termMap)
        {
            _writer.WriteLine("Term map {0} produced null term", termMap.Node);
        }

        /// <summary>
        /// Logs a null value retrieved for column
        /// </summary>
        public override void LogNullValueForColumn(string columnName)
        {
        }

        /// <summary>
        /// Logs an error of missing <see cref="ITriplesMap"/>'s <see cref="ITriplesMap.SubjectMap"/>
        /// </summary>
        public override void LogMissingSubject(ITriplesMap triplesMap)
        {
            _writer.WriteLine("Triples map {0} has no subject map", triplesMap.Node);
        }

        /// <summary>
        /// Logs an error in executing an SQL query
        /// </summary>
        public override void LogQueryExecutionError(IQueryMap map, string errorMessage)
        {
            _writer.WriteLine("Could not execute query for {0}: {1}", map.Node, errorMessage);
        }

        /// <summary>
        /// Logs an invalid <a href="http://www.w3.org/TR/r2rml/#term-map">term map</a>
        /// </summary>
        public override void LogInvalidTermMap(ITermMap termMap, string message)
        {
            _writer.WriteLine("Term map {0} was invalid: {1}", termMap.Node, message);
        }

        /// <summary>
        /// Logs an invalid <a href="http://www.w3.org/TR/r2rml/#triples-map">triples map</a>
        /// </summary>
        public override void LogInvaldTriplesMap(ITriplesMap triplesMap, string message)
        {
            _writer.WriteLine("Triples map {0} was invalid: {1}", triplesMap.Node, message);
        }

        /// <summary>
        /// Logs multiple references to a compisote key in a table
        /// </summary>
        /// <param name="table">Table with multiple composite keys referenced</param>
        public override void LogMultipleCompositeKeyReferences(TableMetadata table)
        {
            _writer.WriteLine("Multiple Composite Key references to table {0}", table.Name);
        }
    }
}