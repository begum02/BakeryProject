namespace BakerWebAPI.Dto.MessageDto
{
    public class UpdateMessageDto
    {
        public int MessageId { get; set; }
       
        public bool IsRead { get; set; }
    }
}