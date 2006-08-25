// 
// Copyright (c) 2004,2005 Jaroslaw Kowalski <jkowalski@users.sourceforge.net>
// 
// 
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions 
// are met:
// 
// * Redistributions of source code must retain the above copyright notice, 
//   this list of conditions and the following disclaimer. 
// 
// * Redistributions in binary form must reproduce the above copyright notice,
//   this list of conditions and the following disclaimer in the documentation
//   and/or other materials provided with the distribution. 
// 
// * Neither the name of the Jaroslaw Kowalski nor the names of its 
//   contributors may be used to endorse or promote products derived from this
//   software without specific prior written permission. 
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF 
// THE POSSIBILITY OF SUCH DAMAGE.
// 

using System;
using System.Xml;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace NLogViewer.Events
{
    /// <summary>
    /// Represents the log event.
    /// </summary>
	public class LogEvent
	{
        private int _id;
        private ILogEventColumns _columns;
        private object[] _properties;

        public LogEvent(ILogEventColumns columns)
        {
            _columns = columns;
            int initialCapacity = columns.Count;
            if (initialCapacity < 8)
                initialCapacity = 8;
            _properties = new object[initialCapacity];
        }

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public ILogEventColumns Columns
        {
            get { return _columns; }
        }

        public object this[string key]
        {
            get { return this[GetOrdinal(key)]; }
            set { this[GetOrdinal(key)] = value; }
        }

        public object this[int ordinal]
        {
            get { MakeRoom(ordinal); return _properties[ordinal]; }
            set { MakeRoom(ordinal);  _properties[ordinal] = value; }
        }

        public int GetOrdinal(string name)
        {
            int pos = _columns.GetOrAllocateOrdinal(name);
            MakeRoom(pos);
            return pos;
        }

        private void MakeRoom(int pos)
        {
            if (pos >= _properties.Length)
            {
                int newCapacity = pos + 8; // allocate some in advance
                object[] newTable = new object[newCapacity];
                Array.Copy(_properties, newTable, _properties.Length);
                _properties = newTable;
            }
        }

        public override int GetHashCode()
        {
            return _id;
        }

        public override bool Equals(object obj)
        {
            LogEvent other = obj as LogEvent;
            if (other == null)
                return false;

            return ID == other.ID;
        }
    }
}