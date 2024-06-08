namespace Qvastart___1
{
    public class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public string? description { get; set; }
        public string? essentialData { get; set; }
        public string? errorMessage { get; set; }
        public bool? ServiceSuccess { get; set; } = false;
    }
}
