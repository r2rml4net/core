﻿#region Licence
// Copyright (C) 2012-2014 Tomasz Pluskiewicz
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
using System.Diagnostics;
using System.Threading;

namespace TCode.r2rml4net
{
    /// <summary>
    /// A thread-static scope, which allow changing mapping options for a given time
    /// </summary>
    /// <remarks>See http://msdn.microsoft.com/en-us/magazine/cc300805.aspx</remarks>
    public sealed class MappingScope : IDisposable
    {
        private bool _disposed;
        private readonly MappingOptions _instance;
        private readonly MappingScope _parent;
        [ThreadStatic]
        private static MappingScope _head;

        /// <summary>
        /// Creates a new instance of <see cref="MappingScope"/> with a given set of options
        /// </summary>
        /// <param name="instance"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public MappingScope(MappingOptions instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            _instance = instance;
            _instance.Freeze();

            Thread.BeginThreadAffinity();
            _parent = _head;
            _head = this;
        }

        internal static MappingOptions Current
        {
            get { return _head != null ? _head._instance : null; }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                Debug.Assert(this == _head, "Disposed out of order.");
                _head = _parent;
                Thread.EndThreadAffinity();
            }
        }
    }
}