using System.Text.Json;
using System.Text.Json.Serialization;
using EShop.Basket.Domain.Basket.Entities;

namespace EShop.Basket.DataSource.JsonConverters;

public class ShoppingCartItemConverter: JsonConverter<ShoppingCartItemEntity>
{
    public override ShoppingCartItemEntity? Read(
        ref Utf8JsonReader reader, 
        Type typeToConvert, 
        JsonSerializerOptions options
    )
    {
        var jsonDocument = JsonDocument.ParseValue(ref reader);
        var rootElement = jsonDocument.RootElement;

        var id = rootElement.GetProperty("id").GetGuid();
        var shoppingCartId = rootElement.GetProperty("shoppingCartId").GetGuid();
        var productId = rootElement.GetProperty("productId").GetGuid();
        var quantity = rootElement.GetProperty("quantity").GetInt32();
        var color = rootElement.GetProperty("color").GetString()!;
        var price = rootElement.GetProperty("price").GetDecimal();
        var productName = rootElement.GetProperty("productName").GetString()!;

        return new ShoppingCartItemEntity(id, shoppingCartId, productId, quantity, color, price, productName)
        {
            Id = Guid.Empty
        };
    }

    public override void Write(Utf8JsonWriter writer, ShoppingCartItemEntity value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WriteString("id", value.Id.ToString());
        writer.WriteString("shoppingCartId", value.ShoppingCartId.ToString());
        writer.WriteString("productId", value.ProductId.ToString());
        writer.WriteNumber("quantity", value.Quantity);
        writer.WriteString("color", value.Color);
        writer.WriteNumber("price", value.Price);
        writer.WriteString("productName", value.ProductName);

        writer.WriteEndObject();
    }
}