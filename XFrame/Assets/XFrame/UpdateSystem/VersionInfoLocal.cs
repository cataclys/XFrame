using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class LocalVersion
{
    public int ver
    {
        get;
        set;
    }
    public void BeginInit(Action onInit, IEnumerable<string> _groups)
    {
        //InitEmbedVer()
        //MergeLocalVer()
        Action onInitEmbed = () =>
            {
                string path = System.IO.Path.Combine(Application.persistentDataPath, "vercache");
                if (System.IO.Directory.Exists(path) == false)
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                string verfile = System.IO.Path.Combine(path, "vercache.ver.txt");

                MergeLocalVer(path, verfile, _groups);

                onInit();
            };

        InitEmbedVer(_groups, onInitEmbed);
    }
    public void Save(IEnumerable<string> groups)
    {
        string path = Path.Combine(Application.persistentDataPath, "vercache");
        if (Directory.Exists(path) == false)
        {
            Directory.CreateDirectory(path);
        }
        string verfile = Path.Combine(path, "vercache.ver.txt");
        SaveLocalVer(path, verfile, groups);
    }
    public void SaveGroup(string group)
    {
        string path = System.IO.Path.Combine(Application.persistentDataPath, "vercache");
        if (System.IO.Directory.Exists(path) == false)
        {
            System.IO.Directory.CreateDirectory(path);
        }
        this.groups[group].SaveLocal(System.IO.Path.Combine(path, group + ".ver.txt"));
    }
    void InitEmbedVer(IEnumerable<string> _groups, Action onInitEmbed)
    {
        int groupcount = 0;
        Action<UnityWebRequest, string> onLoadGroupInfo = (uwr, tag) =>
        {
            string t = uwr.downloadHandler.text;
            if (t.Length > 0 && t[0] == 0xFEFF)
            {
                t = t.Substring(1);
            }
            var rhash = ResourceSystem.Instance.sha1.ComputeHash(uwr.downloadHandler.data);
            var shash = Convert.ToBase64String(rhash);
            if (shash != groups[tag].grouphash)
            {
                Debug.Log("(ver)hash 不匹配:" + tag);
            }
            InitGroupOne(t, tag);
            groupcount--;
            if (groupcount == 0)
            {
                Debug.Log("(ver)InitEmbedVer LoadFinish.");
                onInitEmbed();
            }
        };

        Action<UnityWebRequest, string> onLoadAll = (uwr, tag) =>
            {
                if (string.IsNullOrEmpty(uwr.error) == false)
                {
                    onInitEmbed();
                    return;
                }
                string t = uwr.downloadHandler.text;
                if (t[0] == 0xFEFF)
                {
                    t = t.Substring(1);
                }
                InitGroups(t);
                foreach (var g in _groups)
                {
                    if (groups.ContainsKey(g) == false)
                    {
                        //Debug.LogWarning("请求的Group:" + g+  "本地没有");
                        continue;
                    }
                    groupcount++;
                    ResourceSystem.Instance.LoadFromStreamingAssets(g + ".ver.txt", g, onLoadGroupInfo);
                }
            };

        ResourceSystem.Instance.LoadFromStreamingAssets("allver.ver.txt", "", onLoadAll);


    }
    void InitGroups(string txt)
    {
        string[] lines = txt.Split(new string[] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var l in lines)
        {
            if (l.IndexOf("Ver:") == 0)
            {
                ver = int.Parse(l.Substring(4));
            }
            else
            {
                //Debug.Log(l);
                var sp = l.Split('|');
                groups[sp[0]] = new VerInfo(sp[0], sp[1], int.Parse(sp[2]));
            }
        }
    }
    void InitGroupOne(string txt, string group)
    {
        string[] lines = txt.Split(new string[] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var l in lines)
        {
            if (l.IndexOf("Ver:") == 0)
            {
                var sp = l.Split(new string[] { "Ver:", "|FileCount:" }, StringSplitOptions.RemoveEmptyEntries);
                int mver = int.Parse(sp[0]);
                this.groups[group].listverid = mver;
                int mcount = int.Parse(sp[1]);
                if (ver != mver)
                {
                    Debug.Log("版本不匹配:" + group);
                }
                if (mcount != this.groups[group].groupfilecount)
                {
                    Debug.Log("文件数量不匹配:" + group);
                }
            }
            else
            {
                var sp = l.Split(new char[] { '|', '@' });
                //Debug.Log(l);
                this.groups[group].listfiles[sp[0]] = new ResInfo(group, sp[0], sp[1], int.Parse(sp[2]));
            }
        }
    }
    void MergeLocalVer(string path, string filename, IEnumerable<string> groups)
    {
        bool bSave = true;
        if (File.Exists(filename))
        {
            //Read And Merge
            try
            {
                bSave = !ReadLocalVer(path, filename, groups);
            }
            catch
            {

            }
        }
        if (bSave)
        {
            SaveLocalVer(path, filename, groups);
        }
    }
    void SaveLocalVer(string path, string filename, IEnumerable<string> groups)
    {
        string outtxt = "Ver:" + this.ver + "\n";

        //just Write
        foreach (var g in groups)
        {
            if (this.groups.ContainsKey(g))
            {
                this.groups[g].SaveLocal(System.IO.Path.Combine(path, g + ".ver.txt"));
                outtxt += this.groups[g].group + "|" + this.groups[g].grouphash + "|" + this.groups[g].groupfilecount + "\n";
            }
            else
            {
                Debug.LogWarning("指定的Group:" + g + " 不存在于版本库中，无法保存");
            }
            //Debug.Log("生成本地信息:" + g);
        }
        //foreach(var _g in this.groups)
        //{
        //    outtxt += _g.Key + "|" + _g.Value.grouphash + "|" + _g.Value.groupfilecount + "\n";
        //}
        using (var s = File.Create(filename))
        {
            byte[] b = Encoding.UTF8.GetBytes(outtxt);
            s.Write(b, 0, b.Length);
            Debug.Log("(ver)SaveLocalVer 生成全部信息");
        }
    }
    bool ReadLocalVer(string path, string filename, IEnumerable<string> groups)
    {
        string txt = null;
        using (var s = File.OpenRead(filename))
        {
            byte[] bs = new byte[s.Length];
            s.Read(bs, 0, bs.Length);
            txt = Encoding.UTF8.GetString(bs, 0, bs.Length);
        }
        string[] lines = txt.Split(new string[] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
        int mver = 0;
        string mgroup = null;
        string mhash = null;
        int mfilecount = 0;
        foreach (var l in lines)
        {
            if (l.IndexOf("Ver:") == 0)
            {
                mver = int.Parse(l.Substring(4));
                if (mver < this.ver)
                {
                    Debug.Log("(ver)储存版本旧了,使用嵌入");
                    return false;
                }
                if (mver > this.ver)
                {
                    Debug.Log("(ver)储存版本新，覆盖嵌入");
                }
            }
            else
            {
                //Debug.Log(l);
                var sp = l.Split('|');
                mgroup = sp[0];
                mhash = sp[1];
                mfilecount = int.Parse(sp[2]);
                if (this.groups.ContainsKey(mgroup))
                {
                    if (this.groups[mgroup].grouphash == mhash && this.groups[mgroup].groupfilecount == mfilecount)
                    {
                        Debug.Log("group未改变:" + mgroup);
                        continue;
                    }
                }
                var g = new VerInfo(mgroup, mhash, mfilecount);
                bool b = g.ReadLocal(System.IO.Path.Combine(path, mgroup + ".ver.txt"));
                if(b)
                {
                    this.groups[mgroup] = g;
                    Debug.Log("(ver)覆盖Group:" + mgroup);
                }
                else
                {
                     Debug.Log("Group读取失败:" + mgroup);
                }
            }
        }
        return true;
    }
    public enum ResState
    {
        ResState_UseLocal = 0x01,      //使用本地资源
        ResState_UseDownloaded = 0x02, //使用下载的资源
        ResState_UseRemote = 0x03,      //使用远程的资源
    }
    
    public class ResInfo
    {
        public ResInfo(string group, string name, string hash, int size)
        {
            this.name = name;
            this.hash = hash;
            this.size = size;
            this.group = group;
            this.state = ResState.ResState_UseLocal;
            PathList = GetPathList(name);
            FileName = PathList[PathList.Length - 1];
        }
        public string FileName;
        public string[] PathList;
        public static string[] GetPathList(String pathname)
        {
            pathname = pathname.Replace('\\', '/');
            var list = pathname.Split('/');

            return list;
        }
        public ResState state;
        public bool needupdate = false;
        public string group;
        public string name;
        public int size;
        public string hash;

        public override string ToString()
        {
            
            return this.name + "|" + hash + "|" + size + "|" + (needupdate?(16+ (int)state):(int)state);
        }

        public void Download(Action<ResInfo, Exception> onDown, bool UpdateVerInfo = true)
        {

            Action<UnityWebRequest, string> load = (uwr, tag) =>
            {
                Exception _err = null;
                try
                {
                    ResourceSystem.Instance.SaveToCache(group + "/" + name, uwr.downloadHandler.data);
                    this.size = uwr.downloadHandler.data.Length;
                    this.hash = Convert.ToBase64String(ResourceSystem.Instance.sha1.ComputeHash(uwr.downloadHandler.data));
                    this.state = ResState.ResState_UseDownloaded;
                    this.needupdate = false;
                   
                }
                catch (Exception err)
                {
                    _err = err;
                }

                if (this.size == 0)
                {
                    _err = new Exception("下载size==0"+uwr.url);
                }

                if (_err == null && UpdateVerInfo)
                {//保存信息

                        ResourceSystem.Instance.verLocal.SaveGroup(group);

                }
                if (onDown != null)
                {
                    onDown(this, _err);
                }
            };

            ResourceSystem.Instance.LoadFromRemote(group + "/" + name, "", load);
            //int i = (int)this.state & 0x0F;
          //  this.state = ResState.ResState_InUpdate | (ResState)i;
            //this.state |= ResState.ResState_NeedUpdate;
            this.needupdate = true;
            Debug.Log("-Download-------->" + this.state);
        }
        public void BeginLoadAssetBundle(Action<AssetBundle, string> onLoad)
        {
            if ((this.state & ResState.ResState_UseLocal) == ResState.ResState_UseLocal)
            {
                Action<UnityWebRequest, string> onloadw = (uwr, tag) =>
                {
                    try
                    {                       
                        AssetBundle bundle = (uwr.downloadHandler as DownloadHandlerAssetBundle).assetBundle;
                        onLoad(bundle, tag);
                    }
                    catch
                    {
                        Debug.Log("assetBundle加载失败");
                        //onLoad(null, tag);
                    }
                };
                ResourceSystem.Instance.LoadFromStreamingAssets(group + "/" + name, group + "/" + name, onloadw);
            }
            else if ((this.state & ResState.ResState_UseDownloaded) == ResState.ResState_UseDownloaded)
            {
                ResourceSystem.Instance.LoadAssetBundleFromCache(group + "/" + name, group + "/" + name, onLoad);
            }
            else
            {
                throw new Exception("这个资源不能加载");
            }
        }
        public void BeginLoadBytes(Action<byte[], string> onLoad)
        {
            if ((this.state & ResState.ResState_UseLocal) == ResState.ResState_UseLocal)
            {
                Action<UnityWebRequest, string> onloadw = (uwr, tag) =>
                    {
                        onLoad(uwr.downloadHandler.data, tag);
                    };
                ResourceSystem.Instance.LoadFromStreamingAssets(group + "/" + name, group + "/" + name, onloadw);
            }
            else if ((this.state & ResState.ResState_UseDownloaded) == ResState.ResState_UseDownloaded)
            {
                ResourceSystem.Instance.LoadBytesFromCache(group + "/" + name, group + "/" + name, onLoad);
            }
            else
            {
                throw new Exception("这个资源不能加载");
            }
        }
        public void BeginLoadTexture2D(Action<Texture2D, string> onLoad)
        {
            if ((this.state & ResState.ResState_UseLocal) == ResState.ResState_UseLocal)
            {
                Action<UnityWebRequest, string> onloadw = (uwr, tag) =>
                {
                    //Texture2D tex = new Texture2D(1, 1,TextureFormat.ARGB32,false);
                    //tex.LoadImage(uwr.bytes);
                    Texture2D tex = (uwr.downloadHandler as DownloadHandlerTexture).texture;
                    onLoad(tex, tag);
                };
                ResourceSystem.Instance.LoadFromStreamingAssets(group + "/" + name, group + "/" + name, onloadw);
            }
            else if ((this.state & ResState.ResState_UseDownloaded) == ResState.ResState_UseDownloaded)
            {
                ResourceSystem.Instance.LoadTexture2DFromCache(group + "/" + name, group + "/" + name, onLoad);
            }
            else
            {
                throw new Exception("这个资源不能加载");
            }
        }
        public void BeginLoadString(Action<string, string> onLoad)
        {
            if ((this.state & ResState.ResState_UseLocal) == ResState.ResState_UseLocal)
            {
                Action<UnityWebRequest, string> onloadw = (uwr, tag) =>
                {

                    string t = uwr.downloadHandler.text;
                    if (t.Length <= 0) return;
                    if (t[0] == 0xFEFF)
                    {
                        t = t.Substring(1);
                    }
                    onLoad(t, tag);
                };
                ResourceSystem.Instance.LoadFromStreamingAssets(group + "/" + name, group + "/" + name, onloadw);
            }
            else if ((this.state & ResState.ResState_UseDownloaded) == ResState.ResState_UseDownloaded)
            {
                ResourceSystem.Instance.LoadStringFromCache(group + "/" + name, group + "/" + name, onLoad);
            }
            else
            {
                throw new Exception("这个资源不能加载");
            }
        }
    }
    public class VerInfo
    {
        public VerInfo(string group, string hash, int filecount)
        {
            this.group = group;
            this.grouphash = hash;
            this.groupfilecount = filecount;
        }

        public string group
        {
            get;
            private set;
        }
        public string grouphash
        {
            get;
            set;
        }
        public int groupfilecount
        {
            get;
            set;
        }
        public int listverid
        {
            get;
            set;
        }
        public Dictionary<string, ResInfo> listfiles = new Dictionary<string, ResInfo>();

        public void SaveLocal(string filename)
        {
            string outstr = "Ver:" + listverid + "|FileCount:" + groupfilecount + "\n";
            foreach (var l in listfiles)
            {
                outstr += l.Value.ToString() + "\n";
            }
            using (var s = System.IO.File.Create(filename))
            {
                byte[] b = System.Text.Encoding.UTF8.GetBytes(outstr);
                s.Write(b, 0, b.Length);

            }
            //System.IO.File.WriteAllText(filename, outstr);
        }
        public bool ReadLocal(string filename)
        {
            try
            {
                string txt = null;
                using (var s = System.IO.File.OpenRead(filename))
                {
                    byte[] bs = new byte[s.Length];
                    s.Read(bs, 0, bs.Length);
                    txt = System.Text.Encoding.UTF8.GetString(bs, 0, bs.Length);
                }
                //string txt = System.IO.File.ReadAllText(filename);
                string[] lines = txt.Split(new string[] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
                if (lines.Length == 0) return false;
                foreach (var l in lines)
                {
                    if (l.IndexOf("Ver:") == 0)
                    {
                        var sp = l.Split(new string[] { "Ver:", "|FileCount:" }, StringSplitOptions.RemoveEmptyEntries);
                        int mver = int.Parse(sp[0]);
                        int mcount = int.Parse(sp[1]);
                        this.groupfilecount = mcount;
                        this.listverid = mver;

                    }
                    else
                    {
                        var sp = l.Split('|');
                        listfiles[sp[0]] = new ResInfo(this.group, sp[0], sp[1], int.Parse(sp[2]));
                        int spp = int.Parse(sp[3]);
                        listfiles[sp[0]].state = (ResState)(spp % 16);
                        int spneedupadte = spp / 16;
                        listfiles[sp[0]].needupdate = spneedupadte > 0;
                    }
                }
                if (listfiles.Count == 0) return false;
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
    public Dictionary<string, VerInfo> groups = new Dictionary<string, VerInfo>();
    public List<ResInfo> GetResInfoList(String path)
    {
        var pathlist = ResInfo.GetPathList(path);
        //Debug.Log("************* pathlist[0]:" + pathlist[0]);
		if(!groups.ContainsKey(pathlist[0]))
		{
			Debug.Log("path err"+path);
		}
        var GroudList = groups[pathlist[0]].listfiles;
        List<ResInfo> RetList = new List<ResInfo>();
        foreach (var rif in GroudList.Values)
        {
            Boolean flag = true;
            if (pathlist.Length > 1)
            {
                for (int i = 1; i < pathlist.Length; i++)
                {

                    if (rif.PathList[i - 1] != pathlist[i])
                    {
                        flag = false;
                        break;

                    }

                }
            }
            if (flag)
                RetList.Add(rif);
        }

        return RetList;
    }
}
