//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using LOGI.Framework.Toolkit.Foundation.Repository;

namespace LOGI.Framework.Toolkit.Core.SendMessage.Model
{
    ///<summary>
    ///</summary>
    public partial class SMSQueue:EntityBase
    {
        #region Primitive Properties

        public virtual int SMSId
        {
            get;
            set;
        }

        public virtual System.DateTime QueueDate
        {
            get;
            set;
        }

        public virtual Nullable<System.DateTime> SendDate
        {
            get;
            set;
        }

        public virtual string SMSTo
        {
            get;
            set;
        }

        public virtual string SMSFrom
        {
            get;
            set;
        }

        public virtual string SMSSubject
        {
            get;
            set;
        }

        public virtual string SMSBody
        {
            get;
            set;
        }

        public virtual Nullable<int> SMSStatus
        {
            get;
            set;
        }

        #endregion
    }
}