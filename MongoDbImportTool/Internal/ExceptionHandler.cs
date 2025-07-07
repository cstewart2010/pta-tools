using System;

namespace MongoDbImportTool.Internal
{
    internal static class ExceptionHandler
    {
        public static ArgumentException IsNullOrEmpty(string argumentName)
        {
            return new ArgumentException("String value was null or empty", argumentName);
        }
    }
}
