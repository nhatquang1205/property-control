namespace PropertyControl.Commons
{
    public class ResponseInfo
    {
        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = default!;
        public Dictionary<string, string> Data { get; set; } = new Dictionary<string, string>();
    }
}