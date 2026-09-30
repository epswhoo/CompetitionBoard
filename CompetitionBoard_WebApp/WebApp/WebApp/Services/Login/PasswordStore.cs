using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace WebApp.Services.Login
{
    /// <summary>
    /// Speichert das Passwort der Bedienung in der appsettings.json (UI:Password).
    /// Die Konfiguration lädt die Datei danach selbst neu.
    /// </summary>
    public class PasswordStore
    {
        private static readonly JsonSerializerOptions WriteOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        private readonly string _settingsFile;
        private readonly object _lock = new object();

        public PasswordStore(string settingsFile)
        {
            _settingsFile = settingsFile;
        }

        public void SavePassword(string password)
        {
            lock (_lock)
            {
                JsonNode root = JsonNode.Parse(File.ReadAllText(_settingsFile)) ?? new JsonObject();
                if (root["UI"] is not JsonObject ui)
                {
                    ui = new JsonObject();
                    root["UI"] = ui;
                }
                ui["Password"] = password;
                File.WriteAllText(_settingsFile, root.ToJsonString(WriteOptions));
            }
        }
    }
}
