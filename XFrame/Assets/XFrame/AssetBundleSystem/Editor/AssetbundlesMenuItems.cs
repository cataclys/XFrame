using UnityEditor;

public class AssetbundlesMenuItems
{
	const string SimulateAssetBundlesMenu = "AssetBundles/Simulate AssetBundles";

	[MenuItem(SimulateAssetBundlesMenu)]
	public static void ToggleSimulateAssetBundle ()
	{
		AssetBundleManager.SimulateAssetBundleInEditor = !AssetBundleManager.SimulateAssetBundleInEditor;
	}

	[MenuItem(SimulateAssetBundlesMenu, true)]
	public static bool ToggleSimulateAssetBundleValidate ()
	{
        UnityEditor.Menu.SetChecked(SimulateAssetBundlesMenu, AssetBundleManager.SimulateAssetBundleInEditor);
		return true;
	}
	
	[MenuItem ("AssetBundles/Build AssetBundles")]
	static public void BuildAssetBundles ()
	{
		BuildScript.BuildAssetBundles();
	}

	//[MenuItem ("AssetBundles/Build Player")]
	//static void BuildPlayer ()
	//{
	//	BuildScript.BuildPlayer();
	//}
}
