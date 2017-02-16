//Can be generated by ADO.NET Entity Data Model by code first. Refactoring to required entity class
//

namespace ISV1.web.Areas.CU.DAL.ADO_EF.Model
{
    using Sage.CA.SBS.ERP.Sage300.Common.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ARCUS")]
    public partial class ARCustomer 
    {
        [Key]
        [Column("IDCUST")]
        public string CustomerNumber { get ; set; }

        [Column("NAMECUST")]
        public string CustomerName { get; set; }

        [Column("EMAIL2")]
        public string Email { get; set; }

        [Column("TEXTPHON2")]
        public string FaxNumber { get; set; }

        [Column("NAMECTAC")]
        public string ContactName { get; set; }

        [Column("EMAIL1")]
        public string ContactsEmail { get; set; }

        [Column("IDACCTSET")]
        public string AccountSet { get; set; }

        [Column("WEBSITE")]
        public string WebSite { get; set; }

        [Column("BILLMETHOD")]
        public short BillMethod { get; set; }

        [Column("PAYMCODE")]
        public string PaymentCode { get; set; }

        public IList<ARCustomerOptionalField> ARCustomerOptionalFields { get; set; }
    }
}
