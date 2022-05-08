﻿using AngleSharp.Dom;
using AngleSharp.Dom.Events;
using AngleSharp.Io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothesScraper.Core.Wrappers
{
    public class RequesterWrapper : AngleSharp.Io.IRequester
    {
        public event DomEventHandler Requesting;
        public event DomEventHandler Requested;

        public void AddEventListener(string type, DomEventHandler? callback = null, bool capture = false)
        {
            throw new NotImplementedException();
        }

        public bool Dispatch(Event ev)
        {
            throw new NotImplementedException();
        }

        public void InvokeEventListener(Event ev)
        {
            throw new NotImplementedException();
        }

        public void RemoveEventListener(string type, DomEventHandler? callback = null, bool capture = false)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IResponse> RequestAsync(Request request, CancellationToken cancel)
        {
            throw new NotImplementedException();
        }

        public virtual bool SupportsProtocol(string protocol)
        {
            throw new NotImplementedException();
        }
    }
}
