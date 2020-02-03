namespace Deviser.Admin.Config
{
    public enum ValidationType
    {
        None=0,
        Email = 1,
        NumberOnly = 2,
        LettersOnly = 3,
        Password = 4,
        UserExist = 5,
        UserExistByEmail = 6,
        RegEx = 7,
        Custom = 8
    }
}
