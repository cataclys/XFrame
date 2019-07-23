using System;

[ConfigPath(@"..\DataAcquisitionTool.ini")]
public static class Config
{
    public const string DefaultIPAddress = "127.0.0.1";
    public const int DefaultPort = 7000;

    public const string AssetServer = "http://localhost:59086";


    [ConfigSection("Network")]
    public static bool UseNetworkConfig { get; set; } = false;
    public static string IPAddress { get; set; } = DefaultIPAddress;
    public static int Port { get; set; } = DefaultPort;
    public static TimeSpan TimeOutDuration { get; set; } = TimeSpan.FromSeconds(15);


    public static string GetAPIAddress(string api)
    {
        return $"http://{DefaultIPAddress}:{DefaultPort}{api}";
    }

}
