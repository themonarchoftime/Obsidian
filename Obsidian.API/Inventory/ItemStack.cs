﻿using Obsidian.API.Inventory.DataComponents;
using Obsidian.API.Registries;

namespace Obsidian.API.Inventory;

public sealed record class ItemStack : DataComponentsStorage, IEquatable<ItemStack>
{
    public static readonly ItemStack Air = new(ItemsRegistry.Air, 0);

    public int Count { get; set; }

    public Item Holder { get; }

    public Material Type => this.Holder.Type;

    public bool Unbreakable => this.GetComponent<SimpleDataComponent<bool>>(DataComponentType.Unbreakable)?.Value ?? false;
    public int MaxStackSize => this.GetComponent<SimpleDataComponent<int>>(DataComponentType.MaxStackSize).Value;
    public ChatMessage? CustomName => this.GetComponent<SimpleDataComponent<ChatMessage>>(DataComponentType.CustomName)?.Value;
    public ChatMessage? ItemName => this.GetComponent<SimpleDataComponent<ChatMessage>>(DataComponentType.ItemName)?.Value;

    public int Damage => this.GetComponent<SimpleDataComponent<int>>(DataComponentType.Damage)?.Value ?? 0;

    public bool IsAir => Type == Material.Air;

    public ItemStack(Item holder, int count = 1, params List<IDataComponent> components)
    {
        this.Holder = holder;
        this.Count = count;

        this.InitializeComponents(components);
    }

    public static ItemStack operator -(ItemStack item, int value)
    {
        if (item.Count <= 0)
            return Air;

        item.Count = Math.Max(0, item.Count - value);

        return item;
    }

    public static ItemStack operator +(ItemStack item, int value)
    {
        if (item.Count >= item.MaxStackSize)
            return item;

        item.Count = Math.Min(item.MaxStackSize, item.Count + value);

        return item;
    }

    public static ItemStack operator +(ItemStack item, ItemStack value)
    {
        if (item.Count >= item.MaxStackSize)
            return item;

        item.Count = Math.Min(item.MaxStackSize, item.Count + value.Count);

        return item;
    }

    private void InitializeComponents(params List<IDataComponent> components)
    {
        // Every item gets these components
        foreach (var defaultComponent in ComponentBuilder.DefaultItemComponents)
            this.Add(defaultComponent);

        foreach (var component in components)
        {
            if (this.TryGetComponent(component.Type, out var resolvedComponent))
            {
                resolvedComponent = component;
                continue;
            }

            this.Add(component);
        }
    }

}
