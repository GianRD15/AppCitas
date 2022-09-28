namespace AppCitas.Service.Entities.DOTs;

public class MemberDTO
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public int Age { get; set; }
    public string Knowas { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime LastActive { get; set; } = DateTime.Now;
    public string Gender { get; set; }
    public string Introduccion { get; set; }
    public string LookingFor { get; set; }
    public string Interests { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public ICollection<PhotoDTO> Photos { get; set; }
}
