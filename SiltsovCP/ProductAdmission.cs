//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SiltsovCP
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProductAdmission
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public System.DateTime TakedDate { get; set; }
    
        public virtual Product Product { get; set; }
    }
}
