﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTore
{
    public class RESToreException : Exception
    {
        public RESToreException(string message): base(message)
        {

        }
    }
}
