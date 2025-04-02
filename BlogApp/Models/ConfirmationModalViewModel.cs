namespace BlogApp.Models
{
    public class ConfirmationModalViewModel
    {
        public string ModalId { get; set; } = null!;
        public string ModalIdLabel { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string ConfirmUrl { get; set; } = null!;
    }
}
