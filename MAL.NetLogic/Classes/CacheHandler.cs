using System;
using System.Collections.Concurrent;
using System.Runtime.Caching;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using MAL.NetLogic.Interfaces;

namespace MAL.NetLogic.Classes
{
    public class CacheHandler : ICacheHandler
    {
        #region Variables

        private readonly IAnimeRetriever _animeRetriever;
        private readonly MemoryCache _animeCahce;
        private readonly IConsoleWriter _consoleWriter;
        public const string AnimeCache = "AnimeCache";
        public readonly ConcurrentDictionary<string, object> AnimePadlock;  

        #endregion

        #region Constructor

        public CacheHandler(IAnimeRetriever animeRetriever, IConsoleWriter consoleWriter)
        {
            _animeCahce = new MemoryCache(AnimeCache);
            AnimePadlock = new ConcurrentDictionary<string, object>();
            _animeRetriever = animeRetriever;
            _consoleWriter = consoleWriter;
        }

        #endregion

        #region Public Methods

        public async Task<IAnime> GetAnime(int id)
        {
            IAnime finalItem = null;
            var item = _animeCahce.Get(id.ToString());
            if (item == null)
            {
                Console.WriteLine($"{DateTime.Now} - [Cache] Cache miss [{id}]");

                var anime = await _animeRetriever.GetAnime(id);
                finalItem = anime;
                var cip = new CacheItemPolicy
                {
                    AbsoluteExpiration = DateTime.Now.AddHours(1),
                    RemovedCallback = RemovedCallback
                };
                lock (AnimePadlock.GetOrAdd(id.ToString(), new object()))
                {
                    item = _animeCahce.Get(id.ToString());
                    if (item == null)
                    {
                        _animeCahce.Add(id.ToString(), finalItem, cip);
                        Console.WriteLine($"{DateTime.Now} [Cache] Added to cache [{id}]");
                    }
                }
            }
            else
            {
                Console.WriteLine($"{DateTime.Now} - [Cache] Cache hit [{id}]");
                finalItem = (IAnime) item;
            }
            return finalItem;
        }

        public async Task<IAnime> GetAnime(int id, string username, string password)
        {
            Console.WriteLine($"{DateTime.Now} - [Cache] Cache ignore - User spesific request");
            return await _animeRetriever.GetAnime(id, username, password);
        }

        #endregion

        #region Private Methods

        private void RemovedCallback(CacheEntryRemovedArguments arguments)
        {
            Console.WriteLine($"{DateTime.Now} - {_consoleWriter.WriteInline($"[Cache] {arguments.CacheItem.Key} expired. Removed from cache", ConsoleColor.DarkYellow)}");
        }

        #endregion
    }
}