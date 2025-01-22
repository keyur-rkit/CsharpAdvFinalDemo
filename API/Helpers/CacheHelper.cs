using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace API.Helpers
{
    /// <summary>
    /// A helper class for interacting with the ASP.NET Cache.
    /// Provides methods for retrieving, inserting, and removing items from the cache.
    /// </summary>
    public static class CacheHelper
    {
        /// <summary>
        /// Retrieves the cached item associated with the specified key.
        /// </summary>
        /// <param name="key">The unique identifier for the cached item.</param>
        /// <returns>The cached object if found, otherwise null.</returns>
        public static object Get(string key)
        {
            // Return the cached object associated with the key, or null if not found
            return HttpContext.Current.Cache[key];
        }

        /// <summary>
        /// Inserts or updates a cache entry with the specified key, value, and expiration duration.
        /// </summary>
        /// <param name="key">The unique key for the cache entry.</param>
        /// <param name="value">The value to store in the cache.</param>
        /// <param name="duration">The duration for which the item should remain in the cache.</param>
        public static void Set(string key, object value, TimeSpan duration)
        {
            // Insert the value into the cache with the specified duration and an absolute expiration time
            HttpContext.Current.Cache.Insert(key, value, null, DateTime.Now.Add(duration), Cache.NoSlidingExpiration);
        }

        /// <summary>
        /// Removes the cached item associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the cached item to remove.</param>
        public static void Remove(string key)
        {
            // Remove the cached item associated with the key
            HttpContext.Current.Cache.Remove(key);
        }
    }
}