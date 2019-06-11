using System.Net;

namespace Pspl.Api.Settings
{
    public class MongoDbSettings
    {
        public string Database { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

        public string ConnectionString()
            => (string.IsNullOrWhiteSpace(User) || string.IsNullOrWhiteSpace(Password))
                    ? $@"mongodb+srv://{Host}"
                    : $@"mongodb+srv://{User}:{WebUtility.UrlEncode(Password)}@{Host}";
    }
}