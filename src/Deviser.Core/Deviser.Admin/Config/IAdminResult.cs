namespace Deviser.Admin.Config
{
    public interface IAdminResult
    {
        string SuccessMessage { get; set; }
        string ErrorMessage { get; set; }
        
        object Result { get; }
        bool IsSucceeded { get; set; }
    }

    public interface IAdminResult<out TResult> : IAdminResult
    {
        TResult Value { get; }
    }
}
