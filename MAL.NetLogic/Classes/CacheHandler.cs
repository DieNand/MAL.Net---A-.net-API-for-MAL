using System;
using System.Collections.Concurrent;
using System.Runtime.Caching;
using System.Threading.Tasks;
using MAL.NetLogic.Interfaces;
using Serilog;

namespace MAL.NetLogic.Classes
{
    public class CacheHandler : ICacheHandler
    {
        #region Variables

        private readonly IAnimeRetriever _animeRetriever;
        private readonly MemoryCache _animeCahce;
        private const string AnimeCache = "AnimeCache";
        private readonly ConcurrentDictionary<string, object> _animePadlock;  

        #endregion

        #region Constructor

        public CacheHandler(IAnimeRetriever animeRetriever)
        {
            _animeCahce = new MemoryCache(AnimeCache);
            _animePadlock = new ConcurrentDictionary<string, object>();
            _animeRetriever = animeRetriever;
        }

        #endregion

        #region Public Methods

        public async Task<IAnime> GetAnime(int id)
        {
            IAnime finalItem;
            var item = _animeCahce.Get(id.ToString());
            if (item == null)
            {
                Log.Information("Cache miss for {AnimeId}", id);

                var anime = await _animeRetriever.GetAnime(id);
                finalItem = anime;
                var cip = new CacheItemPolicy
                {
                    AbsoluteExpiration = DateTime.Now.AddHours(1),
                    RemovedCallback = RemovedCallback
                };
                lock (_animePadlock.GetOrAdd(id.ToString(), new object()))
                {
                    item = _animeCahce.Get(id.ToString());
                    if (item == null)
                    {
                        _animeCahce.Add(id.ToString(), finalItem, cip);
                        Log.Information("Added {AnimeId} to cache", id);
                    }
                }
            }
            else
            {
                Log.Information("Cache hit for {AnimeId}", id);
                finalItem = (IAnime) item;
            }
            return finalItem;
        }

        public async Task<IAnime> GetAnime(int id, string username, string password)
        {
            Log.Information("User spesific request - Ignoring cache");
            return await _animeRetriever.GetAnime(id, username, password);
        }

        #endregion

        #region Private Methods

        private void RemovedCallback(CacheEntryRemovedArguments arguments)
        {
            Log.Information("{AnimeId} cache expired. Removed from cache", arguments.CacheItem.Key);
        }

        #endregion
    }
}