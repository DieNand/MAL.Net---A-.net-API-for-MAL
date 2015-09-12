using System;
using System.Collections.Concurrent;
using System.Runtime.Caching;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MAL.NetLogic.Interfaces;

namespace MAL.NetLogic.Classes
{
    public class CacheHandler : ICacheHandler
    {
        #region Variables

        private readonly IAnimeRetriever _animeRetriever;
        private readonly IUserAuthentication _userAuthentication;
        private readonly MemoryCache _animeCahce;
        private readonly MemoryCache _authCache;
        public const string AnimeCache = "AnimeCache";
        public const string AuthCache = "AuthCache";
        public readonly ConcurrentDictionary<string, object> AnimePadlock;
        public readonly ConcurrentDictionary<string, object> AuthPadlock; 

        #endregion

        #region Constructor

        public CacheHandler(IAnimeRetriever animeRetriever, IUserAuthentication userAuthentication)
        {
            _animeCahce = new MemoryCache(AnimeCache);
            AnimePadlock = new ConcurrentDictionary<string, object>();
            _authCache = new MemoryCache(AuthCache);
            AuthPadlock = new ConcurrentDictionary<string, object>();
            _animeRetriever = animeRetriever;
            _userAuthentication = userAuthentication;
        }

        #endregion

        #region Public Methods

        public async Task<IAnime> GetAnime(int id)
        {
            IAnime finalItem;
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

        public async Task<ILoginData> GetAuth(string username, string password, bool canCache = true)
        {
            ILoginData finalItem;
            if (canCache)
            {
                var sha256 = SHA256.Create();
                var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes($"{username}{password}"));
                //We use a hash of the username and password to cache the values if the login was a success
                var userToken = BitConverter.ToString(hash);

                var item = _authCache.Get(userToken);
                if (item == null)
                {
                    Console.WriteLine($"{DateTime.Now} - [Cache] Cache miss for [{userToken}]");
                    var loginData = _userAuthentication.Login(username, password);
                    if (!loginData.LoginValid) return loginData;
                    finalItem = loginData;
                    var cip = new CacheItemPolicy
                    {
                        AbsoluteExpiration = DateTime.Now.AddHours(1),
                        RemovedCallback = RemovedAuthCallback
                    };
                    lock (AuthPadlock.GetOrAdd(userToken, new object()))
                    {
                        item = _authCache.Get(userToken);
                        if (item != null) return finalItem;
                        _authCache.Add(userToken, finalItem, cip);
                        Console.WriteLine($"{DateTime.Now} - [Cache] Added to cache [{userToken}]");
                    }
                }
                else
                {
                    Console.WriteLine($"{DateTime.Now} - [Cache] Cache hit [{userToken}]");
                    finalItem = (ILoginData) item;
                }
            }
            else
            {
                Console.WriteLine($"{DateTime.Now} - [Cache] Requesting login with NoCache");
                return _userAuthentication.Login(username, password, false);
            }
            return finalItem;
        }

        #endregion

        #region Private Methods

        private void RemovedCallback(CacheEntryRemovedArguments arguments)
        {
            Console.WriteLine($"{DateTime.Now} - [Cache] {arguments.CacheItem.Key} expired. Removed from cache");
        }

        private void RemovedAuthCallback(CacheEntryRemovedArguments arguments)
        {
            Console.WriteLine($"{DateTime.Now} - [Cache] {arguments.CacheItem.Key} expired. Removed from cache");
        }

        #endregion
    }
}