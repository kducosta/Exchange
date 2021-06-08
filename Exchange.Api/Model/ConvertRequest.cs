namespace Exchange.Api.Model
{
    public class ConvertRequest
    {
        public string From { get; set; }

        public string To { get; set; }

        public float Amount { get; set; } = 1f;
    }
}