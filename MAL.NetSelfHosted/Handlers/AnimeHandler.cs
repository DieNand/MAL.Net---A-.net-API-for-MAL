using System.Threading.Tasks;
using MAL.NetLogic.Interfaces;
using MAL.NetSelfHosted.Interfaces;

namespace MAL.NetSelfHosted.Handlers
{
    public class AnimeHandler : IAnimeHandler
    {
        #region Variables

        private readonly IAnimeRetriever _retriever;
        private readonly IMappingToJson _jsonMapper;

        #endregion

        #region Constructor

        public AnimeHandler(IAnimeRetriever retriever, IMappingToJson jsonMapper)
        {
            _retriever = retriever;
            _jsonMapper = jsonMapper;
        }

        #endregion

        #region Public Methods

        public async Task<string> HandleRequest(int id)
        {
            var obj = await _retriever.GetAnime(id);
            var jsonObj = _jsonMapper.ConvertAnimeToJson(obj);

            return jsonObj;
        }

        #endregion

    }

}