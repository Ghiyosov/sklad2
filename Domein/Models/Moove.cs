namespace Domein.Models;

public class Moove
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int AtSklad { get; set; }
    public int ToSklad { get; set; }
    public int Kol { get; set; }
    public string Times { get; set; }
}
