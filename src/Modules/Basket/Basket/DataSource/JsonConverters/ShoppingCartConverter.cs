using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using EShop.Basket.Domain.Basket.Entities;

namespace EShop.Basket.DataSource.JsonConverters;

public class ShoppingCartConverter: JsonConverter<ShoppingCartEntity>
{
    public override ShoppingCartEntity? Read(
        ref Utf8JsonReader reader, 
        Type typeToConvert, 
        JsonSerializerOptions options
    )
    {
        var jsonDocument = JsonDocument.ParseValue(ref reader);
        var rootElement = jsonDocument.RootElement;

        var id = rootElement.GetProperty("id").GetGuid();
        var userName = rootElement.GetProperty("userName").GetString()!;
        var itemsElement = rootElement.GetProperty("items");

        var shoppingCart = ShoppingCartEntity.Create(id, userName);

        var items = itemsElement.Deserialize<List<ShoppingCartItemEntity>>(options);
        if (items == null) return shoppingCart;
        
        var itemsField = typeof(ShoppingCartEntity)
            .GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance);
        itemsField?.SetValue(shoppingCart, items);

        return shoppingCart;
    }

    public override void Write(Utf8JsonWriter writer, ShoppingCartEntity value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WriteString("id", value.Id.ToString());
        writer.WriteString("userName", value.UserName);

        writer.WritePropertyName("items");
        JsonSerializer.Serialize(writer, value.Items, options);

        writer.WriteEndObject();
    }
}