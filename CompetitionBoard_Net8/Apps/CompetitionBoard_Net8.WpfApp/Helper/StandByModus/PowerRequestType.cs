
namespace CompetitionBoard_Net8.WpfApp.Helper.StandByModus
{
    public enum PowerRequestType
    {
        PowerRequestDisplayRequired = 0, // Not to be used by drivers
        PowerRequestSystemRequired,
        PowerRequestAwayModeRequired, // Not to be used by drivers
        PowerRequestExecutionRequired // Not to be used by drivers
    }
}
