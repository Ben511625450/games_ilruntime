using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;
namespace LuaFramework
{
	public class WWWAsync : MonoBehaviour
	{
        public static WWWAsync instance = null;
        public UnityWebPacket pak;
        public string fileName;
        private bool isCancel;
        private bool isStart = false;
        public TaskCompletionSource<UnityWebPacket> tcs;
        private bool isDestroy = false;
        private List<UnityWebPacket> pakList = new List<UnityWebPacket>();

        public string Text
		{
			get
			{
				return this.pak.www.downloadHandler.text;
			}
		}

		public byte[] Bytes
		{
			get
			{
				return this.pak.www.downloadHandler.data;
			}
		}

		public void Awake()
		{
			bool flag = WWWAsync.instance != null;
			if (flag)
			{
				DebugTool.LogError("重复使用组件,WWWAsync.instance的唯一对象");
			}
			WWWAsync.instance = this;
		}
		
		public void Update()
		{
			bool flag = !this.isStart;
			if (!flag)
			{
				bool flag2 = this.pak == null;
				if (!flag2)
				{
					bool flag3 = this.pak.www == null;
					if (!flag3)
					{
						bool flag4 = this.isCancel;
						if (flag4)
						{
							this.tcs.SetResult(this.pak);
						}
						else
						{
							bool flag5 = !string.IsNullOrEmpty(this.pak.www.error);
							if (flag5)
							{
                                DebugTool.LogError("WWW error: " + this.pak.www.error + ":" + this.pak.localPath);
                                this.pak.func?.Invoke(-1f, this);
                                this.pakList.Add(this.pak);
								this.isCancel = true;
								this.pak = null;
								this.Dispose();
								this.Destroy();
							}
							else
							{
                                this.pak.func?.Invoke(this.pak.www.downloadProgress, this);
                                bool flag6 = !this.pak.www.isDone;
								if (!flag6)
								{
									this.isStart = false;
									bool flag7 = this.pak.urlPath.Contains("http");
									if (flag7)
									{
										//NetworkManager.instance.downCount += 1UL;
										//NetworkManager.instance.downSize += (ulong)((long)this.pak.www.bytes.Length);
                                        AppFacade.Instance.GetManager<NetworkManager>().downCount += 1UL;
                                        AppFacade.Instance.GetManager<NetworkManager>().downSize += (ulong)pak.www.downloadHandler.data.Length;
                                    }
									this.pakList.Add(this.pak);
                                    this.pak.func?.Invoke(1f, this);
                                    this.tcs.SetResult(this.pak);
								}
							}
						}
					}
				}
			}
		}
	
		public void Save()
		{
			string directoryName = Path.GetDirectoryName(this.pak.localPath);
			bool flag = !Directory.Exists(directoryName);
			if (flag)
			{
				Directory.CreateDirectory(directoryName);
			}
			File.WriteAllBytes(this.pak.localPath, this.Bytes);
		}
		public void DownloadAsync(string url, Action<float, object> f)
		{
			bool flag = this.isDestroy;
			if (!flag)
			{
				this.DownloadAsync(new UnityWebPacket
				{
					urlPath = url,
					localPath = string.Empty,
					func = f,
					size = 1UL,
					version = 1
				});
			}
		}
		
		public Task<UnityWebPacket> DownloadAsync(UnityWebPacket pak)
		{
			bool flag = this.isDestroy;
			Task<UnityWebPacket> result;
			if (flag)
			{
				result = null;
			}
			else
			{
				bool flag2 = this.pak != null;
				if (flag2)
				{
					UnityWebRequest www = this.pak.www;
					www?.Dispose();
					this.pak = null;
				}
				string text = string.Empty;
				bool flag3 = pak.urlPath != string.Empty;
				if (flag3)
				{
					text = pak.urlPath;
				}
				else
				{
					text = pak.localPath;
				}
				text = text.Replace(" ", "%20");
				this.pak = pak;
				this.pak.www = new UnityWebRequest(text);
				this.tcs = new TaskCompletionSource<UnityWebPacket>();
				this.isStart = true;
				CancellationToken token = pak.token;
				bool flag4 = true;
				if (flag4)
				{
					pak.token.Register(delegate
					{
						this.isCancel = true;
					});
				}
				result = this.tcs.Task;
			}
			return result;
		}
		
		public async Task<bool> DownloadAsync(List<UnityWebPacket> paks, Action callBack)
		{
			foreach (UnityWebPacket p in paks)
			{
				await this.DownloadAsync(p);
				
			}

			bool result;
			if (this.isDestroy)
			{
				result = false;
			}
			else
			{
				callBack();
				result = true;
			}
			return result;
		}
		
		public async Task<bool> DownloadAsync(List<UnityWebPacket> paks, Action<string, object> callBack)
		{
			foreach (UnityWebPacket p in paks)
			{
				UnityWebPacket unityWebPacket = await this.DownloadAsync(p);
				UnityWebPacket pak = unityWebPacket;
				unityWebPacket = null;
				if (this.isDestroy)
				{
					return false;
				}
				callBack(pak.name, pak);
				this.pak.www.Dispose();
				pak = null;
				
			}

			return true;
		}
	
		public void Dispose()
		{
			foreach (UnityWebPacket unityWebPacket in this.pakList)
			{
				bool flag = unityWebPacket == null;
				if (!flag)
				{
					UnityWebRequest www = unityWebPacket.www;
					if (www != null)
					{
						www.Dispose();
					}
					unityWebPacket.www = null;
				}
			}
		}
	
		private void OnDestroy()
		{
			this.isDestroy = true;
			WWWAsync.instance = null;
			this.Dispose();
		}
		
		public void Destroy()
		{
			this.isDestroy = true;
			WWWAsync.instance = null;
			Object.Destroy(base.gameObject);
		}
	}
}
