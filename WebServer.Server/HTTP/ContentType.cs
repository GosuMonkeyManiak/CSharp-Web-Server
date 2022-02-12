namespace WebServer.Server.HTTP
{
    public static class ContentType
    {
        public const string PlainText = "text/plain; charset=UTF-8";
        public const string Html = "text/html; charset=UTF-8";
        public const string FormUrlEncoded = "application/x-www-form-urlencoded";

        public static string GetTypeByFileExtension(string fileExtension)
            => fileExtension switch
            {
                "css" => "text/css",
                "js" => "application/javascript",
                "ico" => "image/x-icon",
                "pdf" => "application/pdf",
                "jpeg" or "jpg" => "image/jpeg",
                "xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "pptx" => "application/vnd.openxmlformats-officedocument.presentationml.presentation",
                "docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                _ => PlainText
            };
    }
}
