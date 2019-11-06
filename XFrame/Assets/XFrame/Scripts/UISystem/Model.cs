using UnityEngine;
using UniRx;
using XFrame.UI;

/// <summary>
// 该文件根据 Model.xlsx 由程序自动生成
/// </summary>
public class Model
{
	/// <summary>
	/// ID
	/// </summary>
	public string Id { get; set; }
	/// <summary>
	/// 姓名
	/// </summary>
	public string Name { get; set; }
	/// <summary>
	/// 地址
	/// </summary>
	public string Address { get; set; }

}

public class ModelReactive : ISetData<Model>
{
	private Model Data { get; set; } = new Model();
	/// <summary>
	/// ID
	/// </summary>
	public ReactiveProperty<string> Id { get; private set; } = new ReactiveProperty<string>();
	/// <summary>
	/// 姓名
	/// </summary>
	public ReactiveProperty<string> Name { get; private set; } = new ReactiveProperty<string>();
	/// <summary>
	/// 地址
	/// </summary>
	public ReactiveProperty<string> Address { get; private set; } = new ReactiveProperty<string>();
	public ModelReactive()
	{
		Id.Subscribe(value => Data.Id = value);
		Name.Subscribe(value => Data.Name = value);
		Address.Subscribe(value => Data.Address = value);
	}
	public void SetData(Model data)
	{
		Data = data;
		Id.Value = data.Id;
		Name.Value = data.Name;
		Address.Value = data.Address;
	}
	public Model GetData()
	{
		return Data;
	}
}

