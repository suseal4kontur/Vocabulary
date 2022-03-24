using System;
using System.Text;

namespace Client.ClientResults
{
    public class ClientException : Exception
    {
        public ClientException(int statusCode, ClientError error = null)
            : base(GetExceptionMessage(statusCode, error))
        {
            this.StatusCode = statusCode;
        }

        public int StatusCode { get; }

        private static string GetExceptionMessage(int statusCode, ClientError error)
        {
            var stringBuilder = new StringBuilder($"Response status code: {statusCode}");

            if (error != null)
            {
                stringBuilder.Append($". Error: {error}");
            }

            return stringBuilder.ToString();
        }
    }
}
