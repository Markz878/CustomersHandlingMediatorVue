using System;

namespace Core.Abstractions;

public interface ICacheableQuery
{
    public bool BypassCache { get; }
    public string Cachekey { get; }
    public TimeSpan? SlidingExpiration { get; }
}
