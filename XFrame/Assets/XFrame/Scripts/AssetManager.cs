using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.Networking;

public class AssetManager : Singleton<AssetManager>
{
    public static string SinglepointIconPath = "SinglepointIcon";

    public static string MultiplepointIconClosedPath = "MultiplepointIconClosed";

    public static string MultiplepointCloseShadow = "MultiplepointCloseShadow";
}
