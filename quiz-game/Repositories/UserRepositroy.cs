using AutoMapper;
using Microsoft.EntityFrameworkCore;
using quiz_game.Database;
using quiz_game.Database.Entites;
using quiz_game.Models.Commands;
using quiz_game.Repositories.Interfaces;

namespace quiz_game.Repositories
{
    public class UserRepositroy : IUserRepository
    {
        private readonly QuizGameContext _dbContext;
        private readonly IMapper _mapper;

        public UserRepositroy(QuizGameContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public  UserEntity LoginUser(UserCommands command)
        {
            UserEntity userEntity = null;
            try { 
                userEntity =  _dbContext.Users.FirstOrDefault(u => u.Username == command.Username);
                if (userEntity == null || userEntity.Password != command.Password)
                {
                    throw new Exception("Username or password dosn't match");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return userEntity;
        }

        public async Task<UserEntity> RegisterUser(UserCommands command)
        { 

            UserEntity userEntity = _mapper.Map<UserEntity>(command);
            
            await _dbContext.Users.AddAsync(userEntity);
            await _dbContext.SaveChangesAsync();

            return userEntity;

        }
        public async Task<UserEntity> Logout(LogoutCommand command)
        {
            UserEntity user = await _dbContext.Users.FirstOrDefaultAsync(e => e.Username == command.Username);
            if(user == null)
            {
                throw new Exception($"Username {command.Username} dosne't exist ");
            }
            if (user.BestScore < command.Score)
            {
                user.BestScore = command.Score;
                _dbContext.Update(user);
                await _dbContext.SaveChangesAsync();
            }
            return user;

        }
    }
}
