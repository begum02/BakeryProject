namespace BakerUI.Dto.MessageDto
{
    public class CreateMessageDto
    {
        public string NameSurname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string MessageDetail { get; set; } = null!;
    }
}