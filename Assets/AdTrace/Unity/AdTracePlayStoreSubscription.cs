//
//  Created by Nasser Amini (namini40@github.com) on April 2022.
//  Copyright (c) AdTrace (adtrace.io) . All rights reserved.

using System;
using System.Collections.Generic;

namespace io.adtrace.sdk
{
    public class AdTracePlayStoreSubscription
    {
        internal string price;
        internal string currency;
        internal string sku;
        internal string orderId;
        internal string signature;
        internal string purchaseToken;
        internal string billingStore;
        internal string purchaseTime;
        internal List<string> partnerList;
        internal List<string> callbackList;

        public AdTracePlayStoreSubscription(string price, string currency, string sku, string orderId, string signature, string purchaseToken)
        {
            this.price = price;
            this.currency = currency;
            this.sku = sku;
            this.orderId = orderId;
            this.signature = signature;
            this.purchaseToken = purchaseToken;
        }

        public void setPurchaseTime(string purchaseTime)
        {
            this.purchaseTime = purchaseTime;
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
    }
}
