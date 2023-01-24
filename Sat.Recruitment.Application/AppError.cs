namespace Sat.Recruitment.Application
{
    public class AppError
    {
        public string Message { get; set; }
        public ErrorTypeEnum Type { get; set; }

        public enum ErrorTypeEnum
        {
            InputDataError,
            BusinessError,
            InternalError
        }
    }
}