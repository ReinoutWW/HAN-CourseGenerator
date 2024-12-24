using HAN.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HAN.Data.EF;

public class EntityCollectionSynchronizer(AppDbContext context)
{
    public void SynchronizeCollection<T>(ICollection<T> existing, ICollection<T> updated) where T : BaseEntity
    {
        RemoveObsoleteItems(existing, updated);
        UpdateOrAddItems(existing, updated);
    }

    private void RemoveObsoleteItems<T>(ICollection<T> existing, ICollection<T> updated) where T : BaseEntity
    {
        var itemsToRemove = existing.Where(e => !updated.Any(u => u.Id.Equals(e.Id))).ToList();
        itemsToRemove.ForEach(e => existing.Remove(e));
    }

    private void UpdateOrAddItems<T>(ICollection<T> existing, ICollection<T> updated) where T : BaseEntity
    {
        foreach (var item in updated)
        {
            var existingItem = FindExistingItem(existing, item.Id);
            if (existingItem != null)
            {
                UpdateExistingItem(existingItem, item);
                continue;
            }

            DetachTrackedItem<T>(item.Id);
            existing.Add(item);
        }
    }

    private T FindExistingItem<T>(ICollection<T> collection, object id) where T : BaseEntity
    {
        return collection.FirstOrDefault(e => e.Id.Equals(id));
    }

    private void UpdateExistingItem<T>(T existingItem, T updatedItem) where T : BaseEntity
    {
        context.Entry(existingItem).CurrentValues.SetValues(updatedItem);
    }

    private void DetachTrackedItem<T>(object id) where T : BaseEntity
    {
        var trackedItem = context.Set<T>().Local.FirstOrDefault(e => e.Id.Equals(id));
        if (trackedItem != null)
        {
            context.Entry(trackedItem).State = EntityState.Detached;
        }
    }
}
