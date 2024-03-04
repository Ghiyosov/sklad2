using Dapper;
using Domein.Models;

namespace Infrastraction.Services;

public class ProductService
{
    private readonly DapperContext _context;
    public ProductService()
    {
        _context = new DapperContext();
    }
    public void AddProdact(Product product)
    {
        Product pname = _context.Connection().QueryFirstOrDefault<Product>("slect * from product as p where p.skladid=@id", new { Id = product.SkladId });
        if (pname == null)//agar nabosha da sklad dobavit mekna
        {
            var sql = "insert into product(name,kol,skladid)values(@name,@kol,@skladid)";
            _context.Connection().Execute(sql, product);
        }
        else if (pname!=null)//agar bosha producy.kol-sha bisyor mekna
        {
            _context.Connection().Execute("update product as p set kol=@kol where p.name=@name,pskladid=@id",new{Kol=(pname.Kol+product.Kol),Name=product.Name,Id=product.SkladId});
        }

    }
}
