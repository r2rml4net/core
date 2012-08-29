﻿#region Licence
			
/* 
Copyright (C) 2012 Tomasz Pluskiewicz
http://r2rml.net/
r2rml@r2rml.net
	
------------------------------------------------------------------------
	
This file is part of r2rml4net.
	
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal 
in the Software without restriction, including without limitation the rights 
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
copies of the Software, and to permit persons to whom the Software is 
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all 
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS 
OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE 
OR OTHER DEALINGS IN THE SOFTWARE.
	
------------------------------------------------------------------------

r2rml4net may alternatively be used under the LGPL licence

http://www.gnu.org/licenses/lgpl.html

If these licenses are not suitable for your intended use please contact
us at the above stated email address to discuss alternative
terms. */
			
#endregion

using TCode.r2rml4net.Mapping;

namespace TCode.r2rml4net.Log
{
    /// <summary>
    /// Interface for abstract logging of the triples generation process
    /// </summary>
    public interface ITriplesGenerationLog
    {
        /// <summary>
        /// Logs an error of missing <see cref="ITriplesMap"/>'s <see cref="ITriplesMap.SubjectMap"/>
        /// </summary>
        void LogMissingSubject(ITriplesMap triplesMap);
        /// <summary>
        /// Logs an error in executing an SQL query
        /// </summary>
        void LogQueryExecutionError(IQueryMap map, string errorMessage);
        /// <summary>
        /// Logs an invalid <a href="http://www.w3.org/TR/r2rml/#term-map">term map</a>
        /// </summary>
        void LogInvalidTermMap(ITermMap termMap, string message);
        /// <summary>
        /// Logs an invalid <a href="http://www.w3.org/TR/r2rml/#triples-map">triples map</a>
        /// </summary>
        void LogInvaldTriplesMap(ITriplesMap triplesMap, string message);
    }
}