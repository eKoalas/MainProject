using MainProject.DTOs;

namespace MainProject.Services.Abstract
{
    public interface IAuthService
    {

        //string döndürecez

        string Login(string username, string password);
        bool Register(string username, string password);
        List<UserResponseDto> GetAllUsers();



    }
}
