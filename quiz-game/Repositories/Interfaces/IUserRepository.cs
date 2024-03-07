using quiz_game.Database.Entites;
using quiz_game.Models.Commands;

namespace quiz_game.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task<UserEntity> RegisterUser(UserCommands command);
        public UserEntity LoginUser(UserCommands command);
        public Task<UserEntity> Logout(LogoutCommand command);
    }
}
