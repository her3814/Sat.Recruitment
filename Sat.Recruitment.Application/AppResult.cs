namespace Sat.Recruitment.Application
{
    public class AppResult<T> : AppResult
    {
        public T Data { get; set; }
        public AppResult() : base() { }
        public AppResult(T obj) : this()
        {
            Data = obj;
        }
    }
    public class AppResult
    {
        public bool IsSuccess { get { return !Errors.Any(); } }
        public List<AppError> Errors { get; private set; }

        public AppResult()
        {
            Errors = new List<AppError>();
        }

        public string ErrorsText { get { return string.Join(", ", Errors.Select(p => p.Message)); } }

        private void AddError(string message, AppError.ErrorTypeEnum type)
        {
            Errors.Add(new AppError { Message = message, Type = type });
        }

        public void AddInputDataError(string message)
        {
            AddError(message, AppError.ErrorTypeEnum.InputDataError);
        }

        public void AddBusinessError(string message)
        {
            AddError(message, AppError.ErrorTypeEnum.BusinessError);
        }

        public void AddInternalError(string message)
        {
            AddError(message, AppError.ErrorTypeEnum.InternalError);
        }
    }

}