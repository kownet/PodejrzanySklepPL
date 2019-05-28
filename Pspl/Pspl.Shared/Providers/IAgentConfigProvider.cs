namespace Pspl.Shared.Providers
{
    public interface IAgentConfigProvider
    {
        bool Loop();
        int TimeThreshold();
        string Url();
    }

    public class AgentConfigProvider : IAgentConfigProvider
    {
        private readonly string _loop;
        private readonly string _timeThreshold;
        private readonly string _url;

        public AgentConfigProvider(
            string loop,
            string timeThreshold,
            string url)
        {
            _loop = loop;
            _timeThreshold = timeThreshold;
            _url = url;
        }

        public int TimeThreshold()
            => string.IsNullOrWhiteSpace(_timeThreshold)
            ? int.Parse(_timeThreshold)
            : 60;

        public bool Loop()
            => string.IsNullOrWhiteSpace(_loop)
            ? bool.Parse(_loop)
            : true;

        public string Url()
            => string.IsNullOrWhiteSpace(_url)
            ? _url.ToLowerInvariant()
            : string.Empty;
    }
}