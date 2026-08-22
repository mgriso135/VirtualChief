namespace VirtualChief.Components.Customer
{
    /// <summary>
    /// Flat display row for the customer register (legacy Views/Customer/List.cshtml
    /// bound the same fields of KIS.App_Code.Cliente). A DTO keeps the fat domain
    /// entity out of the Blazor circuit serialization.
    /// </summary>
    public record CustomerRow(
        string Code,
        string BusinessName,
        string VatNumber,
        string Identification,
        string Town,
        string Province,
        string State,
        string Phone,
        string Email);
}
