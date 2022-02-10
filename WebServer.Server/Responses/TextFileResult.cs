namespace WebServer.Server.Responses
{
    using HTTP;

    public class TextFileResult : ActionResult
    {
        public TextFileResult(Response response, string fileName) 
            : base(response)
        {
            this.FileName = fileName;

            this.Headers.Add(Header.ContentType, ContentType.PlainText);
        }

        public string FileName { get; init; }

        public override string ToString()
        {
            if (File.Exists(this.FileName))
            {
                this.Body = File.ReadAllTextAsync(this.FileName).Result;

                var fileBytesCount = new FileInfo(this.FileName).Length;
                this.Headers.Add(Header.ContentLength, fileBytesCount.ToString());

                this.Headers.Add(Header.ContentDisposition, $"attachment; filename\"{this.FileName}\"");
            }

            return base.ToString();
        }
    }
}
