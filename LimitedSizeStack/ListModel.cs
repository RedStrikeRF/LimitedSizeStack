using DynamicData;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace LimitedSizeStack;

public class ListModel<TItem>
{
    public List<TItem> Items { get; }
    private LimitedSizeStack<TItem> numberHistory;
    private LimitedSizeStack<int> removedIndexHistory;
    public ListModel(int undoLimit) : this(new List<TItem>(), undoLimit)
    {
    }

    public ListModel(List<TItem> items, int undoLimit)
    {
        Items = items;
        numberHistory = new LimitedSizeStack<TItem>(undoLimit);
        removedIndexHistory = new LimitedSizeStack<int>(undoLimit);
        
    }

    public void AddItem(TItem item)
    {
        Items.Add(item);
        numberHistory.Push(item);
    }

    public void RemoveItem(int index)
    {
        if (Items.Count != 0)
        {
            numberHistory.Push(Items[index]);
            removedIndexHistory.Push(index);
            Items.RemoveAt(index);
        }
    }
    public void ReplaceItem(int index)
    {
        var temp = Items[index - 1];

        removedIndexHistory.Push(index);
        numberHistory.Push(Items[index]);
        numberHistory.Push(temp);

        Items[index-1] = Items[index];
        Items[index] = temp;
    }
    public bool CanUndo()
    {
        return numberHistory.Count != 0;
    }

    public void Undo()
    {
        if (CanUndo() && numberHistory.Count == 1)
        {
            var lastOp = numberHistory.Pop();
            if (Items.Contains(lastOp))
            {
                Items.Remove(lastOp);
            }
            else
            {
                Items.Insert(removedIndexHistory.Pop(), lastOp);
            }
        }
        else
        {
            var lastOp = numberHistory.Pop();
            var last1p = numberHistory.Pop();
            if (Items.Contains(lastOp) && Items.Contains(last1p))
            {
                var index = removedIndexHistory.Pop();
                Items[index - 1] = lastOp;
                Items[index] = last1p;
            }
            else
            {
                if (Items.Contains(lastOp))
                {
                    Items.Remove(lastOp);
                }
                else
                {
                    Items.Insert(removedIndexHistory.Pop(), lastOp);
                }
            }
        }
    }
}