using System.Linq.Expressions;
using EShop.Basket.Domain.Basket.Entities;
using EShop.Shared.Domain.Specifications;

namespace EShop.Basket.DataSource.Specifications;

public class BasketByUserNameSpecification: Specification<ShoppingCartEntity>
{
   
    public string UserName { get; }

    public BasketByUserNameSpecification(string userName)
    {
        UserName = userName;
        AddInclude(x => x.Items);
    }
    
    public override Expression<Func<ShoppingCartEntity, bool>> ToExpression()
    {
        return  shoppingCart => shoppingCart.UserName == UserName;
    }
}