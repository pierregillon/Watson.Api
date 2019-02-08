using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Queries;
using StructureMap;

namespace Watson.Infrastructure
{
    public class StructureMapQueryProcessor : IQueryProcessor
    {
        private readonly IContainer _container;

        public StructureMapQueryProcessor(IContainer container)
        {
            _container = container;
        }

        public async Task<TResponse> Query<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default(CancellationToken))
        {
            var type = query.GetType();
            var handlers = _container.GetAllInstances(typeof(IQueryHandler<,>).MakeGenericType(type, typeof(TResponse)));
            var handlerList = new List<object>();
            foreach(var handler in handlers) {
                handlerList.Add(handler);
            }
            if (handlerList.Any() == false)
                throw new InvalidOperationException($"No handler registered for {type.FullName}");
            if (handlerList.Count() != 1)
                throw new InvalidOperationException($"Cannot send to more than one handler of {type.FullName}");
            
            var singleHandler = handlerList.Single();
            return await (Task<TResponse>)singleHandler.GetType().GetMethod("Handle").Invoke(singleHandler, new object[]{ query });
        }
    }
}