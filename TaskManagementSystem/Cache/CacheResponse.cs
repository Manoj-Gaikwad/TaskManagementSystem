using Microsoft.Extensions.Caching.Memory;
using System;
using TaskManagementSystem.Models;

namespace TaskManagementSystem.Cache
{
    public class CacheResponse:ICacheResponse
    {
        private readonly IMemoryCache _memoryCache;
       public CacheResponse(IMemoryCache memoryCache)
        {
            this._memoryCache = memoryCache;
        }
        //Cache is useful for if api called firsttime its store data in cache for 10 minutes,
        //befor 10 minutes you called api its provide same data for 10 minutes not hitting api
        //after 10 minutes complite then api can hit 
        public void Add(string userId,LoginResponse loginResponse)
        {
            this._memoryCache.Set(userId, loginResponse,new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10)));
        }

        public void Remove(string userId)
        {
            this._memoryCache.Remove(userId);
        }

        public LoginResponse Get(string userId)
        {
          return  this._memoryCache.Get<LoginResponse>(userId);
        }
    }
}
