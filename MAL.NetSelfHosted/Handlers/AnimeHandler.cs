using System.Threading.Tasks;
using MAL.NetLogic.Interfaces;
using MAL.NetSelfHosted.Interfaces;

namespace MAL.NetSelfHosted.Handlers
{
    public class AnimeHandler : IAnimeHandler
    {
        #region Variables

        private readonly ICacheHandler _cacheHandler;
        private readonly IMappingToJson _jsonMapper;

        #endregion

        #region Constructor

        public AnimeHandler(ICacheHandler cacheHandler, IMappingToJson jsonMapper)
        {
            _cacheHandler = cacheHandler;
            _jsonMapper = jsonMapper;
        }

        #endregion

        #region Public Methods

        public async Task<string> HandleRequest(int id)
        {
            var obj = await _cacheHandler.GetAnime(id);
            var jsonObj = _jsonMapper.ConvertAnimeToJson(obj);

            return jsonObj;
        }

        public async Task<string> HandleRequest(int id, string username, string password)
        {
            //var obj = await _retriever.GetAnime(id, username, password);
            var obj = await _cacheHandler.GetAnime(id, username, password);
            var jsonObj = _jsonMapper.ConvertAnimeToJson(obj);

            return jsonObj;
        }

        #endregion

    }

}