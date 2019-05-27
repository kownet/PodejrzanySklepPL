namespace Pspl.Shared.Providers
{
    public interface IAgentConfigProvider
    {
        bool Loop();
        int TimeThreshold();
    }

    public class AgentConfigProvider : IAgentConfigProvider
    {
        private readonly string _loop;
        private readonly string _timeThreshold;

        public AgentConfigProvider(
            string loop,
            string timeThreshold)
        {
            _loop = loop;
            _timeThreshold = timeThreshold;
        }

        public int TimeThreshold() => int.Parse(_timeThreshold);
        public bool Loop() => bool.Parse(_loop);
    }
}