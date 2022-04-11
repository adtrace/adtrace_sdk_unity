//
//  Created by Nasser Amini (namini40@github.com) on April 2022.
//  Copyright (c) AdTrace (adtrace.io) . All rights reserved.

using System;
using System.Collections.Generic;

namespace io.adtrace.sdk
{
    public class AdTraceEvent
    {
        internal string currency;
        internal string eventToken;
        internal string callbackId;
        internal string transactionId;
        internal double? revenue;
        internal List<string> partnerList;
        internal List<string> callbackList;
        // iOS specific members
        internal string receipt;
        internal bool isReceiptSet;

        public AdTraceEvent(string eventToken)
        {
            this.eventToken = eventToken;
            this.isReceiptSet = false;
        }

        public void setRevenue(double amount, string currency)
        {
            this.revenue = amount;
            this.currency = currency;
        }

        public void addCallbackParameter(string key, string value)
        {
            if (callbackList == null)
            {
                callbackList = new List<string>();
            }
            callbackList.Add(key);
            callbackList.Add(value);
        }

        public void addPartnerParameter(string key, string value)
        {
            if (partnerList == null)
            {
                partnerList = new List<string>();
            }
            partnerList.Add(key);
            partnerList.Add(value);
        }

        public void setTransactionId(string transactionId)
        {
            this.transactionId = transactionId;
        }

        public void setCallbackId(string callbackId)
        {
            this.callbackId = callbackId;
        }

        // iOS specific methods
        [Obsolete("This is an obsolete method. Please use the adtrace purchase SDK for purchase verification (https://github.com/adtrace/unity_purchase_sdk)")]
        public void setReceipt(string receipt, string transactionId)
        {
            this.receipt = receipt;
            this.transactionId = transactionId;
            this.isReceiptSet = true;
        }
    }
}
