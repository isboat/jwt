using jwt_api.Controllers;

namespace jwt_api
{
    public class UserService : IUserService
    {
        public UserData Get(UserModel model)
        {
            return new UserData
            {
                Id = Guid.NewGuid().ToString(),
                Username = model.Username,
                Name = "Tom",
            };
        }
    }

    public interface IUserService
    {
        UserData Get(UserModel model);
    }
}