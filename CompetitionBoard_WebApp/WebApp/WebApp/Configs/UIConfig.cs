namespace WebApp.Configs
{
    public class UIConfig
    {
        public int EditTime { get; set; }

        public int RefreshTime { get; set; }

        /// <summary>
        /// Port, auf dem die Tafel nur angezeigt wird (ohne Bearbeitung). 0 = kein Anzeige-Port.
        /// </summary>
        public int DisplayPort { get; set; }

        /// <summary>
        /// Passwort für die Bedienung (alle Ports außer dem Anzeige-Port).
        /// Wird bei jedem Seitenaufruf abgefragt. Der Admin kann es in der Tafel ändern.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Admin-Passwort: Bedienung wie mit Password, zusätzlich kann Password geändert werden.
        /// Sind beide Passwörter leer, gibt es keinen Passwortschutz.
        /// </summary>
        public string AdminPassword { get; set; }
    }
}
