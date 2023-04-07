using ReactiveUI;

namespace LimitedSizeStack.UI;

public class TaskListViewModel : ReactiveObject
{
	public string[] Items => items;

	public bool CanUndo => model.CanUndo();

	private readonly ListModel<string> model;

	private string[] items;

	public TaskListViewModel(ListModel<string> listModel)
	{
		model = listModel;
		Update();
	}

	public void AddItem(string item)
	{
		model.AddItem(item);
		Update();
	}

	public void RemoveItem(int index)
	{
		model.RemoveItem(index);
		Update();
	}

	public void Undo()
	{
		model.Undo();
		Update();
	}

    public void MoveUpItem(int index)
    {
        if (index < 0 || index >= model.Items.Count + 1) return;

        // Получаем элементы, которые нужно поменять местами
        //var item1 = model.Items[index];
        //var item2 = model.Items[index - 1];
        //// Меняем местами элементы
        //model.Items[index] = item2;
        //model.Items[index - 1] = item1;

        model.ReplaceItem(index);
        
        Update();
    }

    private void Update()
	{
		this.RaiseAndSetIfChanged(ref items, model.Items.ToArray(), nameof(Items));
	}
}