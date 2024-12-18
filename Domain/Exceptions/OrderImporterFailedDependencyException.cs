using System.Globalization;

namespace Domain.Exceptions
{
    public class OrderImporterFailedDependencyException : Exception
    {
        public OrderImporterFailedDependencyException()
            : base()
        {
        }

        public OrderImporterFailedDependencyException(string message)
            : base(message)
        {
        }

        public OrderImporterFailedDependencyException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {

        }
    }
}
