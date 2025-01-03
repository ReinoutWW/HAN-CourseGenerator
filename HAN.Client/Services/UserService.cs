using HAN.Client.Services.Models;

namespace HAN.Client.Services;

public class UserService(IProfileApiAdapter profileApiAdapter)
{
    public async Task<Profile> GetUserProfile()
    {
        try
        {
            return await profileApiAdapter.GetUserProfile();
        }
        catch (UnauthorizedAccessException)
        {
            throw new UnauthorizedAccessException("Not able to retreive user profile :(. Unauthorized");
        }
        catch (Exception ex)
        {
            throw new Exception("Not able to retreive user profile :(. Skill issue");
        }
    }
}