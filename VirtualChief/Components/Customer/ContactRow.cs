namespace VirtualChief.Components.Customer
{
    /// <summary>
    /// Flat display row for the contacts bound to a customer (legacy
    /// ContattiClienti.ascx repeater row). Phones/emails are pre-joined into
    /// display strings exactly like rptContattiClienti_ItemDataBound did, so the
    /// fat KIS.App_Code.Contatto entity never crosses the Blazor circuit.
    /// </summary>
    public record ContactRow(
        int Id,
        string FirstName,
        string LastName,
        string Role,
        string Phones,
        string Emails);
}
