using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitionBoardWpfApp.Helper
{
    public class ErrorMessageSvc
    {
        public string GetMessage(int errorCode)
        {
            switch (errorCode)
            {
                case 1:
                    return "Unbekannter Fehler aus einem Service";
                case 10:
                    return "Exception aus dem DB-Service";
                case 20:
                    return "Exception aus einem Repository";
                case 30:
                    return "Zu hinzufügendes Element liegt außerhalb des erlaubten Bereichs";
                case 26:
                    return "Eine Reiter-Pferd-Kombi kann nicht gleichzeitig disqualifiziert und platziert sein";
                case 25:
                    return "Eine Reiter-Pferd-Kombi kann nicht gleichzeitig disqualifiziert und benotet sein";
                case 24:
                    return "Die Note ist nicht gültig. Sie muss zwischen 0,0 und 10,0 liegen";
                case 27:
                    return "Eine Reiter-Pferd-Kombi kann nicht gleichzeitig platziert und nicht benotet sein";
                case 21:
                    return "Eine Reiter-Pferd-Kombi kann nicht gleichzeitig disqualifiziert sein und einen nicht abgeschlossenen Status haben";
                case 23:
                    return "Eine Reiter-Pferd-Kombi kann nicht gleichzeitig platziert sein und einen nicht abgeschlossenen Status haben";
                case 22:
                    return "Eine Reiter-Pferd-Kombi kann nicht gleichzeitig benotet sein und einen nicht abgeschlossenen Status haben";
                case 40:
                    return "Die Note ist kein gültiger Zahlenwert";
                default:
                    return "Unbekannter Fehler";
            }
        }
    }
}
