using System.Security.Cryptography;
using System.Text;
using WebApp.Configs;

namespace WebApp.Services.Login
{
    /// <summary>
    /// Regeln für den Passwortschutz der Bedienung.
    /// </summary>
    public static class PasswordCheck
    {
        public static bool IsDisplayPort(UIConfig uiConfig, int localPort)
        {
            return uiConfig.DisplayPort > 0 && localPort == uiConfig.DisplayPort;
        }

        /// <summary>
        /// Die Bedienung ist gesperrt, sobald eines der beiden Passwörter gesetzt ist.
        /// </summary>
        public static bool IsLockEnabled(UIConfig uiConfig)
        {
            return !string.IsNullOrEmpty(uiConfig.Password) || !string.IsNullOrEmpty(uiConfig.AdminPassword);
        }

        public static AccessLevel GetAccessLevel(UIConfig uiConfig, string password)
        {
            if (IsEqual(uiConfig.AdminPassword, password))
            {
                return AccessLevel.Admin;
            }
            if (IsEqual(uiConfig.Password, password))
            {
                return AccessLevel.Operator;
            }
            return AccessLevel.Locked;
        }

        /// <summary>
        /// Liefert eine Fehlermeldung, wenn das neue Passwort nicht verwendet werden kann, sonst null.
        /// </summary>
        public static string ValidateNewPassword(UIConfig uiConfig, string password, string repeatedPassword)
        {
            if (string.IsNullOrEmpty(password))
            {
                return "Das Passwort darf nicht leer sein.";
            }
            if (password != repeatedPassword)
            {
                return "Die Passwörter stimmen nicht überein.";
            }
            if (password == uiConfig.AdminPassword)
            {
                return "Das Passwort darf nicht dem Admin-Passwort entsprechen.";
            }
            return null;
        }

        private static bool IsEqual(string configured, string entered)
        {
            if (string.IsNullOrEmpty(configured) || entered == null)
            {
                return false;
            }
            return CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(entered),
                Encoding.UTF8.GetBytes(configured));
        }
    }
}
