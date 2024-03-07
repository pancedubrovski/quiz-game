using AutoMapper;
using quiz_game.Database.Entites;
using quiz_game.Models.Commands;

namespace quiz_game.Utilites
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {
            CreateMap<UserEntity, UserCommands>();
            CreateMap<UserCommands, UserEntity>();
        }
    }
}
