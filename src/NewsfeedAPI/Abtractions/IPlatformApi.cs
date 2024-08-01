using Refit;

namespace NewsfeedAPI.Abtractions
{
    public interface IPlatformApi
    {
        [Get("/user/list")]
        Task<object> GetUserAsync();
    }
}
