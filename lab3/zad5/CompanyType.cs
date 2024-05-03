namespace zad5;

internal static class CompanyType
{
    public const string CUSTOMER = "customer";
    public const string SUPPLIER = "supplier";

    public static List<string> AllTypes = new()
    {
        CUSTOMER,
        SUPPLIER
    };
}