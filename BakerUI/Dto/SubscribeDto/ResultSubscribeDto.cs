namespace BakerUI.Dto.SubscribeDto
{
    public class ResultSubscribeDto
    {
        public int SubscribeId { get; set; }
        public string Email { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
