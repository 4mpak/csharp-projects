using Lab3.Task1.Application.Abstractions.Repositories;
using Lab3.Task1.Application.Abstractions.Repositories.Queries;
using Lab3.Task1.Domain.Entities;
using Lab3.Task1.Infrastructure.Database.Connections;
using Npgsql;
using System.Data;

namespace Lab3.Task1.Infrastructure.Database.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly PostgresProvider _postgresProvider;

    public ProductRepository(PostgresProvider postgresProvider)
    {
        _postgresProvider = postgresProvider;
    }

    public async Task<long> CreateProductAsync(Product product)
    {
        const string sql = """
                           insert into products(product_name, product_price)
                           values(:product_name, :product_price)
                           returning product_id;
                           """;
        await using NpgsqlConnection connection = await _postgresProvider.ConnectAsync();

        await using var command = new NpgsqlCommand(sql, connection)
        {
            Parameters =
            {
                new NpgsqlParameter("product_name", product.Name),
                new NpgsqlParameter("product_price", product.Price),
            },
        };

        NpgsqlDataReader reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return reader.GetInt64("product_id");
        }

        throw new InvalidOperationException();
    }

    public async IAsyncEnumerable<Product> QueryProductAsync(ProductQuery query)
    {
        const string sql = """
                           select product_id, product_name, product_price
                           from products
                           where
                               (product_id > :cursor)
                           and (cardinality(:ids) = 0 or product_id = any (:ids))
                           and (:min_price is null or product_price > :min_price)
                           and (:max_price is null or product_price <=  :max_price)
                           and (:name_pattern is null or product_name like :name_pattern)
                           limit :page_size;
                           """;
        await using NpgsqlConnection connection = await _postgresProvider.ConnectAsync();

        await using var command = new NpgsqlCommand(sql, connection)
        {
            Parameters =
            {
                new NpgsqlParameter("ids", query.Ids),
                new NpgsqlParameter("name_pattern", query.NamePattern),
                new NpgsqlParameter("min_price", query.MinPrice),
                new NpgsqlParameter("max_price", query.MaxPrice),
                new NpgsqlParameter("cursor", query.Cursor),
                new NpgsqlParameter("page_size", query.PageSize),
            },
        };

        await using NpgsqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            yield return new Product
            {
                Id = DataReaderExtensions.GetInt64(reader, "product_id"),
                Name = DataReaderExtensions.GetString(reader, "product_name"),
                Price = DataReaderExtensions.GetDecimal(reader, "product_price"),
            };
        }
    }
}