namespace ProductRestApi.Common.Constants
{
    public static class ConstMessages
    {
        public static string Success = "Success.";
        public static string Fail = "Error!";

        public static string NotFound404Generic(int id) => $"No record found with ID -{id}-.";
        public static string ProductAddedSuccessfully => "Product added successfully.";

        public static string DUPLICATE_PRODUCT => "DUPLICATE_PRODUCT";

        public static string DUPLICATE_PRODUCT_Description(string product) =>
            $"The value -{product}- already exists in the system. Please try a different name.";

        public static string REQUEST_NULL_OR_INVALID => "REQUEST_NULL_OR_INVALID";
        public static string REQUEST_NULL_OR_INVALID_Description => "The request is null or invalid.";

        public static string INVALID_INPUT => "INVALID_INPUT";
        public static string INVALID_INPUT_Description => "The entered data is invalid.";

        public static string DATA_NOTFOUND => "DATA_NOTFOUND";
        public static string DATA_NOTFOUND_Description(string data) => $"Data named {data} was not found.";

        public static string INTERNAL_SERVER_ERROR => "INTERNAL_SERVER_ERROR";
        public static string INTERNAL_SERVER_ERROR_Description => "An internal server error occurred.";

        public static string DELETE_FAILED => "DELETE_FAILED";
        public static string DELETE_FAILED_Description => "An error occurred during the delete operation.";

        public static string UNAUTHORIZED => "UNAUTHORIZED";
        public static string UNAUTHORIZED_Description => "You are not authorized to perform this action.";

        public static string FORBIDDEN => "FORBIDDEN";
        public static string FORBIDDEN_Description => "You do not have permission to access this resource.";

        public static string BAD_REQUEST => "BAD_REQUEST";

        public static string BAD_REQUEST_Description =>
            "The request could not be understood or was missing required parameters.";

        public static string XProductFound(int count) => $"{count} cities found.";

        public static string Product_Name_NullError => "Product name cannot be null.";
        public static string Product_Name_EmptyError => "Product name cannot be empty.";
        public static string Product_Name_MaxLengthError => "Product name is too long.";
        public static string Product_About_MaxLengthError => "Product about is too long.";
        
        public static string LogStartFinishDash => "------------------------------------------------------------------------";
    }
}