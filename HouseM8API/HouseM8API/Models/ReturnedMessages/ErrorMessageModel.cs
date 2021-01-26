namespace HouseM8API.Models
{
    public class ErrorMessageModel
    {
        public ErrorMessageModel(string error)
        {
            this.Error = error;
        }

        public string Error { get; init; }
    }
}
