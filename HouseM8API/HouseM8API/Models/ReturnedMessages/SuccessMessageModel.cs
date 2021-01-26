namespace HouseM8API.Models.ReturnedMessages
{
    public class SuccessMessageModel
    {
        public SuccessMessageModel(string message)
        {
            this.Success = message;
        }
        public string Success { get; init; }
    }
}
