using System.Linq.Expressions;
using EShop.Basket.Domain.Basket.Entities;
using EShop.Shared.Domain.Specifications;

namespace EShop.Basket.DataSource.Specifications;

public class BasketByUserNameSpecification: Specification<ShoppingCartEntity>
{
   
    private readonly string _userName;

    public BasketByUserNameSpecification(string userName)
    {
        _userName = userName;
        AddInclude(x => x.Items);
    }
    
    public override Expression<Func<ShoppingCartEntity, bool>> ToExpression()
    {
        return  shoppingCart => shoppingCart.UserName == _userName;
    }
}