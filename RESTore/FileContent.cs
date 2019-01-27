namespace RESTore
{
    public class FileContent
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string ContentDispositionName { get; set; }
        public byte[] Content { get; set; }
    }
}
