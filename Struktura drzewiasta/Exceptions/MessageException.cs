﻿using System;

namespace Struktura_drzewiasta.Exceptions
{
    public class MessageException : Exception
    {
        public MessageException(string message) : base(message) {}
    }
}
