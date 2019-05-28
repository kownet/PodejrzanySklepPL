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

        public int TimeThreshold() => int.Parse(_timeThreshold);
        public bool Loop() => bool.Parse(_loop);
        public string Url() => _url.ToLowerInvariant();
    }
}