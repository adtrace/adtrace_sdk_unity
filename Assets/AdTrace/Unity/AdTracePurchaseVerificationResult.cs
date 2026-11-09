namespace io.adtrace.sdk
{
    public class AdTracePurchaseVerificationResult
    {
        public string VerificationStatus { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }

        public AdTracePurchaseVerificationResult(string verificationStatus, int code, string message)
        {
            this.VerificationStatus = verificationStatus;
            this.Code = code;
            this.Message = message;
        }

        public AdTracePurchaseVerificationResult(string jsonString)
        {
            JSONNode node = JSON.Parse(jsonString);
            if (node == null)
            {
                return;
            }

            this.VerificationStatus = node["verificationStatus"].Value;
            this.Code = node["code"].AsInt;
            this.Message = node["message"].Value;
        }
    }
}
