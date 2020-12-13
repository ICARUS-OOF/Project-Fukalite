using System;
namespace ProjectFukalite.Custom.Exceptions
{
    [Serializable]
    public class PropertyOrFieldNotFoundException : Exception
    {
        public PropertyOrFieldNotFoundException() { }

        public PropertyOrFieldNotFoundException(string message) : base(message) { }

        public PropertyOrFieldNotFoundException(string message, Exception inner) : base(message, inner) { }

        protected PropertyOrFieldNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}