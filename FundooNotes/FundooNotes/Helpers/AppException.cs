using System;
using System;
using System.Globalization;

namespace FundooNotes.Helpers
{  // In Helpers folder we can create the Exception classes
    public class AppException : Exception
    {
        /// <summary>
        /// Custom exception class for throwing application specific exceptions 
        /// that can be caught and handled within the application
        /// </summary>

        public AppException() : base() { }

        public AppException(string message) : base(message) { }

    }
}





