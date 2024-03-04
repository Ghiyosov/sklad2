using System.Xml.XPath;
using Dapper;
using Domein.Models;

namespace Infrastraction.Services;

public class SkladService
{
    private readonly DapperContext _context;
    public SkladService()
    {
        _context = new DapperContext();
    }
    public void AddSklad(Sklad sklad)
    {
        var sql = "inser into sklad(name,address)values(@name,@address)";
        _context.Connection().Execute(sql, sklad);
    }
    public List<Product> GetProdactFromSklad(int id)
    {
        var sql = @"select * from product as p where p.skladid=@id";

        var result = _context.Connection().Query<Product>(sql, new { Id = id }).ToList();
        return result;
    }
}
