namespace WebServer.Server.Results
{
    using Common;
    using HTTP;

    public class TextFileResult : ActionResult
    {
        public TextFileResult(Response response, string fileName, string disposition = Header.AttachmentFile)
            : base(response)
        {
            this.FileName = fileName;
            this.Disposition = disposition;
        }

        public string FileName { get; init; }

        public string Disposition { get; init; }

        public override string ToString()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var wwwRootDirectory = Path.Combine(currentDirectory, Settings.FilesFolder);
            var fileDirectory = Path.Combine(wwwRootDirectory, this.FileName);

            if (!File.Exists(fileDirectory))
            {
                throw new InvalidOperationException("File doesn't exist");
;           }

            var contentBytes = File.ReadAllBytes(fileDirectory);

            var fileExtension = Path.GetExtension(fileDirectory).TrimStart('.');
            var contentType = ContentType.GetTypeByFileExtension(fileExtension);

            var fileName = Path.GetFileName(fileDirectory);

            string disposition = string.Format(Header.AttachmentFile, fileName);

            if (this.Disposition == Header.InlineFile)
            {
                disposition = Header.InlineFile;
            }

            this.Headers.Add(Header.ContentDisposition, disposition);

            this.SetContent(contentBytes, contentType);

            return base.ToString();
        }
    }
}
