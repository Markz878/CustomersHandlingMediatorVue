namespace Core.Behaviors;

public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : ICacheableQuery, IRequest<TResponse>
{
    private readonly IDistributedCache cache;
    private readonly ILogger<TResponse> logger;
    private readonly CacheSettings cacheSettings;

    public CachingBehavior(IDistributedCache cache, ILogger<TResponse> logger, IOptionsSnapshot<CacheSettings> cacheSettings)
    {
        this.cache = cache;
        this.logger = logger;
        this.cacheSettings = cacheSettings.Value;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        if (request.BypassCache)
        {
            return await next();
        }
        TResponse? response;
        byte[] cachedResponse = await cache.GetAsync(request.Cachekey, cancellationToken);
        if (cachedResponse != null)
        {
            response = JsonSerializer.Deserialize<TResponse>(cachedResponse);
            logger.LogInformation("Retrieved value from cache with key {cachekey}", request.Cachekey);
            return response ?? throw new ArgumentNullException("Cache value");
        }
        response = await next();
        TimeSpan slidingExpiration = request.SlidingExpiration ?? TimeSpan.FromSeconds(cacheSettings.SlidingExpirationSeconds);
        DistributedCacheEntryOptions cacheOptions = new() { SlidingExpiration = slidingExpiration };
        await cache.SetAsync(request.Cachekey, JsonSerializer.SerializeToUtf8Bytes(response), cacheOptions, cancellationToken);
        logger.LogInformation("Set value to cache with key {cachekey}", request.Cachekey);
        return response;
    }
}
