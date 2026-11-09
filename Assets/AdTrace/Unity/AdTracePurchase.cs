namespace io.adtrace.sdk
{
    public class AdTracePurchase
    {
        internal string productId;
        internal string purchaseToken;
        internal string transactionId;
        internal string receipt;

        public AdTracePurchase(string productId, string purchaseToken)
        {
            this.productId = productId;
            this.purchaseToken = purchaseToken;
        }

        public AdTracePurchase(string transactionId, string productId, string receipt)
        {
            this.transactionId = transactionId;
            this.productId = productId;
            this.receipt = receipt;
        }
    }
}
