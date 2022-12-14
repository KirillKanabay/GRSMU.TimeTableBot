namespace GRSMU.Bot.Common.Common.CoR
{
    public abstract class ChainHandlerBase<TQuery> 
        where TQuery : class
    {
        private ChainHandlerBase<TQuery> _next;

        public void Add(ChainHandlerBase<TQuery> handler)
        {
            if (_next != null)
            {
                _next.Add(handler);
            }
            else
            {
                _next = handler;
            }
        }

        public virtual Task Handle(TQuery query) => _next?.Handle(query) ?? Task.CompletedTask;
    }
}
