namespace AppCitas.Service.Helpers;

public class LikesParams : PaginationParams
{
    public int UserID { get; set; }
    public string Predicate { get; set; }
}
