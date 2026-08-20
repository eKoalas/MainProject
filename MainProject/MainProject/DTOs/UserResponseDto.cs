namespace MainProject.DTOs
{
    public class UserResponseDto
    {
        public string Username { get; set; }
        public string Role { get; set; }                // şfre koymadık çünkü dışarıdan şifrelere ulaşım istemiyoruz
    }
}