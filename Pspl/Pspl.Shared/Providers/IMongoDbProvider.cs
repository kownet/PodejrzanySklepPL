using System.Net;

namespace Pspl.Shared.Providers
{
    public interface IMongoDbProvider
    {
        string Database();
        string Host();
        int Port();
        string User();
        string Password();

        string ConnectionString();
    }

    public class MongoDbProvider : IMongoDbProvider
    {
        private readonly string _database;
        private readonly string _host;
        private readonly string _port;
        private readonly string _user;
        private readonly string _password;

        public MongoDbProvider(
            string database,
            string host,
            string port,
            string user,
            string password)
        {
            _database = database;
            _host = host;
            _port = port;
            _user = user;
            _password = password;
        }

        public string Database()
            => !string.IsNullOrWhiteSpace(_database)
            ? _database.ToLowerInvariant()
            : string.Empty;

        public string Host()
            => !string.IsNullOrWhiteSpace(_host)
            ? _host.ToLowerInvariant()
            : string.Empty;

        public int Port()
            => !string.IsNullOrWhiteSpace(_port)
            ? int.Parse(_port)
            : 80;

        public string User()
            => string.IsNullOrWhiteSpace(_user)
            ? _user.ToLowerInvariant()
            : string.Empty;

        public string Password()
            => !string.IsNullOrWhiteSpace(_password)
            ? _password.ToLowerInvariant()
            : string.Empty;

        public string ConnectionString()
            => (string.IsNullOrWhiteSpace(_user) || string.IsNullOrWhiteSpace(_password))
                    ? $@"mongodb+srv://{_host}"
                    : $@"mongodb+srv://{_user}:{WebUtility.UrlEncode(_password)}@{_host}";
    }
}