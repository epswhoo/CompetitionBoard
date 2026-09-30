namespace WebApp.Services.Login
{
    public enum AccessLevel
    {
        /// <summary>Noch nicht angemeldet: nur die Passwortabfrage.</summary>
        Locked,

        /// <summary>Bedienung der Tafel.</summary>
        Operator,

        /// <summary>Bedienung der Tafel und Ändern des Passworts.</summary>
        Admin
    }
}
