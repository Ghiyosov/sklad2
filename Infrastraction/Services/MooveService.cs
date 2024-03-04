using System.Data.Common;
using Dapper;
using Domein.Models;
namespace Infrastraction.Services;

public class MooveService
{
    private readonly DapperContext _context;
    public MooveService()
    {
        _context = new DapperContext();
    }
    public void AddMoove(Moove moove)
    {
        Product p1 = _context.Connection().QuerySingle<Product>("select * from product as p where p.id = @id", new { Id = moove.ProductId }); //producti roikardagira megirem
        string name = _context.Connection().QueryFirstOrDefault<string>("selekt p.name from product as p where p.id=@id", new { Id = moove.ProductId }); //nomi producta megirem
        Product p2 = _context.Connection().QuerySingleOrDefault<Product>(@"select * 
                                                                           from product as p where name=@name
                                                                           join sklad as s on p.skladid=@id", new { Name = name, Id = moove.ToSklad });//skladi qabulkaragira mebinem ku hast hamikheli product
        if (p1.Kol < moove.Kol) Console.WriteLine($"{name} kol: {p1.Kol} < {moove.Kol} ");
        else if (p2 == null)//mesanja agar nabosha ykta producti nav mesoza ay roikardagi minus mekna
        {
            _context.Connection().Execute("insert into product(name,kol,skladid)values(@name,@kol,@skladid)", new { Name = name, Kol = moove.Kol, SkladId = moove.ToSklad });
            _context.Connection().Execute("update product set kol = @kol where id=@id", new { Kol = (p1.Kol - moove.Kol), Id = moove.ProductId });
        }
        else if (p2 != null)//agar bosha obnavitsh mekna ayroikardagi minus mekna bad dai pilus
        {
            _context.Connection().Execute("update product set kol = @kol where skladid = @id", new { Kol = (p2.Kol + moove.Kol), Id = p2.Id });
            _context.Connection().Execute("update product set kol = @kol where id=@id", new { Kol = (p1.Kol - moove.Kol), Id = moove.ProductId });

        }
    }

    public void EqualizeSklad()
    {
        List<int> skladId = _context.Connection().Query<int>("select s.id from sklad").ToList(); // Id-hoi saklodora megirem 
        List<string> productName = _context.Connection().Query<string>(@"select distinct p.name
                                                                         from product as p").ToList();//nomoi productohora megirem
        int skladCount = _context.Connection().QueryFirstOrDefault<int>("select count(sk.id) from sklad as sk");
        List<ProductCount> productCounts = new List<ProductCount>();
        foreach (var item in productName)
        {
            int pcount = _context.Connection().QueryFirstOrDefault<int>("select sum(p.kol) from product as p where p.name = @name", new { Name = item });
            var pc = new ProductCount();
            pc.Name = item;
            pc.Count = pcount;
            productCounts.Add(pc);
        }
        foreach (var field in productCounts)
        {
            foreach (var item in skladId)
            {
                Product p2 = _context.Connection().QuerySingleOrDefault<Product>(@"select * 
                                                                           from product as p where name=@name
                                                                           join sklad as s on p.skladid=@id", new { Name = field.Name, Id = item });
                if (p2 == null)//mesanja agar nabosha kolichestvoi productora taksimi couti sklado mekna bad da producti nav mesoza 
                {
                    _context.Connection().Execute("insert into product(name,kol,skladid)values(@name,@kol,@skladid)", new { Name = field.Name, Kol = (field.Count / skladCount) });
                }
                else if (p2!=null)//mesnaja agar bohs kolichestvoi productora taksimi couti sklado mekna bad da product.kol update mekna
                {
                    _context.Connection().Execute("update product set kol=@kol where name=@name,skladid=@id",new{Kol =(field.Count / skladCount),Name=field.Name,Id=item });
                }
            }
        }


    }

    public List<Story> MooveInfo()
    {
        var stor = _context.Connection().Query<Story>(@"select p.name as product, s.name as atsklad, sa.name as tosklad  
                                                        from moove as m
                                                        join product as p on m.productid=p.id
                                                        join sklad as s on m.atsklad=s.id
                                                        join sklad as sa on m.tosklad=sa.id").ToList();
        return stor;
    }
}
