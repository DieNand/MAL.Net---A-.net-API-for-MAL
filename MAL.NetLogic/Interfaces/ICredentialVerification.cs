using System.Threading.Tasks;

namespace MAL.NetLogic.Interfaces
{
    public interface ICredentialVerification
    {
        Task<bool> VerifyCredentials(string username, string password);
    }
}