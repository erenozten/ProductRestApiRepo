using ProductRestApi.Entities;

namespace ProductRestApi.Common.Helpers;

public static class ProductLogHelper
{
    public static void LogProductNotFound(ILogger logger, int id)
    {
        logger.LogWarning("[PRODUCT_NOT_FOUND] Product not found. {@Product}", new Product { Id = id });
    }

    public static void LogProductRetrieved(ILogger logger, Product product)
    {
        logger.LogInformation("[PRODUCT_RETRIEVED] Product found. {@Product}", new Product
        {
            Id = product.Id,
            Name = product.Name,
            About = product.About
        });
    }

    public static void LogTotalProductCount(ILogger logger, int count)
    {
        logger.LogInformation("[PRODUCT_TOTAL_COUNT] Total {Count} products retrieved.", count);
    }

    public static void LogProductDeleteFailed(ILogger logger, int id)
    {
        logger.LogWarning("[PRODUCT_DELETE_FAILED] Product was found but failed to delete. {@Product}",
            new Product { Id = id });
    }

    public static void LogProductDeleted(ILogger logger, int id)
    {
        logger.LogInformation("[PRODUCT_DELETED] Product successfully deleted. {@Product}", new Product { Id = id });
    }

    public static void LogInvalidId(ILogger logger, int id, string operation)
    {
        logger.LogWarning("[INVALID_ID] Invalid ID for {Operation}. {@Product}", operation, new Product { Id = id });
    }

    public static void LogValidationFailed(ILogger logger, string operation)
    {
        logger.LogWarning("[VALIDATION_FAILED] {Operation} request validation failed.", operation);
    }

    public static void LogDuplicateProductName(ILogger logger, string name)
    {
        logger.LogWarning("[DUPLICATE_NAME] Duplicate product name: {Name}", name);
    }

    public static void LogProductUpdated(ILogger logger, Product product)
    {
        logger.LogInformation("[PRODUCT_UPDATED] Product successfully updated. {@Product}", new Product
        {
            Id = product.Id,
            Name = product.Name,
            About = product.About
        });
    }

    public static void LogProductCreated(ILogger logger, Product product)
    {
        logger.LogInformation("[PRODUCT_CREATED] New product created. {@Product}", new Product
        {
            Id = product.Id,
            Name = product.Name,
            About = product.About
        });
    }

    public static void LogProductPatched(ILogger logger, Product product)
    {
        logger.LogInformation("[PRODUCT_PATCHED] Product successfully patched. {@Product}", new Product
        {
            Id = product.Id,
            Name = product.Name,
            About = product.About
        });
    }
}