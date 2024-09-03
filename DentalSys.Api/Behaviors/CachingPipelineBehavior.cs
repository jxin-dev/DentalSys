using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
//using Microsoft.Extensions.Caching.Memory; // Use for memory cache service

namespace DentalSys.Api.Behaviors
{
    public class CachingPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : class
    {
        // Use for memory cache service
        //private readonly IMemoryCache _cache;
        //public CachingPipelineBehavior(IMemoryCache cache)
        //{
        //    _cache = cache;
        //}

        private readonly IDistributedCache _cache;

        public CachingPipelineBehavior(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {

            // Use for memory cache service
            //var cacheKey = GetCacheKey(request);
            //if (_cache.TryGetValue(cacheKey, out TResponse cachedResponse))
            //{
            //    return cachedResponse;
            //}
            //var response = await next();

            //_cache.Set(cacheKey, response, TimeSpan.FromSeconds(60));


            var cacheKey = GetCacheKey(request);
            var cachedResponse = await _cache.GetStringAsync(cacheKey);

            TResponse? response;

            if (!string.IsNullOrEmpty(cachedResponse))
            {
                try
                {
                    response = JsonConvert.DeserializeObject<TResponse>(cachedResponse);
                    return response!;
                }
                catch (JsonException)
                {
                    await _cache.RemoveAsync(cacheKey);
                }
            }

            response = await next();

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(60) 
            };

            await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(response), options);

            return response;
        }

        private string GetCacheKey(TRequest request)
        {
            return $"Cache_{request!.GetHashCode()}";
        }
    }
}
