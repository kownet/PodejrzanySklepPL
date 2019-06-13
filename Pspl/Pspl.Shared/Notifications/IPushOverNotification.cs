using Pspl.Shared.Providers;
using System.Collections.Specialized;
using System.Net;

namespace Pspl.Shared.Notifications
{
    public interface IPushOverNotification
    {
        void Send(string title, string message);
    }

    public class PushOverNotification : IPushOverNotification
    {
        private readonly IPushOverProvider _pushOverProvider;

        public PushOverNotification(IPushOverProvider pushOverProvider)
        {
            _pushOverProvider = pushOverProvider;
        }

        public void Send(string title, string message)
        {
            if (!string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(message))
            {
                var parameters = new NameValueCollection
                {
                    { "token", _pushOverProvider.Token() },
                    { "user", _pushOverProvider.Recipients() },
                    { "message", message },
                    { "title", title },
                    { "html", "1" }
                };

                using (var client = new WebClient())
                {
                    client.UploadValues(_pushOverProvider.Endpoint(), parameters);
                }
            }
        }
    }
}