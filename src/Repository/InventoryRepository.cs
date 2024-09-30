using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FusionTech.src.Database;
using Microsoft.EntityFrameworkCore;

namespace FusionTech.src.Entity
{
    public class InventoryRepository
    {
        protected DbSet<Inventory> _Inventory;
        protected DatabaseContext _databaseContext;

        public InventoryRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
            _Inventory = databaseContext.Set<Inventory>();
        }

        public async Task<Inventory> GetByIdAsync(int id)
        {
            return await _Inventory.FindAsync(id);
        }

        public async Task<Inventory> AddAsync(Inventory newInventory)
        {
            await _Inventory.AddAsync(newInventory);
            await _databaseContext.SaveChangesAsync();
            return newInventory;
        }

        public async Task<bool> UpdateOnAsync(Inventory updateInventory)
        {
            _Inventory.Update(updateInventory);
            await _databaseContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteOnAsync(Inventory inventory)
        {
            
                _Inventory.Remove(inventory);
                await _databaseContext.SaveChangesAsync();
            return true;
        }
    }
}