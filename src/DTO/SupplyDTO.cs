using FusionTech.src.Entity;

namespace FusionTech.src.DTO
{
    public class SupplyDTO
    {
        public class SupplyCreateDto
        {
            public float SupplierQuantity { get; set; }
            public DateTime SupplierDate { get; set; }
            public Guid SupplierId { get; set; }
            public Guid InventoryId { get; set; }
            public Guid ViduoGameVersionId { get; set; }
            
            }
        }

        public class SupplyReadDto
        {
            public Guid SupplyId { get; set; }
            public Guid SupplierId { get; set; }
            public Guid GamesId { get; set; }
            public float SupplierQuantity { get; set; }
            public DateTime SupplierDate { get; set; }
            public Guid InventoryId { get; set; }
            public Supplier? Supplier { get; set; }
            public Inventory? Inventory{ get; set; }
        }

        public class SupplyUpdateDto
        {
            public float SupplierQuantity { get; set; }
            public DateTime SupplierDate { get; set; }
        }
    }

