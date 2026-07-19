namespace PharmacyDAL.Enums
{
    public enum StockTransactionType
    {
        Purchase,
        Sale,
        Return,
        Adjustment,

        /// <summary>Stock sent back to the supplier from a received purchase Order.</summary>
        PurchaseReturn,

        /// <summary>Stock brought back in because a customer returned a sold item.</summary>
        SaleReturn
    }
}
