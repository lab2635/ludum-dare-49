using System;
using System.Collections.Generic;
using System.Linq;

namespace Utility
{
    public static class ThrowIf
    {
        public static void True(bool condition, string message, params object[] args)
        {
            if (!condition)
            {
                return;
            }

            ThrowIf.ThrowException(message, args);
        }

        public static void False(bool condition, string message, params object[] args)
        {
            ThrowIf.True(!condition, message, args);
        }

        public static void Null(object obj, string message, params object[] args)
        {
            ThrowIf.True(obj == null, message, args);
        }

        public static void Empty<T>(IEnumerable<T> obj, string message, params object[] args)
        {
            ThrowIf.Null(obj, message, args);
            ThrowIf.False(obj.Any(), message, args);
        }

        public static void ArgumentIsNull(object argument, string argumentName)
        {
            ThrowIf.Null(argument, "Argument \"{0}\" should not be null.", argumentName);
        }

        public static void ArgumentIsEmpty<T>(IEnumerable<T> argument, string argumentName)
        {
            ThrowIf.Empty(argument, "Argument \"{0}\" should not be null or empty.", argumentName);
        }

        private static void ThrowException(string message, params object[] args)
        {
            throw new Exception(string.Format(message, args));
        }
    }
}
